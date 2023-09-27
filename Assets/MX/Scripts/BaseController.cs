using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MX
{

    public class BaseController : Behaviour
    {
        [SerializeField]
        protected MX.CameraRoot _camera;

        [SerializeField]
        protected MX.InputController _input_controller = null;

        public override void OnInitialize()
        {
            base.OnInitialize();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

        }

        public override void OnUpdateFrame()
        {
            base.OnUpdateFrame();

        }
    }


}