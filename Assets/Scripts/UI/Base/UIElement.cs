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
        //
        ViewUser = 10,

        //Sub views
        PanelNone = 100,

        //Popup
        PopupNone = 1000,

        //Dialog
        DialogNone = 2000,
    }

    public enum UIElementStatus
    {
        //Base
        None = 0,
        Ready = 1,
        Active = 2,
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

        /// <summary>
        /// 
        /// </summary>
        protected UIElementID _ID = UIElementID.None;
        public UIElementID ID { get { return this._ID; } }
        protected UILayout _layout = null;
        public UILayout Layout { get { return this._layout; } }

        [SerializeField]
        protected UIElementStatus _status = UIElementStatus.None;
        public UIElementStatus Status { get { return this._status; } }

        public bool IsReady { get { return this._status == UIElementStatus.Ready; } }

        /// <summary>
        /// 默认不显示：false
        /// </summary>
        protected virtual bool GetActiveOn() { return false; }
        protected bool IsActive { get { return this.GetActiveOn(); } }

        protected virtual bool CheckReadyAll() { return false; }

        void Awake()
        {
            if (this._status == UIElementStatus.None)
            {
                this.OnInitialize();

                this.OnReady();

                // 如果默认不显示这里在初始化后，立即执行非激活。
                if (!this.IsActive)
                {
                    this.gameObject.SetActive(false);
                }

                //
                this._status = UIElementStatus.Ready;
            }
        }

        void Ready()
        {
            //没有调用Awake
            if (this._status == UIElementStatus.None)
            {
                this.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// 该函数会执行多次
        /// </summary>
        void OnEnable()
        {
            if(this._status == UIElementStatus.Ready)
            {
                this.OnActive();

                this._status = UIElementStatus.Active;
            }
        }

        void OnDisable()
        {
            if(this._status ==  UIElementStatus.Active)
            {
                this._status = UIElementStatus.Ready;
            }
        }

        protected virtual void OnReady()
        {
            if (this.CheckReadyAll())
            {
                int count = this.transform.childCount;
                for (int i = 0; i < count; i++)
                {
                    var child = this.transform.GetChild(i).GetComponent<UIElement>();
                    if (child == null)
                    {
                        continue;
                    }
                    this._layout.InvokeElementReadyMethod(child, child.GetType());
                }
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
            this._layout = this.transform.root.GetComponent<UILayout>();
            if(!UIManager.InitElement(this))
            {
                Debug.LogWarning("[UIElement] (Initialize) Init Element (" + Id + ") failed.");
                return;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void OnActive()
        {

        }

        // Start is called before the first frame update
        void Start()
        {

        }


        // Update is called once per frame
        void Update()
        {

        }
    }

}
