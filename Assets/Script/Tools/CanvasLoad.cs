using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CanvasLoad : MonoBehaviour
{
    public static CanvasLoad canvas = null;
    public static CanvasLoad instance
    {
        get {
            return canvas;
        }
    }

    public static GameObject canvasui = null;
    public GameObject background;
    public GameObject actor;
    public GameObject pop;
    public GameObject load;
    public GameObject uiPos;
    public static void loadCanvas()
    {
        GameObject obj1 = Resources.Load<GameObject>("Entity/Base");
        var instance = Instantiate(obj1);
        instance.name = "Base";
        canvas = instance.GetComponent<CanvasLoad>();
        //添加相机控制
        var maincam = instance.GetComponentInChildren<Camera>();
        ViewController.Instance.addCameraDictionary("maincam", maincam);

        GameObject obj2 = Resources.Load<GameObject>("Entity/CanvasUI");
        canvasui = Instantiate(obj2);
        canvasui.name = "CanvasUI";
        canvasui.transform.position = CanvasLoad.instance.uiPos.transform.position;
        //添加相机控制
        var uicam = canvasui.GetComponentInChildren<Camera>();
        ViewController.Instance.addCameraDictionary("uicam", uicam);
        DontDestroyOnLoad(instance);
        DontDestroyOnLoad(canvasui.gameObject);
    }
}
