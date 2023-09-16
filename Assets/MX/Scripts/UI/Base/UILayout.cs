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

        //
        User = 100,
    }

    //
    [RequireComponent(typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster))]
    [RequireComponent(typeof(AudioSource))]
    public class UILayout : MonoBehaviour
    {
        //
        protected int _index = -1;
        public int Index { get { return this._index; } }
        public virtual void SetIndex(int index) { this._index = index; }
        protected UILayoutID _ID = UILayoutID.None;
        public UILayoutID ID { get { return this._ID; } }
        protected virtual UILayoutID GetID()  { return UILayoutID.None; }

        //
        protected Camera _camera;
        protected Canvas _canvas;
        [SerializeField]
        protected int _canvas_layerorder = 1;

        //
        [SerializeField]
        protected List<UIView> _views = new List<UIView>();

        //
        private AudioSource _audio_sound_primary;
        private AudioSource _audio_sound_secondary;

        //
        void Awake()
        {
            var camera = GameObject.FindObjectOfType<Camera>();
            this._canvas = this.GetComponent<Canvas>();
            this._canvas.sortingOrder = this._canvas_layerorder;
            if (this._canvas.worldCamera == null)
            {
                this._canvas.worldCamera = camera;
            }
            this._camera = this._canvas.worldCamera;

            //
            var audios = this.GetComponents<AudioSource>();
            if(audios.Length > 0)
            {
                this._audio_sound_primary = audios[0];
            }
            if (audios.Length > 1)
            {
                this._audio_sound_primary = audios[1];
            }
            if (this._audio_sound_primary == null) { this._audio_sound_primary = this.gameObject.AddComponent<AudioSource>(); }
            if (this._audio_sound_secondary == null) { this._audio_sound_secondary = this.gameObject.AddComponent<AudioSource>(); }
            this._audio_sound_primary.enabled = false;
            this._audio_sound_secondary.enabled = false;

            //
            UIManager.InitLayout(this);

            this.OnReady();
        }

        protected virtual void OnReady()
        {
            int count = this.transform.childCount;
            for (int i = 0; i < count; i++)
            {
                var child = this.transform.GetChild(i).GetComponent<UIView>();
                if (child == null)
                {
                    continue;
                }

                if(!this.InvokeReadyMethod(child, child.GetType()))
                {
                    continue;
                }

                this._views.Add(child);
            }
        }

        public bool InvokeElementReadyMethod(object reference, System.Type type)
        {
            return this.InvokeReadyMethod(reference, type);
        }

        private bool InvokeReadyMethod(object reference, System.Type type)
        {
            System.Reflection.MethodInfo method = type.GetMethod("Ready",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.DeclaredOnly |
                //System.Reflection.BindingFlags.FlattenHierarchy |
                System.Reflection.BindingFlags.InvokeMethod |
                System.Reflection.BindingFlags.Instance);
            if(method == null)
            {
                if (type == typeof(MonoBehaviour))
                {
                    return true;
                }
            }

            if (method != null)
            {
                method.Invoke(reference, new object[] { });
            }
            return this.InvokeReadyMethod(reference, type.BaseType);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        //
        public void PlayingSound(AudioClip clip, bool primary = true)
        {
            if (clip == null)
            {
                return;
            }

            if (!primary)
            {
                this._audio_sound_secondary.clip = clip;
                this._audio_sound_secondary.enabled = true;
                this._audio_sound_secondary.Play();
            }
            else
            {
                this._audio_sound_primary.clip = clip;
                this._audio_sound_primary.enabled = true;
                this._audio_sound_primary.Play();
            }
        }

        private void CheckingPlayingSound()
        {
            if (this._audio_sound_primary.enabled && !this._audio_sound_primary.isPlaying)
            {
                this._audio_sound_primary.enabled = false;
            }
            if (this._audio_sound_secondary.enabled && !this._audio_sound_secondary.isPlaying)
            {
                this._audio_sound_secondary.enabled = false;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        void FixedUpdate()
        {
            this.CheckingPlayingSound();    
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}