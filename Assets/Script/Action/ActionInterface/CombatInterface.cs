using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//战斗接口  实现战斗的基本流程
public interface CombatInterface 
{
    void initData(List<CombatMessage> data);
    void roundInitData();
    AttackResultData doAction(AnalyzeResult action);    //行动处理器
    wholeRoundData roundAnalyzeAction();                //回合处理器
    void normalAction(AnalyzeResult action);    //攻击分析
    void specialAction(AnalyzeResult action);
    void moveAnalyze(AnalyzeResult action);     //移动分析
    void takeEffectAttackResult();      //伤害生效
    void hitTakeEffect(int i);
    void buffTakeEffect(int i);
    void cureTakeEffect(int i);
    void territoryTypeAction(); //技能类型处理(场地)
    //
    void stateTypeAction(); //技能类型处理(增益)
    //
    void harmTypeAction(); //技能类型处理(伤害)
    void executeHarmEffect();   //伤害效果分析
    //
    void calculateSpecialEffect(CombatMessage target);  //特殊攻击计算
    //
    //--内置判断
    bool checkCombatResult();           //判断输赢(该局战斗)
    bool checkCombatContinue();     //是否继续(该局战斗)
    void settleActorDead(CombatMessage actor);      //目标击败
    //
    //----奖励
    void comulativeReword(CombatMessage actor);         //计算目标奖励
    int randomReword(int id, UnitSpoilStaticData spoil);        //随机奖励（必出）
}

//攻击适配器 
/*
 * 实现所有战斗接口的方法  并被其他实现方法抽象继承
 */
public abstract class CombatAdapter : CombatInterface
{
    private List<CombatMessage> dataList;       //总数据
    private spoilsResult spoils;    //战利品
    private AttackResultData atkResult;     //全局攻击返回变量**
    private wholeRoundData roundData;       //回合数据**
    private SkillStaticData skill;  //暂存技能数据
    CombatMessage sourceActor;          //来源
    List<CombatMessage> takeActors; //目标
    public int distance;                //记录距离
    string PLAYER = "player";   //玩家name
    /// <summary>
    /// 初始化方法
    /// </summary>
    public void initData(List<CombatMessage> data)
    {
        dataList = data;
        spoils = new spoilsResult();
        distance = 2;   //+++初始距离为2(根据怪物属性 变化)
    }

    /// <summary>
    /// 攻击操作        ************(主调用方法)*****************
    /// </summary>
    public AttackResultData doAction(AnalyzeResult action)
    {
        roundInitData();
        moveAnalyze(action);
        //移动没有后续操作  则返回
        if (!action.isExtraHit) return atkResult;
        if (action.isNormalAtk)
            normalAction(action);
        else
            specialAction(action);
        if (atkResult == null) Debug.LogError("战斗数据赋值错误");
        takeEffectAttackResult();   //伤害生效
        return atkResult;
    }
    /// <summary>
    /// 初始化回合数据
    /// </summary>
    public void roundInitData()
    {
        skill = new SkillStaticData();
        atkResult = new AttackResultData();
        roundData = new wholeRoundData();
    }
    /// <summary>
    /// 移动分析
    /// </summary>
    public void moveAnalyze(AnalyzeResult action)
    {
        if (action.isMoveInstruct)      //有move指令
        {
            //判断移动
            atkResult.isMoveInstruct = true;
            atkResult.moveDistance = distance == 0 ? 0 : action.moveDistance;      //0 则代表 虽然移动了 但是距离最近了  针对强制位移的技能  主动不会移动0以内
            atkResult.moveDistance = distance - action.moveDistance < 0 ? distance : action.moveDistance;   //防止走过头
            distance -= atkResult.moveDistance;
        }
    }
    /// <summary>
    /// 结算伤害生效
    /// </summary>
    public void takeEffectAttackResult()
    {
        for (int i = 0; i < takeActors.Count; i++)
        {
            //计算伤害
            if (atkResult.isHit)
                hitTakeEffect(i);
            //叠buff
            if (atkResult.isBuff)
                buffTakeEffect(i);
            //计算治疗
            if (atkResult.iscure)
                cureTakeEffect(i);
        }
    }
    public void hitTakeEffect(int i)
    {
        int hitfin = 0;
        if (atkResult.isSpecial)
        {
            foreach (int k in atkResult.specialCount[i])
                hitfin += k;
        }
        hitfin += atkResult.hitNum[i];
        if (takeActors[i].hitCurPhysical(hitfin))
        {
            atkResult.willDeadActor.Add(i);
        }
    }
    public void buffTakeEffect(int i)
    {

    }
    public void cureTakeEffect(int i)
    {
        takeActors[i].hitCurPhysical(-atkResult.cureNum[i]);
    }


