using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MX
{

    public class UISafeAreaView : UIView
    {
        [SerializeField]
        public bool enabled_safearea = true;

        public override void OnInitialize()
        {
            base.OnInitialize();

            this.UpdateSafeArea();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="area"></param>
        void UpdateSafeArea()
        {
            if(!this.enabled_safearea)
            {
                return;
            }

            var safeArea = Screen.safeArea;
            var anchorMin = safeArea.position;
            var anchorMax = anchorMin + safeArea.size;

            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;

            var rt = this.transform.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.anchorMin = anchorMin;
                rt.anchorMax = anchorMax;
            }
        }


    }

}
