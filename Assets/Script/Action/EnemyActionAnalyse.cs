using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//AI行动分析器   现在内嵌到战斗单位里去了
public class EnemyActionAnalyse
{
    private List<int> atkType;          //攻击型
    private List<int> beneficialType;   //有利型
    private List<int> specialType;      //特殊
    private List<int> cureType;         //恢复
    private List<int> defType;          //防御
    private List<int> priorityType;         //优先型
    private CombatMessage player;
    // 内部结构体  分析详情
    private struct AnalyzeDetail
    {
        int advantageLevel; //优势等级  0-10 平均5
        int advantageType; //优势类型  0均势  1属性优势 
        bool advantageJudge;  //优势评判
    }
    public EnemyActionAnalyse()
    {
        initList();
    }

    public AnalyzeResult analyseCombatAttack(List<CombatMessage> list,CombatMessage item, CombatMessage play)
    {
        player = play;
        AnalyzeResult result = new AnalyzeResult();
        //可能需要设置目标  或者多加一个目标分析
        //技能解析
        //skillAnalyze(item.SkillData);
        //分析行动
        //派发攻击事件

        result.skillID=randomSkill(item);
        if(AllUnitData.getSkillData(result.skillID)[4].Equals("200")|| AllUnitData.getSkillData(result.skillID)[4].Equals("201"))
            result.takeNum = item.NumID;
        else if(AllUnitData.getSkillData(result.skillID)[4].Equals("203"))
            result.takeNum = player.NumID;
        result.selfNum = item.NumID;
        result.skillType = int.Parse(AllUnitData.getSkillData(result.skillID)[3]);

        //如果是伤害型技能  或 指向性debuff
        //测试
        /*
        result.takeNum = player.NumID;
        result.selfNum = item.NumID;
        int skillid = item.AttackID;//测试
        result.skillID =skillid; 
        result.skillType = int.Parse(AllUnitData.getSkillData(skillid)[3]);
        */
        return result;
    }
    //随机技能
    private int randomSkill(CombatMessage item)
    {
        int id = item.AttackID;
        if (item.SkillOdds>0&&Random.Range(0, 100) < item.SkillOdds)   //触发技能
            id = item.SkillData[Random.Range(0, item.SkillData.Count)].skillID;
        return id;
    }

    private void initList()
    {
        atkType=new List<int>();
        beneficialType = new List<int>();
        specialType = new List<int>();
        cureType = new List<int>();
        defType = new List<int>();
        priorityType = new List<int>();
    }
    public void skillAnalyze(List<skillSave> item)
    {//技能解析
        foreach(var skill in item)
        {
            int id = skill.skillID;
            switch (AllUnitData.getSkillData(id)[33])
            {
                case "901": atkType.Add(id); break;
                case "902": beneficialType.Add(id); break;
                case "903": defType.Add(id); break;
                case "904": cureType.Add(id); break;
                case "905": priorityType.Add(id); break;
                case "906": specialType.Add(id); break;
            }
        }
    }
    private void tendencyAnalyze(int id)
    {//优势分析  状态倾向分析

    }
    private void existAnalyze(int id)
    {//生存分析
        
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
