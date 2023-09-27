using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ThridCharacterController : MX.BaseController
{
    [SerializeField]
    private Character _character;
    //
    private Vector3 _direction_move;

    //
    [SerializeField]
    private bool _jumping;
    private float _jumping_time;

    //
    public override void OnInitialize()
    {
        base.OnInitialize();
    }

    protected override void OnReady()
    {
        base.OnReady();

        GameManager.Instance.OnLevelStartListener += OnLevelStart;
        GameManager.Instance.OnLevelEndListener += OnLevelEnd;

        //
        this._input_controller = UIManager.Instance.input_controller;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public bool SetCharacter(Character character)
    {
        this._character = character;
        this._direction_move = Vector3.zero;

        this._jumping = false;
        this._jumping_time = 0.0f;

        return true;
    }

    public void FreeCharacter(Character character)
    {
        this._character = null;

        //
        this._direction_move = Vector3.zero;
    }

    protected virtual void OnLevelStart(MX.ILevelData data)
    {
        this._input_controller.gameObject.SetActive(true);
    }

    protected virtual void OnLevelEnd(MX.ILevelData data)
    {
        this._input_controller.gameObject.SetActive(false);
    }

    protected virtual void OnUpdateCharacter(Character character)
    {
        if (character == null)
        {
            return;
        }

        //
        Vector3 direction = Vector3.zero; 
        if (this._input_controller.is_actor_down)
        {
            direction = this._input_controller.directionXZ;
        }

        if(this._input_controller.is_jumping)
        {
            if (this._jumping == false)
            {
                this._jumping = true;
                this._jumping_time = 0.0f;
            }
            
        }

        if(this._jumping)
        {
            this._jumping_time += Time.fixedDeltaTime;

            direction += Vector3.up;
        }

        this._direction_move = direction;
    }

    protected virtual void OnUpdateCharacterMove(Character character)
    {
        if (character == null)
        {
            return;
        }

        character.UpdateMovement(this._direction_move);
        if (!character.is_jumping)
        {
            this._jumping = false;
        }
        this.UpdatePosition(character);
    }

    protected void UpdatePosition(Character character)
    {
        if(character == null)
        {
            return;
        }

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
        this.OnUpdateCharacter(this._character);
    }

    // Update is called once per frame
    public override void OnUpdateFrame()
    {
        base.OnUpdateFrame();

        //
        this.OnUpdateCharacterMove(this._character);
    }
}

