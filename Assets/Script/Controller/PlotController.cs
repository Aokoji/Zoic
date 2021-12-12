using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//剧情控制器
public class PlotController : DDOLController<PlotController>
{
    private PlotView plotview;
    private int plotCount;      //主线进度
    private bool ismainPloting = false;
    private bool issidePloting = false;
    public void initData()
    {
        plotCount = GameData.Data.playerBridge.getplotCount();
        initPlotView();
        initStartGameEvent();
    }
    // 初始化进入游戏事件（判断是否有剧情强制触发）（外部调用）
    public void initStartGameEvent()
    {
        EventTransfer.instance.gameStartSceneEvent += onGameStartCheck;
    }

    private void onGameStartCheck()
    {
        if (GameData.Data.playerBridge.getFirstIn())
        {//第一次进入
            Debug.Log("加载入场动画！！！");
            PubTool.instance.addLogger("载入初始入场动画。");
            PlayerControl.instance.setControl(false);

            Action show = delegate ()
            {
                PlayerControl.instance.setControl(true);
                ViewController.instance.cameraFollowPlayer();
            };
            PubTool.instance.laterDo(2, show);
        }
        else
        {//否则判断其他剧情点

        }
    }

    //触发主线!
    public void mainplotTrigger()
    {

    }

    //支线强制剧情
    public void sideplotTrigger()
    {

    }

    //加载剧情父级视图   (加入到主ui的二级  与 图标父级平级)
    private void initPlotView()
    {
        GameObject loadobj = Resources.Load<GameObject>("Entity/PlotUI");
        var uiTarget = Instantiate(loadobj);
        uiTarget.name = "PlotView";
        ViewController.instance.addToUIMod(uiTarget);
        plotview = uiTarget.GetComponent<PlotView>();
        plotview.initData();
    }
}
