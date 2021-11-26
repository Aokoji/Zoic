using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
//游戏管理器
public class GameManager : MonoBehaviour
{
    private static GameManager gamemamager = null;
    public static GameManager gameManager
    {
        get {
            return gamemamager;
        }
    }
    // Start is called before the first frame update
    void Awake()
    {
        PubTool.Instance.addLogger("游戏启动");
        //+++待修改  先显示完标题  然后显示读档  读档成功再开放点击开始游戏（或是继续游戏）
        AllUnitData.Data.loadData();
        GameData.Data.initGameData();
        loadBaseGameController();
    }
    //创建manager
    public static void initManager()
    {
        GameObject manage = new GameObject("DDOL");
        var ma = manage.AddComponent<GameManager>();
        gamemamager = ma;
        DontDestroyOnLoad(manage);
    }

    //加载游戏控制器
    public void loadBaseGameController()
    {
        EnvironmentManager.Instance.initData(); //加载场景                                                  （ps因为awake已经读档了  否则应该先读档再加载场景）
        ViewController.Instance.initCreateViewController(); //初始化视图
        EventTransfer.Instance.initEvent();                         //初始化事件派发器
        CanvasLoad.instance.initData();                                    //UI
        PlayerControl.Instance.initCreatePlayer();          //初始化玩家
        PlayerManager.Instance.loadPlayerManager();     //加载玩家管理器
        MainController.Instance.initController();
        CombatController.Instance.initController();
        PlotController.Instance.initData();                     //载入剧情组件
        initTools();
        initOverAllEvent();
    }
    private void initTools()
    {
        BagControl.Instance.initData();
    }
    //初始化全局事件
    private void initOverAllEvent()
    {
        PlotController.Instance.initStartGameEvent();
    }
    //------------------------------------------------------------------------------------------------------场景切换主动方法------------
    //--------类似于场景管理器的职责   目前还没有单独的场景管理器
    //点击主页面的开始  切换场景
    public void startGame()     //赋给了开始游戏按钮
    {
        //开始界面切换到  游戏界面
        EnvironmentManager.Instance.checkStartGameSceneAndDo(onstartGame);
        EventTransfer.Instance.gameStartSceneAction();    //派发进入游戏事件
    }
    //开始游戏检查
    public void onstartGame()
    {
        //之前先加载的场景，加载完成后回调检查剧情
        EventTransfer.Instance.gameStartSceneAction();      //派发游戏开始事件  检查剧情是否第一次进入
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //PlayerControl.Instance.setControl(false);
            //MainController.Instance.openCombat();
            List<int> list = new List<int>();
            list.Add(1);
            //MainController.Instance.SendMessage("receiveCombatInformation", list, SendMessageOptions.DontRequireReceiver);
            MainController.Instance.receiveCombatInformation( list);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //CombatController.Instance.playerDoAttack2();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            //测试时间间隔
            /*
            DateTime data = DateTime.Now.ToLocalTime();
            DateTime load = module.lastCatchTime;
            int num = DateTime.Compare(data, load);
            */
        }
    }

    //----------------------------------------------------------------公共事件--------------------------------------------
    /*private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            Time.timeScale = 0f;
            AudioListener.pause = true;
        }
        else
        {
            Time.timeScale = 1f;
            AudioListener.pause = false;
        }
    }*/
    private void OnApplicationPause(bool pause)
    {
        PubTool.Instance.addLogger( "游戏暂停");
    }
    private void OnApplicationQuit()
    {
        PubTool.Instance.addLogger( "游戏退出\n\n\n\n-----------------------------------------------------------------");
    }
    /*
    private void testFunction()
    {
        void aa(Action step)
        {
            Debug.Log("=================aaa");
            PubTool.Instance.laterDo(1, delegate ()
            {
                step();
            });
        }
        PubTool.Instance.addStep(aa);
        PubTool.Instance.addStep(aa);
    }*/


}
