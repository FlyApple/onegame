using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum UIPanelIndex
{
    //
    None = -1,
    //Main Views

    //Sub views
    ViewNone        = 100,
    ViewDemo        = 101,

    //Popup
    PopupNone = 1000,
    PopupDemo = 1001,

    //Dialog
    DialogNone = 2000,
}


/// <summary>
/// 
/// </summary>
public interface UIIPanel
{
    public static UIPanelIndex TIndex { get; }

    //
    public UIPanelIndex Index { get; }
    public void OnInitialize();
}

/// <summary>
/// 
/// </summary>
public class UIPanel : MonoBehaviour, UIIPanel
{
    public static UIPanelIndex TIndex { get { return UIPanelIndex.None; } }

    [SerializeField]
    protected UIPanelIndex _index = UIPanelIndex.None;
    public UIPanelIndex Index { get { return this._index; } }

    [SerializeField]
    protected bool _ready = false;
    /// <summary>
    /// 默认不显示：false
    /// </summary>
    [SerializeField]
    protected bool _active = false;
    

    void Awake()
    {
        if (!this._ready)
        {
            this.OnInitialize();

            // 如果默认不显示这里在初始化后，立即执行非激活。
            this.gameObject.SetActive(this._active);
            // 在不触发OnEnable的情况，需要手动调用执行。保证其都会触发
            if(this._active == false)
            {
                this.OnActive(false);
            }
            this._ready = true;
        }
    }

    /// <summary>
    /// 该函数会执行多次
    /// </summary>
    void OnEnable()
    {
        this.OnActive(true);
    }

    void OnDisable()
    {
        this.OnDisactive(true);
    }

    /// <summary>
    /// OnActive 在OnInitialize之后，在Start之前
    /// </summary>
    public virtual void OnActive(bool active)
    {

    }

    public virtual void OnDisactive(bool cancel)
    {
        
    }

    protected UIPanelIndex GetPanelIndex<T>()
    {
        return GetPanelIndex(typeof(T));
    }

    protected UIPanelIndex GetPanelIndex(System.Type type)
    {
        System.Reflection.PropertyInfo property = type.GetProperty("TIndex", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
        if (property == null)
        {
            return UIPanelIndex.None;
        }

        UIPanelIndex index = (UIPanelIndex)property.GetValue(null);
        return index;
    }

    public virtual void OnInitialize()
    {
        UIPanelIndex index = this.GetPanelIndex(this.GetType());
        if (index == UIPanelIndex.None || index == UIPanelIndex.ViewNone ||
            index == UIPanelIndex.PopupNone || index == UIPanelIndex.DialogNone)
        {
            return ;
        }
        this._index = index;

        UIManager.InitPanel(this);
    }

    /// <summary>
    /// 
    /// </summary>
    public void Active()
    {
        this.ActiveImpl();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected void ActiveImpl()
    {
        //没有调用Awake
        if (this._ready == false)
        {
            this.gameObject.SetActive(true);
        }
    }

    protected void ShowImpl()
    {
        this.ActiveImpl();
        

        // 显示
        this.gameObject.SetActive(true);
    }

    protected void HideImpl()
    {
        if (this.isActiveAndEnabled || this.gameObject.activeInHierarchy)
        {
            //
            this.OnDisactive(false);

            // 显示
            this.gameObject.SetActive(false);
        }
    }

    public void Show(bool shown = true)
    {
        if (shown)
        {
            this.ShowImpl();
        }
        else
        {
            this.HideImpl();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
