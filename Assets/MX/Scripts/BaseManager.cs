using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MX
{

    public class BaseManager : MonoBehaviour
    {
        //
        void Awake()
        {
            this.OnReady();
        }



        protected virtual void OnReady()
        {
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


        // Update is called once per frame
        void Update()
        {

        }
    }

}
