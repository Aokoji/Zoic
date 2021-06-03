using System.Collections;
//using System;
using System.Collections.Generic;
using UnityEngine;
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
        ViewController.Instance.initCreateViewController(); //初始化视图
        CanvasLoad.loadCanvas();
        PlayerControl.Instance.initCreatePlayer();          //初始化玩家
        PlayerManager.Instance.loadPlayerManager();     //加载玩家管理器
        MainController.Instance.initController();
        CombatController.Instance.initController();
        initTools();
    }
    //创建manager
    public static void initManager()
    {
        GameObject manage = new GameObject("DDOL");
        var ma = manage.AddComponent<GameManager>();
        gamemamager = ma;
        DontDestroyOnLoad(manage);
    }

    private void initTools()
    {

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
    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            Time.timeScale = 0f;
            AudioListener.pause = true;
        }
        else
        {
            Time.timeScale = 1f;
            AudioListener.pause = false;
        }
    }
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
