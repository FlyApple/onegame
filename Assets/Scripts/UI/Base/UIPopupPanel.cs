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
    [RequireComponent(typeof(Animator))]
    public class UIPopupPanel : UIPanel
    {
        public static new UIElementID TID { get { return UIElementID.PopupNone; } }

        //
        private bool _force = false;
        protected int _result = (int)UIPopupResult.None;
        public int Result { get { return this._result; } }

        /// <summary>
        /// Button close
        /// </summary>
        [SerializeField]
        [InspectorName("Button Close")]
        protected Button button_close;
        //
        protected System.Action<int> on_closed = null;
        protected System.Action<int> on_shown = null;

        /// <summary>
        /// Mask
        /// </summary>
        [SerializeField]
        protected Image image_mask = null;

        /// <summary>
        /// 
        /// </summary>
        protected Animator _animator = null;
        [SerializeField]
        protected AudioClip _audio_show = null;
        [SerializeField]
        protected AudioClip _audio_close = null;


        public override void OnInitialize()
        {
            base.OnInitialize();

            this._animator = this.GetComponent<Animator>();
            if (this._animator != null) {
                this._animator.enabled = false;
                var controller = this._animator.runtimeAnimatorController;
                if(controller == null)
                {
                    this._animator = null;
                }
            }

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

        public void Show(System.Action<int> onclosed = null, System.Action<int> onshown = null)
        {
            if(this._panelstatus == UIPanelStatus.Opened || this._panelstatus == UIPanelStatus.Opening)
            {
                return;
            }

            this.on_closed = onclosed;
            this.on_shown = onshown;

            base.ShowImpl();
        }

        public void Close(int result = (int)UIPopupResult.Closed)
        {

            this.CloseAndResult(true, (UIPopupResult)result);
        }


        private void CloseAndResult(bool force = false, UIPopupResult result = UIPopupResult.None)
        {
            if (this._panelstatus == UIPanelStatus.Closed || this._panelstatus == UIPanelStatus.Closing)
            {
                return;
            }

            this._force = force;
            this._result = (int)result;

            base.CloseImpl();
        }

        protected virtual void OnClickedClose()
        {
            this.CloseAndResult(true, UIPopupResult.Closed);
        }

        protected override void OnShowing()
        {
            base.OnShowing();

            if(on_shown != null)
            {
                on_shown(0);
            }

            if (this._animator != null)
            {
                this._animator.enabled = true;
                this._animator.Play("on_show", 0, 0.0f);
            }

            this._layout.PlayingSound(this._audio_show, false);
        }

        protected override void OnShow()
        {
            base.OnShow();
        }

        protected override void OnClosing()
        {
            base.OnClosing();

            if (this._animator != null)
            {
                this._animator.enabled = true;
                this._animator.Play("on_close", 0, 0.0f);
            }

            this._layout.PlayingSound(this._audio_close, false);
        }

        protected override void OnClose()
        {
            //Callback
            if (this.on_closed != null)
            {
                this.on_closed(this._result);
            }

            if (this._animator != null)
            {
                this._animator.enabled = false;
            }

            //
            base.OnClose();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}