using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MX.UIBaseLayout
{
    private static UIManager _instance = null;
    public static UIManager Instance
    {
        get { return _instance; }
    }

    [SerializeField]
    private MX.InputController _input_controller = null;
    public MX.InputController input_controller { get { return this._input_controller; } }

    //
    protected override void OnReady()
    {
        UIManager._instance = this;
        base.OnReady();

        this._input_controller.gameObject.SetActive(false);
    }

    public virtual void LevelStart()
    {
        GameManager.Instance.LevelStart();

    }

    public virtual void LevelEnd(bool success = true)
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
