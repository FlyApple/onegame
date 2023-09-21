using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGame : MX.UIPanel
{
    public static new MX.UIElementID TID { get { return MX.UIElementID.PanelNone + 1; } }

    //
    [SerializeField]
    private TMPro.TextMeshProUGUI _time_meter;

    //
    public override void OnInitialize()
    {
        base.OnInitialize();

    }


    // Start is called before the first frame update
    void Start()
    {
        this._time_meter.text = string.Format("{0:D2}:{1:D2}", 0, 0);

        //
        GameManager.Instance.OnLevelStartListener += OnLevelStart;
    }

    protected virtual void OnLevelStart(MX.ILevelData data)
    {

    }


    void UpdateGameTime()
    {
        float time = GameManager.Instance.level_time;
        int second = Mathf.RoundToInt(time) % 60;
        int minute = Mathf.RoundToInt(time) / 60;
        string text = string.Format("{0:D2}:{1:D2}", minute, second);
        if (this._time_meter.text != text)
        {
            this._time_meter.text = text;
        }
    }

    void UpdateGame()
    {
        this.UpdateGameTime();
    }

    void FixedUpdate()
    {
        this.UpdateGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
