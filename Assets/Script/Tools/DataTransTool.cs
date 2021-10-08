using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTransTool 
{

    private static float defCoefAmp = 1.25f;   //防御增强系数
    /// <summary>
    /// 伤害防御减伤 计算       返回系数
    /// </summary>
    /// <param name="hit">伤害参考(力或智)</param>
    /// <param name="def">防御（目标）</param>
    public static float defenceTrans(int pro,int def)
    {
        float coef = 0;
        coef = pro / pro + def * defCoefAmp;
        return coef;
    }

    private static float magicSpecialAmp=1.45f;  //魔法伤害专用系数额外增幅
    /// <summary>
    /// 属性差距增幅
    /// </summary>
    public static float propertyGapAmp(float property,int level,bool ispower)
    {
        
        return 1f;
    }
    /// <summary>
    /// 力量转攻击
    /// </summary>
    public static int forceTransAttack(int force)
    {
        return 3 * force;
    }
    public static int agilityTransDefence(int agility)
    {
        return agility*2;
    }
    public static int agilityTransSpeed(int agility)
    {
        return agility;
    }
    public static int wisdomTransCure(int wisdom)
    {
        return wisdom;
    }

    //------------------------------------------------------------动画名称转换--------------------------
    public static string animTypeNameTrans(int id)
    {
        string name = "";
        switch (id)
        {

        }
        return name;
    }

    //--------------------------------
    //-------------------------------------------------类型转换工具类----------------------------------------

    public static CombatMessage playerSaveToCombat(PlayerMessage play)     //玩家数据  转换  战斗场景数据
    {
        CombatMessage mess = new CombatMessage();
        mess.id = 0;
        mess.Data.physical_base = play.data.physical_last;
        mess.Data.vigor_base = play.data.vigor_last;
        mess.Data.attack_base = play.data.attack_fin;
        mess.Data.speed_base = play.data.speed_fin;
        mess.Data.adPat_base = play.data.adPat_last;
        mess.Data.apPat_base = play.data.apPat_last;
        mess.Data.strike_base = play.data.strike_last;
        mess.Data.dodge_base = play.data.dodge_last;
        mess.Data.defence_base = play.data.defence_fin;
        mess.Data.hitRate_base = play.data.hitRate_last;
        mess.Data.force_base = play.data.force_last;
        mess.Data.agility_base = play.data.agility_last;
        mess.Data.wisdom_base = play.data.wisdom_last;
        mess.Data.curHp = play.data.hpcur;
        mess.Data.curMp = play.data.mpcur;
        mess.IconName = GameData.Data.PLAYER;
        mess.IsPlayer = true;
        mess.NumID = 0;
        mess.AttackID = play.attackID;
        mess.SkillData = play.skills;
        mess.Name1 = "player";
        mess.initData();
        return mess;
    }
    public static CombatMessage enemyToCombat(int id)                //单位数据  转换  战斗场景数据
    {
        CombatMessage mess = new CombatMessage();
        UnitTypeStaticData play = AllUnitData.Data.getJsonData<UnitTypeStaticData>("allUnitData", id);
        mess.Data.id = id;
        mess.Data.physical_base = play.physical;
        mess.Data.vigor_base = play.vigor;
        mess.Data.attack_base = play.attack;
        mess.Data.speed_base = play.speed;
        mess.Data.adPat_base = play.adPat;
        mess.Data.apPat_base = play.apPat;
        mess.Data.strike_base = play.strike;
        mess.Data.dodge_base = play.dodge;
        mess.Data.defence_base = play.defence;
        mess.Data.hitRate_base = play.hitRate;

        mess.Data.force_base = play.force;
        mess.Data.agility_base = play.agility;
        mess.Data.wisdom_base = play.wisdom;

        mess.Data.curHp = play.physical;
        mess.Data.curMp = play.vigor;
        mess.IconName = play.name;
        mess.IsPlayer = false;
        mess.Name1 = play.name;
        UnitSkillStaticData skills = AllUnitData.Data.getJsonData<UnitSkillStaticData>("allUnitSkillData", id);
        mess.AttackID = skills.attackNum;
        foreach (var sk in skills.skills)
        {
            mess.SkillData.skillHold.Add(AllUnitData.Data.getSkillStaticData(sk));
        }
        mess.initData();
        return mess;
    }
}
