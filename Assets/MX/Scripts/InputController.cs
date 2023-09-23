using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MX
{

    public class InputController : MonoBehaviour
    {
        [SerializeField]
        private MCS.UniversalButton _actor;
        public MCS.UniversalButton actor { get { return this._actor; } }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}