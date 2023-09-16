using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MX
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance = null;

        /// <summary>
        /// 
        /// </summary>
        private static int _index = 0;
        /// <summary>
        /// 
        /// </summary>
        private static Dictionary<int, List<UILayout>> _layouts = new Dictionary<int, List<UILayout>>();
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="layout"></param>
        /// <returns></returns>
        public static bool InitLayout(UILayout layout)
        {
            if (layout.ID < 0 || layout.Index > 0)
            {
                return false;
            }

            layout.SetIndex(++UIManager._index);
            var layouts = UIManager._layouts[(int)layout.ID];
            if(layouts == null)
            {
                layouts = new List<UILayout>();
            }
            layouts.Add(layout);
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static UILayout GetLayout(UILayoutID Id)
        {
            var layouts  = UIManager._layouts[(int)Id];
            if(layouts == null || layouts.Count == 0)
            {
                return null;
            }
            return layouts[0];
        }

        private static Dictionary<UIElementID, UIIElement> _elements = new Dictionary<UIElementID, UIIElement>();
        public static bool InitElement(UIElement element)
        {
            if (element.ID == UIElementID.None ||
                element.ID == UIElementID.ViewNone ||
                element.ID == UIElementID.PanelNone ||
                element.ID == UIElementID.PopupNone ||
                element.ID == UIElementID.DialogNone)
            {
                return false;
            }

            if(UIManager._elements.ContainsKey(element.ID))
            {
                return false;
            }

            UIManager._elements.Add(element.ID, element);
            return true;
        }

        public static T GetElement<T>() where T : UIIElement
        {
            return (T)UIManager.GetElement(typeof(T));
        }

        private static UIIElement GetElement(System.Type type)
        {
            System.Reflection.PropertyInfo property = type.GetProperty("TID", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            if (property == null)
            {
                return null;
            }

            UIElementID Id = (UIElementID)property.GetValue(null);
            if (Id == UIElementID.None ||
                Id == UIElementID.ViewNone ||
                Id == UIElementID.PanelNone ||
                Id == UIElementID.PopupNone ||
                Id == UIElementID.DialogNone)
            {
                return null;
            }

            UIIElement element = null;
            UIManager._elements.TryGetValue(Id, out element);
            return element;
        }

        /// <summary>
        /// 
        /// </summary>
        [SerializeField]
        public FPSStats FPSStats;
        [SerializeField]
        public bool FPSEnabled = false;

        //
        void Awake()
        {
            UIManager.Instance = this;

            if (this.FPSStats != null)
            {
                this.FPSStats.gameObject.SetActive(this.FPSEnabled);
            }
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
