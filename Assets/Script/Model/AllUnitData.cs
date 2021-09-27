using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;

public class AllUnitData:DDOLData<AllUnitData>
{/*
    private static AllUnitData unitData = new AllUnitData();
    public static AllUnitData Data
    {
        get {
            return unitData;
        }
    }*/
    /*
     * 待更改
     * 所有csv文件均作为基本数据文件  作为参考基值
     * 检测json文件存在，如果没有json记录  则读取csv文件并做json存储
     * 每当修改csv文件参数时  需要手动删除生成的json文件  让加载时重新读取达到刷新的目的
     */
    private JsonReadToolTest jsonRead;
    
    /// <summary>
    /// 加载静态数据(全部读取,不包括存档)  (游戏启动自动调用)
    /// </summary>
    public void loadData()
    {   //读取配置
        jsonRead = new JsonReadToolTest();
        jsonRead.readAllJsonData();
    }

    //外部调用
    /// <summary>
    /// 获取静态数据参考值
    /// </summary>
    /// <typeparam name="T">返回值类型</typeparam>
    /// <param name="name">存变量参数名(要输对)</param>
    /// <param name="num">id</param>
    /// <returns></returns>
    public T getJsonData<T>(string name, int num)
    {
        return jsonRead.getJsonData<T>(name, num);
    }

    /// <summary>
    /// 获取技能 单独分支方法  （便捷）
    /// </summary>
    public SkillStaticData getSkillStaticData(int num)
    {
        return jsonRead.getJsonData<SkillStaticData>("allSkillData", num);
    }

    //外部调用
    /// <summary>
    /// 获取战时数据编号对应属性
    /// </summary>
    /// <param name="unit">作战单位</param>
    /// <param name="i">属性编号</param>
    /// <returns></returns>
    public int getCombatParamData(combatUnitProperty unit,int i)
    {
        switch (i)
        {
            case 101:return unit.physical_last;
            case 102:return unit.curHp;
            case 103:return unit.vigor_last;
            case 104:return unit.curMp;
            case 105:return unit.force_last;
            case 106:return unit.agility_last;
            case 107:return unit.wisdom_last;
            case 108:return unit.attack_last;
            case 109:return unit.speed_last;
            case 110:return unit.defence_last;
            case 111:return unit.strike_last;
            case 112:return unit.dodge_last;
            case 113:return unit.hitRate_last;
        }
        return 0;
    }
    //战斗数据  buff计算赋值
    public combatUnitProperty combatPropertyChange(combatUnitProperty unit,int index,int multi,int num)
    {
        switch (index)
        {
            case 101:unit.physical_last = (int)Mathf.Floor(unit.physical_last * (1 - multi)) + num;
                if (unit.curHp > unit.physical_last) unit.curHp = unit.physical_last;
                break;
            case 102:break;
            case 103:unit.vigor_last = (int)Mathf.Floor(unit.vigor_last * (1 - multi)) + num;
                if (unit.curMp > unit.vigor_last) unit.curMp = unit.vigor_last;
                break;
            case 104:break;
            case 105:unit.force_last = (int)Mathf.Floor(unit.force_last * (1 - multi)) + num;break;
            case 106:unit.agility_last = (int)Mathf.Floor(unit.agility_last * (1 - multi)) + num;break;
            case 107:unit.wisdom_last = (int)Mathf.Floor(unit.wisdom_last * (1 - multi)) + num;break;
            case 108:unit.attack_last = (int)Mathf.Floor(unit.attack_last * (1 - multi)) + num;break;
            case 109:unit.speed_last = (int)Mathf.Floor(unit.speed_last * (1 - multi)) + num;break;
            case 110:unit.defence_last = (int)Mathf.Floor(unit.defence_last * (1 - multi)) + num;break;
            case 111:unit.strike_last = unit.strike_last + multi + num;break;
            case 112:unit.dodge_last = unit.dodge_last + multi + num;break;
            case 113:unit.hitRate_last = unit.hitRate_last + multi + num;break;
        }
        return unit;
    }

    /// <summary>
    /// 玩家装备数据赋值
    /// </summary>
    public playerParam playerEquipCalculate(playerParam player,int id)
    {
        if(id==0)
            return player;
        EquipStaticData eqp = getJsonData<EquipStaticData>("allEquipData", id);
        player.physical_equip += eqp.physicalAdd;
        player.vigor_equip += eqp.vigorAdd;
        player.attack_equip += eqp.attackAdd;
        player.speed_equip += eqp.speedAdd;
        player.defence_equip += eqp.defenceAdd;
        player.strike_equip += eqp.strikeAdd;
        player.dodge_equip += eqp.dodgeAdd;
        player.hitRate_equip += eqp.hitRateAdd;
        player.adPat_equip += eqp.adPatAdd;
        player.apPat_equip += eqp.apPatAdd;
        player.force_equip += eqp.forceAdd;
        player.wisdom_equip += eqp.wisdomAdd;
        player.agility_equip += eqp.agilityAdd;
        return player;
    }


    //-----------------------------------------json 存储测试
    public static void testLoad()
    {
        unitMessage m = new unitMessage();
        m.name = "aaa";
        m.id = 2;
        //存json
        string json=JsonUtility.ToJson(m);
        byte[] js = Encoding.ASCII.GetBytes(json.ToCharArray());
        File.WriteAllBytes(Application.streamingAssetsPath + "Json.json", js);
        Debug.Log(json);
        //读json
        if (!File.Exists(Application.streamingAssetsPath + "Json.json"))
        {
            return; //不存在
        }
        byte[] jsbt = File.ReadAllBytes(Application.streamingAssetsPath + "Json.json");
        string read = Encoding.ASCII.GetString(jsbt);
        unitMessage a = JsonUtility.FromJson<unitMessage>(read);
        Debug.Log(a.id + a.name);
    }

    //--------------------------------------------------------测试部分-------------------------
    static string unitPath = "";
    public static void loadtext()   //写文件1   测试
    {
        StreamWriter sw;
        if (File.Exists(unitPath))
        {
            sw = File.AppendText(unitPath);
        }
        else
        {
            sw = File.CreateText(unitPath);
        }
        sw.WriteLine("dsafg");
        sw.Close();
        sw.Dispose();
        Debug.Log("write success!");
    }
    public static void changeText()      //写文件2   测试
    {
        StreamWriter sw;
        FileInfo file = new FileInfo(unitPath);
        sw = file.AppendText();
        sw.WriteLine("dsafg");
        sw.Close();
        sw.Dispose();
        Debug.Log("write success!");
    }
    public static void readText()       //读文件1  测试
    {
        StreamReader sr;
        string apath = "Assets/Resources/Data//saveMessage.csv";
        sr = File.OpenText(apath);
        List<string[]> list = new List<string[]>();
        string str;
        while ((str = sr.ReadLine()) != null)
        {
            list.Add(str.Split(','));
            Debug.Log("once");
        }
        foreach (var j in list)
        {
            foreach (var i in j)
                Debug.Log(i.ToString());
        }
        sr.Close();
        sr.Dispose();
        Debug.Log("end");
    }
}
public class unitMessage
{//测试空类  没用
    public int id;
    public string name;
}

