using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MX
{

    public enum UIElementID
    {
        //
        None = -1,
        //Main Views
        ViewNone = 0,

        //Sub views
        PanelNone = 100,

        //Popup
        PopupNone = 1000,

        //Dialog
        DialogNone = 2000,
    }

    public enum UIElementStatus
    {
        None = 0,
        Ready = 1,
        Opened = 2,
        Closed = 3,
        Opening = 7,
        Closing = 8,
    }


    /// <summary>
    /// 
    /// </summary>
    public interface UIIElement
    {
        //
        UIElementID ID { get; }
        //protected  virtual UIElementID GetID();
        UIElementStatus Status { get; }

        void OnInitialize();
    }


    /// <summary>
    /// 
    /// </summary>
    public class UIElement : MonoBehaviour, UIIElement
    {
        public static UIElementID TID { get { return UIElementID.None; } }

        [SerializeField]
        protected UIElementID _ID = UIElementID.None;
        public UIElementID ID { get { return this._ID; } }
        [SerializeField]
        protected UIElementStatus _status = UIElementStatus.None;
        public UIElementStatus Status { get { return this._status; } }

        [SerializeField]
        protected bool _ready = false;
        public bool IsReady { get { return this._ready; } }

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

                this.OnReady();

                // 如果默认不显示这里在初始化后，立即执行非激活。
                if (!this._active)
                {
                    this.gameObject.SetActive(false);
                }

                //
                this._ready = true;
                this._status = UIElementStatus.Ready;
            }
        }

        void Ready()
        {
            //没有调用Awake
            if (this._ready == false)
            {
                this.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// 该函数会执行多次
        /// </summary>
        void OnEnable()
        {
            if(this._ready)
            {
                this.ShowImpl();
            }
        }

        void OnDisable()
        {
        }

        protected virtual void OnReady()
        {
            int count = this.transform.childCount;
            for(int i = 0; i < count; i ++)
            {
                var child = this.transform.GetChild(i).GetComponent<UIElement>();
                if(child == null)
                {
                    continue;
                }
                child.Ready();
            }
        }


        protected UIElementID GetID<T>()
        {
            return this.GetID(typeof(T));
        }

        protected virtual UIElementID GetID(System.Type type)
        {
            System.Reflection.PropertyInfo property = type.GetProperty("TID", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            if (property == null)
            {
                return UIElementID.None;
            }

            UIElementID Id = (UIElementID)property.GetValue(null);
            return Id;
        }

        public virtual void OnInitialize()
        {
            UIElementID Id = this.GetID(this.GetType());
            if (Id == UIElementID.None || Id == UIElementID.ViewNone || Id == UIElementID.PanelNone ||
                Id == UIElementID.PopupNone || Id == UIElementID.DialogNone)
            {
                Debug.LogWarning("[UIElement] (Initialize) Invalid ID :" + Id);
                return;
            }
            this._ID = Id;

            if(!UIManager.InitElement(this))
            {
                Debug.LogWarning("[UIElement] (Initialize) Init Element (" + Id + ") failed.");
                return;
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        public void Show()
        {
            this.ShowImpl();
        }

        public void Close()
        {
            this.CloseImpl();
        }

        protected virtual void OnShowing()
        {
        }

        protected virtual void OnClosing()
        {
        }

        protected virtual void OnShow()
        {
            this.gameObject.SetActive(true);
        }

        protected virtual void OnClose()
        {
             this.gameObject.SetActive(false);
        }

        protected void ShowImpl()
        {
            if(this._status == UIElementStatus.Ready || this._status == UIElementStatus.Closed)
            {
                this._status = UIElementStatus.Opening;
                this.OnShowing();
            }
        }

        protected void CloseImpl()
        {
            if (this._status == UIElementStatus.Ready || this._status == UIElementStatus.Opened)
            {
                this._status = UIElementStatus.Closing;
                this.OnClosing();
            }
        }

        protected virtual void UpdateStatus()
        {
            if(this._status == UIElementStatus.Opening)
            {
                this.OnShow();
                this._status = UIElementStatus.Opened;
            } else if(this._status == UIElementStatus.Closing) {
                this._status = UIElementStatus.Closed;
                this.OnClose();
            }
        }

        void FixUpdate()
        {
            this.UpdateStatus();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
