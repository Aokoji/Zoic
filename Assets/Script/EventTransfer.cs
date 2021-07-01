using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
//公共事件监听与派发
public class EventTransfer : DDOLController<EventTransfer>
{
    //升级事件  （任何升级）
    private delegate void UpgradeEvent();

    /// <summary>
    /// 升级事件
    /// </summary>
    private event UpgradeEvent levelUp =null;
    public void levelUpAction() { levelUp(); }  // 升级

    /// <summary>
    /// 获得技能点
    /// </summary>
    private event UpgradeEvent skillGet = null;
    public void getSkillAction() { skillGet(); } 



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
