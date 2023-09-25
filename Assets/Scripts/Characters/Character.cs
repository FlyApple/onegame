using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class Character : MX.StateBehaviour
{ 
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
            float velocity = this._jump_velocity * Mathf.Max(1.0f - this._jump_time, 0.0f);
            velocity = velocity - this._gravity_velocity * Mathf.Min(this._jump_time, 1.0f);

            float v = this.transform.position.y + 1.0f * Time.fixedDeltaTime * velocity;

            this.transform.position = new Vector3(this.transform.position.x, v, this.transform.position.z);

            this._jump_time += Time.fixedDeltaTime;

            if(this.transform.position.y <= 0.0f)
            {
                this._jump_time = 0.0f;
            }
        }
    }

}
