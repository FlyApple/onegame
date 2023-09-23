using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace MX
{
    public class CameraRoot : Behaviour
    {
        [SerializeField]
        private Transform _cameraH;
        [SerializeField]
        private Transform _cameraV;
        [SerializeField]
        private Camera _camera;

        //
        protected override void OnReady()
        {
            base.OnReady();

            this._cameraH = this.transform.Find("H");
            this._cameraV = this.transform.Find("H/V");
            this._camera = this.transform.Find("H/V").GetComponentInChildren<Camera>();
        }

        // Start is called before the first frame update
        void Start()
        {

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

}