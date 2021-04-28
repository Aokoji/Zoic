using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//战斗缓存处理器
public class AttackAction
{
    private List<CombatMessage> dataList;
    private AttackResult atkResult;
    private spoilsResult spoils;    //战利品
    CombatMessage sourceActor;          //来源
    List<CombatMessage> takeActors; //目标

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
    //----------------------------------总处理---------------------------------------------------------------------------
    //普通的战斗处理
    public AttackResult normalAction(AnalyzeResult action)
    {
        atkResult = new AttackResult();
        string type = AllUnitData.getSkillData(action.skillID)[SKILL_TYPE];//获取技能类型
        sourceActor = dataList[action.selfNum];
        takeActors = new List<CombatMessage>();
        switch (type)
        {
            case "1": territoryTypeAction(action); break;    //场地类型处理
            case "2": stateTypeAction(action); break;    //增益类型处理
            case "3": harmTypeAction(action); break;    //攻击类型处理
            case "11":break; //道具类处理1
            default: break;
        }
        //行动结算
        settleOnceAction();
        if (atkResult == null) Debug.Log("动画赋值错误");
        //战斗结果分析

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
        string[] skill = AllUnitData.getSkillData(action.skillID);
        //分析作用范围
        effectActorAllocation(skill[4], action);
        //执行技能
        executeBuffEffect(skill);
    }
    private void harmTypeAction(AnalyzeResult action)
    {//伤害类型处理
        string[] skill = AllUnitData.getSkillData(action.skillID);
        //分析作用范围
        effectActorAllocation(skill[4], action);
        //执行技能
        executeHarmEffect(skill);
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
            float pat;
            if(skillPat.Equals("0")) pat = (int)Mathf.Round((float)taken.UnitData[AllUnitData.getEncode("7")]) / 100;     //分析技能攻击类型（ad  ap）
            else pat = (int)Mathf.Round((float)taken.UnitData[AllUnitData.getEncode("8")]) / 100;
            int hit = (int)(finalNum * (1 - pat));
            int extrahit = 0;
            if(skill[15].Equals("1")) extrahit=settleExtraSubjoin(taken);   //计算攻击特效附加伤害
            int phy = taken.UnitData["curHp"] - hit- extrahit;
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
                    taken.IsDead = true;
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
        atkResult.isHitRare = true;
        atkResult.hitCount = hitCountSum;
        atkResult.sourceActor = sourceActor;
        atkResult.takenActor = takeActors;
    }
    //执行作用-------------------------------------------------------增益/减益型
    private void executeBuffEffect(string[] skill)
    {
        if (Random.Range(0, 100) < int.Parse(skill[SUBJOIN_NUM + 1]))
        {
            //  1名称  2持续回合	3影响参考  4影响参数 	5参考对象   6伤害参考	 7伤害参数 8作用参考
            abnormalState abnormal = new abnormalState();
            abnormal.id = int.Parse(skill[SUBJOIN_NUM]);
            string[] state = AllUnitData.getAbnormalData(abnormal.id);
            //计算回合  区分参考型回合和固定回合     获得持续回合
            if (skill[9].Equals("2"))
            {
                abnormal.round = int.Parse(skill[10]) + getSustionNum(skill[9]);
            }
            else
            {
                abnormal.round = int.Parse(state[2]);
            }
            if (state[5].Equals("1"))
            {//参考释放者
                abnormal.perHit = Mathf.CeilToInt(sourceActor.UnitData[AllUnitData.getEncode(state[6])]*float.Parse(state[7])/100);
            }
            abnormal.refer = int.Parse(state[8]);   //作用于
            abnormal.effectAbility = int.Parse(state[3]);
            abnormal.effectMulti = int.Parse(state[4]);
            foreach (var item in takeActors)
            {
                if (state[5].Equals("0"))
                {//参考被释放者
                    abnormal.perHit = Mathf.CeilToInt(item.UnitData[AllUnitData.getEncode(state[6])] * float.Parse(state[7]) / 100);
                }
                if (!state[3].Equals("-1") && !abnormal.isEffect)
                {//修改影响数值
                    abnormal.isEffect = true;
                    item.UnitData[AllUnitData.getEncode(state[3])] *= int.Parse(state[4]) / 100;    
                }
                bool ishave = false;
                for(int i=0;i<item.Abnormal.Count;i++)
                {
                    if (item.Abnormal[i].id == abnormal.id)
                    {
                        switch (skill[9])
                        {
                            case "1":break;
                            case "2": ishave = true; break;
                            case "3": item.Abnormal[i] = abnormal;ishave=true ; break;
                            case "4":
                                abnormal.round+=item.Abnormal[i].round;
                                item.Abnormal[i] = abnormal;
                                ishave = true;
                                break;
                        }
                    }
                }
                if(!ishave)  item.Abnormal.Add(abnormal);   //添加异常状态
            }
            atkResult.inflictionID = abnormal.id;
            atkResult.isHitRare = true;
            //添加攻击特效        12特殊附加  
            if (!skill[12].Equals("0"))
            {
                specialAttackExtra extra = new specialAttackExtra();
                extra.constant = int.Parse(skill[32]);  //固定值
                extra.id = int.Parse(skill[12]);
                extra.type= int.Parse(AllUnitData.getExtraData(extra.id)[2]);
                extra.refer = int.Parse(AllUnitData.getExtraData(extra.id)[3]);
                extra.multi = int.Parse(AllUnitData.getExtraData(extra.id)[4]);
                extra.round = int.Parse(AllUnitData.getExtraData(extra.id)[5]);
                extra.target = int.Parse(AllUnitData.getExtraData(extra.id)[6]);
                bool ishave = false;
                for(int i=0;i< sourceActor.AtkExtra.Count; i++)
                {
                    if (sourceActor.AtkExtra[i].id == extra.id)
                    {
                        sourceActor.AtkExtra[i] = extra;
                        ishave = true;
                    }
                }
                if (!ishave) sourceActor.AtkExtra.Add(extra);
            }
        }
        else
        {
            atkResult.isHitRare = false;
        }
        atkResult.type = 2;
        atkResult.sourceActor = sourceActor;
        atkResult.takenActor = takeActors;
    }
    //行动结算  结算附加效果等 的值
    public void settleOnceAction()
    {
        List<abnormalState> list= sourceActor.Abnormal;
        List<specialAttackExtra> extra= sourceActor.AtkExtra;
        int[] subID = new int[list.Count];
        int[] subHit = new int[list.Count];
        for(int i = 0; i < list.Count; i++)
        {
            sourceActor.UnitData[AllUnitData.getEncode(list[i].refer.ToString())] -= list[i].perHit;
            list[i].round -= 1;
            if (list[i].round <= 0)
            {
                list.Remove(list[i]);
                i--;
            }
            subID[i] = list[i].id;
            subHit[i] = list[i].perHit;
            Debug.Log("【结算伤害】" + list[i].id + "    伤害值" + list[i].perHit);
        }
        for(int i=0;i< extra.Count; i++)
        {
            extra[i].round--;
            if (extra[i].round <= 0)
            {
                extra.Remove(extra[i]);
                i--;
            }
        }
        atkResult.subjoinID = subID;
        atkResult.subjoinHit = subHit;
    }
    //--------------------------------------------第三级调用-工具类、计算类方法----------------------------------------------------------------------------
    //计算攻击特效附加
    public int settleExtraSubjoin(CombatMessage taken)
    {
        int[] extraType = new int[sourceActor.AtkExtra.Count];
        int[] extrahit = new int[sourceActor.AtkExtra.Count];
        int count = 0;
        int mix = 0;
        foreach(var list in sourceActor.AtkExtra)
        {
            int hitnum = 0;
            if (list.target == 0)//参考自身
            {
                hitnum = (int)((float)sourceActor.UnitData[AllUnitData.getEncode(list.refer.ToString())] * list.multi / 100) + list.constant;
            }
            else//参考目标
            {
                hitnum = (int)((float)taken.UnitData[AllUnitData.getEncode(list.refer.ToString())] * list.multi / 100) + list.constant;
            }
            extrahit[count]=hitnum;
            extraType[count] = list.type;
            count++;
            mix += hitnum;
        }
        atkResult.extraHit = extrahit;
        return mix;
    }
    //目标击败
    public void settleActorDead(CombatMessage actor)
    {
        for(int i = 0; i < dataList.Count; i++)
        {
            if (dataList[i].Name == actor.Name)
            {
                //增加战利品
                if (actor.Name != PLAYER) comulativeReword(actor.UnitData["id"]);
                dataList.Remove(dataList[i]);
                i--;
            }
        }
    }
    //获得参考人数
    public int getSustionNum(string type)
    {
        int num = 0;
        switch (type)
        {
            case "0":break;
            case "1":num=dataList.Count; break;
            case "2":
                if (sourceActor.Name.Equals(PLAYER))
                    num = 1;
                else
                    num = dataList.Count - 1;
                break;
            case "3":
                if (sourceActor.Name.Equals(PLAYER))
                    num = dataList.Count - 1;
                else
                    num = 1;
                break;
            case "4":
                foreach(var res in dataList)
                {
                    if (sourceActor.Name.Equals(PLAYER))
                        if(!res.Name.Equals(PLAYER)&&res.Abnormal.Count!=0)
                            num += 1;
                    else
                        if (res.Name.Equals(PLAYER) && res.Abnormal.Count != 0)
                            num += 1;
                }
                break;
        }
        return num;
    }
    //判断游戏结束
    public bool checkCombatContinue()
    {
        if (dataList.Count == 1 && dataList[0].Name == PLAYER)
            return false;
        foreach(var item in dataList)
        {
            if (item.Name == PLAYER)
                return true;
        }
        return false;
    }
    //检查战斗结果胜利方
    public bool checkCombatResult()
    {
        if (dataList.Count == 1 && dataList[0].Name == PLAYER)
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
    public List<int> hitCount;  //这是次数  每个人都要受list长度次数攻击  总和为总伤害
    public CombatMessage sourceActor;//攻击方
    public List<CombatMessage> takenActor;//受击方
    public string changeTarget;//变动目标(显示槽)
    public int changeTo;//变动参数
    public int inflictionID;    //施加状态id  针对异常状态  类型2增益类
    public int[] subjoinID;     //异常状态id
    public int[] subjoinHit;    //异常结算伤害
    public int[] extraType;     //攻击特效类型  （和下边伤害   下标同步）
    public int[] extraHit;      //攻击特效伤害
    public bool isHitRare;  //是否命中
}
//返回类  存储类   当局战利品结算
public class spoilsResult
{
    public List<int> spoils=new List<int>();    //战利品id表
    public int coins;       //结算货币（待定）
}

