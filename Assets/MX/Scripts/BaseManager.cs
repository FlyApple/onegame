using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MX
{

    public class BaseManager : Behaviour
    {
        public override void OnInitialize()
        {
            base.OnInitialize();
        }

        protected override void OnReady()
        {
            base.OnReady();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        public double Timestamp64()
        {
            long ticks = System.DateTime.Now.Ticks;
            return (double)ticks / System.TimeSpan.TicksPerMillisecond;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        // Update is called once per frame
        public override void OnUpdateFrame()
        {
            base.OnUpdateFrame();

        }
    }

}
