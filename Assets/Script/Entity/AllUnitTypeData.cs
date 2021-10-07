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

    public int force_coefficient;   //力量成长系数  基础值+等级*系数
    public int agility_coefficient;
    public int wisdom_coefficient;
}
//----------------------------------------技能参数------------------------------------------
[System.Serializable]
public class AllSkillStaticData{ public List<SkillStaticData> childDic = new List<SkillStaticData>();}
[System.Serializable]
public class SkillStaticData
{//ps  带*的都要参考  类型手册表 SkillType_Manual
    //几率参数都是默认要/100
    public int id;
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
    public int buffRefer;      //影响变动比例
    public int buffAbility;     //影响属性编号
    public int buffConstant;    //影响固定值

    //攻击 防御 速度 闪避 命中 暴击 力量 智力 敏捷
    //降属性只会降三维
    //异常状态  烧伤治愈参考智力、撕裂流血参考力量、中毒 参考敏捷
    //1力量=3攻击 
    //1智力= 基于智力魔攻+精力回复速度1   idle 3  (普通状态原地休息每过10s 恢复基础值 3*（1+等级*0.2）hp 0mp)
    //1敏捷=1速度 2防御

    public int damageType;  //伤害类型  191,192  物理 魔法
    public bool isSpecialEffect;     //附带攻击特效
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
    public int unitId;
    public int attackNum;
    public List<int> skills = new List<int>();
}
//----------------------------------------单位爆率--------------------------------------------
[System.Serializable]
public class AllUnitSpoilStaticData{public List<UnitSpoilStaticData> childDic = new List<UnitSpoilStaticData>();}
[System.Serializable]
public class UnitSpoilStaticData
{
    public int id;
    public List<int> spoilItems = new List<int>();      //可出素材
    public List<int> mgSpoil = new List<int>();         //几率
    public int[] awardNum = new int[9];    //奖励个数 1-9  几率  支持1位小数  即0.5%    会存5
    public int coinType;    //货币种类
    public int coinNum;     //钱 
    public int coinFloat;   //浮动值   给正数 10倍  如5%浮动给50 
    public int coinLevelMulti;  //参考等级提升比率
    public int minum1;  //低保
    public int minum2;
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
    public bool isTarget;       //是否  * 作用 *  taken对面目标

    public int resCount;    //剩余次数（有次数优先次数）不使用为-1
    public bool isGain;         //是否增益（用来显示说明）
    public int abnormalLogo;        //异常参数种类图标  (攻击，力量，敏捷，智力，暴击，闪避，命中，速度，防御，逃跑)（中毒，流血，灼烧）

    public int effectAbility;       //影响属性编号
    public int effectMulti;         //影响幅度（基础）(百分比或固定  目前百分比)(增减益)
    public int effectConstant;      //固定值

    public bool isSettleHit;      //是否结算伤害
    public bool isSettleCure;   //是否结算治疗
    public bool isBuff;     //是否buff    (区分攻击特效和结算伤害)

    public bool isSelf;     //特殊单独计算参考目标取值（参考自身还是目标）
    public int effectType;      //伤害类型(计算减伤用)
    public int effectTypeShow;  //伤害类型（显示用）
    public int effectRefer;         //影响效果参考属性编号（攻击特效和异常伤害状态）(攻击特效取 effectHitMulti  计算伤害 毒 治疗)       (比如参考当前生命)
    public int effectHitMulti;      //伤害取值      （目前是百分比）
    public int effectReferNum;         //影响效果参考值(释放时记录) (针对例 毒)
}

//-----------------------------------装备参数---------------------------------------------------------
[System.Serializable]
public class AllEquipStaticData { public List<EquipStaticData> childDic = new List<EquipStaticData>(); }
[System.Serializable]
public class EquipStaticData
{
    public int id;
    public string name;
    public string describe;
    public int physicalAdd;    //体力
    public int vigorAdd;       //精力
    public int attackAdd;      //基础攻击
    public int speedAdd;       //基础速度
    public int type;        //种类
    public int adPatAdd;       //物理减伤
    public int apPatAdd;       //魔法减伤
    public int strikeAdd;      //暴击
    public int dodgeAdd;   //闪避(物理全值  魔法默认为1/2)
    public int hitRateAdd;     //普通命中
    public int defenceAdd; //防御
    public int forceAdd;       //力量    (人物基础值)
    public int wisdomAdd;      //智力
    public int agilityAdd;     //敏捷
    public int specialType;
}

