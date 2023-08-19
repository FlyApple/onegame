using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//
public enum StateContextType
{
    None        = 0,    //Loop
    Default     = 1,    //Loop
    Once        = 2,
    Loop        = 3,
}

public enum StateContextStatus
{
    None        = 0,
    Starting    = 1,
    Update      = 2,
    Ending      = 3,
    Ended       = 4,
}

///
public interface IStateListener
{
    IStateContext AddListener(System.Action<IStateContext> action);
    IStateContext RemoveListener(System.Action<IStateContext> action);
    IStateContext RemoveAllListener(System.Action<IStateContext> action);
}

/// <summary>
/// 
/// </summary>
public interface IStateContext
{
    int Index { get; }
    string Name { get; }
    StateContextType Type { get; }
    StateContextStatus Status { get; }

    //
    IStateListener OnStarting { get; }
    IStateListener OnUpdate { get; }
    IStateListener OnEnding { get; }

    //
    void Start();
    void Update();
    void Ending();
    void Ended();

}


//
public interface IStateManager
{

}

/// <summary>
/// 
/// </summary>
public class StateContextCallbackAttribute : System.Attribute
{
    public string name = "";

    public int context_index = StateContext.INDEX_INVALID;
    public string context_name = StateContext.NAME_INVALID;
    public StateContextCallbackAttribute(string name = StateContext.CALLBACK_STARTING,
        int index = StateContext.INDEX_INVALID,
        string cname = StateContext.NAME_INVALID)
    {
        this.name = name;
        this.context_index = index;
        this.context_name = cname.Trim().ToLower();
        if(this.context_name != StateContext.NAME_INVALID)
        {
            this.context_index = StateContext.INDEX_INVALID;
        }

    }
}

//
public class StateListener : IStateListener
{
    private IStateContext _context;
    private event System.Action<IStateContext> _listener;

    public StateListener(IStateContext context)
    {
        this._context = context;
    }

    public IStateContext AddListener(System.Action<IStateContext> action)
    {
        _listener += action;
        return this._context;
    }
    public IStateContext RemoveListener(System.Action<IStateContext> action)
    {
        _listener -= action;
        return this._context;
    }
    public IStateContext RemoveAllListener(System.Action<IStateContext> action)
    {
        _listener = null;
        return this._context;
    }

    public void Invoke()
    {
        if(this._listener != null)
        {
            this._listener(this._context);
        }
    }
}

//
public class StateContext : IStateContext
{
    //
    public const int INDEX_INVALID = -1;
    public const int INDEX_NONE = 0;
    public const int INDEX_DEFAULT = 1;
    //
    public const string NAME_INVALID = "invalid";
    public const string NAME_NONE = "none";
    public const string NAME_DEFAULT = "default";
    //
    public const string CALLBACK_STARTING = "on_starting";
    public const string CALLBACK_UPDATE = "on_update";
    public const string CALLBACK_ENDING = "on_ending";

    protected int _index = StateContext.INDEX_INVALID;
    protected string _name = StateContext.NAME_INVALID;
    protected StateContextType _type = StateContextType.None;
    protected StateContextStatus _status = StateContextStatus.None;

    private double _tick = 0.0f;
    private float _time = 0.0f;
    private long _count = 0;

    public int Index { get { return _index; } }
    public string Name { get { return _name; } }
    public StateContextType Type { get { return _type; } }
    public StateContextStatus Status { get { return _status; } }

    /// <summary>
    /// 
    /// </summary>
    public object object_reference = null;
    public System.Reflection.MethodInfo object_method_starting = null;
    public System.Reflection.MethodInfo object_method_update = null;
    public System.Reflection.MethodInfo object_method_ending = null;

    private StateListener _on_starting;
    private StateListener _on_update;
    private StateListener _on_ending;

    public IStateListener OnStarting
    {
        get { return this._on_starting; }
    }

    public IStateListener OnUpdate
    {
        get { return this._on_update; }
    }

    public IStateListener OnEnding
    {
        get { return this._on_ending; }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="index"></param>
    /// <param name="name"></param>
    /// <param name="type"></param>
    /// <param name="status"></param>
    public StateContext(int index, string name,
        StateContextType type = StateContextType.None,
        StateContextStatus status = StateContextStatus.None)
    {
        this._index = index;
        this._name = name.Trim().ToLower();
        this._type = type;
        this._status = status;

        //
        this._tick = 0.0f;
        this._time = 0.0f;

        //
        this._on_starting = new StateListener(this);
        this._on_update = new StateListener(this);
        this._on_ending = new StateListener(this);
    }

    public double Timestamp64()
    {
        long ticks = System.DateTime.Now.Ticks;
        return (double)ticks / TimeSpan.TicksPerMillisecond;
    }

    public void Start()
    {
        this._tick = this.Timestamp64();
        this._time = 0.0f;
        this._count = 0;
        this._status = StateContextStatus.Starting;

        //
        if(this.object_reference != null && this.object_method_starting != null)
        {
            this.object_method_starting.Invoke(this.object_reference, new object[] { this });
        }

        this._on_starting.Invoke();
    }

    public void Update()
    {
        this._status = StateContextStatus.Update;
        this._count++;

        if (this.object_reference != null && this.object_method_update != null)
        {
            this.object_method_update.Invoke(this.object_reference, new object[] { this });
        }

        this._on_update.Invoke();
    }

    public void Ending()
    {
        this._status = StateContextStatus.Ending;
        this._time = (float)(this.Timestamp64() - this._tick);

        if (this.object_reference != null && this.object_method_ending != null)
        {
            this.object_method_ending.Invoke(this.object_reference, new object[] { this });
        }

        this._on_ending.Invoke();
    }

    public void Ended()
    {
        this._status = StateContextStatus.Ended;
    }
}

//
public class StateManager : IStateManager
{
    protected IStateContext _default_context = null;
    protected IStateContext _current_context = null;
    protected IStateContext _next_context = null;

