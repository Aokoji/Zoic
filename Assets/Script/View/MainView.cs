using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainView : MonoBehaviour
{
    //控制集
    public GameObject btnGroup;         //按钮集
    public GameObject curtain;  //转场幕布

    //功能集
    public Button bagBtn;

    public void initData()
    {
        initLayout();
        initEvent();
    }
    private void initLayout()
    {
        //目前layout的任务是  隐藏所有控制集  保证初始界面的正常显示
        btnGroup.SetActive(false);
        curtain.SetActive(false);
    }
    private void initEvent()
    {
        bagBtn.onClick.AddListener(openBag);
    }

    //幕布操作
    public void playSceneChangeCurtainShow(Action callback)
    {
        AnimationController.Instance.playAnimation(curtain, "showCurtain", false, callback);
    }
    public void playSceneChangeCurtainHide(Action callback)
    {
        AnimationController.Instance.playAnimation(curtain, "hideCurtain", false, callback);
    }
    public void setCurtainVisible(bool visit)
    {
        curtain.SetActive(visit);
    }


    //显示界面按钮集
    public void showBtnGroup()
    {
        btnGroup.SetActive(true);
    }
    public void hideBtnGroup()
    {
        btnGroup.SetActive(false);
    }
    public void hideNormalUI()
    {
        hideBtnGroup();
    }

    private void openBag()
    {
        BagControl.Instance.openBag(null);
    }
























    //外部初始化调用方法
    //public static void initCreateMainView()
    //{
    //    mainview = null;
    //    GameObject obj = Resources.Load<GameObject>("Entity/MainView");
    //    var view = Instantiate(obj);
    //    view.name = "MainView";
    //    view.transform.SetParent(GameObject.Find("Canvas").transform, false);
    //    mainview = view.GetComponent<MainView>();
    //    mainview.initData();
    //    mainview.gameObject.SetActive(true);
    //}
    

}
