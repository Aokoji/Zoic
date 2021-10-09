using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatMessage
{
    //--------以下为内部引用量
    private string name;
    private string iconName;
    private combatUnitProperty data = new combatUnitProperty();

    private float curSpeed; //这个记录跑条位置   不进行手动更改
    public string originalState = "";   //初始状态描述

    //---------以下为外部引用量
    public int id;
    public int type;
    private int numID;          //战斗中list序号  玩家默认为0
    private GameObject prefab;      //战斗人物实体
    private GameObject showActor;       //换图的组件image
    private GameObject iconActor;   //战斗人物头像
    private List<abnormalState> abnormal=new List<abnormalState>();     //状态（buff/debuff）
    private List<specialAttackExtra> atkExtra = new List<specialAttackExtra>();     //+++这个属性待定
    private skillSave skillData = new skillSave();  //技能

    private bool isPlayer;
    private bool isDead;
    //private Dictionary<int, skillDetail> skillData;//技能id  等级
    private EnemyActionAnalyse analyse = new EnemyActionAnalyse();//ai分析器 （玩家不需要）

    private int attackID;   //平Aid
    private int skillOdds;  //技能概率  (忘了是啥了)

    public float CurSpeed { get => curSpeed; set => curSpeed = value; }
    public string Name1 { get => name; set => name = value; }
    public GameObject IconActor { get => iconActor; set => iconActor = value; }
    public bool IsPlayer { get => isPlayer; set => isPlayer = value; }
    public GameObject Prefab { get => prefab; set => prefab = value; }
    public int AttackID { get => attackID; set => attackID = value; }
    public List<abnormalState> Abnormal { get => abnormal; set => abnormal = value; }
    public List<specialAttackExtra> AtkExtra { get => atkExtra; set => atkExtra = value; }
    public skillSave SkillData { get => skillData; set => skillData = value; }
    public bool IsDead { get => isDead; set => isDead = value; }
    public EnemyActionAnalyse Analyse { get => analyse; set => analyse = value; }
    public string IconName { get => iconName; set => iconName = value; }
    public int NumID { get => numID; set => numID = value; }
    public int SkillOdds { get => skillOdds; set => skillOdds = value; }
    public combatUnitProperty Data { get => data; set => data = value; }
    public GameObject ShowActor { get => showActor; set => showActor = value; }

    public void initData()
    {
        //初始化技能
        foreach (var sk in skillData.skillHold)
            sk.runDown = 0;
        paddingData();
    }
    /// <summary>
    /// 赋完值后 拉栓 计算属性赋最终值
    /// </summary>
    public void paddingData()
    {
        int force = data.force_base + (isPlayer ? 0: data.force_point);
        int wisdom = data.wisdom_base + (isPlayer ? 0: data.wisdom_point);
        int agility = data.agility_base + (isPlayer ? 0: data.agility_point);
        data.physical_last = data.physical_base;
        data.vigor_last = data.vigor_base;
        data.strike_last = data.strike_base;
        data.dodge_last = data.dodge_base;
        data.adPat_last = data.adPat_base;
        data.apPat_last = data.apPat_base;
        data.hitRate_last = data.hitRate_base;
        data.force_last = force;
        data.agility_last = agility;
        data.wisdom_last = wisdom;
        //计算buff增减益
        foreach (var abs in abnormal)
        {
            if (abs.isBuff)
            {
                data=AllUnitData.Data.combatPropertyChange(data, abs.effectAbility, abs.effectMulti, abs.effectConstant);
            }
        }
        data.attack_last = data.attack_base + DataTransTool.forceTransAttack(data.force_last);
        data.defence_last = data.defence_base + DataTransTool.agilityTransDefence(data.agility_last);
        data.speed_last = data.speed_base + DataTransTool.agilityTransSpeed(data.agility_last);
    }
    /// <summary>
    /// 获取该单位  数值编号对应属性
    /// </summary>
    public int getCombatParamData(int i)
    {
        return AllUnitData.Data.getCombatParamData(data, i);
    }
    /// <summary>
    /// 掉血
    /// </summary>
    /// <param name="hit">值</param>
    public bool hitCurPhysical(int hit)
    {
        data.curHp -= hit;
        if(data.curHp<=0)
        {
            IsDead = true;
            return true;
        }
        if (data.curHp > data.physical_last)
        {
            data.curHp = data.physical_last;
        }
        return false;
    }
    /// <summary>
    ///播完攻击效果后 显示的掉血  1.5s
    /// </summary>
    public void showPhysicalChange(bool ishit)
    {
        prefab.GetComponent<CombatActorItem>().changePhysicalLine(ishit,(float)data.curHp/data.physical_last);
    }

}

public class abnormalState
{//状态表  buff  debuff        (普通buff)
    public int id;  //异常状态id(包括正面状态和攻击特效)
    public string name;

    public bool isSpecial;
    public bool isTarget;
    public int round;// 剩余回合
    public int resCount;    //剩余次数（有次数优先次数）不使用为-1
    public bool isGain;         //是否增益（用来显示说明）
    public int abnormalLogo;        //异常参数种类图标  (攻击，力量，敏捷，智力，暴击，闪避，命中，速度，防御，逃跑)（中毒，流血，灼烧）

    public int effectAbility;       //影响属性编号
    public int effectMulti;         //影响幅度（基础）(百分比或固定  目前百分比)(增减益)
    public int effectConstant;      //固定值

    public bool isSettleHit;      //是否结算伤害
    public bool isSettleCure;   //是否结算治疗
    public bool isBuff;     //是否buff    (区分攻击特效和结算伤害)
    
    public bool isSelf;     //特殊单独计算参考目标取值（参考自身还是目标）
    public int effectType;      //伤害类型(计算减伤用)
    public int effectTypeShow;  //伤害类型（显示用）
    public int effectRefer;         //影响效果参考属性编号（攻击特效和异常伤害状态）(攻击特效取 effectHitMulti  计算伤害 毒 治疗)       (比如参考当前生命)
    public int effectHitMulti;      //伤害取值      （目前是百分比）
    public int effectReferNum;         //影响效果参考值(释放时记录) (针对例 毒)
}
public class specialAttackExtra
{//特殊攻击附加
    public int id;//特殊附加编号
    public int target;//作用范围
    public int refer;   //参考
    public int multi;   //倍率
    public int constant;    //固定值参数
    public int round;
    public int type;            //下标2类型
    public int actOn;   //作用于
    public int specialRefer;   //特殊参考
}

//战斗用人物属性细分
public class combatUnitProperty
{
    public int id;
    public int level;      //所有属性基于等级的比例值  （曲线值） 部分角色越高级越强
    // 基础值                          （敌人需要赋值基础值和浮动天赋 最终计算面板）
    public int physical_base;    
    public int vigor_base;      
    public int attack_base;       
    public int speed_base;        
    public int strike_base;        
    public int dodge_base;    
    public int hitRate_base;       
    public int defence_base;       
    public int adPat_base;         
    public int apPat_base;      
    public int force_base;      
    public int wisdom_base; 
    public int agility_base;
    //天赋
    public int force_point;      //力量
    public int wisdom_point; //智力
    public int agility_point;    //敏捷
    //最终面板          (玩家也要付基础值 但不赋值天赋)
    public int physical_last;
    public int vigor_last;
    public int attack_last;
    public int speed_last;
    public int strike_last;
    public int dodge_last;
    public int hitRate_last;
    public int defence_last;
    public int adPat_last;
    public int apPat_last;
    public int force_last;
    public int wisdom_last;
    public int agility_last;
    //战斗可变动值
    public int curHp;
    public int curMp;
}