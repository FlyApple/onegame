using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//
public class AppManager : MonoBehaviour
{
    private static AppManager _manager = null;
    private static DataManager _data_manager = null;
    public static AppManager Instance
    {
        get
        {
            if (AppManager._manager == null)
            {
                AppManager manager = GameObject.FindObjectOfType<AppManager>();
                AppManager._manager = manager;

                AppManager._data_manager = new DataManager(manager);
                AppManager._manager.OnInitialize();

            }
            return AppManager._manager;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [SerializeField, Range(30, 60)]
    public int FPSMaxLimit = 30;
    [SerializeField]
    public bool DontDestroy = false;

    //
    private float _time_start = 0;
    public float TimeStart
    {
        get { return Time.realtimeSinceStartup - this._time_start; }
    }
    public long TimeStamp64
    {
        get {
            var now = System.DateTime.Now;
            return (long)now.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
        }
    }

    public static void Vibrate() {
        if (DataManager.AppStats.enabled_vibrate)
        {
            //Vibration.vibrator(100);
        }
    }

    //
    public static bool LoadSceneAsync(string name)
    {
        //
        LoadSceneParameters parameters = new LoadSceneParameters();
        parameters.loadSceneMode = LoadSceneMode.Single;
        AsyncOperation operation = SceneManager.LoadSceneAsync(name, parameters);
        Scene scene = SceneManager.GetSceneByName(name);
        if(!scene.IsValid())
        {
            return false;
        }
        if (AppManager.Instance != null)
        {
            AppManager.Instance.LoadSceneAsync(scene, operation);
        }

        //
        return true;
    }

    public void LoadSceneAsync(Scene scene, AsyncOperation operation)
    {
        this.StartCoroutine(this.ProcessLoadScene(scene, operation));
    }

    private IEnumerator ProcessLoadScene(Scene scene, AsyncOperation operation)
    {
        bool loaded = false;
        //operation.allowSceneActivation = false;
        while(!operation.isDone)
        {
            yield return 0;
        }

        if(scene.isLoaded)
        {
        }

        //
        loaded = true;
        yield return 0;
    }

    private void OnInitialize()
    {
        //
        DataManager.Load();

        //
        if (this.DontDestroy)
        {
            DontDestroyOnLoad(gameObject);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        AppManager._manager = this;

        //
        //QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = this.FPSMaxLimit;

        //
        this._time_start = Time.realtimeSinceStartup;

        //
        DataManager.HandleApplicationLaunch();

        //
        Debug.Log("[App] Launch (count:" + DataManager.AppStats.launch_count + ")");
    }

    void OnApplicationQuit()
    {
        DataManager.HandleApplicationQuit();

        Debug.Log("[AppManager] : OnApplicationQuit");
    }

    void OnApplicationPause(bool isPaused)
    {
        // 需要调用
        long start_time = AppManager.Instance.TimeStamp64;
        DataManager.HandleApplicationPause(isPaused, start_time);

        Debug.Log("[AppManager] : OnApplicationPause = " + isPaused);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
