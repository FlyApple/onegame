using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MX.StateBehaviour
{
    [SerializeField, Range(0.0f, 1000.0f)]
    private float _walking_speed = 500.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void UpdateMovement(Vector3 direction)
    {
        if(direction == Vector3.zero)
        {
            return;
        }

        // Rotation
        direction = direction.normalized;
        float angle = Vector3.SignedAngle(Vector3.right,
            //new Vector3(this.transform.rotation.x, this.transform.rotation.y, this.transform.rotation.z), 
            direction, Vector3.up);

        this.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.up);

        //
        float speed = this._walking_speed;
        speed = speed * 2.0f * 0.001f;

        // 1:
        //Vector3 destination = this.transform.position + Vector3.ClampMagnitude(direction, 1.0f);
        //this.transform.position = Vector3.Lerp(this.transform.position, destination, Time.fixedDeltaTime * this._walking_speed);
        // 2:
        this.transform.Translate(direction * speed * Time.fixedDeltaTime, Space.World);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
