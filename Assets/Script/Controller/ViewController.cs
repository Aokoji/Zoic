using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//控制主UI  并且兼职 控制视图显示功能
public class ViewController : DDOLController<ViewController>
{
    private MainView mainview = null;
    //特殊加入的方法  canvasLoad写法规范错误纠正
    public void setMainView(MainView view){
        mainview = view;
        mainview.initData();
    }

    //   maincam   uicam    combatcam
    private Dictionary<string, Camera> cameraView = null;

    public void initCreateViewController()
    {
        cameraView = new Dictionary<string, Camera>();
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
