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
        AllUnitData.loadData();
        GameData.initGameData();
        
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
        EventTransfer.Instance.gameStartSceneAction();    //派发进入游戏事件
        EventTransfer.Instance.loadNewSceneAction();      //派发加载新场景完成事件
        PubTool.Instance.addLogger("加载进入基础场景完成，准备载入场景跳转。");
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
    //点击主页面的开始  切换场景
    public void startGame()
    {
        //开始界面切换到  游戏界面
        changeScene("BaseMain");
        StartCoroutine(waitForLoadScene("BaseMain", loadBaseGameController));
    }
    //------------------------------------------------------------------------------------------------------场景切换方法-  end  -------------------------
    //切换场景 公共方法
    public void changeScene(string name)
    {
        SceneManager.LoadScene(name);
    }
    /// <summary>
    ///     场景加载方法      场景名，回调
    /// </summary>
    private IEnumerator waitForLoadScene(string name, Action callback)
    {
        while (SceneManager.GetActiveScene().name != name)
        {
            yield return null;
        }
        PubTool.Instance.addLogger("开始场景加载完成");
        callback();
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
            MainController.Instance.SendMessage("receiveCombatInformation", list, SendMessageOptions.DontRequireReceiver);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CombatController.Instance.playerDoAttack2();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PubTool.Instance.addLogger("ceshi");
            //Debug.Log(Random.Range(0, 2));
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
