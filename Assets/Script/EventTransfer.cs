using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
//公共事件监听与派发
public class EventTransfer : DDOLController<EventTransfer>
{
    public void initEvent()
    {
        
    }
    //升级事件  （任何升级）
    public delegate void UpgradeEvent();
    //操作事件
    public delegate void OperationEvent();
    //场景加载事件
    public delegate void SceneLoadEvent();
    //参数事件int
    public delegate void ParamEventInt(int num);

    /// <summary>
    /// 升级事件
    /// </summary>
    public event UpgradeEvent levelUp = new UpgradeEvent(nullfunction);
    public void levelUpAction() {
        //GameData.Data.levelUp();
        levelUp(); // 升级
    }

    /// <summary>
    /// 获得技能点
    /// </summary>
    public event UpgradeEvent skillGet = new UpgradeEvent(nullfunction);
    public void getSkillAction() {
        GameData.Data.skillPointGot();
        skillGet();
    }



    /// <summary>
    /// 游戏开始
    /// </summary>
    public event OperationEvent gameStartSceneEvent = new OperationEvent(nullfunction);
    public void gameStartSceneAction()
    {
        gameStartSceneEvent();
    }
    /// <summary>
    /// 新场景加载完成
    /// </summary>
    public event OperationEvent loadNewSceneEvent = new OperationEvent(nullfunction);
    public void loadNewSceneAction()
    {
        loadNewSceneEvent();
    }
    /// <summary>
    /// 退出场景
    /// </summary>
    public event OperationEvent exitSceneEvent = new OperationEvent(nullfunction);
    public void exitSceneAction() { exitSceneEvent(); }
    /// <summary>
    /// 存档
    /// </summary>
    public event OperationEvent saveLoadEvent = new OperationEvent(nullfunction);
    public void doSaveLoad() {
        saveLoadEvent();
    }
    /// <summary>
    /// 存档配置（自动存档）
    /// </summary>
    public event OperationEvent saveGameMessageEvent = new OperationEvent(nullfunction);
    public void doSaveGameMessage()
    {
        GameData.Data.saveGameMessageData();
        saveGameMessageEvent();
    }


    //场景编号001
    public event SceneLoadEvent sceneNum001Event = new SceneLoadEvent(nullfunction);
    public void loadSceneNum001()
    {
        sceneNum001Event();
    }

    //场景切换事件派发
    //+++尝试不用代理事件  用简单的耦合调用实现公共单一指向事件
    //public event ParamEventInt changeSceneEvent = new ParamEventInt(nullfunction);
    //public void changeSceneAction(int id) { changeSceneEvent(id); }
    public void changeSceneAction(int id,Action action)
    {
        EnvironmentManager.instance.changeScene(id, action);
    }


    private static void nullfunction() { }
    private static void nullfunction(int num) { }
    /*
     
     enemy  
     场景1森林空地
     道具图标几个
     idle界面ui  头像 背包 属性 设置
     idle二级界面
     
     
     
     */



}
