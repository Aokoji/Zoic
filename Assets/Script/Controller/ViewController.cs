using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//控制主UI  并且兼职 控制视图显示功能
public class ViewController : DDOLController<ViewController>
{
    private MainView mainview = null;
    private CanvasLoad baseview = null;

    //   maincam   uicam    combatcam
    private Dictionary<string, Camera> cameraView = null;

    //特殊加入的方法  canvasLoad写法规范错误纠正
    public void setMainView(MainView view)
    {
        mainview = view;
        mainview.initData();
    }
    public void initCreateViewController()
    {
        cameraView = new Dictionary<string, Camera>();
        loadBaseModView();
        loadMainCameraView();
        initEvent();
    }
    private void initEvent()
    {
        EventTransfer.instance.loadNewSceneEvent += onSceneEnterRefresh;
    }
    /// <summary>
    /// 展示最低限度显示的ui（基础ui）
    /// </summary>
    public void showNormalUIView()
    {
        mainview.showBtnGroup();
    }

    //======================================================================
    //工具  添加相机引用
    public void addCameraDictionary(string name,Camera cam)
    {
        if (!cameraView.ContainsKey(name))
        {
            cameraView.Add(name, cam);
        }
    }          
    public void removeCameraDictionary(string name)
    {
        if (cameraView.ContainsKey(name))  cameraView.Remove(name);

    }
    /// <summary>设置相机显示  
    /// </summary>
    /// <param name="name">相机字典中的名称</param>
    /// <param name="isGeneral">true   为强制单显示，false  为仅操作开该相机</param>
    public void setCameraVisible(string name,bool isGeneral)
    {
        if (cameraView.ContainsKey(name))
        {
            if (isGeneral)
            {
                foreach(KeyValuePair<string,Camera> pair in cameraView)
                {
                    if (pair.Key == name) { pair.Value.gameObject.SetActive(true); }
                    else { pair.Value.gameObject.SetActive(false); }
                }
            }
            else
            {
                cameraView[name].gameObject.SetActive(true);
            }
        }
    }

    public void showMainCam()
    {
        setCameraVisible("maincam", true);
        setCameraVisible("uicam", false);
    }

    //重置控制器
    public void resetViewController(bool isDestory)
    {
        if (isDestory)
        {
            Destroy(mainview.gameObject);
            mainview = null;
        }
        else
        {
           // ObjectUtil.Destroy(mainview.gameObject);
        }
    }

    public void setMainUIActive(bool visible)
    {
        mainview.gameObject.SetActive(visible);
    }

    //进入场景刷新按钮集合
    public void onSceneEnterRefresh()
    {
        showNormalUIView();
        showMainCam();
        //+++刷新部分数据
    }

    public void cameraFollowPlayer() { baseview.cameraFollowPlayer(); }
    public CanvasLoad getBaseMod() { return baseview; }
    public MainView getUIMod() { return mainview; }

    public void addToUIMod(GameObject obj)
    {
        obj.transform.SetParent(mainview.transform, false);
        obj.transform.position = mainview.transform.position;
    }

    public void addToBaseMod_Actor(GameObject obj)
    {
        obj.transform.SetParent(baseview.actor.transform);      //待定 需要设置视图层级
    }

    private void loadBaseModView()
    {
        GameObject obj1 = Resources.Load<GameObject>("Entity/Base");
        var instance = Instantiate(obj1);
        instance.name = "Base";
        baseview = instance.GetComponent<CanvasLoad>();
        baseview.initData();
        //添加相机控制
        var maincam = instance.GetComponentInChildren<Camera>();
        addCameraDictionary("maincam", maincam);
        DontDestroyOnLoad(instance);
    }
    private void loadMainCameraView()
    {
        GameObject obj2 = Resources.Load<GameObject>("Entity/CanvasUI");
        var instance = Instantiate(obj2);
        instance.name = "CanvasUI";
        instance.transform.position = baseview.uiPos.transform.position;
        //纠正方法
        mainview= instance.GetComponent<MainView>();
        mainview.initData();
        //添加相机控制
        var uicam = instance.GetComponentInChildren<Camera>();
        addCameraDictionary("uicam", uicam);
        DontDestroyOnLoad(instance);
    }
}
