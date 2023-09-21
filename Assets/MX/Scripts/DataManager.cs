using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MX
{

    //
    public interface TIData
    {
        //static string name { get; }
    }

    //
    [System.Serializable]
    public class TDataAppStats
    {
        public int launch_count;
        public int quit_count;
        public int pause_count;

        //
        public bool enabled_music = true;
        public bool enabled_sound = true;
        public bool enabled_vibrate = true;
    }


    //
    public class DataManager
    {
        private static DataManager _data_manager = null;
        private static AppManager _app_manager = null;

        private bool _refuse_saving = false;

        private TDataAppStats _app_stats;
        public static TDataAppStats AppStats
        {
            get { return _data_manager._app_stats; }
        }

        private Dictionary<string, System.Type> _types = new Dictionary<string, System.Type>();
        private Dictionary<string, TIData> _data_list = new Dictionary<string, TIData>();

        //
        public DataManager(AppManager manager)
        {
            _data_manager = this;
            _app_manager = manager;
        }

        public static void Load()
        {
            _data_manager.OnLoad();
        }

        public static void Save()
        {
            _data_manager.OnSave();
        }

        public static void ClearAll()
        {
#if UNITY_EDITOR
            if (EditorApplication.isPlaying)
            {
                _data_manager.OnClearAll();
            }
#endif
        }

        public static void HandleApplicationLaunch()
        {
            _data_manager.OnApplicationLaunch();
        }

        public static void HandleApplicationPause(bool paused, long timestamp)
        {
            _data_manager.OnApplicationPause(paused, timestamp);
        }

        public static void HandleApplicationQuit()
        {
            _data_manager.OnApplicationQuit();
        }

        public static void Add<T>()
        {
            _data_manager.Add(typeof(T));
        }

        public static T Get<T>() where T : TIData
        {
            return (T)_data_manager.GetData(typeof(T));
        }

        public void Add(System.Type type)
        {
            try
            {
                // 取静态name;
                System.Reflection.PropertyInfo property = type.GetProperty("name", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
                string name = (string)property.GetValue(null);
                if (!this._types.ContainsKey(name))
                {
                    this._types.Add(name, type);
                }

                System.Reflection.ConstructorInfo constructor = type.GetConstructor(new System.Type[] { });
                if (!this._data_list.ContainsKey(name))
                {
                    this._data_list.Add(name, (TIData)constructor.Invoke(new object[] { }));
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e);
                return;
            }
        }

        public object GetData(System.Type type)
        {
            try
            {
                // 取静态name;
                System.Reflection.PropertyInfo property = type.GetProperty("name", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
                string name = (string)property.GetValue(null);

                //
                TIData data = null;
                if (!this._data_list.TryGetValue(name, out data) || data == null)
                {
                    return null;
                }
                return data;
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e);
                return null;
            }
        }

        public void OnLoad()
        {
            string text = PlayerPrefs.GetString("app", "{}");
            this._app_stats = JsonUtility.FromJson<TDataAppStats>(text);

            // 遍历所有data
            int count = 0;
            foreach (var t in this._types)
            {
                text = PlayerPrefs.GetString(t.Key, "{}");
                var data = JsonUtility.FromJson(text, t.Value);
                if (this._data_list.ContainsKey(t.Key))
                {
                    this._data_list[t.Key] = (TIData)data;
                }
                else
                {
                    this._data_list.Add(t.Key, (TIData)data);
                }
                count++;
            }
        }

        public void OnSave()
        {
            if (this._refuse_saving)
            {
                return;
            }

            string text = JsonUtility.ToJson(this._app_stats);
            PlayerPrefs.SetString("app", text);

            foreach (var v in this._data_list)
            {
                text = JsonUtility.ToJson(v.Value);
                PlayerPrefs.SetString(v.Key, text);
            }
        }

        public void OnClearAll()
        {
            PlayerPrefs.SetString("app", "{}");
            foreach (var v in this._data_list)
            {
                PlayerPrefs.SetString(v.Key, "{}");
            }
            this._refuse_saving = true;
        }



        public void OnApplicationLaunch()
        {
            this._app_stats.launch_count++;
        }

        public void OnApplicationPause(bool paused, long timestamp)
        {
            if (paused)
            {
                this._app_stats.pause_count++;
            }
        }

        public void OnApplicationQuit()
        {
            this._app_stats.quit_count++;

            this.OnSave();
        }
    }


}