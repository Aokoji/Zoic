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
            enemy.Add(enemyToCombat(i));
        }
        openCombat(enemy);
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
        PubTool.instance.addLogger("遭遇战斗,战斗序号：combat"+GameData.Data.DataPlaymessage.combatIDCount);
        PubTool.instance.addCombatLogger("combat" + GameData.Data.DataPlaymessage.combatIDCount, "进入战斗");
        GameData.Data.DataPlaymessage.combatIDCount++;
        GameData.Data.saveGameMessageData();
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
    public CombatMessage enemyToCombat(int id)                //单位数据  转换  战斗场景数据
    {
        CombatMessage mess = new CombatMessage();
        string[] enemy = AllUnitData.getUnitData(id);
        mess.UnitData["physical"] = int.Parse(enemy[2]);
        mess.UnitData["vigor"] = int.Parse(enemy[3]);
        mess.UnitData["attack"] = int.Parse(enemy[4]);
        mess.UnitData["speed"] = int.Parse(enemy[5]);
        mess.UnitData["type"] = int.Parse(enemy[6]);
        mess.UnitData["adPat"] = int.Parse(enemy[7]);
        mess.UnitData["apPat"] = int.Parse(enemy[8]);
        mess.UnitData["strike"] = int.Parse(enemy[9]);
        mess.UnitData["dodge"] = int.Parse(enemy[10]);
        mess.UnitData["curHp"] = int.Parse(enemy[2]);
        mess.UnitData["curMp"] = int.Parse(enemy[3]);
        mess.UnitData["defence"] = int.Parse(enemy[13]);
        mess.IconName = enemy[1];
        mess.IsPlayer = false;
        skillSave skill;
        List<skillSave> list = new List<skillSave>();
        string[] skills = AllUnitData.getUnitSkillData(id);
        mess.AttackID = int.Parse(skills[1]);
        for(int i = 2; i < skills.Length; i+=2)
        {
            skill = new skillSave();
            skill.skillID = int.Parse(skills[i]);
            skill.skillLevel = int.Parse(skills[i+1]);
            list.Add(skill);
        }
        mess.SkillData = list;
        return mess;
    }

}