    protected List<IStateContext> _contexts;

    public IStateContext Current
    {
        get { return _current_context; }
    }

    public StateManager()
    {
        this._contexts = new List<IStateContext>();

        //
        var context = new StateContext(StateContext.INDEX_NONE, StateContext.NAME_NONE,
            StateContextType.None, StateContextStatus.None);

        this.RegisterImpl(context);
    }

    public IStateContext Register(int index, object reference = null, bool loop = false)
    {
        if(index <= StateContext.INDEX_NONE)
        {
            return null;
        }

        StateContextType type = StateContextType.None;
        if(index == StateContext.INDEX_DEFAULT)
        {
            type = StateContextType.Default;
        }
        else if(index > StateContext.INDEX_DEFAULT)
        {
            type = StateContextType.Once;
            if(loop)
            {
                type = StateContextType.Loop;
            }
        }

        var state = new StateContext(index, StateContext.NAME_INVALID, type);
        if(!this.RegisterImpl(state, reference))
        {
            return null;
        }
        return state;
    }


    public IStateContext Register(string name, object reference = null, bool loop = false)
    {
        name = name.Trim().ToLower();
        if (name == StateContext.NAME_INVALID || name == StateContext.NAME_NONE)
        {
            return null;
        }

        StateContextType type = StateContextType.None;
        if (name == StateContext.NAME_DEFAULT)
        {
            type = StateContextType.Default;
        }
        else
        {
            type = StateContextType.Once;
            if (loop)
            {
                type = StateContextType.Loop;
            }
        }

        var state = new StateContext(StateContext.INDEX_INVALID, name, type);
        if(!this.RegisterImpl(state, reference))
        {
            return null;
        }
        return state;
    }

    private bool RegisterImpl(StateContext context, object reference = null)
    {
        if(context.Index >= StateContext.INDEX_NONE)
        {
            var item = this._contexts.Find((v) => { return v.Index == context.Index; });
            if(item != null)
            {
                return false;
            }
        }
        else if(context.Name.Length > 0 && context.Name != StateContext.NAME_INVALID)
        {
            var item = this._contexts.Find((v) => { return v.Name == context.Name; });
            if (item != null)
            {
                return false;
            }
        }
        else
        {
            return false;
        }

        //
        if (reference != null)
        {
            context.object_reference = reference;
            System.Type type = reference.GetType();
            var methods = type.GetMethods(System.Reflection.BindingFlags.Default | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            foreach (var method in methods)
            {
                var attributes = method.GetCustomAttributes(typeof(StateContextCallbackAttribute), true);
                if (attributes == null || attributes.Length == 0) { continue; }

                var attribute = (StateContextCallbackAttribute)attributes[0];
                if (attribute.name == StateContext.CALLBACK_STARTING)
                {
                    if (context.Index >= StateContext.INDEX_NONE && attribute.context_index == context.Index)
                    {
                        context.object_method_starting = method;
                    }
                    else if(context.Name != StateContext.NAME_INVALID && attribute.context_name == context.Name)
                    {
                        context.object_method_starting = method;
                    }
                    else if(context.object_method_starting == null &&
                        attribute.context_index == StateContext.INDEX_INVALID && attribute.context_name == StateContext.NAME_INVALID)
                    {
                        context.object_method_starting = method;
                    }
                }
                else if (attribute.name == StateContext.CALLBACK_UPDATE)
                {
                    if (context.Index >= StateContext.INDEX_NONE && attribute.context_index == context.Index)
                    {
                        context.object_method_update = method;
                    }
                    else if (context.Name != StateContext.NAME_INVALID && attribute.context_name == context.Name)
                    {
                        context.object_method_update = method;
                    }
                    else if (context.object_method_starting == null &&
                        attribute.context_index == StateContext.INDEX_INVALID && attribute.context_name == StateContext.NAME_INVALID)
                    {
                        context.object_method_update = method;
                    }
                }
                else if (attribute.name == StateContext.CALLBACK_ENDING)
                {
                    if (context.Index >= StateContext.INDEX_NONE && attribute.context_index == context.Index)
                    {
                        context.object_method_ending = method;
                    }
                    else if (context.Name != StateContext.NAME_INVALID && attribute.context_name == context.Name)
                    {
                        context.object_method_ending = method;
                    }
                    else if (context.object_method_starting == null &&
                        attribute.context_index == StateContext.INDEX_INVALID && attribute.context_name == StateContext.NAME_INVALID)
                    {
                        context.object_method_ending = method;
                    }
                }
            }
        }

        //
        this._contexts.Add(context);
        if(context.Type == StateContextType.None || context.Type == StateContextType.Default)
        {
            this._default_context = context;
        }
        //
        if(this._current_context == null || this._current_context.Type == StateContextType.None)
        {
            this._current_context = this._default_context;
        }
        return true;
    }

    public void UpdateContext(IStateContext context)
    {
        if(context == null)
        {
            return;
        }

        if(context.Status == StateContextStatus.None)
        {
            context.Start();
        }
        else if(context.Status == StateContextStatus.Starting)
        {
            context.Update();
        }
        else if(context.Status == StateContextStatus.Update)
        {
            if (context.Type == StateContextType.Once)
            {
                context.Ending();
            }
            else
            {
                context.Update();
            }
        }
        else if(context.Status == StateContextStatus.Ending)
        {
            context.Ended();
        }
    }

    public void Update()
    {
        if(this._current_context != null)
        {
            this.UpdateContext(this._current_context);
        }
    }
}
