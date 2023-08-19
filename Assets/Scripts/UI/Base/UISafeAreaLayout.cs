using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MX
{

    /// <summary>
    /// 
    /// </summary>
    public class UISafeAreaLayout : UILayout
    {
        protected override UILayoutID GetID() { return UILayoutID.SafeArea; }

        [SerializeField]
        public bool enabled_safearea = true;
        [SerializeField]
        protected List<UISafeArea> _safearea = new List<UISafeArea>();

        protected override void OnReady()
        {
            base.OnReady();

            this._safearea.Clear();
            for (int i = 0; i < this.transform.childCount; i++)
            {
                var child = this.transform.GetChild(i).GetComponent<RectTransform>();
                var area = child.GetComponent<UISafeArea>();
                if (area == null)
                {
                    area = child.transform.gameObject.AddComponent<UISafeArea>();
                }

                if (area.enabled_safearea)
                {
                    this.UpdateSafeArea(area);
                }
                this._safearea.Add(area);
            }
        }

        void UpdateSafeArea(UISafeArea area)
        {
            var safeArea = Screen.safeArea;
            var anchorMin = safeArea.position;
            var anchorMax = anchorMin + safeArea.size;

            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;

            var rt = area.transform.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.anchorMin = anchorMin;
                rt.anchorMax = anchorMax;
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