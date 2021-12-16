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
        refreshPlotCount();
        initPlotView();
        initStartGameEvent();
    }
    // 初始化进入游戏事件（判断是否有剧情强制触发）（外部调用）
    public void initStartGameEvent()
    {
        EventTransfer.instance.gameStartSceneEvent += onGameStartCheck;
    }
    private void refreshPlotCount() { plotCount = GameData.Data.playerBridge.getplotCount(); }

    private void onGameStartCheck()
    {
        if (GameData.Data.playerBridge.getFirstIn())
        {//第一次进入
            Debug.Log("加载入场动画！！！");
            PubTool.instance.addLogger("载入初始入场动画。");
            GameData.Data.playerBridge.initPlotCount();     //初始化玩家剧情序号
            mainplotTrigger();
            normalPlot();       //普通式 剧情触发
        }
        else
        {//否则判断其他剧情点

        }
    }

    // 普通类型剧情
    public void normalPlot()
    {
        PubTool.instance.addLogger("剧情编号：" + plotCount + "  开始剧情。");
        normalLock();
        plotview.doplot(GameData.Data.playerBridge.getplotCount(),normalDelegate);
    }
    //普通类型回调
    private void normalDelegate()
    {
        PubTool.instance.addLogger("剧情编号：" + plotCount + "  结束。");
        PlayerControl.instance.setControl(true);
        ViewController.instance.cameraFollowPlayer();
        //GameData.Data.playerBridge.goonPlot();  //剧情记录        测试中  待开放
        if (GameData.Data.playerBridge.getFirstIn())
        {
            //第一次结束   赋值
        }
        refreshPlotCount();
    }
    //普通剧情锁  锁部分控制和显示
    private void normalLock()
    {
        PlayerControl.instance.setControl(false);
        //+++隐藏ui操作界面（去调mainview的打包方法）
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
