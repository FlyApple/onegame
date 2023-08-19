using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace MX
{
    //
    public enum UILayoutID
    {
        //
        None = -1,
        Base = 0,
        SafeArea = 1,
        //
        User = 100,
    }

    //
    [RequireComponent(typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster))]
    public class UILayout : MonoBehaviour
    {
        //
        protected int _index = -1;
        public int Index { get { return this._index; } }
        public virtual void SetIndex(int index) { this._index = index; }
        protected UILayoutID _ID = UILayoutID.None;
        public UILayoutID ID { get { return this._ID; } }
        protected virtual UILayoutID GetID()  { return UILayoutID.Base; }

        //
        protected Camera _camera;
        protected Canvas _canvas;

        void Awake()
        {
            var camera = GameObject.FindObjectOfType<Camera>();
            this._canvas = this.GetComponent<Canvas>();
            if (this._canvas.worldCamera == null)
            {
                this._canvas.worldCamera = camera;
            }
            this._camera = this._canvas.worldCamera;

            //
            UIManager.InitLayout(this);

            this.OnReady();
        }

        protected virtual void OnReady()
        {

        }

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