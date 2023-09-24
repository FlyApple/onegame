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
        this._input_controller.gameObject.SetActive(true);
    }

    protected virtual void OnLevelEnd(MX.ILevelData data)
    {
        this._input_controller.gameObject.SetActive(false);
        //this._input_controller = null;
    }

    protected virtual void OnUpdateCharacter()
    {
        Vector3 direction = this._input_controller.actor.directionXZ;
        if (this._character == null)
        {
            return;
        }

        //
        if (this._input_controller.actor.isFingerDown)
        {
            //
            this._character.UpdateMovement(direction);

        }
        else
        {
            //
            this._character.UpdateMovement(Vector3.zero);

        }

        //
        this.UpdatePosition(this._character);
    }

    protected void UpdatePosition(Character character)
    {
        Vector3 destination = new Vector3(character.transform.position.x,
            0.0f,
            character.transform.position.z);
        this.UpdatePositionFinal(destination);
    }

    protected void UpdatePositionFinal(Vector3 destination)
    {

        //
        this._camera.transform.position = new Vector3(destination.x, this._camera.transform.position.y, destination.z);
        this.transform.position = new Vector3(destination.x, this.transform.position.y, destination.z);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //
        this.OnUpdateCharacter();
    }

    // Update is called once per frame
    void Update()
    {

    }
}

