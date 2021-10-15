using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : DDOLController<MainController>
{
    public EventManager eventManager;
    private List<CombatMessage> combatList = new List<CombatMessage>();
    public void initController()
    {
        
    }
    /// <summary>
    /// 进入战斗方法    会先根据怪物编号转换为战斗数据
    /// </summary>
    /// <param name="list">怪物编号 列表</param>
    public void receiveCombatInformation(List<int> list)
    {
        if (list.Count==0)
        {
            Debug.Log("信息传递错误");
            return;
        }
        List<CombatMessage> enemy = new List<CombatMessage>();
        foreach(var i in list)
        {
            enemy.Add(DataTransTool.enemyToCombat(i));
        }
        openCombat(enemy);
    }
    private void openCombat(List<CombatMessage> enemy)
    {
        //遭遇怪物后  会派发给maincontroller事件  传递怪物信息   控制器封装所有信息进入战斗
        List<CombatMessage> list=new List<CombatMessage>();
        //封装player信息
        list.Add(DataTransTool.playerSaveToCombat(GameData.Data.playerBridge.getInstance()));
        int id = 1;
        foreach(var item in enemy)
        {
            item.id = id;
            item.NumID = id;
            list.Add(item);
            id++;
        }
        CombatConfigMessage config = new CombatConfigMessage();
        config.combatLogName = "combat" + GameData.Data.DataPlaymessage.combatIDCount;
        //根据实际情况赋距离初值
        config.initialDistance = 2;
        PubTool.instance.addLogger("遭遇战斗,战斗序号：combat"+GameData.Data.DataPlaymessage.combatIDCount);
        PubTool.instance.addCombatLogger(config.combatLogName, "进入战斗");
        GameData.Data.DataPlaymessage.combatIDCount++;
        GameData.Data.saveGameMessageData();
        CombatController.instance.openCombat(list, config);
    }


    //-------------------------------------------------------------------------------------------------------------------------


}