    public void normalAction(AnalyzeResult action)
    {
        skill = AllUnitData.Data.getSkillStaticData(action.skillID);//获取技能
        //伤害来源目标
        sourceActor = dataList[action.selfNum];
        //被伤目标
        takeActors = new List<CombatMessage>();
        foreach (int i in action.takeNum)
        {
            takeActors.Add(dataList[i]);
        }
        //结果目标 赋值
        atkResult.sourceActor = action.selfNum;
        atkResult.takenActor = action.takeNum;
        //分析技能类型
        if (skill.isDomain) territoryTypeAction();//场地类型处理
        if (skill.isBuff) stateTypeAction();//增益类型处理
        if (skill.isHit) harmTypeAction();//攻击类型处理
        if (skill.isCure) harmTypeAction();//恢复类型处理           //+++没写完
        if (skill.isProp) harmTypeAction();//道具类处理1
        //行动结算
        //settleOnceAction();
        //计算消耗
        //settleExpend(action.skillID);
        //战斗结果分析
        //settleActionEnd();
    }

    public void specialAction(AnalyzeResult action)
    {
        
    }
    //--------------------------------类型分类处理-------------------------------------------------------------------
    public void territoryTypeAction()
    {//场地类型处理

    }
    public void stateTypeAction()
    {//增益类型处理
        //分析作用范围
        //effectActorAllocation(skill[4], action);
        //执行技能
        //executeBuffEffect(skill);
    }
    public void harmTypeAction()
    {//伤害类型处理
        //执行技能
        executeHarmEffect();
    }
    //-------------------------------第二级调用  技能解析类方法------------------------------------------------------------------------------

    //执行伤害技能效果-------------------------------------------------伤害型
    public void executeHarmEffect()
    {
        //标记为伤害
        atkResult.isHit = true;
        //记录伤害类型
        atkResult.hitType = skill.damageType;
        bool holy = atkResult.hitType == 190;   //临时记录是否真伤
        //记录攻击特效
        atkResult.isSpecial = skill.isSpecialEffect;
        //----------计算攻击----------
        //获得参考属性值
        int basePro = sourceActor.getCombatParamData(skill.damageRefer);
        //计算基础伤害
        int baseDam = skill.damageMulti / 100 * basePro;
        //计算暴击
        baseDam = holy ? baseDam : baseDam * (Random.Range(0, 100) < sourceActor.Data.strike_last ? 2 : 1);
        //计算作用目标伤害
        for (int i = 0; i < takeActors.Count; i++)
        {
            //计算命中和闪避
            bool israte = holy ? holy : Random.Range(0, 100) < sourceActor.Data.hitRate_last;       //true命中
            if (israte)
            {
                israte = holy ? holy : Random.Range(0, 100) > takeActors[i].Data.dodge_last;   //true命中
                if (israte)
                {//命中
                    //计算防御  
                    int hitresult = holy ? baseDam : DataTransTool.defenceTrans(baseDam, takeActors[i].Data.defence_last);
                    //获得减伤类型数据
                    if (!holy)
                    {
                        int pat = 0;
                        if (skill.damageType == 191) pat = takeActors[i].Data.adPat_last;
                        if (skill.damageType == 192) pat = takeActors[i].Data.apPat_last;
                        //计算减伤
                        hitresult = (int)Mathf.Round(hitresult * (float)(1 - (pat / 100)));
                    }
                    //计算攻击频率
                    int[] hitnums = new int[skill.damageNum];
                    int hitper = (int)Mathf.Round(hitresult / skill.damageNum);
                    for (int k = 0; k < skill.damageNum; k++)
                    {
                        hitnums[k] = hitper;
                    }
                    atkResult.hitCount.Add(hitnums);
                    atkResult.hitNum.Add(hitresult);
                    //-----------------------------------------------
                    //计算攻击特效
                    if (skill.isSpecialEffect)
                    {
                        atkResult.isSpecial = true;
                        calculateSpecialEffect(takeActors[i]);
                    }
                    else atkResult.isSpecial = false;
                }
            }
            atkResult.isHitRare.Add(israte);
        }
    }

