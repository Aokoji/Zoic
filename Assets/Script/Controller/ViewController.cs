using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewController : DDOLController<ViewController>
{
    private MainView mainview = null;

    //   maincam   uicam    combatcam
    private Dictionary<string, Camera> cameraView = null;

    public void initCreateViewController()
    {
        cameraView = new Dictionary<string, Camera>();
        //mainview=initCreateMainView();
    }
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

    //初始化视图1            ui 界面
    public MainView initCreateMainView()
    {
        GameObject baseain = Resources.Load<GameObject>("Entity/MainView");
        var baseMain = Instantiate(baseain);
        baseMain.name = "MainView";
        baseMain.transform.SetParent(CanvasLoad.canvasui.transform, false);
        var mainview = baseMain.GetComponent<MainView>();
        mainview.gameObject.SetActive(false);       //todo  待修改
        mainview.initData();
        return mainview;
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


}
