using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterType
{
    None = -1,
    Base = 0,
}

[RequireComponent(typeof(CapsuleCollider))]
public class Character : MX.StateBehaviour
{
    [SerializeField]
    private int _index = -1;
    [SerializeField]
    private CharacterType _type = CharacterType.None;

    //
    private MX.BaseController _controller = null;
    public bool has_controller { get { return this._controller != null; } }

    [SerializeField]
    private bool _on_ground = false;
    [SerializeField]
    private LayerMask _ground_layer;

    //
    [SerializeField, Range(1.0f, 20.0f)]
    private float _gravity_velocity = 9.8f;
    [SerializeField, Range(1.0f, 20.0f)]
    private float _jump_velocity = 5.0f;
    private float _jump_time = 0.0f;
    public bool is_jumping { get { return this._jump_time > 0.0f; } }

    //
    [SerializeField, Range(0.0f, 1000.0f)]
    private float _walking_speed = 500.0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public virtual bool InitCharacter(CharacterData data)
    {
        this._index = data.index;
        this._type = data.type;
        return true;
    }

    public bool AddController(MX.BaseController controller)
    {
        this._controller = controller;

        return true;
    }

    public bool RemoveController()
    {
        this._controller = null;

        return true;
    }

    public void UpdateMovement(Vector3 direction)
    {
        direction = direction.normalized;
        Vector3 move_direction = new Vector3(direction.x, 0.0f, direction.z);

        // Rotation
        if (move_direction != Vector3.zero)
        {
            float angle = Vector3.SignedAngle(Vector3.right,
                //new Vector3(this.transform.rotation.x, this.transform.rotation.y, this.transform.rotation.z), 
                direction, Vector3.up);

            this.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.up);
        }

        // Movement
        if (move_direction != Vector3.zero)
        {
            float speed = this._walking_speed;
            speed = speed * 2.0f * 0.001f;

            // 1:
            //Vector3 destination = this.transform.position + Vector3.ClampMagnitude(direction, 1.0f);
            //this.transform.position = Vector3.Lerp(this.transform.position, destination, Time.fixedDeltaTime * this._walking_speed);
            // 2:
            this.transform.Translate(move_direction * speed * Time.fixedDeltaTime, Space.World);
        }

        // Jump
        if(direction.y > 0.0f)
        {
            Vector3 offset = Vector3.up * this._jump_velocity * Mathf.Max(1.0f - this._jump_time, 0.0f) * Time.fixedDeltaTime;
            offset = offset + Vector3.down * this._gravity_velocity * Mathf.Min(this._jump_time, 1.0f) * Time.fixedDeltaTime;
            this.transform.position = this.transform.position + offset;

            this._jump_time += Time.fixedDeltaTime;
        }

        if (!this._on_ground)
        {
            if(this._jump_time == 0.0f)
            {
                Vector3 offset = Vector3.down * this._gravity_velocity * Time.fixedDeltaTime;
                this.transform.position = this.transform.position + offset;
            }
        }
        else
        {
            this._jump_time = 0.0f;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        var go = collision.gameObject;
        string layer = LayerMask.LayerToName(go.layer);
        int mask = LayerMask.GetMask(layer);

        if ((mask & this._ground_layer) > 0 && (go.CompareTag("Ground") || go.CompareTag("Architecture")))
        {
            this._on_ground = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        var go = collision.gameObject;
        string layer = LayerMask.LayerToName(go.layer);
        int mask = LayerMask.GetMask(layer);

        if ((mask & this._ground_layer) > 0 && (go.CompareTag("Ground") || go.CompareTag("Architecture")))
        {
            this._on_ground = false;
        }
    }

}
