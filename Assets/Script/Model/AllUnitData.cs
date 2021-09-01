using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;

public class AllUnitData:MonoBehaviour
{
    private static AllUnitData unitData = null;
    public static AllUnitData Data
    {
        get {
            return unitData;
        }
    }
    /*
     * 待更改+++
     * 所有csv文件均作为基本数据文件  作为参考基值
     * 检测json文件存在，如果没有json记录  则读取csv文件并做json存储
     * 每当修改csv文件参数时  需要手动删除生成的json文件  让加载时重新读取达到刷新的目的
     */
    private JsonReadToolTest jsonRead = new JsonReadToolTest();

    public T getJsonData<T>(string name, int num)
    {
        return jsonRead.getJsonData<T>(name, num);
    }
   


    //单个可收集资源组件信息配置  （类型编号 ，类型信息）
    private static Dictionary<string, ModuleOneCollect> oneCollectionTypeData = new Dictionary<string, ModuleOneCollect>();
    
    //------------------------静态单位信息----------------------
    static List<string[]> allUnitData = new List<string[]>();           //单位信息
    static List<string[]> allUnitSkillData = new List<string[]>();        //单位技能对照
    static List<string[]> allSkillData = new List<string[]>();           //技能基础参数信息
    static List<string[]> allSpoilData = new List<string[]>();         //单位爆率清单
    static List<string[]> allGoodData = new List<string[]>();        //所有物品清单
    static List<string[]> allAbnormalData = new List<string[]>();        //所有异常列表
    static List<string[]> allExtraData = new List<string[]>();        //所有攻击特殊列表
    //------------------------初始化分类信息----------------
    static List<string[]> allCollectionTypeData = new List<string[]>();        //初始化场景数据类型信息(可收集互动物体)

    /*
     * 这部分规范性需要修改+++
     * 改成统一的字符数组
     * 只需要添加路径配置即可加载
     * 不需要重新赋变量
     */

    private static string unitPath = "Assets/Resources/Data//unitMessage.csv";
    private static string unitSkillPath = "Assets/Resources/Data//unitSkillMessage.csv";
    private static string skillPath = "Assets/Resources/Data//skillMessage.csv";
    private static string spoilPath = "Assets/Resources/Data//spoilMessage.csv";
    private static string goodPath = "Assets/Resources/Data//goodMessage.csv";
    private static string abnormalPath = "Assets/Resources/Data//abnormalMessage.csv";
    private static string extraPath = "Assets/Resources/Data//extraMessage.csv";
    //private static string collectionPath = "Assets/Resources/Data//extraMessage.csv";
    private static Dictionary<string, string> natureName = new Dictionary<string, string>();
    private static Dictionary<List<string[]>, string> loadMessage = new Dictionary<List<string[]>, string>();       //<信息，路径>
    
    /// <summary>
    /// 加载静态数据(全部读取,不包括存档)
    /// </summary>
    public void loadData()
    {   //读取配置
        jsonRead.readAllJsonData();

        loadMessage.Add(allUnitData, unitPath);
        loadMessage.Add(allUnitSkillData, unitSkillPath);
        loadMessage.Add(allSkillData, skillPath);
        loadMessage.Add(allSpoilData, spoilPath);
        loadMessage.Add(allGoodData, goodPath);
        loadMessage.Add(allAbnormalData, abnormalPath);
        loadMessage.Add(allExtraData, extraPath);
        //loadMessage.Add(allCollectionTypeData, collectionPath);
        //加载数据
        //loadPath();
        //setnatureName();
    }
    private static void loadPath()
    {
        StreamReader sr=null;
        foreach(var item in loadMessage)
        {
            if (File.Exists(item.Value))
            {
                sr = File.OpenText(item.Value);
            }
            else
            {
                Debug.LogError("读取静态数据异常!!!!"+item.Value);
                return;
            }
            string str;
            while ((str = sr.ReadLine()) != null)
            {
                item.Key.Add(str.Split(','));
            }
            sr.Close();
        }
        if (sr != null)
        {
            sr.Dispose();
        }
        else
        {
            Debug.LogError("静态数据加载异常!!!!");
            return;
        }

    }
    private static void setnatureName()
    {
        natureName.Add("0","id");
        natureName.Add("1","name");
        natureName.Add("2","physical");
        natureName.Add("3","vigor");
        natureName.Add("4","attack");
        natureName.Add("5","speed");
        natureName.Add("6","type");
        natureName.Add("7","adPat");
        natureName.Add("8","apPat");
        natureName.Add("9","strike");
        natureName.Add("10","dodge");
        natureName.Add("11", "curHp");
        natureName.Add("12", "curMp");
        natureName.Add("13","state");
    }
    public static string getEncode(string num)
    {
        return natureName[num];
    }

    public static string[] getUnitData(int i)
    {       //根据编号获取怪物数据
        return allUnitData[i];
    }
    public static string[] getUnitSkillData(int i)
    {       //根据编号获取怪物数据技能
        return allUnitSkillData[i];
    }
    public static string[] getSkillData(int i)
    {       //根据编号获取技能数据
        return allSkillData[i];
    }
    public static string[] getGoodData(int i)
    {       //根据编号获取物品数据
        return allGoodData[i];
    }
    public static string[] getSpoilData(int i)
    {       //根据编号获取爆率数据
        return allSpoilData[i];
    }
    public static string[] getAbnormalData(int i)
    {       //根据编号获取异常状态数据
        return allAbnormalData[i];
    }
    public static string[] getExtraData(int i)
    {       //根据编号获取异常状态数据
        return allExtraData[i];
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

