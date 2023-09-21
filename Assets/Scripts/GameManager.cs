using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MX.GameManager
{
    private static GameManager _instance = null;
    public static GameManager Instance
    {
        get { return _instance; }
    }

    //
    [SerializeField]
    private InputController _input_controller = null;

    protected override void OnReady()
    {
        GameManager._instance = this;
        base.OnReady();

    }

    public override void LevelStart()
    {
        base.LevelStart();

        this._input_controller = UIManager.Instance.input_controller;
    }

    public override void LevelEnd(bool success = true)
    {
        base.LevelEnd(success);

        UIManager.Instance.LevelEnd(success);
        this._input_controller = null;
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
