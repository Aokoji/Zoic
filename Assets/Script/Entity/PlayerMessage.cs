using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerMessage 
{
    //系统
    public bool isFirstIn;  //第一次进入

    //=================================                    人物属性          ======================
    public string name;
    public int level;               //等级
    public int expmax;         //最大经验
    public int expcur;          //当前经验

    public playerParam data=new playerParam();        //人物属性
    public int jobOrder;        //职阶
    //==================================                 技能与物品                  =======================

    public int attackID;               //攻击默认序号
    public skillSave skills=new skillSave();    //技能
    public itemSave items=new itemSave();   //背包
    public bool isDead;

    //+++任务 这个要分开记录  和下边
    public int plotCount;   //剧情进度 (主)
    public List<int> completeSideQuestNum=new List<int>();  //完成的支线任务编号  
    public List<sideQuest> unfinishedSideQuest = new List<sideQuest>();  //未完成  但接受的支线

    //存档
    public int lastSceneNum;     //最后场景编号
    public int lastPosX;     //信标位置
    public int lastPosY;     

    public void paddingData()
    {
        //+++计算装备加成  赋值
        /*
        data = AllUnitData.Data.playerEquipCalculate(data,data.equip1);
        data = AllUnitData.Data.playerEquipCalculate(data,data.equip2);
        data = AllUnitData.Data.playerEquipCalculate(data,data.equip3);
        data = AllUnitData.Data.playerEquipCalculate(data,data.equip4);
        data = AllUnitData.Data.playerEquipCalculate(data,data.equip5);
        data = AllUnitData.Data.playerEquipCalculate(data,data.equip6);*/
        //+++计算最终面板
        data.physical_last = data.physical_base + data.physical_equip;
        data.vigor_last = data.vigor_base + data.vigor_equip;
        data.attack_fin = data.attack_base + data.attack_equip;
        data.speed_fin = data.speed_base + data.speed_equip;
        data.defence_fin = data.defence_base + data.defence_equip;
        data.force_last = data.force_base + data.force_equip+data.force_point;
        data.agility_last = data.agility_base + data.agility_equip+data.agility_point;
        data.wisdom_last = data.wisdom_base + data.wisdom_equip+data.wisdom_point;
        data.adPat_last = data.adPat_base + data.adPat_equip;
        data.apPat_last = data.apPat_base + data.apPat_equip;
        data.strike_last = data.strike_base + data.strike_equip;
        data.dodge_last = data.dodge_base + data.dodge_equip;
        data.hitRate_last = data.hitRate_base + data.hitRate_equip;
        if (data.hpcur > data.physical_last) data.hpcur = data.physical_last;
        if (data.mpcur > data.vigor_last) data.mpcur = data.vigor_last;

        data.attack_last = data.attack_fin + DataTransTool.forceTransAttack(data.force_last);
        data.speed_last = data.speed_fin + DataTransTool.forceTransAttack(data.agility_last);
        data.defence_last = data.defence_fin + DataTransTool.forceTransAttack(data.agility_last);
        data.physical_recover = data.physical_recBase + data.phyRecover_equip;
        data.vigor_recover = data.vigor_recBase + data.vigRecover_equip;
    }
    /// <summary>
    /// 按减法计算当前体力
    /// </summary>
    public bool subCurPhysical(int hit)
    {
        data.hpcur -= hit;
        if (data.hpcur <= 0)
        {
            isDead = true;
            return true;
        }
        if (data.hpcur > data.physical_last)
        {
            data.hpcur = data.physical_last;
        }
        return false;
    }
    /// <summary>
    /// 按减法计算当前精力
    /// </summary>
    public void subCurVigor(int hit)
    {
        data.mpcur -= hit;
        if (data.mpcur <= 0)
        {
            data.mpcur = 0;
        }
        if (data.mpcur > data.vigor_last)
        {
            data.mpcur = data.vigor_last;
        }
    }

}

//人物属性
[System.Serializable]
public class playerParam
{
    public int hpcur;            //当前体力
    public int mpcur;           //当前精力
    //(基础)
    public int physical_base;           //最大体力
    public int vigor_base;         //最大精力
    public int attack_base;                 //攻击
    public int speed_base;               //速度
    public int strike_base;              //暴击
    public int dodge_base;               //闪避
    public int hitRate_base;               //命中
    public int defence_base;                 //防御
    public int adPat_base;               //物理减伤
    public int apPat_base;               //魔法减伤
    public int force_base;      //力量
    public int wisdom_base; //智力
    public int agility_base;    //敏捷
    //(天赋点加成)   (后天加成)
    public int talentPoint;     //未用天赋
    public int maxTalentPoint;  //总点
    public int force_point;      //力量
    public int wisdom_point; //智力
    public int agility_point;    //敏捷
    //装备加成  id
    public int equip1;      //武器1     
    public int equip2;      //武器2
    public int equip3;      //防具 头
    public int equip4;      //防具 身
    public int equip5;      //配饰
    public int equip6;      //配饰
    public int equip7;      //配饰

    //装备加成  属性      ***该部分不用赋值***
    public int physical_equip;
    public int vigor_equip;
    public int attack_equip;
    public int speed_equip;
    public int strike_equip;
    public int dodge_equip;
    public int hitRate_equip;
    public int defence_equip;
    public int adPat_equip;
    public int apPat_equip;
    public int force_equip;
    public int wisdom_equip;
    public int agility_equip;

    public int phyRecover_equip;
    public int vigRecover_equip;
    //最终面板          ***(该部分不赋值，用启动方法计算赋值)****
    public int attack_fin;      //分开存储的终值  但未加算到last值
    public int speed_fin;
    public int defence_fin;

    public int physical_last;
    public int vigor_last;
    public int attack_last;
    public int speed_last;
    public int strike_last;
    public int dodge_last;
    public int hitRate_last;
    public int defence_last;
    public int adPat_last;
    public int apPat_last;
    public int force_last;
    public int wisdom_last;
    public int agility_last;

    //其他属性
    //每10s 恢复属性     基础
    public int physical_recBase;
    public int vigor_recBase;
    public int physical_recover;
    public int vigor_recover;
}
//技能信息
[System.Serializable]
public class skillSave
{
    public int skillPoint;
    public List<SkillStaticData> skillHold = new List<SkillStaticData>();
}

//持有物信息
[System.Serializable]
public class itemSave
{
    public int coins;
    public List<oneGood> goods = new List<oneGood>();
}

//单个物品信息
[System.Serializable]
public class oneGood
{
    public int id;      //id
    public string picture;      //图标（目前不用）
    public string typeName; //物品类型（统类图标）
    public int bagType;     //背包分类  (1装备，2道具，3材料，4任务)
    public int num; //持有数
}

//支线任务
[System.Serializable]
public class sideQuest
{
    public int id;
}
