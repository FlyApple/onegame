using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
public class AppInstances : MonoBehaviour
{
    private static AppInstances _instance = null;
    public static AppInstances Instance
    {
        get {
            if (AppInstances._instance == null)
            {
                var inst = GameObject.FindObjectOfType<AppInstances>(true);
                AppInstances._instance = inst;
            }
            return AppInstances._instance;
        }
    }

    //
    [SerializeField]
    public UIManager UIInstance;
    [SerializeField]
    public GameManager GameInstance;


    //
    void Awake()
    {
        AppInstances._instance = this;
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
