using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//返回类   攻击结果  返回给回合结算
public class AttackResultData
{
    public CombatMessage sourceActor;//攻击方
    public List<CombatMessage> takenActor;//受击方(主攻击)
    public int animType;    // * 动画类型  标记播放的攻击动画
    public bool isHit;  //是否伤害/治疗
    public List<bool> hitType = new List<bool>();      //受击类型  (伤害，治疗)
    public List<int[]> hitCount = new List<int[]>();    //受击伤害(包含多频)
    public List<bool> isHitRare = new List<bool>();   //该次攻击类型技能是否命中

    public bool isBuff; //是否buff
    public int buffId;  //buffid  对应abnormal数据
    public List<bool> isHitBuff = new List<bool>();   //该次buff类型技能是否命中
    //+++  需要完善buff的变动修改类型

    public bool isDomain;   //是否场地变化
    public int domainId;    //场地id
     //+++   需要完善场地数据    (目前没有这样技能  可以先跳过)

    public List<CombatMessage> willDeadActor = new List<CombatMessage>();//给我死
    /*
    public string changeTarget;//变动目标(显示槽)
    public int changeTo;//变动参数
    public List<int> inflictionID = new List<int>();    //施加状态id  针对异常状态  类型2增益类
    public int[] subjoinID;     //异常状态id
    public int[] subjoinHit;    //异常结算伤害
    public int[] extraType;     //攻击特效类型  （和下边伤害   下标同步）0无  其他情况丢去动画判断
    public int[] extraHit;      //攻击特效伤害
    public bool isBuffRare;  //是否命中(buff)
    public List<CombatMessage> willDeadActor = new List<CombatMessage>();//给我死
    */
}
//完整回合数据 对接选择完成后的动画
public class wholeRoundData
{
    //------------------------------------------------（主动操作部分）-------------------------------
    public AttackResultData result;
    //-------------------------------------------(回合结算部分)--------------------------------
    public List<CombatMessage> settleActor = new List<CombatMessage>(); //结算对象
    public List<bool> settleHitType = new List<bool>();      //结算受击类型  (伤害，治疗)
    public List<int[]> settleHitCount = new List<int[]>();    //结算受击伤害(包含多频)
    public List<int> settleBuffExist = new List<int>();         //剩余buff
    public List<CombatMessage> roundDeadActor = new List<CombatMessage>();//给我死
}
