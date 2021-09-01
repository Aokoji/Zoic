using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

//csv手动配置
public class StaticCSVDataTool
{
    //------------------------静态数据单位信息----------------------
    private string unitPath = "Assets/Resources/Data//unitMessage.csv";
    private List<string[]> allUnitData = new List<string[]>();           //单位信息

    private string unitSkillPath = "Assets/Resources/Data//unitSkillMessage.csv";
    private List<string[]> allUnitSkillData = new List<string[]>();        //单位技能对照

    private string skillPath = "Assets/Resources/Data//skillMessage.csv";
    private List<string[]> allSkillData = new List<string[]>();           //技能基础参数信息

    private string spoilPath = "Assets/Resources/Data//spoilMessage.csv";
    private List<string[]> allSpoilData = new List<string[]>();         //单位爆率清单

    private string goodPath = "Assets/Resources/Data//goodMessage.csv";
    private List<string[]> allGoodData = new List<string[]>();        //所有物品清单

    private string abnormalPath = "Assets/Resources/Data//abnormalMessage.csv";
    private List<string[]> allAbnormalData = new List<string[]>();        //所有异常列表

    private string extraPath = "Assets/Resources/Data//extraMessage.csv";
    private List<string[]> allExtraData = new List<string[]>();        //所有攻击特殊列表

    /// <summary>
    /// 物品数据  csv读取方法
    /// </summary>
    public AllGoodStaticData loadData_allGoodData()
    {
        loadCSV(goodPath);
        AllGoodStaticData data = new AllGoodStaticData();
        foreach(string[] mess in allGoodData)
        {
            GoodStaticData item = new GoodStaticData();
            item.id = int.Parse(mess[0]);
            item.name = mess[1];
            item.describe= mess[2];
            item.bagType= int.Parse(mess[3]);

            data.childDic.Add(item.id, item);
        }
        return data;
    }





    private void loadCSV(string path)
    {
        StreamReader sr = null;
        if (File.Exists(path))
        {
            sr = File.OpenText(goodPath);
        }
        else
        {
            Debug.LogError("读取静态数据异常!!!!" + path);
            return;
        }
        string str;
        while ((str = sr.ReadLine()) != null)
        {
            allGoodData.Add(str.Split(','));
        }
        sr.Close();
        sr.Dispose();
        if (allGoodData == null || allGoodData.Count <= 0)
        {
            Debug.LogError("读取错误" + path);
        }
    }
}
        //反射调用方法
        /*
        loadConfigurationPath obj = new loadConfigurationPath();    
        Type t = Type.GetType("StaticDataSavePath.loadConfigurationPath");      //获得类
        testdelegate method = (testdelegate)Delegate.CreateDelegate(t, obj, obj.allFunctionConfiguration[0]);
        method();
        */
