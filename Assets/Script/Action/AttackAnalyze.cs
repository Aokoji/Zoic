using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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


    public void initData(List<CombatMessage> data)
    {
        dataList = data;
        spoils = new spoilsResult();
    }
    public wholeRoundData doAction(AnalyzeResult action)
    {
        //初始化技能存储、战斗返回数据
        skill = new SkillStaticData();
        atkResult = new AttackResultData();
        roundData = new wholeRoundData();
        if (action.isNormalAtk)
            normalAction(action);
        else
            specialAction(action);
        if (atkResult == null) Debug.LogError("战斗数据赋值错误");
        else roundAnalyzeAction(atkResult);
        if (roundData == null) Debug.LogError("回合数据赋值错误");
        return roundData;
    }
    //----------------------------------总处理方法-------------------------------------------------------------------
    //------------输入  AnalyzeResult  类   操作的内容类   只处理当次动作产生的数值
    //------------输出  AttackResultData    类   统合该次行动的数据  返回给回合结算控制器

    //普通的战斗处理
    public AttackResultData normalAction(AnalyzeResult action)
    {
        skill = AllUnitData.Data.getJsonData<SkillStaticData>("allSkillData",action.skillID);//获取技能
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
        //----------计算攻击----------
        //获得参考属性值
        int basePro =sourceActor.getCombatParamData(skill.damageRefer);
        //计算基础伤害
        int baseDam = skill.damageMulti / 100 * basePro;
        //计算暴击
        baseDam = baseDam * (Random.Range(0,100)<sourceActor.Data.strike_last?2:1);
        //计算作用目标伤害
        for(int i= 0;i<takeActors.Count;i++)
        {
            //计算命中和闪避
            bool israte = Random.Range(0, 100) < sourceActor.Data.hitRate_last;       //true命中
            if (israte)
            {
                israte = Random.Range(0, 100) > takeActors[i].Data.dodge_last;   //true命中
                if (israte)
                {//命中
                    //计算防御  
                    int hitresult = DataTransTool.defenceTrans(baseDam,takeActors[i].Data.defence_last);
                    //获得减伤类型数据
                    int pat = skill.damageType == 0 ? takeActors[i].Data.adPat_last : takeActors[i].Data.apPat_last;
                    //计算减伤
                    hitresult = (int)Mathf.Round(hitresult * (float)(1 - (pat / 100)));
                    //计算攻击频率
                    int[] hitnums = new int[skill.damageNum];
                    int hitper = (int)Mathf.Round(hitresult / skill.damageNum);
                    for (int k=0;k< skill.damageNum;k++)
                    {
                        k = hitper;
                    }
                    atkResult.hitCount.Add(hitnums);
                }
            }
            atkResult.isHitRare.Add(israte);
        }
    }
    //=======================================       回合处理器       ============================
    private void roundAnalyzeAction(AttackResultData result)
    {
        //计算伤害
        //计算攻击特效
        foreach(var abState in dataList[result.sourceActor].Abnormal)
        {
            //判断是否攻击特效
            if (abState.isSpecial)
            {

            }
        }
        //判断死亡
        //计算buff生效
        //判断死亡
        //buff回合刷新
        //返回动画
    }












    //目标击败
    public void settleActorDead(CombatMessage actor)
    {
        for (int i = 0; i < dataList.Count; i++)
        {
            if (dataList[i].Name == actor.Name)
            {
                //增加战利品
                //if (actor.Name != PLAYER) comulativeReword(actor.Data.id);
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