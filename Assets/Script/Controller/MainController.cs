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

    public void receiveCombatInformation(List<CombatMessage> list)
    {
        if (list.Count==0)
        {
            Debug.Log("信息传递错误");
            return;
        }
        openCombat(list);
    }
    public void openCombat(List<CombatMessage> enemy)
    {
        //遭遇怪物后  会派发给maincontroller事件  传递怪物信息   控制器封装所有信息进入战斗
        List<CombatMessage> list=new List<CombatMessage>();
        //封装player信息
        list.Add(playerSaveToCombat(GameData.Data.Playermessage));
        int id = 1;
        foreach(var item in enemy)
        {
            item.UnitData["id"] = id;
            item.NumID = id;
            list.Add(item);
            id++;
        }
        CombatController.instance.openCombat(list);
    }


    //-------------------------------------------------------------------------------------------------------------------------
    //类型转换工具类
    public CombatMessage playerSaveToCombat(PlayerMessage play)     //玩家数据  转换  战斗场景数据
    {
        CombatMessage mess = new CombatMessage();
        mess.UnitData["id"] = 0;
        mess.UnitData["physical"] = play.hpmax;
        mess.UnitData["vigor"] = play.mpmax;
        mess.UnitData["attack"] = play.atk;
        mess.UnitData["speed"] = play.speed;
        mess.UnitData["type"] = 0;
        mess.UnitData["adPat"] = play.adPat;
        mess.UnitData["apPat"] = play.apPat;
        mess.UnitData["strike"] = play.strike;
        mess.UnitData["dodge"] = play.dodge;
        mess.UnitData["curHp"] = play.hpcur;
        mess.UnitData["curMp"] = play.mpcur;
        mess.UnitData["defence"] = play.def;
        mess.IconName = GameData.Data.PLAYER;
        mess.IsPlayer = true;
        mess.NumID = 0;
        mess.AttackID = 2;
        mess.SkillData = play.skills;
        return mess;
    }

}
