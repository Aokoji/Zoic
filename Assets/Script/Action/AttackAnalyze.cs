using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
//战斗分析器（新）+++待完成
public class AttackAnalyze : MonoBehaviour
{
    private List<CombatMessage> dataList;       //总数据
    private AttackResultData atkResult;         //战斗数据**
    private wholeRoundData roundData;       //回合数据**
    private SkillStaticData skill;  //暂存技能数据
    private spoilsResult spoils;    //战利品
    CombatMessage sourceActor;          //来源
    List<CombatMessage> takeActors; //目标
    public bool isFaild = false;
    string PLAYER = "player";   //玩家name

    public int distance;


    public void initData(List<CombatMessage> data)
    {
        dataList = data;
        spoils = new spoilsResult();
        distance = 2;   //+++初始距离为2(根据怪物属性 变化)
    }
    public AttackResultData doAction(AnalyzeResult action)
    {
        if (action.isMoveInstruct)      //有move指令
        {
            //判断移动
            atkResult.isMoveInstruct = true;
            atkResult.moveDistance = distance == 0?0: action.moveDistance;      //0 则代表 虽然移动了 但是距离最近了  针对强制位移的技能  主动不会移动0以内
            atkResult.moveDistance = distance - action.moveDistance < 0 ? distance : action.moveDistance;   //防止走过头
            distance -= atkResult.moveDistance;
            //移动没有后续操作  则返回
            if (!action.isExtraHit) return atkResult;
        }
        //初始化技能存储、战斗返回数据
        skill = new SkillStaticData();
        atkResult = new AttackResultData();
        roundData = new wholeRoundData();
        if (action.isNormalAtk)
            normalAction(action);
        else
            specialAction(action);
        if (atkResult == null) Debug.LogError("战斗数据赋值错误");
        return takeEffectAttackResult();
    }

