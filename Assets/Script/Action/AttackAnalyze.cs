using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//战斗分析器（新）+++待完成
public class AttackAnalyze : MonoBehaviour
{
    private List<CombatMessage> dataList;       //总数据
    private AttackResult atkResult;
    private spoilsResult spoils;    //战利品
    CombatMessage sourceActor;          //来源
    List<CombatMessage> takeActors; //目标
    public bool isFaild = false;

    string PLAYER = "player";   //玩家name
    int SKILL_TYPE = 3;
    int SUBJOIN_NUM = 5;    //特殊效果下标
    int REFER_START = 16;   //参考值开始下标
    int REFER_INTREVAL = 5; //取比例间隔
    int REWORD_NUM = 10;//最高掉落数  9-1=8
    int DAMAGE_TYPE = 14;   //伤害类型下标
    public void initData(List<CombatMessage> data)
    {
        dataList = data;
        spoils = new spoilsResult();
    }
    //----------------------------------总处理方法-------------------------------------------------------------------
    //------------输入  AnalyzeResult  类   包含（int）：自身下标、技能id、技能类型（判断范围）、目标序号（仅单体，群体类型根据技能类型区分）
    //------------输出  AttackResult    类   统合该次行动的数据

    //普通的战斗处理
    public AttackResult normalAction(AnalyzeResult action)
    {
        atkResult = new AttackResult();
        string type = AllUnitData.getSkillData(action.skillID)[SKILL_TYPE];//获取技能类型
        sourceActor = dataList[action.selfNum];     //伤害来源目标
        takeActors = new List<CombatMessage>();     //被伤目标
        switch (type)
        {
            case "101": territoryTypeAction(action); break;    //场地类型处理
            case "102": stateTypeAction(action); break;    //增益类型处理
            case "103": harmTypeAction(action); break;    //攻击类型处理
            case "111": break; //道具类处理1
            default: break;
        }
        //行动结算
        //settleOnceAction();
        //计算消耗
        //settleExpend(action.skillID);
        if (atkResult == null) Debug.Log("动画赋值错误");
        //战斗结果分析
        //settleActionEnd();
        return atkResult;
    }
    //特殊的战斗处理
    public void specialAction(AnalyzeResult action)
    {

    }
    //-----------------------------------------------------------------------------------------------------------------------
    //--------------------------------类型分类处理-------------------------------------------------------------------
    private void territoryTypeAction(AnalyzeResult action)
    {//场地类型处理

    }
    private void stateTypeAction(AnalyzeResult action)
    {//增益类型处理
        string[] skill = AllUnitData.getSkillData(action.skillID);
        //分析作用范围
        effectActorAllocation(skill[4], action);
        //执行技能
        //executeBuffEffect(skill);
    }
    private void harmTypeAction(AnalyzeResult action)
    {//伤害类型处理
        string[] skill = AllUnitData.getSkillData(action.skillID);
        //分析作用范围
        effectActorAllocation(skill[4], action);
        //执行技能
        executeHarmEffect(skill);
    }
    //-------------------------------第二级调用  技能解析类方法------------------------------------------------------------------------------

    //分析作用范围
    private void effectActorAllocation(string effect, AnalyzeResult action)             //+++未完成  技能范围分析
    {
        int type = int.Parse(effect);
        switch (effect)
        {
            case "200": takeActors.Add(sourceActor); break;                                                  //自身
            case "201": takeActors.Add(dataList[action.takeNum]); break;                            //己方单体
            case "202": break;
            case "203": takeActors.Add(dataList[action.takeNum]); break;                            //敌方单体
            case "204": break;
            case "205": break;
            case "206": break;
            case "207": break;
            default: break;
        }
    }
    //执行伤害技能效果-------------------------------------------------伤害型
    private void executeHarmEffect(string[] skill)
    {
        int finalNum = 0;
        List<int> hitCountSum = new List<int>();      //伤害次数
        //计算攻击
        for (int i = 0; i < 5; i++)
        {
            if (skill[REFER_START + i] == "-1") break;
            int atk = sourceActor.UnitData[AllUnitData.getEncode(skill[REFER_START + i])];      //获取参考值
            int multi = int.Parse(skill[REFER_START + i + REFER_INTREVAL]);                 //获取系数
            int hitnum = (int)Mathf.Round((float)multi / 100 * atk);
            finalNum += hitnum;                             //累加结果
            hitCountSum.Add(hitnum);
        }
        string skillPat = skill[DAMAGE_TYPE];
        //计算作用目标伤害
        foreach (var taken in takeActors)
        {
            if (Random.Range(0, 100) < 100 - taken.UnitData[AllUnitData.getEncode("10")]) //计算闪避
            {
                //if (!skill[5].Equals("0")) executeBuffEffect(skill);
                atkResult.isHitRare.Add(true);
                float pat;
                if (skillPat.Equals("0")) pat = (int)Mathf.Round((float)taken.UnitData[AllUnitData.getEncode("7")]) / 100;     //分析技能攻击类型（ad  ap）
                else pat = (int)Mathf.Round((float)taken.UnitData[AllUnitData.getEncode("8")]) / 100;
                int hit = (int)(finalNum * (1 - pat));
                int extrahit = 0;
                //if (skill[15].Equals("1")) extrahit = settleExtraSubjoin(taken, hit);   //计算攻击特效附加伤害
                int phy = taken.UnitData["curHp"] - hit - extrahit;
                atkResult.changeTarget = "";        //+++待修改  变动目标
                if (phy <= 0) phy = 0;
                taken.UnitData["curHp"] = phy;
                atkResult.changeTo = phy;
            }
            else
            {
                //settleExtraSubjoin(taken, 0);
                atkResult.isHitRare.Add(false);
            }
        }
        atkResult.type = 3;     //攻击类型
        atkResult.hitCount = hitCountSum;
        atkResult.sourceActor = sourceActor;
        atkResult.takenActor = takeActors;
    }

}
