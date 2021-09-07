using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatMessage
{
    //--------以下为内部引用量
    private string name;
    private string iconName;
    private UnitTypeStaticData data;
    //战斗可变动值
    private int curHp;
    private int curMp;
    private float curSpeed; //这个记录跑条位置   不进行手动更改

    //---------以下为外部引用量
    private int level;      //所有属性基于等级的比例值  （曲线值） 部分角色越高级越强
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
    public UnitTypeStaticData Data { get => data; set => data = value; }
    public int CurHp { get => curHp; set => curHp = value; }
    public int CurMp { get => curMp; set => curMp = value; }
    public int Level { get => level; set => level = value; }
    
}

public class abnormalState
{//状态表  buff  debuff        (普通buff)
    public int id;  //异常状态id(包括正面状态)
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
