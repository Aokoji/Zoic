using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
//战斗分析器（新）+++待完成
public class AttackAnalyze : CombatAdapter
{
    public AttackAnalyze(List<CombatMessage> data)
    {
        initData(data);
    }




}

//返回类   攻击结果  返回给动画组
public class AttackResult
{
    public int type;    //类型
    public List<int> hitCount;  //这是次数  每个人都要受list长度次数攻击  总和为总伤害
    public CombatMessage sourceActor;//攻击方
    public List<CombatMessage> takenActor;//受击方
    public string changeTarget;//变动目标(显示槽)
    public int changeTo;//变动参数
    public List<int> inflictionID = new List<int>();    //施加状态id  针对异常状态  类型2增益类
    public int[] subjoinID;     //异常状态id
    public int[] subjoinHit;    //异常结算伤害
    public int[] extraType;     //攻击特效类型  （和下边伤害   下标同步）0无  其他情况丢去动画判断
    public int[] extraHit;      //攻击特效伤害
    public bool isBuffRare;  //是否命中(buff)
    public List<bool> isHitRare = new List<bool>();   //攻击类型技能是否命中
    public List<CombatMessage> willDeadActor = new List<CombatMessage>();//给我死
}
//返回类  存储类   当局战利品结算
public class spoilsResult
{
    public List<int> spoils = new List<int>();    //战利品id表
    public int coins;       //结算货币
    public int coinType;    //货币种类
}