using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MX
{

    public interface IBehaviour
    {
        void Ready();
        void OnInitialize();
        void OnUpdate();
        void OnUpdateFrame();
    }

    public enum BehaviourStatus
    {
        //Base
        None = 0,
        Initialized = 1,
        Ready = 2,
    }

    public class Behaviour : MonoBehaviour, IBehaviour
    {
        [SerializeField]
        private BehaviourStatus _status = BehaviourStatus.None;
        public bool IsReady { get { return this._status >= BehaviourStatus.Ready; } }

        void Awake()
        {
            if(this._status == BehaviourStatus.None)
            {
                this.OnInitialize();

                this._status = BehaviourStatus.Initialized;
            }
        }

        public void Ready()
        {
            //没有调用Awake
            if (this._status == BehaviourStatus.None)
            {
                this.gameObject.SetActive(true);
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        public virtual void OnInitialize()
        {

        }

        protected virtual void OnReady()
        {
        }

        public virtual void OnUpdate()
        {
            if (this._status == BehaviourStatus.Initialized)
            {
                this.OnReady();
                this._status = BehaviourStatus.Ready;
            }
        }

        // Update is called once per frame
        public virtual void OnUpdateFrame()
        {

        }

        void FixedUpdate()
        {
            this.OnUpdate();
        }

        // Update is called once per frame
        void Update()
        {
            this.OnUpdateFrame();
        }
    }

}