using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerMessage 
{
    //系统
    public bool isFirstIn;  //第一次进入
    
    //人物属性
    public int level;               //等级
    public int hpmax;           //最大体力
    public int hpcur;            //当前体力
    public int mpmax;         //最大精力
    public int mpcur;           //当前精力
    public int expmax;         //最大经验
    public int expcur;          //当前经验
    public int atk;                 //攻击
    public int speed;               //速度
    public int strike;              //暴击
    public int dodge;               //闪避
    public int adPat;               //物理减伤
    public int apPat;               //魔法减伤

    //技能与物品
    public int attackID;               //攻击默认序号
    public int skillPoint;          //技能点
    public List<skillSave> skills=new List<skillSave>();    //技能
    public itemSave items=new itemSave();   //背包

    //任务
    public int plotCount;   //剧情进度 (主)
    public List<int> completeSideQuestNum=new List<int>();  //完成的支线任务编号  
    public List<sideQuest> unfinishedSideQuest = new List<sideQuest>();  //未完成  但接受的支线

    //存档
    public int lastSceneNum;     //最后场景编号
    public Vector2 lastBornPos;     //信标位置
}

//技能信息
[System.Serializable]
public class skillSave
{
    public int skillID;
    public int skillLevel;
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
