using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActionAnalyse
{
    public delegate void AnalyseEvent();
    public event AnalyseEvent combatNextStep = null;

    public AnalyzeResult analyseCombatAttack(List<CombatMessage> list,CombatMessage item)
    {
        AnalyzeResult result = new AnalyzeResult();
        //+++可能需要设置目标  或者多加一个目标分析
        //分析行动
        //调用eventManager的攻击方法


        //测试
        for (int i=0;i< list.Count;i++) { 
            if (list[i].Name == "player")
                result.takeNum = i;
            if (list[i].Name == item.Name)
                result.selfNum = i;
        }//测试
        result.skillID = 1;//测试
        result.skillType = 3;//测试

        //combatNextStep();
        return result;
    }

    private void existAnalyze(int id)
    {//生存分析
        
    }
    private void tendencyAnalyze(int id)
    {//优势分析  状态倾向分析

    }
    private void conditionAnalyze(int id)
    {//状态分析

    }
    private void anotherAnalyze(int id)
    {//其他分析

    }
}

//返回类  分析结果  返回给攻击动作组
public class AnalyzeResult{
    public int selfNum;
    public int skillID;
    public int skillType;   //生效类型  0自身，1己方单体，2己方全体，3敌方单体，4敌方全体，5全体，6特殊
    public int takeNum;     //对象序号 （相对于当此的list的序号，非单体不读取）
}
