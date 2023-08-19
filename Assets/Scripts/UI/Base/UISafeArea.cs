using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISafeArea : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        var rt = GetComponent<RectTransform>();
        var safeArea = Screen.safeArea;
        var anchorMin = safeArea.position;
        var anchorMax = anchorMin + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        rt.anchorMin = anchorMin;
        rt.anchorMax = anchorMax;
    }
}
