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

    private bool isContraintNormal = true;  //强制平A
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
    /// <summary>
    /// 分析行动
    /// </summary>
    /// <param name="list">所有单位信息</param>
    /// <param name="item">攻击方</param>
    /// <returns></returns>
    public AnalyzeResult analyseCombatAttack(List<CombatMessage> list,CombatMessage item,CombatConfigMessage config)
    {
        player = list[0];
        AnalyzeResult result = new AnalyzeResult();
        //可能需要设置目标  或者多加一个目标分析
        //技能解析
        //skillAnalyze(item.SkillData);
        //分析行动
        //派发攻击事件

        if (isContraintNormal)
        {
            result.isNormalAtk = true;
            result.isExtraHit = true;
            result.isMoveInstruct = false;
            result.selfNum = item.NumID;
            result.skillID = item.AttackID;
            result.takeNum.Add(player.NumID);
            return result;
        }
        else
        {
            //不强制普攻   才进行动作分析
        }

        result.skillID=randomSkill(item);
        SkillStaticData skill = AllUnitData.Data.getSkillStaticData( result.skillID);
        if(skill.effectType==310)
            result.takeNum .Add( item.NumID);
        else if(skill.effectType==313)
            result.takeNum.Add( player.NumID);
        result.selfNum = item.NumID;

        result.isNormalAtk = true;
        result.isExtraHit = true;
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
            id = item.SkillData.skillHold[Random.Range(0, item.SkillData.skillHold.Count)].id;
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
    public void skillAnalyze(skillSave item)
    {//技能解析
        foreach(var skill in item.skillHold)
        {
            int id = skill.id;
            switch (skill.ToString())
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
    public bool isMoveInstruct; //是否移动指令
    public int moveDistance;    //移动距离
    public bool isfrontMove;        //是否先判断移动
    public bool isonlyRun;
    public int distance;        //移动前距离(当前距离)
    public bool isExtraHit;         //是否有后续指令  区分纯移动
    public bool isNormalAtk;    //是否普通攻击(普通类型攻击 包含简单攻击 buff 和场地变化)
    public int selfNum;
    public int skillID;
    public List<int> takeNum = new List<int>();         //对象序号 
}
