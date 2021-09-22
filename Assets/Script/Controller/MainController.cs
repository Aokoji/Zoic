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
            enemy.Add(enemyToCombat(i));
        }
        openCombat(enemy);
    }
    private void openCombat(List<CombatMessage> enemy)
    {
        //遭遇怪物后  会派发给maincontroller事件  传递怪物信息   控制器封装所有信息进入战斗
        List<CombatMessage> list=new List<CombatMessage>();
        //封装player信息
        list.Add(playerSaveToCombat(GameData.Data.Playermessage));
        int id = 1;
        foreach(var item in enemy)
        {
            item.id = id;
            item.NumID = id;
            list.Add(item);
            id++;
        }
        string combatname = "combat" + GameData.Data.DataPlaymessage.combatIDCount;
        PubTool.instance.addLogger("遭遇战斗,战斗序号：combat"+GameData.Data.DataPlaymessage.combatIDCount);
        PubTool.instance.addCombatLogger(combatname, "进入战斗");
        GameData.Data.DataPlaymessage.combatIDCount++;
        GameData.Data.saveGameMessageData();
        CombatController.instance.openCombat(list, combatname);
    }


    //-------------------------------------------------------------------------------------------------------------------------
    //类型转换工具类
    public CombatMessage playerSaveToCombat(PlayerMessage play)     //玩家数据  转换  战斗场景数据
    {
        CombatMessage mess = new CombatMessage();
        mess.id = 0;
        mess.Data.physical_base = play.physical_last;
        mess.Data.vigor_base= play.vigor_last;
        mess.Data.attack_base = play.attack_last;
        mess.Data.speed_base = play.speed_last;
        mess.Data.adPat_base = play.adPat_last;
        mess.Data.apPat_base = play.apPat_last;
        mess.Data.strike_base = play.strike_last;
        mess.Data.dodge_base = play.dodge_last;
        mess.Data.defence_base = play.defence_last;
        mess.Data.curHp = play.hpcur;
        mess.Data.curMp = play.mpcur;
        mess.IconName = GameData.Data.PLAYER;
        mess.IsPlayer = true;
        mess.NumID = 0;
        mess.AttackID = play.attackID;
        mess.SkillData = play.skills;
        return mess;
    }
    public CombatMessage enemyToCombat(int id)                //单位数据  转换  战斗场景数据
    {
        CombatMessage mess = new CombatMessage();
        UnitTypeStaticData play = AllUnitData.Data.getJsonData<UnitTypeStaticData>("allUnitData", id);
        mess.Data.physical_base = play.physical;
        mess.Data.vigor_base = play.vigor;
        mess.Data.attack_base = play.attack;
        mess.Data.speed_base = play.speed;
        mess.Data.adPat_base = play.adPat;
        mess.Data.apPat_base = play.apPat;
        mess.Data.strike_base = play.strike;
        mess.Data.dodge_base = play.dodge;
        mess.Data.defence_base = play.defence;
        mess.Data.curHp = play.physical;
        mess.Data.curMp = play.vigor;
        mess.IconName = play.name;
        mess.IsPlayer = false;
        UnitSkillStaticData skills = AllUnitData.Data.getJsonData<UnitSkillStaticData>("allUnitSkillData", id);
        mess.AttackID = skills.attakcNum;
        foreach(var sk in skills.skills)
        {
            mess.SkillData.skillHold.Add(sk);
        }
        return mess;
    }

}
