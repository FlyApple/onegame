using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ThridCharacterController : MX.Behaviour
{
    [SerializeField]
    private MX.CameraRoot _camera;
    [SerializeField]
    private Character _character;
    //
    private MX.InputController _input_controller = null;

    public override void OnInitialize()
    {
        base.OnInitialize();
    }

    protected override void OnReady()
    {
        base.OnReady();

        GameManager.Instance.OnLevelStartListener += OnLevelStart;
        GameManager.Instance.OnLevelEndListener += OnLevelEnd;

        this._input_controller = UIManager.Instance.input_controller;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public bool InitCharacter(Character character)
    {
        this._character = character;
        return true;
    }

    protected virtual void OnLevelStart(MX.ILevelData data)
    {
        Debug.Log("Test start");
        this._input_controller.gameObject.SetActive(true);
    }

    protected virtual void OnLevelEnd(MX.ILevelData data)
    {
        this._input_controller.gameObject.SetActive(false);
        //this._input_controller = null;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    // Update is called once per frame
    void Update()
    {

    }
}

