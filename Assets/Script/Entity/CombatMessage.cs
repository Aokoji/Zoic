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
        UnitData.Add("maxHp", maxHp);
        UnitData.Add("maxMp", maxMp);
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

    private float curSpeed;
    private bool isPlayer;
    private Dictionary<string, int> unitData;

    private int attackID;

    public float CurSpeed { get => curSpeed; set => curSpeed = value; }
    public string Name { get => name; set => name = value; }
    public GameObject IconActor { get => iconActor; set => iconActor = value; }
    public Dictionary<string, int> UnitData { get => unitData; set => unitData = value; }
    public bool IsPlayer { get => isPlayer; set => isPlayer = value; }
    public GameObject Prefab { get => prefab; set => prefab = value; }
    public int AttackID { get => attackID; set => attackID = value; }

    //技能
}
