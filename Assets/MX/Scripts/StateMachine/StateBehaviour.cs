using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//
public class StateBehaviour : MonoBehaviour, IStateManager
{
    [SerializeField]
    private int _state_index = StateContext.INDEX_INVALID;
    [SerializeField]
    private string _state_name = StateContext.NAME_INVALID;

    //
    private StateManager _state_manager = new StateManager();

    //
    void Awake()
    {
        this._state_manager.Register(StateContext.INDEX_DEFAULT, this)
            .OnStarting.AddListener((context) =>
            {
                Debug.Log("Test");
            })
            .OnUpdate.AddListener((context) => { })
            .OnEnding.AddListener((context) => { });
        this._state_manager.Register("idle", this, true);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    [StateContextCallback(StateContext.CALLBACK_STARTING)]
    protected void OnStateContextStarting(IStateContext context)
    {
        this._state_index = _state_manager.Current.Index;
        this._state_name = _state_manager.Current.Name;

        Debug.Log(context.Name + " Starting");
    }

    [StateContextCallback(StateContext.CALLBACK_UPDATE)]
    protected void OnStateContextUpdate(IStateContext context)
    {
        //Debug.Log(context.Name);
    }

    [StateContextCallback(StateContext.CALLBACK_ENDING)]
    protected void OnStateContextEnding(IStateContext context)
    {
        Debug.Log(context.Name + " Ending");
    }

    //
    void FixedUpdate()
    {
        if(this._state_manager != null)
        {
            this._state_manager.Update();
        }
    }

    // Update is called once per frame
    void Update()
    {  
    }
}
