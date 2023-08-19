using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//
public class UIPanelViewDemo : UIPanel
{
    public static new UIPanelIndex TIndex { get { return UIPanelIndex.ViewDemo; } }

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
        this.panelDemo.Active();

        //
        this.button_demo1.onClick.AddListener(() =>
        {
            UIManager.GetPanel<UIPopupPanelDemo>().Show((result) => {

            });
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
