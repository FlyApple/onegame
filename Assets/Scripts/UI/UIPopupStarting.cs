using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupStarting : MX.UIPopupPanel
{
    public static new MX.UIElementID TID { get { return MX.UIElementID.PopupNone + 1; } }

    [SerializeField]
    private Button _button_play;



    public override void OnInitialize()
    {
        base.OnInitialize();

    }

    void Ready()
    {
        //
        this._button_play.onClick.AddListener(() =>
        {
            this.StartPlaying();
        });
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void StartPlaying()
    {
        //close current popup
        MX.UIManager.GetElement<UIPopupStarting>().Close(1);

        //

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
