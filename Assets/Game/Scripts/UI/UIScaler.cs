using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIScaler : MonoBehaviour
{
    [SerializeField] private float resoX;
    [SerializeField] private float resoY;

    private CanvasScaler canvas;

    void Start()
    {
        canvas = GetComponent<CanvasScaler>();
        UpdateResolution();
    }

    void UpdateResolution()
    {
        resoX = (float)Screen.currentResolution.width;
        resoY = (float)Screen.currentResolution.height;
        canvas.referenceResolution = new Vector2(resoX, resoY);
    }
}
