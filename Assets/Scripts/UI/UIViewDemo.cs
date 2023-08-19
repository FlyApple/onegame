using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using MX;

//
public class UIViewDemo : UIView
{
    public static new UIElementID TID { get { return UIElementID.ViewNone + 1; } }

    public UIPopupPanelDemo panelDemo;
    public Button button_demo1;

    //
    public override void OnInitialize()
    {
        base.OnInitialize();

    }

    // Start is called before the first frame update
    void Start()
    {
        //
        this.button_demo1.onClick.AddListener(() =>
        {
            UIManager.GetElement<UIPopupPanelDemo>().Show((result) => {

            });
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
