using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace MX
{
    /// <summary>
    /// 
    /// </summary>
    public enum UIPopupResult
    {
        /// <summary>
        /// 
        /// </summary>
        None = -1,
        Closed = 0,
    };

    //
    public class UIPopupPanel : UIPanel
    {
        public static new UIElementID TID { get { return UIElementID.PopupNone; } }

        //
        protected int _result = (int)UIPopupResult.None;
        public int Result { get { return this._result; } }

        /// <summary>
        /// Button close
        /// </summary>
        [SerializeField]
        [InspectorName("Button Close")]
        protected Button button_close;
        protected System.Action<int> on_closed = null;

        /// <summary>
        /// Mask
        /// </summary>
        [SerializeField]
        protected Image image_mask = null;


        public override void OnInitialize()
        {
            base.OnInitialize();

            //
            if (this.button_close != null)
            {
                this.button_close.onClick.AddListener(() => { this.OnClickedClose(); });
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        public void Show(System.Action<int> onclosed = null)
        {
            this.on_closed = onclosed;

            base.ShowImpl();
        }

        public void Close(int result = (int)UIPopupResult.Closed)
        {
            this._result = result;
            base.CloseImpl();
        }


        protected virtual void OnClickedClose()
        {
            this._result = (int)UIPopupResult.Closed;
            base.CloseImpl();
        }

        protected override void OnClosing()
        {
            base.OnClosing();

            //Callback
            if (this.on_closed != null)
            {
                this.on_closed(this._result);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}