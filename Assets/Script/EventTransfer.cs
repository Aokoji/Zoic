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
    private delegate void UpgradeEvent();
    //操作事件
    private delegate void OperationEvent();

    /// <summary>
    /// 升级事件
    /// </summary>
    private event UpgradeEvent levelUp =null;
    public void levelUpAction() {
        //GameData.Data.levelUp();
        levelUp(); // 升级
    } 

    /// <summary>
    /// 获得技能点
    /// </summary>
    private event UpgradeEvent skillGet = null;
    public void getSkillAction() {
        GameData.Data.skillPointGot();
        skillGet();
    }
    



    /// <summary>
    /// 存档
    /// </summary>
    private event OperationEvent saveLoad = null;
    public void doSaveLoad() {
        GameData.Data.saveLoad();
        saveLoad();
    }
    /// <summary>
    /// 存档配置（自动存档）
    /// </summary>
    private event OperationEvent saveGameMessage = null;
    public void doSaveGameMessage()
    {
        GameData.Data.saveGameMessageData();
        saveGameMessage();
    }

    /*
     
     enemy  
     combat头像（player和enemy）
     场景道具 ： 小草丛123  
     花丛12  
     果丛1   
     场景1森林空地
     道具图标几个
     idle界面ui  头像 背包 属性 设置
     idle二级界面
     
     
     
     */



}
