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

    //---------以下为外部引用量
    private int numID;          //战斗中list序号  玩家默认为0
    private GameObject prefab;      //战斗人物实体
    private GameObject iconActor;   //战斗人物头像
    private List<abnormalState> abnormal=new List<abnormalState>();     //状态（buff/debuff）
    private List<specialAttackExtra> atkExtra = new List<specialAttackExtra>();     //+++这个属性待定
    private List<skillSave> skillData = new List<skillSave>();  //技能

    private bool isPlayer;
    private bool isDead;
    //private Dictionary<int, skillDetail> skillData;//技能id  等级
    private EnemyActionAnalyse analyse = new EnemyActionAnalyse();//ai分析器 （玩家不需要）

    private int attackID;   //平Aid
    private int skillOdds;  //技能概率  (忘了是啥了)

    public float CurSpeed { get => curSpeed; set => curSpeed = value; }
    public string Name { get => name; set => name = value; }
    public GameObject IconActor { get => iconActor; set => iconActor = value; }
    public bool IsPlayer { get => isPlayer; set => isPlayer = value; }
    public GameObject Prefab { get => prefab; set => prefab = value; }
    public int AttackID { get => attackID; set => attackID = value; }
    public List<abnormalState> Abnormal { get => abnormal; set => abnormal = value; }
    public List<specialAttackExtra> AtkExtra { get => atkExtra; set => atkExtra = value; }
    public List<skillSave> SkillData { get => skillData; set => skillData = value; }
    public bool IsDead { get => isDead; set => isDead = value; }
    public EnemyActionAnalyse Analyse { get => analyse; set => analyse = value; }
    public string IconName { get => iconName; set => iconName = value; }
    public int NumID { get => numID; set => numID = value; }
    public int SkillOdds { get => skillOdds; set => skillOdds = value; }
    public combatUnitProperty Data { get => data; set => data = value; }

    /// <summary>
    /// 赋完值后 拉栓
    /// </summary>
    public void paddingData()
    {

    }
    /// <summary>
    /// 获取该单位  数值编号对应属性
    /// </summary>
    public int getCombatParamData(int i)
    {
        return AllUnitData.Data.getCombatParamData(data, i);
    }
}

public class abnormalState
{//状态表  buff  debuff        (普通buff)
    public int id;  //异常状态id(包括正面状态)

    public bool isSpecial;
    public bool isTarget;

    public int round;// 剩余回合
    public int refer;//能力参数
    public int perHit;//每回合数值(伤害/回复)
    public int effectAbility;   //影响能力
    public int effectMulti;     //影响参数
    public bool isEffect;   //是否已作用
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
    //最终面板          (玩家直接赋值最终面板)
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
    private int curHp;
    private int curMp;
}