using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerMessage 
{
    public bool isFirstIn;  //第一次进入






    public int level;               //等级
    public int hpmax;           //最大体力
    public int hpcur;            //当前体力
    public int mpmax;         //最大精力
    public int mpcur;           //当前精力
    public int expmax;         //最大经验
    public int expcur;          //当前经验
    public int atk;                 //攻击
    public int def;                 //防御
    public int speed;               //速度
    public int strike;              //暴击
    public int dodge;               //闪避
    public int adPat;               //物理减伤
    public int apPat;               //魔法减伤


    public int attackID;               //攻击默认序号
    public int skillPoint;          //技能点
    public List<skillSave> skills=new List<skillSave>();

}
[System.Serializable]
public class skillSave
{
    public int skillID;
    public int skillLevel;
}
