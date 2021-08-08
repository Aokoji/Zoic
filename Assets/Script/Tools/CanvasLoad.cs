using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//这个改为controller 管理base 预制体
public class CanvasLoad : MonoBehaviour
{
    public static CanvasLoad canvas = null;     //base预制体
    public static CanvasLoad instance
    {
        get {
            if (canvas == null)
            {
                loadCanvas();
            }
            return canvas;
        }
    }

    private CameraView camView;     //相机控制的脚本

    public static GameObject canvasui = null;
    public GameObject background;
    public GameObject actor;
    public GameObject pop;
    public GameObject load;
    public GameObject uiPos;
    public GameObject mainCamera;

    //这个基本就是init方法的意思了
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
        //纠正方法
        ViewController.Instance.setMainView(canvasui.GetComponent<MainView>());
        //添加相机控制
        var uicam = canvasui.GetComponentInChildren<Camera>();
        ViewController.Instance.addCameraDictionary("uicam", uicam);
        DontDestroyOnLoad(instance);
        DontDestroyOnLoad(canvasui.gameObject);
    }
    public void initData()
    {
        camView = mainCamera.GetComponent<CameraView>();
    }

    //相机跟随玩家
    public void cameraFollowPlayer()
    {
        camView.cameraFollowPlayer();
    }
    //相机离开玩家
    public void cameraLeavePlayer()
    {
        camView.cameraLeavePlayer();
    }
}
