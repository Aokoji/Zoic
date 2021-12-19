using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//返回类   攻击结果  返回给回合结算
public class AttackResultData
{
    public bool isMoveInstruct; //是否移动指令
    public bool ismoveAndBlock;     //移动但是阻挡
    public int moveDistance;    //移动距离
    public int finDistance; //最终距离(针对敌人)
    public bool isfrontAll;     //是否在所有指令前

    public int sourceActor;//攻击方
    public List<int> takenActor = new List<int>();//受击方(主攻击)      顺序001
    public string animTypeSource;    // * 动画类型  标记播放的攻击动画
    public List<string> animTypeTaken=new List<string>();     //受击方动画        顺序001

    public bool isHit;  //是否伤害
    public List<int> hitNum = new List<int>();    //受击伤害(不包含多频)        顺序001
    public List<int[]> hitCount = new List<int[]>();    //受击伤害(包含多频)        顺序001
    public int hitType;
    public List<bool> isHitRare = new List<bool>();   //该次攻击类型技能是否命中        顺序001
    public bool isSpecial;      //是否附带攻击特效
    public List<int[]> specialCount = new List<int[]>();    //受攻击特效数       顺序001
    public List<int[]> specialType = new List<int[]>(); //受攻击特效类型（用于显示字体颜色）     顺序001

    public bool iscure;     //治疗
    public List<int> cureNum = new List<int>();

    public bool isBuff; //是否buff
    public int buffId;  //buffid  对应abnormal数据
    public List<bool> isHitBuff = new List<bool>();   //该次buff类型技能是否命中
    //+++  需要完善buff的变动修改类型

    public bool isDomain;   //是否场地变化
    public int domainId;    //场地id
    //+++   需要完善场地数据    (目前没有这样技能  可以先跳过)
    public bool isOnlyRun;  //是否选择逃跑
    public bool isrun;  //逃跑

    public List<int> awayActor = new List<int>();   //脱离战斗了=isdead  被甩远了
    public List<int> willDeadActor = new List<int>();//给我死
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
    public int index;   //下标序号
    public bool isPhy;                          //伤害类型
    public List<int> specialNumber = new List<int>();       //特效挨打(受毒、出血、治疗的特效标识)
    public List<int> specialType = new List<int>();             //特效显示数字类型  ( 颜色 ) 

    public List<abnormalState> settleBuffExist = new List<abnormalState>();         //剩余buff
    public bool isRoundDead;    //给我死
}

//战斗受指定单位 操作信息
public class combatSustainData
{

}

