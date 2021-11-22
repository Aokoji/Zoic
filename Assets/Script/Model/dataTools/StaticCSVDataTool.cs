﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

//=====================================================================================
//-----------------------------------------     csv手动配置          ------------------------------------------------------------
public class StaticCSVDataTool
{
    //------------------------静态数据单位信息----------------------
    private string unitPath = "Assets/Resources/Data/CsvData//unitMessage.csv";//单位信息

    private string unitSkillPath = "Assets/Resources/Data/CsvData//unitSkillMessage.csv"; //单位技能对照

    private string skillPath = "Assets/Resources/Data/CsvData//skillMessage.csv";//技能基础参数信息

    private string spoilPath = "Assets/Resources/Data/CsvData//spoilMessage.csv";//单位爆率清单

    private string goodPath = "Assets/Resources/Data/CsvData//goodMessage.csv";//所有物品清单

    private string collectPath = "Assets/Resources/Data/CsvData//collectMessage.csv";//所有收集清单

    private string abnormalPath = "Assets/Resources/Data/CsvData//abnormalMessage.csv";//所有异常列表 （增益减益类型参数）

    private string equipPath = "Assets/Resources/Data/CsvData//equipMessage.csv";//所有装备列表 

    /// <summary>
    /// 物品数据  csv读取方法
    /// </summary>
    //所有单位
    public AllUnitTypeStaticData loadData_allUnitData()
    {
        AllUnitTypeStaticData data = new AllUnitTypeStaticData();
        List<string[]> list = loadCSV(unitPath);
        foreach (string[] mess in list)
        {
            UnitTypeStaticData item = new UnitTypeStaticData();
            item.id = int.Parse(mess[0]);
            item.name = mess[1];
            item.physical = int.Parse(mess[2]);
            item.vigor = int.Parse(mess[3]);
            item.attack = int.Parse(mess[4]);
            item.speed = int.Parse(mess[5]);
            item.type = int.Parse(mess[6]);
            item.adPat = int.Parse(mess[7]);
            item.apPat = int.Parse(mess[8]);
            item.strike = int.Parse(mess[9]);
            item.dodge = int.Parse(mess[10]);
            item.defence = int.Parse(mess[11]);
            item.hitRate = int.Parse(mess[12]);
            item.force= int.Parse(mess[13]);
            item.agility= int.Parse(mess[14]);
            item.wisdom= int.Parse(mess[15]);
            item.force_coefficient= int.Parse(mess[16]);
            item.agility_coefficient= int.Parse(mess[17]);
            item.wisdom_coefficient= int.Parse(mess[18]);

            data.childDic.Add(item);
        }
        return data;
    }
    //所有技能
    public AllSkillStaticData loadData_allSkillData()
    {
        AllSkillStaticData data = new AllSkillStaticData();
        List<string[]> list = loadCSV(skillPath);
        foreach (string[] mess in list)
        {
            SkillStaticData item = new SkillStaticData();
            item.id = int.Parse(mess[0]);
            item.name = mess[1];
            item.describe = mess[2];
            item.isHit = mess[3].Equals("1");
            item.isBuff = mess[4].Equals("1");
            item.isDomain = mess[5].Equals("1");
            item.isCure = mess[6].Equals("1");
            item.isProp = mess[7].Equals("1");
            item.effectType = int.Parse(mess[8]);
            item.sustainType = int.Parse(mess[9]);
            item.sustainTimeBase = int.Parse(mess[10]);
            item.sustainRefType = int.Parse(mess[11]);
            item.sustainMulti = int.Parse(mess[12]);
            item.subOdds = int.Parse(mess[13]);
            item.isFrontBuff = mess[14].Equals("1");
            item.buffId = int.Parse(mess[15]);
            item.buffRefer = int.Parse(mess[16]);
            item.buffAbility = int.Parse(mess[17]);
            item.buffConstant = int.Parse(mess[18]);
            item.damageType = int.Parse(mess[19]);
            item.isSpecialEffect = mess[20].Equals("1");
            item.damageRefer = int.Parse(mess[21]);
            item.damageMulti = int.Parse(mess[22]);
            item.damageNum = int.Parse(mess[23]);
            item.expend1 = int.Parse(mess[24]);
            item.expend2 = int.Parse(mess[25]);
            item.coolDown = int.Parse(mess[26]);
            item.runDown = int.Parse(mess[27]);
            item.ismove = mess[28].Equals("1");
            item.isfrontMove = mess[29].Equals("1");
            item.moveDistance = int.Parse(mess[30]);
            item.takeLength = int.Parse(mess[31]);
            item.animTypeTake = int.Parse(mess[32]);

            data.childDic.Add(item);
        }
        return data;
    }
    //所有技能对照
    public AllUnitSkillStaticData loadData_allUnitSkillData()
    {
        AllUnitSkillStaticData data = new AllUnitSkillStaticData();
        List<string[]> list = loadCSV(unitSkillPath);
        int beforeCount = 3;
        foreach (string[] mess in list)
        {
            UnitSkillStaticData item = new UnitSkillStaticData();
            item.id = int.Parse(mess[0]);
            item.unitId = int.Parse(mess[1]);
            item.attackNum = int.Parse(mess[2]);
            for(int i = beforeCount; i < mess.Length; i++)
            {
                item.skills.Add(int.Parse(mess[i]));
            }
            data.childDic.Add(item);
        }
        return data;
    }
    //所有爆率
    public AllUnitSpoilStaticData loadData_allSpoilData()
    {
        AllUnitSpoilStaticData data = new AllUnitSpoilStaticData();
        List<string[]> list = loadCSV(spoilPath);
        foreach (string[] mess in list)
        {
            UnitSpoilStaticData item = new UnitSpoilStaticData();
            item.id = int.Parse(mess[0]);

            data.childDic.Add(item);
        }
        return data;
    }
    //所有物品
    public AllGoodStaticData loadData_allGoodData()
    {
        AllGoodStaticData data = new AllGoodStaticData();
        List<string[]> list=loadCSV(goodPath);
        foreach(string[] mess in list)
        {
            GoodStaticData item = new GoodStaticData();
            item.id = int.Parse(mess[0]);
            item.name = mess[1];
            item.describe= mess[2];
            item.bagType= int.Parse(mess[3]);

            data.childDic.Add(item);
        }
        return data;
    }
    //所有收集点
    public AllCollectTypeStaticData loadData_allCollcetData()
    {
        AllCollectTypeStaticData data = new AllCollectTypeStaticData();
        List<string[]> list = loadCSV(collectPath);
        foreach (string[] mess in list)
        {
            CollectTypeStaticData item = new CollectTypeStaticData();
            item.id = int.Parse(mess[0]);

            data.childDic.Add(item);
        }
        return data;
    }
    //所有状态
    public AllAbnormalStaticData loadData_allAbnormalData()
    {
        AllAbnormalStaticData data = new AllAbnormalStaticData();
        List<string[]> list = loadCSV(abnormalPath);
        foreach (string[] mess in list)
        {
            AbnormalStaticData item = new AbnormalStaticData();
            item.id = int.Parse(mess[0]);

            data.childDic.Add(item);
        }
        return data;
    }
    //所有装备
    public AllEquipStaticData loadData_allEquipData()
    {
        AllEquipStaticData data = new AllEquipStaticData();
        List<string[]> list = loadCSV(equipPath);
        foreach (string[] mess in list)
        {
            EquipStaticData item = new EquipStaticData();
            item.id = int.Parse(mess[0]);

            data.childDic.Add(item);
        }
        return data;
    }


    //================================================================================================
    //-------------------------------------------------------------以下非操作-----------------------------------------------------------------------
    private List<string[]> loadCSV(string path)
    {
        List<string[]> data = new List<string[]>();
        StreamReader sr = null;
        if (File.Exists(path))
        {
            sr = File.OpenText(path);
        }
        else
        {
            Debug.LogError("读取静态数据异常!!!!" + path);
            return null;
        }
        string str;
        while ((str = sr.ReadLine()) != null)
        {
            data.Add(str.Split(','));
        }
        sr.Close();
        sr.Dispose();
        if (data == null || data.Count <= 0)
        {
            Debug.LogError("读取错误" + path);
        }
        return data;
    }
}
        //反射调用方法
        /*
        loadConfigurationPath obj = new loadConfigurationPath();    
        Type t = Type.GetType("StaticDataSavePath.loadConfigurationPath");      //获得类
        testdelegate method = (testdelegate)Delegate.CreateDelegate(t, obj, obj.allFunctionConfiguration[0]);
        method();
        */
