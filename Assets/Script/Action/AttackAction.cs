using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//战斗缓存器
public class AttackAction
{
    private List<CombatMessage> dataList;
    private AttackResult atkResult;
    private spoilsResult spoils;    //战利品
    CombatMessage sourceActor;          //来源
    List<CombatMessage> takeActors; //目标

    int SKILL_TYPE = 3;
    int REFER_START = 16;   //参考值开始下标
    int REFER_INTREVAL = 5; //取比例间隔
    int REWORD_NUM = 10;//最高掉落数  9-1=8

    public void initData(List<CombatMessage> data)
    {
        dataList = data;
        atkResult = new AttackResult();
        spoils = new spoilsResult();
    }
    //----------------------------------总处理---------------------------------------------------------------------------
    //普通的战斗处理
    public AttackResult normalAction(AnalyzeResult action)
    {
        string type = AllUnitData.getSkillData(action.skillID)[SKILL_TYPE];//获取技能类型
        switch (type)
        {
            case "1": territoryTypeAction(action); break;    //场地类型处理
            case "2": stateTypeAction(action); break;    //增益类型处理
            case "3": harmTypeAction(action); break;    //攻击类型处理
            default: break;
        }


        if (atkResult == null) Debug.Log("动画赋值错误");
        return atkResult;
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
        //行动结算
        settleOnceAction();
    }


    //-------------------------------第二级调用  解析类方法------------------------------------------------------------------------------
    private void effectActorAllocation(string effect, AnalyzeResult action)
    {//目标分配赋值
        int type = int.Parse(effect);
        switch (type)
        {
            case 0: takeActors.Add(sourceActor); break;                                                  //自身
            case 1: takeActors.Add(dataList[action.takeNum]); break;                            //己方单体
            case 2: break;
            case 3: takeActors.Add(dataList[action.takeNum]); break;                            //敌方单体
            case 4: break;
            case 5: break;
            case 6: break;
            case 7: break;
            default: break;
        }
    }

    private AttackResult executeHarmEffect(string[] skill)
    {//执行伤害技能效果
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
        string skillPat = skill[REFER_START + 6 + REFER_INTREVAL];
        //计算作用目标伤害
        foreach (var taken in takeActors)
        {
            float pat;
            if(skillPat=="0") pat = (int)Mathf.Round((float)taken.UnitData[AllUnitData.getEncode("7")]) / 100;     //分析技能攻击类型（ad  ap）
            else pat = (int)Mathf.Round((float)taken.UnitData[AllUnitData.getEncode("8")]) / 100;
            int hit = (int)(finalNum * (1 - pat));
            int phy = taken.UnitData["curHp"] - hit;
            settleExtraSubjoin();   //++++计算攻击特效附加伤害
            if (phy <= 0)
            {
                if (checkImmortalState())
                {
                    taken.UnitData["curHp"] = 1;
                }
                else
                {
                    //判定目标死亡
                    taken.UnitData["curHp"] = 0;
                    settleActorDead(taken);
                }
            }
            else
            {
                taken.UnitData["curHp"] = phy;
                atkResult.changeTarget = "";        //+++待修改  变动目标
                atkResult.changeTo = phy;
            }
        }
        atkResult.type = 3;     //攻击类型
        atkResult.hitCount = hitCountSum;
        atkResult.sourceActor = sourceActor;
        atkResult.takenActor = takeActors;
        return atkResult;
    }
    //行动结算  结算附加效果等 的值
    public void settleOnceAction()
    {
        
    }
    //--------------------------------------------第三级调用-工具类、计算类方法----------------------------------------------------------------------------
    //计算攻击特效附加
    public void settleExtraSubjoin()
    {

    }
    //目标击败
    public void settleActorDead(CombatMessage actor)
    {
        for(int i = 0; i < dataList.Count; i++)
        {
            if (dataList[i].Name == actor.Name)
            {
                //增加战利品
                if (actor.Name != "player") comulativeReword(actor.UnitData["id"]);
                dataList.Remove(dataList[i]);
                i--;
            }
        }
    }
    public bool checkCombatContinue()
    {//判断游戏结束
        if (dataList.Count == 1 && dataList[0].Name == "player")
            return false;
        foreach(var item in dataList)
        {
            if (item.Name == "player")
                return true;
        }
        return false;
    }
    //检查战斗结果胜利方
    public bool checkCombatResult()
    {
        if (dataList.Count == 1 && dataList[0].Name == "player")
            return true;  //如果只剩玩家  则玩家胜利
        else
            return false;
    }
    //计算特殊状态 不屈
    private bool checkImmortalState()
    {
        return false;
    }

    //-------------------计算累积奖励
    private void comulativeReword(int id)
    {
        int num = 0;    //最终个数
        int[] nums=new int[9];
        //计算数量
        for(int i = 1; i < REWORD_NUM; i++)
        {
            nums[i-1] = int.Parse(AllUnitData.getSpoilData(id)[i]);    //+++奖励数量几率参数
            //下标 1 2 3 4 5 6 7 8 9是个数倍率  接受1位小数  即0.5%
        }
        int tem = Random.Range(0, 1000);
        int ratherNum = 0;
        for(int i = 0; i < nums.Length; i++)
        {
            ratherNum += nums[i]*10;
            if (tem < ratherNum)
            {
                tem = i;
                break;
            }
        }
        //逐个计算概率
        for(int i = 0; i < tem; i++)
        {
            //添加结果
            int rewordID = randomReword(id);
            spoils.spoils.Add(rewordID);
            Debug.Log("【掉落物品】"+AllUnitData.getGoodData(rewordID)[1]);
        }
    }
    private int randomReword(int id)
    {//从可获得物品中随机一个(有保底)  调该方法时意味着比得一个
        int tem = Random.Range(0, 1000);
        string[] nums = AllUnitData.getSpoilData(id);
        int ratherNum = 0;
        for(int i = REWORD_NUM; i < nums.Length-3; i += 2)
        {
            ratherNum += int.Parse(nums[i + 1]) * 10;
            if (tem < ratherNum)
            {
                return int.Parse(nums[i]);
            }
        }
        tem = Random.Range(0, 2);
        if (tem == 0) return int.Parse(nums[nums.Length - 2]);
        else return int.Parse(nums[nums.Length - 1]);
    }
    //---------类结束----------------
}

//返回类   攻击结果  返回给动画组
public class AttackResult
{
    public int type;    //类型
    public List<int> hitCount;
    public CombatMessage sourceActor;//攻击方
    public List<CombatMessage> takenActor;//受击方
    public string changeTarget;//变动目标
    public int changeTo;//变动参数
    public int[] subjoinID;     //异常状态id
    public int[] subjoinHit;    //异常结算伤害
}
//返回类  存储类   当局战利品结算
public class spoilsResult
{
    public List<int> spoils=new List<int>();    //战利品id表
    public int coins;       //结算货币（待定）
}

