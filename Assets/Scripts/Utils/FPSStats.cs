using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSStats : MonoBehaviour
{
    private int frame_second = 0;
    private int frame_count = 0;
    private float time_frame = 0.0f;
    private float time_delta = 0.01f;

    [SerializeField]
    private Text _label_fps;
    [SerializeField]
    private Text _label_tms;

    // Start is called before the first frame update
    void Start()
    {
    }

    void FixedUpdate()
    {
        //float fps = 1.0f / this.time_delta;
        //string text = string.Format("FPS {0:F2}", fps);
        string text = string.Format("FPS {0}", this.frame_second);
        if(this._label_fps != null && text != this._label_fps.text)
        {
            this._label_fps.text = text;
        }
        text = string.Format("{0:D} ms", Mathf.RoundToInt(this.time_delta * 1000));
        if(this._label_tms != null && text != this._label_tms.text)
        {
            this._label_tms.text = text;
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.time_delta += (Time.unscaledDeltaTime - this.time_delta) * 0.1f;

        this.frame_count++;
        this.time_frame += this.time_delta; //Time.fixedDeltaTime;
        if(this.time_frame >= 1.0f)
        {
            this.time_frame = 0.0f;
            this.frame_second = this.frame_count;
            this.frame_count = 0;
        }
        
    }
}
