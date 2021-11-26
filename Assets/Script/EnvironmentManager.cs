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
    private bool isfirst;

    public void initData()
    {
        //初始化并创建实例  目前还不知道初始化啥
        sceneID = GameData.Data.playerBridge.getLastScene();
    }
    //进入游戏的场景检查
    public void checkStartGameSceneAndDo(Action action)
    {
        if (sceneID == 0)
            initalGameEnvironment();
        changeScene(sceneID, action);
    }
    //=====================================     内调      ==========
    //初始化最初场景
    ///没有场景数据  则为第一次进入  需要先加载 等待剧情结束再调用显示
    private void initalGameEnvironment()
    {
        isfirst = true;
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
        SceneManager.LoadScene("Map" + id);
        sceneID = id;
        //+++ 显示过场动画
        StartCoroutine(waitForLoadScene(id, action));
    }

    public void exitScene()
    {
        //+++保存切图数据 (存到数据层，当执行存储才会全部写文件)
    }

    /// <summary>
    ///     场景加载方法      场景名，回调
    /// </summary>
    private IEnumerator waitForLoadScene(int sceneid, Action callback)
    {
        while (SceneManager.GetActiveScene().name != "Map" + sceneid)
        {
            yield return null;
        }
        loadEnvironment();
        EventTransfer.Instance.loadNewSceneAction();      //派发加载新场景完成事件
        PubTool.Instance.addLogger(sceneid + "  加载场景完成，准备载入场景跳转。");
        callback();
    }
}
