using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MX
{
    public enum UIPanelStatus
    {
        //Base
        None = -1,
        //Panel
        Opened = 1,
        Closed = 0,
        Opening = 11,
        Closing = 10,
    }

    
    public class UIPanel : UIElement
    {
        public static new UIElementID TID { get { return UIElementID.PanelNone; } }
        [SerializeField]
        protected bool _active = false;
        protected override bool GetActiveOn() { return this._active; }
        [SerializeField]
        protected bool _checkready = false;
        protected override bool CheckReadyAll() { return this._checkready; }

        [SerializeField]
        protected UIPanelStatus _panelstatus = UIPanelStatus.None;
        public UIPanelStatus PanelStatus { get { return this._panelstatus; } }

        private float _tick = 0.0f;
        [SerializeField, Range(0.0f, 3.0f)]
        protected float _showing_delay = 0.1f;
        [SerializeField, Range(0.0f, 3.0f)]
        protected float _closing_delay = 0.1f;

        //
        public override void OnInitialize()
        {
            base.OnInitialize();

            var rt = this.GetComponent<RectTransform>();
            rt.anchorMax = new Vector2(1.0f, 1.0f);
            rt.anchorMin = new Vector2(0.0f, 0.0f);
            rt.pivot = new Vector2(0.5f, 0.5f);
        }

        protected override void OnReady()
        {
            base.OnReady();

            if(!this.IsActive)
            {
                this._panelstatus = UIPanelStatus.Closed;
            }
            else
            {
                this._panelstatus = UIPanelStatus.Opened;
            }
        }

        public void Show()
        {
            this.ShowImpl();
        }

        public void Close()
        {
            this.CloseImpl();
        }

        protected void ShowImpl()
        {
            if (this._panelstatus == UIPanelStatus.Closed)
            {
                this._panelstatus = UIPanelStatus.Opening;

                this.OnShowing();
            }
        }

        protected void CloseImpl()
        {
            if (this._panelstatus == UIPanelStatus.Opened)
            {
                this._panelstatus = UIPanelStatus.Closing;

                this.OnClosing();
            }
        }


        protected virtual void OnShowing()
        {
            this._tick = 0.0f;
            this.gameObject.SetActive(true);
        }

        protected virtual void OnClosing()
        {
            this._tick = 0.0f;
        }

        protected virtual void OnShow()
        {
            
        }

        protected virtual void OnClose()
        {
            this.gameObject.SetActive(false);
        }

        protected virtual void UpdateStatus()
        {
            if (this._panelstatus == UIPanelStatus.Opening)
            {
                this._tick += Time.fixedDeltaTime;
                if (this._tick >= this._showing_delay)
                {
                    this.OnShow();
                    this._panelstatus = UIPanelStatus.Opened;
                }
            }
            else if (this._panelstatus == UIPanelStatus.Closing)
            {
                this._tick += Time.fixedDeltaTime;

                if (this._tick >= this._closing_delay)
                {
                    this._panelstatus = UIPanelStatus.Closed;
                    this.OnClose();
                }
            }
        }


        void FixedUpdate()
        {
            this.UpdateStatus();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
