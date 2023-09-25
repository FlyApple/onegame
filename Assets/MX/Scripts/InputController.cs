using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MX
{

    public class InputController : Behaviour
    {
        [SerializeField]
        private MCS.UniversalButton _actor;
        public MCS.UniversalButton actor { get { return this._actor; } }

        public bool is_actor_down { get { return this._actor.isFingerDown; } }
        [SerializeField]
        private bool _jumping;
        [SerializeField]
        private float _jumping_time;
        public bool is_jumping { get { return this._jumping; } }
        public Vector3 direction { get { return this._actor.direction; } }
        public Vector3 directionXZ { get { return this._actor.directionXZ; } }

        public override void OnInitialize()
        {
            base.OnInitialize();

            //
            this._jumping = false;
            this._jumping_time = 0.0f;
        }

        // Start is called before the first frame update
        void Start()
        {

        }
        public override void OnUpdate()
        {
            base.OnUpdate();

            if (this._jumping)
            {
                this._jumping_time += Time.fixedDeltaTime;
            }
        }

        // Update is called once per frame
        public override void OnUpdateFrame()
        {
            base.OnUpdateFrame();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                this._jumping = true;
                this._jumping_time = Time.fixedDeltaTime;
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                this._jumping = false;
                this._jumping_time = 0.0f;
            }
        }
    }
}
