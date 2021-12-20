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
    public List<combatSustainData> takenActor = new List<combatSustainData>();//受击方(主攻击) 
    public string animTypeSource;    // * 动画类型  标记播放的攻击动画

    public bool isHit;  //是否伤害
    public int hitType;
    public bool isSpecial;      //是否附带攻击特效

    public bool iscure;     //治疗
    public List<int> cureNum = new List<int>();

    public bool isBuff; //是否buff
    public int buffId;  //buffid  对应abnormal数据
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
    public int index;   //战斗四人的顺序编号
    public bool israte; //是否被命中
    public int hitresult;   //最终受伤
    public int[] hitNums; //受伤次数组（终值）
    public int[] specialCount;  //攻击特效伤害（受伤）
    public int[] specialType;   //攻击特效类型
    public string animTypeTaken;    //受击动画名
    public int cureNum; //治疗量
}

