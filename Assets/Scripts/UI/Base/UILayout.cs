using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//
public enum UILayoutIndex
{
    //
    None    = -1,
    Base   = 0,
    SafeArea = 1,
    //
    User   = 100,
}

//
[RequireComponent(typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster))]
public class UILayout : MonoBehaviour
{
    //
    protected UILayoutIndex _index = UILayoutIndex.None;
    public UILayoutIndex Index { get { return this._index; } }

    //
    protected Camera _camera;
    protected Canvas _canvas;

    void Awake()
    {
        this._index = UILayoutIndex.Base;

        var camera = GameObject.FindObjectOfType<Camera>();
        this._canvas = this.GetComponent<Canvas>();
        if(this._canvas.worldCamera == null)
        {
            this._canvas.worldCamera = camera;
        }
        this._camera = this._canvas.worldCamera;

        this.OnReady();

        UIManager.InitLayout(this);
    }

    protected virtual void OnReady()
    {

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
