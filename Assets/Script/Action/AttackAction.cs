using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//战斗缓存器
public class AttackAction 
{
    private List<CombatMessage> dataList;
    private AttackResult atkResult;
    CombatMessage sourceActor;          //来源
    List<CombatMessage> takeActors; //目标

    int SKILL_TYPE=3;
    int REFER_START = 16;   //参考值开始下标
    int REFER_INTREVAL = 5; //取比例间隔

    public void initData(List<CombatMessage> data)
    {
        dataList = data;
    }
    //----------------------------------总处理---------------------------------------------------------------------------
    //普通的战斗处理
    public void normalAction(AnalyzeResult action)
    {
        atkResult = new AttackResult();
        string type = AllUnitData.getSkillData(action.skillID)[SKILL_TYPE];//获取技能类型
        switch (type)
        {
            case "1": territoryTypeAction(action); break;    //场地类型处理
            case "2": stateTypeAction(action); break;    //增益类型处理
            case "3": harmTypeAction(action); break;    //攻击类型处理
            default:break;
        }
       
    }

    //特殊的战斗处理
    public void specialAction(AnalyzeResult action)
    {

    }
    //--------------------------------类型分类处理-------------------------------------------------------------------
    private void territoryTypeAction(AnalyzeResult action)
    {//场地类型处理

    }
    private void stateTypeAction(AnalyzeResult action)
    {//增益类型处理

    }
    private void harmTypeAction(AnalyzeResult action)
    {//伤害类型处理
        string[] skill = AllUnitData.getSkillData(action.skillID);
        sourceActor = dataList[action.selfNum];
        takeActors = new List<CombatMessage>();
        //分析作用范围
        effectActorAllocation(skill[4], action);
        //执行技能
        executeHarmEffect(skill);
    }


    //-------------------------------第二级调用  解析类方法------------------------------------------------------------------------------
    private void effectActorAllocation(string effect,AnalyzeResult action)
    {//目标分配赋值
        int type = int.Parse(effect);
        switch (type)
        {
            case 0:takeActors.Add(sourceActor); break;                                                  //自身
            case 1: takeActors.Add(dataList[action.takeNum]); break;                            //己方单体
            case 2:break;
            case 3: takeActors.Add(dataList[action.takeNum]); break;                            //敌方单体
            case 4:break;
            case 5:break;
            case 6:break;
            case 7:break;
            default:break;
        }
    }

    private void executeHarmEffect(string[] skill)
    {//执行伤害技能效果
        int finalNum = 0;
        for (int i = 0; i< 5; i++)
        {
            if (skill[REFER_START + i] == "-1") break;
            int atk = sourceActor.UnitData[AllUnitData.getEncode(skill[REFER_START + i])];
            int multi = int.Parse(skill[REFER_START + i + REFER_INTREVAL]);
            finalNum += (int)Mathf.Round((float)multi / 100 * atk);
        }
    }






    //检查战斗结果
    public bool checkCombatResult()
    {
        return true;
    }

}

//返回类   攻击结果  返回给动画组
public class AttackResult
{

}
