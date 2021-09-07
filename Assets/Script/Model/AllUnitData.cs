using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;

public class AllUnitData:MonoBehaviour
{
    private static AllUnitData unitData = new AllUnitData();
    public static AllUnitData Data
    {
        get {
            return unitData;
        }
    }
    /*
     * 待更改
     * 所有csv文件均作为基本数据文件  作为参考基值
     * 检测json文件存在，如果没有json记录  则读取csv文件并做json存储
     * 每当修改csv文件参数时  需要手动删除生成的json文件  让加载时重新读取达到刷新的目的
     */

    private JsonReadToolTest jsonRead = new JsonReadToolTest();

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
   
    //------------------------静态单位信息----------------------
    //------------------------初始化分类信息----------------
    
    /// <summary>
    /// 加载静态数据(全部读取,不包括存档)
    /// </summary>
    public void loadData()
    {   //读取配置
        jsonRead.readAllJsonData();
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

