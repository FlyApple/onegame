using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance = null;

    /// <summary>
    /// 
    /// </summary>
    private static Dictionary<int, UILayout> _layouts = new Dictionary<int, UILayout>();
    public static bool InitLayout(UILayout layout)
    {
        if(UIManager._layouts.ContainsKey((int)layout.Index))
        {
            return false;
        }
        UIManager._layouts.Add((int)layout.Index, layout);
        return false;
    }

    private static Dictionary<UIPanelIndex, UIIPanel> _panels = new Dictionary<UIPanelIndex, UIIPanel>();
    public static bool InitPanel(UIIPanel panel)
    {
        if (panel.Index == UIPanelIndex.None ||
            panel.Index == UIPanelIndex.ViewNone ||
            panel.Index == UIPanelIndex.PopupNone ||
            panel.Index == UIPanelIndex.DialogNone)
        {
            return false;
        }

        UIManager._panels.Add(panel.Index, panel);
        return true;
    }

    public static T GetPanel<T>() where T : UIIPanel
    {
        return (T)UIManager.GetPanel(typeof(T));
    }

    private static UIIPanel GetPanel(System.Type type)
    {
        System.Reflection.PropertyInfo property = type.GetProperty("TIndex", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
        if (property == null)
        {
            return null;
        }

        UIPanelIndex index = (UIPanelIndex)property.GetValue(null);
        if (index == UIPanelIndex.None ||
            index == UIPanelIndex.ViewNone ||
            index == UIPanelIndex.PopupNone ||
            index == UIPanelIndex.DialogNone)
        {
            return null;
        }

        UIIPanel panel = null;
        UIManager._panels.TryGetValue(index, out panel);
        return panel;
    }

    /// <summary>
    /// 
    /// </summary>
    [HideInInspector]
    public FPSStats FPS;
    [SerializeField]
    public bool FPSEnabled = false;

    //
    void Awake()
    {
        UIManager.Instance = this;

        this.FPS = this.GetComponentInChildren<FPSStats>(true);
        this.FPS.gameObject.SetActive(this.FPSEnabled);
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
