using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Reflection;
using System.Text;

/// <summary>
/// json数据存储方法
/// 读取顺序  ：  读取方法  JsonReadToolTest--》存储数据  JsonDataSave--》没有则读取csv文件  StaticCSVDataTool
/// </summary>
public class JsonDataSave
{
    private static JsonDataSave jsdata0 = new JsonDataSave();
    public static JsonDataSave jsdata
    {
        get { return jsdata0; }
    }
    //============================================================================================
    //----------------------------------------***    手动添加固定数据变量  手动赋值      ***------------------------------------------
    
    public Dictionary<int, GoodStaticData> allExtraData = new Dictionary<int, GoodStaticData>();

    // 命名格式    变量+“Read”
    //**********单位信息***********
    public Dictionary<int, UnitTypeStaticData> allUnitData = new Dictionary<int, UnitTypeStaticData>();
    public void allUnitDataRead(string o)
    {   //转换存储
        AllUnitTypeStaticData data = JsonUtility.FromJson<AllUnitTypeStaticData>(o);
        foreach (var t in data.childDic)
            allUnitData.Add(t.id, t);
    }
    
    //***************技能基础参数信息****************
    public Dictionary<int, SkillStaticData> allSkillData = new Dictionary<int, SkillStaticData>();
    public void allSkillDataRead(string o)
    {
        AllSkillStaticData data = JsonUtility.FromJson<AllSkillStaticData>(o);
        foreach (var t in data.childDic)
            allSkillData.Add(t.id, t);
    }

    //************单位技能对照***********
    public Dictionary<int, UnitSkillStaticData> allUnitSkillData = new Dictionary<int, UnitSkillStaticData>();
    public void allUnitSkillDataRead(string o)
    {
        AllUnitSkillStaticData data = JsonUtility.FromJson<AllUnitSkillStaticData>(o);
        foreach (var t in data.childDic)
            allUnitSkillData.Add(t.id, t);
    }

    //*************单位爆率清单**********************
    public Dictionary<int, UnitSpoilStaticData> allSpoilData = new Dictionary<int, UnitSpoilStaticData>();
    public void allSpoilDataRead(string o)
    {
        AllUnitSpoilStaticData data = JsonUtility.FromJson<AllUnitSpoilStaticData>(o);
        foreach (var t in data.childDic)
            allSpoilData.Add(t.id, t);
    }

    //*******************所有物品清单***********************
    public Dictionary<int, GoodStaticData> allGoodData = new Dictionary<int, GoodStaticData>();
    public void allGoodDataRead(string o)
    {
        AllGoodStaticData data = JsonUtility.FromJson<AllGoodStaticData>(o);
        foreach (var t in data.childDic)
            allGoodData.Add(t.id, t);
    }

    //*******************所有收集单位信息*****************
    public Dictionary<int, CollectTypeStaticData> allCollcetData = new Dictionary<int, CollectTypeStaticData>();
    public void allCollcetDataRead(string o)
    {
        AllCollectTypeStaticData data = JsonUtility.FromJson<AllCollectTypeStaticData>(o);
        foreach (var t in data.childDic)
            allCollcetData.Add(t.id, t);
    }

    //*********************所有状态信息***********************
    public Dictionary<int, AbnormalStaticData> allAbnormalData = new Dictionary<int, AbnormalStaticData>();
    public void allAbnormalDataRead(string o)
    {
        AllAbnormalStaticData data = JsonUtility.FromJson<AllAbnormalStaticData>(o);
        foreach (var t in data.childDic)
            allAbnormalData.Add(t.id, t);
    }

    //*********************所有装备信息***********************
    public Dictionary<int, EquipStaticData> allEquipData = new Dictionary<int, EquipStaticData>();
    public void allEquipDataRead(string o)
    {
        AllEquipStaticData data = JsonUtility.FromJson<AllEquipStaticData>(o);
        foreach (var t in data.childDic)
            allEquipData.Add(t.id, t);
    }

    //------------------------------------------------***********************----------------------------------------------------------------------------
    //-===============================================================================================
    //-----------------------------------------------------以下区域不用操作-------------------
    delegate object csvdelegate();
    ///测试转换方法
    public static T convertType<T>(object obj)
    {
        Type tp = typeof(T);
        if (tp.IsGenericType) tp = tp.GetGenericArguments()[0];
        //反射
        var tryparse = tp.GetMethod("TryParse",BindingFlags.Public|BindingFlags.Static,Type.DefaultBinder,new Type[] { typeof(string), tp.MakeByRefType() },
            new ParameterModifier[] {new ParameterModifier(2) });
        var parameters = new object[] {obj,Activator.CreateInstance(tp) };
        bool success = (bool)tryparse.Invoke(null, parameters);
        if (success)
            return (T)parameters[1];
        else
            return default(T);
    }

    /// <summary>
    ///     初始化json数据
    /// </summary>
    public void createNewJson(string path, string name)
    {
        //读取csv得出了初始化类型
        //      invoke方法读取
        StaticCSVDataTool obj = new StaticCSVDataTool();
        Type csvEnt = obj.GetType();
        MethodInfo method = csvEnt.GetMethod("loadData_" + name);
        /*
        //      代理方法读取
        StaticCSVDataTool obj0 = new StaticCSVDataTool();
        Type t0 = typeof(StaticCSVDataTool);      //获得类
        csvdelegate method0 = (csvdelegate)Delegate.CreateDelegate(t0, obj0, "StaticCSVDataTool.loadData_" + name);
        object m0=method0();
        */
        string json = JsonUtility.ToJson(method.Invoke(obj, new object[] { }));
        byte[] js = Encoding.UTF8.GetBytes(json.ToCharArray());
        File.WriteAllBytes(Application.dataPath + path, js);
    }
}

//反射类型工具
namespace typeTools
{
    public class typeTrans
    {
        /// <summary>
        /// 输入  类型变量   属性名    设置内容.Tostring
        /// 返回  是否成功
        /// </summary>
        public bool setValue(object obj,string name,string v)
        {
            try
            {
                Type ts = obj.GetType();
                object vv = Convert.ChangeType(v, ts.GetProperty(name).PropertyType);
                ts.GetProperty(name).SetValue(obj, vv, null);
                return true;
            }
            catch
            {
                return false;
            }
            
        }
    }
}