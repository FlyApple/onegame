using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MX
{
    //
    public interface ILevelData
    {
        int Index { get; } //level 
        string Name { get; }

        int Result { get; } //1:successed, 0:failured
        //
        bool Retry { get; }
        bool Pass { get; }
    }

    //
    public class GameManager : BaseManager
    {
        //
        public delegate void CallbackLevelStart(ILevelData data);
        public delegate void CallbackLevelEnd(ILevelData data);
        public event CallbackLevelStart OnLevelStartListener;
        public event CallbackLevelEnd OnLevelEndListener;

        [SerializeField]
        private float _level_tick = 0.0f;
        private float _level_time = 0.0f;
        public float level_time_end {  get { return this._level_time; } }
        public float level_time { get { return this._level_tick; } }

        protected override void OnReady()
        {
            base.OnReady();

        }

        // Start is called before the first frame update
        void Start()
        {

        }

        public virtual void LevelLoading()
        {

        }

        public virtual void LevelLoaded()
        {

        }

        public virtual void LevelStart()
        {
            this._level_tick = 0.0f;
            this._level_time = 0.0f;

            //
            this.OnLevelStart(null);
        }

        public virtual void LevelEnd(bool success = true)
        {
            this._level_time = this._level_tick;

            //
            this.OnLevelEnd(null);
        }

        protected virtual void OnLevelStart(ILevelData data)
        {
            if (this.OnLevelStartListener != null)
            {
                this.OnLevelStartListener(data);
            }
        }

        protected void OnLevelEnd(ILevelData data)
        {
            if (this.OnLevelEndListener != null)
            {
                this.OnLevelEndListener(data);
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            //
            this._level_tick += Time.fixedDeltaTime;
        }

        // Update is called once per frame
        public override void OnUpdateFrame()
        {
            base.OnUpdateFrame();

        }
    }

}

