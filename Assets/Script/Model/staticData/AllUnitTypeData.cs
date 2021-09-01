using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//所有静态数据实体类

//单位数据
public class AllUnitTypeStaticData 
{
    
}
//技能参数
public class AllSkillStaticData
{

}
//单位技能
public class AllUnitSkillStaticData
{

}
//单位爆率
public class AllUnitSpoilStaticData
{

}
//物品数据
public class GoodStaticData
{
    public int id;          //编号
    public string name;     //名称
    public string describe; //描述
    public int bagType;  //在背包类型  道具材料任务装备
}
public class AllGoodStaticData
{
    public Dictionary<int, GoodStaticData> childDic = new Dictionary<int, GoodStaticData>();        //统一命名
}
//状态参数

//地图资源
public class AllCollectStaticData
{
    public Dictionary<int, ModuleOneCollect> childDic = new Dictionary<int, ModuleOneCollect>();        //统一命名
}

