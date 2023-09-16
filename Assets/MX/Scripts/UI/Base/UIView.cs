using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MX
{
    public class UIView : UIElement
    {
        public static new UIElementID TID { get { return UIElementID.ViewNone; } }
        [SerializeField]
        protected bool _active = true;
        protected override bool GetActiveOn() { return this._active; }
        [SerializeField]
        protected bool _checkready = true;
        protected override bool CheckReadyAll() { return this._checkready; }

        public override void OnInitialize()
        {
            base.OnInitialize();
        }


        // Update is called once per frame
        void Update()
        {

        }
    }

}
