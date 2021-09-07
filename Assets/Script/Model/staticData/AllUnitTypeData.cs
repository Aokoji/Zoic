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
    public int physical;
    public int vigor;
    public int attack;
    public int speed;
    public int type;
    public int adPat;
    public int apPat;
    public int strike;
    public int dodge;
    public int defence;
}
//----------------------------------------技能参数------------------------------------------
[System.Serializable]
public class AllSkillStaticData{ public List<SkillStaticData> childDic = new List<SkillStaticData>();}
[System.Serializable]
public class SkillStaticData
{
    public int id;
    public int skillType;
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
}


