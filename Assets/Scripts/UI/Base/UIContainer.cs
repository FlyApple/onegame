using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MX
{
    [RequireComponent(typeof(Image))]
    public class UIContainer : UIElement
    {
        public static new UIElementID TID { get { return UIElementID.Container; } }

        protected override bool GetActiveOn() { return this.gameObject.activeSelf; }

        [SerializeField]
        protected UIPanel _parent;

        [SerializeField]
        protected bool _mask_on = false;
        [SerializeField, Range(0.0f, 1.0f)]
        protected float _mask_alpha = 0.4f;

        [HideInInspector]
        public Image image_background;

        public override void OnInitialize()
        {
            base.OnInitialize();

            this.image_background = this.GetComponent<Image>();

            this._parent = this.GetComponentInParent<UIPanel>();
            if(this._parent.ID > UIElementID.PopupNone)
            {
                this._mask_on = true;
            }
            if(!this._mask_on)
            {
                this.image_background.enabled = false;
            }
            else
            {
                this.image_background.enabled = true;
                this.image_background.color = new Color(0.0f, 0.0f, 0.1f, this._mask_alpha);
            }
        }

        protected override void OnReady()
        {
            base.OnReady();
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