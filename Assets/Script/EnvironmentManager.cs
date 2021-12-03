using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//环境管理器
public class EnvironmentManager : DDOLController<EnvironmentManager>
{
    private SceneInterface scenePrefab;     //暂存当前组件的实体
    private int sceneID;
    private bool isfirst= true;
    private Action nextStep;

    public void initData()
    {
        //初始化并创建实例  目前还不知道初始化啥
        sceneID = GameData.Data.playerBridge.getLastScene();
    }
    //进入游戏的场景检查     点开始游戏会调用  只会调用一次
    public void checkStartGameSceneAndDo(Action action)
    {
        if (sceneID == 0)
            initalGameEnvironment();
        isfirst = true;     //标识第一次跳场景  上个场景没有值故跳过
        changeScene(sceneID, action);
    }
    //获得当前场景id
    public int getCurSceneID() { return sceneID; }
    //=====================================     内调      ==========
    //初始化最初场景
    ///没有场景数据  则为第一次进入  需要先加载 等待剧情结束再调用显示
    private void initalGameEnvironment()
    {
        sceneID = 20101;
    }
    //加载场景信息2  根据当前记录位置加载数据(但不显示)  (刷新当前地图信息)(已经加载进来了)
    private void loadEnvironment()
    {
        scenePrefab.initData();
        sceneID = scenePrefab.getSceneID();
    }

    //------------------------------------------场景管理器   ----------------------------------------
    //-----------           目前暂定 一个地图资源对应一个场景
    //切换场景方法
    public void changeScene(int id,Action action)
    {//BaseMain
        if (isfirst)
            isfirst = false;
        else
            exitScene();
        nextStep = action;
        scenePrefab = null;
        sceneID = id;
        //+++ 显示过场动画
        //先去清理场景
        SceneManager.LoadScene("ClearSceneMap");
    }
    //固定点调用  非必要情况不要主动调用
    public void loadNewSceneOnClearScene()
    {
        SceneManager.LoadScene("Map" + sceneID);
        StartCoroutine(waitForLoadScene(sceneID));
    }
    private IEnumerator waitForLoadScene(int sceneid)
    {
        while (SceneManager.GetActiveScene().name != "Map" + sceneid)
        {
            yield return null;
        }
        EventTransfer.Instance.loadNewSceneAction();      //派发加载新场景完成事件
        PubTool.Instance.addLogger(sceneid + "  加载场景完成。");
    }

    public void exitScene()
    {
        //+++保存切图数据 (存到数据层，当执行存储才会全部写文件)
    }

    //加载场景后 场景控制器来调用的赋值方法
    //
    //  因为获取场景控制器比较浪费性能    所以临时想到由加载好的场景控制来主动赋值
    //（太蠢了，但这是目前简单好用的办法）    但还是太蠢了...有机会要换掉
    public void setValueSceneToStart(SceneInterface scene)
    {
        scenePrefab = scene;
        nextStep?.Invoke();
        nextStep = null;
    }
}
