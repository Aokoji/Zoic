using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatMessage
{
    public CombatMessage()
    {
        UnitData = new Dictionary<string, int>();
        UnitData.Add("id", id);
        UnitData.Add("physical", physical);
        UnitData.Add("vigor", vigor);
        UnitData.Add("attack", attack);
        UnitData.Add("speed", speed);
        UnitData.Add("type", type);
        UnitData.Add("adPat", adPat);
        UnitData.Add("apPat", apPat);
        UnitData.Add("strike", strike);
        UnitData.Add("dodge", dodge);
        UnitData.Add("curHp", curHp);
        UnitData.Add("curMp", curMp);
    }
    //--------以下为内部引用量
    private int id;      //取数据的列表id
    private string name;
    private int physical;
    private int vigor;
    private int attack;
    private int speed;
    private int type;
    private int adPat;
    private int apPat;
    private int strike;
    private int dodge;
    private int[] state;

    private int maxHp;
    private int maxMp;
    private int curHp;
    private int curMp;

    //---------以下为外部引用量
    private GameObject prefab;
    private GameObject iconActor;
    private int level;
    private List<abnormalState> abnormal=new List<abnormalState>();
    private List<specialAttackExtra> atkExtra = new List<specialAttackExtra>();

    private float curSpeed;
    private bool isPlayer;
    private bool isDead;
    private Dictionary<string, int> unitData;

    private int attackID;

    public float CurSpeed { get => curSpeed; set => curSpeed = value; }
    public string Name { get => name; set => name = value; }
    public GameObject IconActor { get => iconActor; set => iconActor = value; }
    public Dictionary<string, int> UnitData { get => unitData; set => unitData = value; }
    public bool IsPlayer { get => isPlayer; set => isPlayer = value; }
    public GameObject Prefab { get => prefab; set => prefab = value; }
    public int AttackID { get => attackID; set => attackID = value; }
    public List<abnormalState> Abnormal { get => abnormal; set => abnormal = value; }
    public List<specialAttackExtra> AtkExtra { get => atkExtra; set => atkExtra = value; }
    public bool IsDead { get => isDead; set => isDead = value; }

    //技能
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
}
