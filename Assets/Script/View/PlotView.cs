using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlotView : MonoBehaviour
{

    public GameObject backimg;
    public GameObject undertip;
    public Text underContext;

    public Button topperClick;

    private PlotListMod plotdata;       //plot数据集
    //------当局数据集  需要刷-------
    private Action nextstep;
    private int curplotShedule;     //当前进度
    private int maxplotShedule;     //总进度


    public void initData()
    {
        plotdata = new PlotListMod();
        initLayout();
        initEvent();
    }
    private void initLayout()
    {
        hideInterface();
    }
    private void initEvent()
    {
        topperClick.onClick.AddListener(nextPlotDialog);
    }

    //刷新变量
    private void refreshVariate()
    {
        nextstep = null;
        curplotShedule = 0;
        maxplotShedule = 0;
    }

    //外接入口    开始剧情
    public void doplot(int plotid,Action callback)
    {
        curplotShedule = 0;
        nextstep = callback;

    }

    private void readConfig()
    {

    }




    //下一段对话
    private void nextPlotDialog()
    {

    }

    private void showInterface()
    {//显示界面  内部用
        gameObject.SetActive(true);
    }
    //隐藏界面
    private void hideInterface()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    ///加载全屏的剧情  || 剧情编号
    /// </summary>
    public void loadToFillScenePlot(int num)
    {

    }
    ///<summary>
    ///加载场景的动作或动画     （需要单独的配置，不能统一配置）
    /// </summary>
    public void loadToActiveScenePlot(int num)  //配置编号(特殊配置)
    {

    }

}