    //结算伤害  (生效)
    public AttackResultData takeEffectAttackResult()
    {
        for(int i = 0; i < takeActors.Count; i++)
        {
            //计算伤害
            if (atkResult.isHit)
            {
                int hitfin=0;
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
            //叠buff
            if (atkResult.isBuff)
            {

            }
            //计算治疗
            if (atkResult.iscure)
            {
                takeActors[i].hitCurPhysical(-atkResult.cureNum[i]);
            }
        }
        return atkResult;
    }
    //----------------------------------总处理方法-------------------------------------------------------------------
    //------------输入  AnalyzeResult  类   操作的内容类   只处理当次动作产生的数值
    //------------输出  AttackResultData    类   统合该次行动的数据  返回给回合结算控制器

    //普通的战斗处理
    public AttackResultData normalAction(AnalyzeResult action)
    {
        skill = AllUnitData.Data.getSkillStaticData(action.skillID);//获取技能
        //伤害来源目标
        sourceActor = dataList[action.selfNum];     
        //被伤目标
        takeActors = new List<CombatMessage>();  
        foreach(int i in action.takeNum)
        {
            takeActors.Add(dataList[i]);
        }
        //结果目标 赋值
        atkResult.sourceActor = action.selfNum;
        atkResult.takenActor = action.takeNum;
        //分析技能类型
        if (skill.isDomain) territoryTypeAction(action);//场地类型处理
        if (skill.isBuff) stateTypeAction(action);//增益类型处理
        if (skill.isHit) harmTypeAction(action);//攻击类型处理
        if (skill.isCure) harmTypeAction(action);//恢复类型处理           //+++没写完
        if (skill.isProp) harmTypeAction(action);//道具类处理1
        //行动结算
        //settleOnceAction();
        //计算消耗
        //settleExpend(action.skillID);
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
        //分析作用范围
        //effectActorAllocation(skill[4], action);
        //执行技能
        //executeBuffEffect(skill);
    }
    private void harmTypeAction(AnalyzeResult action)
    {//伤害类型处理
        //执行技能
        executeHarmEffect();
    }
    //-------------------------------第二级调用  技能解析类方法------------------------------------------------------------------------------
    
    //执行伤害技能效果-------------------------------------------------伤害型
    private void executeHarmEffect()
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
        int basePro =sourceActor.getCombatParamData(skill.damageRefer);
        //计算基础伤害
        int baseDam = skill.damageMulti / 100 * basePro;
        //计算暴击
        baseDam =holy?baseDam:baseDam * (Random.Range(0,100)<sourceActor.Data.strike_last?2:1);
        //计算作用目标伤害
        for(int i= 0;i<takeActors.Count;i++)
        {
            //计算命中和闪避
            bool israte =holy?holy:Random.Range(0, 100) < sourceActor.Data.hitRate_last;       //true命中
            if (israte)
            {
                israte =holy?holy:Random.Range(0, 100) > takeActors[i].Data.dodge_last;   //true命中
                if (israte)
                {//命中
                    //计算防御  
                    int hitresult =holy? baseDam:DataTransTool.defenceTrans(baseDam,takeActors[i].Data.defence_last);
                    //获得减伤类型数据
                    if (!holy)
                    {
                        int pat=0;
                        if (skill.damageType == 191) pat = takeActors[i].Data.adPat_last;
                        if (skill.damageType == 192) pat = takeActors[i].Data.apPat_last;
                        //计算减伤
                        hitresult = (int)Mathf.Round(hitresult * (float)(1 - (pat / 100)));
                    }
                    //计算攻击频率
                    int[] hitnums = new int[skill.damageNum];
                    int hitper = (int)Mathf.Round(hitresult / skill.damageNum);
                    for (int k=0;k< skill.damageNum;k++)
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
            for(int i = abactor.Abnormal.Count - 1; i >= 0; i--)
            {
                abnormalState abstate = abactor.Abnormal[i];
                //结算伤害类型
                if (abstate.isSettleHit)
                {
                    int finHit=(int)Mathf.Floor((float)abstate.effectHitMulti / 100) * abstate.effectReferNum;
                    finHit = DataTransTool.defenceTrans(finHit, abactor.Data.defence_last);
                    if (abstate.effectType!=190)
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
                    if(abstate.round>0)
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

    /// <summary>
    /// 计算攻击特效方法（自动）
    /// </summary>
    /// <param name="target">受害人</param>
    private void calculateSpecialEffect(CombatMessage target)
    {
        List<int> sphit=new List<int>();    //特效次数
        List<int> sptyp=new List<int>();    //特效类型
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
            if (abnormal.effectRefer == 150) {
                refer=atkResult.hitNum[atkResult.hitNum.Count-1];
            }
            refer = (int)Mathf.Round(abnormal.effectHitMulti / 100* refer);
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


    //-------------------计算累积奖励
    private void comulativeReword(CombatMessage actor)
    {
        int num = 0;    //最终个数
        UnitSpoilStaticData spoilData = AllUnitData.Data.getJsonData<UnitSpoilStaticData>("allSpoilData", actor.Data.id);
        //计算数量
        int tem = Random.Range(0, 1000);
        int ratherNum = 0;
        for (int i =spoilData.awardNum.Length-1;i>0 ; i--)
        {
            ratherNum += spoilData.awardNum[i];
            if (tem < ratherNum)
            {
                tem = i+1;
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
        spoils.coins =(int)Mathf.Floor(spoilData.coinNum * (1 - (float)Random.Range(-spoilData.coinFloat, spoilData.coinFloat) / 10) * spoilData.coinLevelMulti * actor.Data.level);
        Debug.Log("【钱】" + spoils.coins);
    }
    private int randomReword(int id, UnitSpoilStaticData spoil)
    {//从可获得物品中随机一个(有保底)  调该方法时意味着比得一个
        int tem = Random.Range(0, 1000);
        int ratherNum = 0;
        for(int i=0;i<spoil.mgSpoil.Count;i++)
        {
            ratherNum += spoil.mgSpoil[i] ;
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