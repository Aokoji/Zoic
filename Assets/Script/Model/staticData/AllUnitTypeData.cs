using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//所有静态数据实体类

//----------------------------------------单位数据-------------------------------------------
[System.Serializable]
public class AllUnitTypeStaticData { public List<UnitTypeStaticData> childDic = new List<UnitTypeStaticData>(); } //统一命名
[System.Serializable]
public class UnitTypeStaticData 
{
    public int id;
    public string name;
    public int physical;    //体力
    public int vigor;       //精力
    public int attack;      //基础攻击
    public int speed;       //基础速度
    public int type;        //种类
    public int adPat;       //物理减伤
    public int apPat;       //魔法减伤
    public int strike;      //暴击
    public int dodge;   //闪避(物理全值  魔法默认为1/2)
    public int hitRate;     //普通命中
    public int defence; //防御
    public int force;       //力量    (人物基础值)
    public int wisdom;      //智力
    public int agility;     //敏捷
}
//----------------------------------------技能参数------------------------------------------
[System.Serializable]
public class AllSkillStaticData{ public List<SkillStaticData> childDic = new List<SkillStaticData>();}
[System.Serializable]
public class SkillStaticData
{//ps  带*的都要参考  类型手册表 SkillType_Manual
    //几率参数都是默认要/100
    public int id;
    public int level;
    public int levelCost;   //升级花费
    public string name;
    public string describe;     //描述
    public bool isHit;           //技能类型(伤害 buff 场地 治疗)
    public bool isBuff;
    public bool isDomain;
    public bool isCure;
    public bool isProp;

    public int effectType;      //生效范围类型*
    public int sustainType;     //持续类型*
    public int sustainTimeBase;     //生效基础回合
    public int sustainRefType;      //参考类型*
    public float sustainMulti;      //参考倍率
    public int subOdds;     //成功率

    public bool isFrontBuff;    //判断先buff还是先别的
    public int buffId;
    public int forceRefer;      //力量变动比例
    public int wisdomRefer;     //智力变动比例    (战斗中不影响mp上限)
    public int agilityRefer;    //敏捷变动比例

    //攻击 防御 速度 闪避 命中 暴击 力量 智力 敏捷
    //降属性只会降三维
    //异常状态  烧伤治愈参考智力、撕裂流血参考力量、中毒 参考敏捷
    //1力量=3攻击 体力
    //1智力= 精力+基于智力魔攻+精力回复速度
    //1敏捷=1速度 2防御

    public int damageType;  //伤害类型  0,1  物理 魔法
    public bool isSpecialEffect;     //附带攻击特效
    public bool isHoly;         //真伤
    public int damageRefer;     //伤害参考属性
    public int damageMulti;     //伤害比例
    public int damageNum;       //伤害次数

    public int expend1;         //消耗体力
    public int expend2;         //消耗精力
    public int coolDown;        //冷却
    public int runDown;         //跑冷却

}
//----------------------------------------单位技能-------------------------------------------
[System.Serializable]
public class AllUnitSkillStaticData{public List<UnitSkillStaticData> childDic = new List<UnitSkillStaticData>();}
[System.Serializable]
public class UnitSkillStaticData
{
    public int id;
    public int skillType;
}
//----------------------------------------单位爆率--------------------------------------------
[System.Serializable]
public class AllUnitSpoilStaticData{public List<UnitSpoilStaticData> childDic = new List<UnitSpoilStaticData>();}
[System.Serializable]
public class UnitSpoilStaticData
{
    public int id;
}
//----------------------------------------物品数据----------------------------------------
[System.Serializable]
public class AllGoodStaticData{public List<GoodStaticData> childDic = new List<GoodStaticData>();}
[System.Serializable]
public class GoodStaticData
{
    public int id;          //编号
    public string name;     //名称
    public string describe; //描述
    public int bagType;  //在背包类型  道具材料任务装备
}
//---------------------------------------地图资源-------------------------------------------
[System.Serializable]
public class AllCollectTypeStaticData{public List<CollectTypeStaticData> childDic = new List<CollectTypeStaticData>();}
//--和实体不一样  这是存储  地图物品类型的类
[System.Serializable]
public class CollectTypeStaticData
{
    public int id;
}
//---------------------------------------状态参数------------------------------------------
[System.Serializable]
public class AllAbnormalStaticData { public List<AbnormalStaticData> childDic = new List<AbnormalStaticData>(); }
[System.Serializable]
public class AbnormalStaticData
{
    public int id;
    public string name;
    public bool isSpecial;      //是否特殊（攻击特效）
    public bool isTarget;       //是否作用taken对面目标
    public int abnormalType;        //异常参数种类  用来区分是否是同一种类异常
    public int effectAbility;       //影响属性
    public int effectRefer;         //影响效果参考
    public int effectReferNum;         //影响效果参考值(释放时记录)
    public int effectMulti;         //影响幅度
}


