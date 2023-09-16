using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MX
{
    //
    public enum UIPopupDialogResult
    {
        /// <summary>
        /// 
        /// </summary>
        Closed = UIPopupResult.Closed,
        Yes = 1,
        Ok = 1,
        Cancel = 0,
        No = 0,

        /// <summary>
        /// 
        /// </summary>
        User_0 = 100,
        User_1 = 101,
        User_2 = 102,
        User_3 = 103,
        User_4 = 104,
        User_5 = 105,
        User_6 = 106,
        User_7 = 107,
        User_8 = 108,
        User_9 = 109,
    };


    //
    public class UIPopupDialog : UIPopupPanel
    {
        public static new UIElementID TID { get { return UIElementID.DialogNone; } }

        //
        public override void OnInitialize()
        {
            base.OnInitialize();

        }


        // Start is called before the first frame update
        void Start()
        {

        }


        public void ShowDialog(System.Action<int> onclosed = null)
        {
            base.Show(onclosed);
        }

        public void CloseDialog(UIPopupDialogResult result = UIPopupDialogResult.Closed)
        {
            base.Close((int)result);
        }


        // Update is called once per frame
        void Update()
        {

        }
    }

}
