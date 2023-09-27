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

    [SerializeField]
    private Character _character;
    [SerializeField]
    private ThridCharacterController _character_controller;

    public override void OnInitialize()
    {
        GameManager._instance = this;
        base.OnInitialize();
    }

    protected override void OnReady()
    {
        base.OnReady();

    }

    public override void LevelStart()
    {
        base.LevelStart();

        //
        this._character_controller.SetCharacter(this._character);
    }

    public override void LevelEnd(bool success = true)
    {
        base.LevelEnd(success);

        this._character_controller.FreeCharacter(this._character);

        UIManager.Instance.LevelEnd(success);
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