    /// <summary>
    /// 计算攻击特效方法（自动）
    /// </summary>
    /// <param name="target">受害人</param>
    public void calculateSpecialEffect(CombatMessage target)
    {
        List<int> sphit = new List<int>();    //特效次数
        List<int> sptyp = new List<int>();    //特效类型
        foreach (var abnormal in sourceActor.Abnormal)
        {
            //判断特效
            if (!abnormal.isSpecial) continue;
            //记录特效参考值   计算伤害
            int refer;
            if (abnormal.isSelf)
            {
                refer = sourceActor.getCombatParamData(abnormal.effectRefer);
            }
            else
            {
                refer = target.getCombatParamData(abnormal.effectRefer);
            }
            if (abnormal.effectRefer == 150)
            {
                refer = atkResult.hitNum[atkResult.hitNum.Count - 1];
            }
            refer = (int)Mathf.Round(abnormal.effectHitMulti / 100 * refer);
            //计算减伤
            if (abnormal.effectType != 190)
            {
                DataTransTool.defenceTrans(refer, target.Data.defence_last);
                int pat = 0;
                if (skill.damageType == 191) pat = target.Data.adPat_last;
                if (skill.damageType == 192) pat = target.Data.apPat_last;
                refer = (int)Mathf.Floor(refer * (float)(1 - (pat / 100)));
            }
            //得到伤害  赋值
            sphit.Add(refer);
            //记录伤害类型
            sptyp.Add(abnormal.effectType);
        }
        //换算  赋值结果
        int num = sphit.Count;
        if (num > 0)
        {
            int[] hitsp = new int[num];
            int[] typsp = new int[num];
            for (int i = 0; i < num; i++)
            {
                hitsp[i] = sphit[i];
                typsp[i] = sptyp[i];
            }
            atkResult.specialCount.Add(hitsp);
            atkResult.specialType.Add(typsp);
        }
    }
    //=======================================       回合处理器       ============================
    public wholeRoundData roundAnalyzeAction()
    {
        int count = 0;
        void runActor()
        {
            //避免多层套for
            if (count >= dataList.Count) return;
            var abactor = dataList[count];
            if (abactor.IsDead)
            {
                runActor();
                return;
            }
            SettleRoundActor actor = new SettleRoundActor();
            actor.index = count;
            int oneSettleHit = 0;   //记录结算伤害
            int oneSettleCure = 0;  //记录结算回复
            //计算buff伤害 倒序
            for (int i = abactor.Abnormal.Count - 1; i >= 0; i--)
            {
                abnormalState abstate = abactor.Abnormal[i];
                //结算伤害类型
                if (abstate.isSettleHit)
                {
                    int finHit = (int)Mathf.Floor((float)abstate.effectHitMulti / 100) * abstate.effectReferNum;
                    finHit = DataTransTool.defenceTrans(finHit, abactor.Data.defence_last);
                    if (abstate.effectType != 190)
                    {
                        int pat = 0;
                        if (abstate.effectType == 191) pat = abactor.Data.adPat_last;
                        if (abstate.effectType == 192) pat = abactor.Data.apPat_last;
                        //计算减伤
                        finHit = (int)Mathf.Round(finHit * (float)(1 - (pat / 100)));
                    }
                    //录入最终伤害
                    actor.specialNumber.Add(finHit);
                    actor.specialType.Add(abstate.effectTypeShow);
                    oneSettleHit += finHit;
                }
                //计算回合结束治疗
                if (abstate.isSettleCure)
                {
                    int fincure = 0;
                    if (abstate.effectRefer != 0)
                    {
                        //有动态参考值
                        float ability = abactor.getCombatParamData(abstate.effectRefer);
                        fincure = (int)Mathf.Floor(abstate.effectHitMulti / 100 * ability);
                    }
                    else
                    {
                        fincure = (int)Mathf.Floor((float)abstate.effectHitMulti / 100) * abstate.effectReferNum;
                    }
                    //录入最终数值
                    actor.specialNumber.Add(fincure);
                    actor.specialType.Add(abstate.effectTypeShow);
                    oneSettleCure += fincure;
                }
                //计算 cd
                if (abstate.isBuff)
                {
                    abstate.round--;
                    if (abstate.round > 0)
                        actor.settleBuffExist.Add(abstate);
                }
            }
            //结算属性
            abactor.Abnormal = actor.settleBuffExist;
            //结算伤害 判断死亡
            if (abactor.hitCurPhysical(oneSettleHit - oneSettleCure))
            {
                actor.isRoundDead = true;
                abactor.IsDead = true;
            }
            else
            {
                abactor.paddingData();
            }
            //记录单个个体 回合结算数据
            roundData.settleActors.Add(actor);
            count++;
            runActor();
        }
        //计算伤害
        runActor();
        //判断死亡
        //计算buff生效
        //判断死亡
        //buff回合刷新
        //返回动画
        return roundData;
    }
    //目标击败
    public void settleActorDead(CombatMessage actor)
    {
        for (int i = 0; i < dataList.Count; i++)
        {
            if (dataList[i].Name == actor.Name)
            {
                //增加战利品
                if (actor.Name != PLAYER) comulativeReword(actor);
            }
        }
    }
    //----------------------------外调------------------------------------
    //判断游戏是否继续
    public bool checkCombatContinue()
    {
        bool isexit = false;
        foreach (var item in dataList)
        {
            if (item.Name == PLAYER)
            {
                if (item.IsDead) return false;
            }
            else
            {
                if (!item.IsDead) isexit = true;
            }
        }
        return isexit;
    }
    //判断输赢
    public bool checkCombatResult()
    {
        foreach (var item in dataList)
        {
            if (item.Name == PLAYER && item.IsDead) return false;
            else return true;//如果玩家存活  则玩家胜利
        }
        Debug.LogError("战斗结算错误!");
        return true;
    }
    //------------------------------------------------------------计算累积奖励--------------------------------
    public void comulativeReword(CombatMessage actor)
    {
        int num = 0;    //最终个数
        UnitSpoilStaticData spoilData = AllUnitData.Data.getJsonData<UnitSpoilStaticData>("allSpoilData", actor.Data.id);
        //计算数量
        int tem = Random.Range(0, 1000);
        int ratherNum = 0;
        for (int i = spoilData.awardNum.Length - 1; i > 0; i--)
        {
            ratherNum += spoilData.awardNum[i];
            if (tem < ratherNum)
            {
                tem = i + 1;
                break;
            }
        }
        //逐个计算概率
        for (int i = 0; i < tem; i++)
        {
            //添加结果
            int rewordID = randomReword(actor.Data.id, spoilData);
            spoils.spoils.Add(rewordID);
            Debug.Log("【掉落物品id】" + rewordID);
        }
        //给钱
        spoils.coinType = spoilData.coinType;
        //计算金币      (奖励值*浮动值 * 等级 * 等级提升倍率)
        spoils.coins = (int)Mathf.Floor(spoilData.coinNum * (1 - (float)Random.Range(-spoilData.coinFloat, spoilData.coinFloat) / 10) * spoilData.coinLevelMulti * actor.Data.level);
        Debug.Log("【钱】" + spoils.coins);
    }
    public int randomReword(int id, UnitSpoilStaticData spoil)
    {//从可获得物品中随机一个(有保底)  调该方法时意味着比得一个
        int tem = Random.Range(0, 1000);
        int ratherNum = 0;
        for (int i = 0; i < spoil.mgSpoil.Count; i++)
        {
            ratherNum += spoil.mgSpoil[i];
            if (tem < ratherNum)
            {
                return spoil.spoilItems[i];
            }
        }
        //没有返回则说明没抽中  则进行保底抽奖  目前设置两个槽位  如果东西少则两个槽赋同一个id
        tem = Random.Range(0, 2);
        return tem == 0 ? spoil.minum1 : spoil.minum2;
    }

    
}
