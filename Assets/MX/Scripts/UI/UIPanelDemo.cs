using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using MX;

//
public class UIPanelDemo : UIPanel
{
    public static new UIElementID TID { get { return UIElementID.PanelNone + 1; } }

    public UIPopupPanelDemo panelDemo;
    public Button button_demo1;

    //
    public override void OnInitialize()
    {
        base.OnInitialize();

    }

    void Ready()
    {
        //
        this.button_demo1.onClick.AddListener(() =>
        {
            MX.UIManager.GetElement<UIPopupPanelDemo>().Show((result) => {

            });
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
