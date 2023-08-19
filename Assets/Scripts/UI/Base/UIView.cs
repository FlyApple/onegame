using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MX
{
    public class UIView : UIElement
    {
        public static new UIElementID TID { get { return UIElementID.ViewNone; } }

        public override void OnInitialize()
        {
            this._active = true;
            base.OnInitialize();
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
