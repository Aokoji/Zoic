using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatMessage 
{
    public CombatMessage()
    {
        UnitData = new Dictionary<string, int>();
        UnitData.Add("attack",attack);
    }
    private string name;
    private GameObject prefab;
    private GameObject iconActor;
    private string rank;   //0,1,2,3,4
    private int level;
    private int maxHP;
    private int maxMp;
    private int attack;
    private int speed;
    private int defence;

    private bool isPlayer;
    private float curSpeed;
    private Dictionary<string, int> unitData;

    public float CurSpeed { get => curSpeed; set => curSpeed = value; }
    public int Speed { get => speed; set => speed = value; }
    public string Name { get => name; set => name = value; }
    public GameObject IconActor { get => iconActor; set => iconActor = value; }
    public int MaxHP { get => maxHP; set => maxHP = value; }
    public int MaxMp { get => maxMp; set => maxMp = value; }
    public int Attack { get => attack; set => attack = value; }
    public Dictionary<string, int> UnitData { get => unitData; set => unitData = value; }

    //技能
}
