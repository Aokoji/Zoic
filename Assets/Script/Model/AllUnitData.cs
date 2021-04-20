﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class AllUnitData
{
    //public static Dictionary<string, unitMessage> allUnitData;
    static List<string[]> allUnitData = new List<string[]>();           //单位信息
    static List<string[]> allSkillData = new List<string[]>();           //技能基础参数信息
    private static string unitPath = "Assets/Resources/Data//unitMessage.txt";
    private static string skillPath = "Assets/Resources/Data//skillMessage.txt";
    private static Dictionary<string, string> natureName = new Dictionary<string, string>();
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
    public static void loadData()
    {   //读取怪物配置
        loadUnit();
        loadSkill();
        setnatureName();
    }
    private static void loadUnit()
    {
        StreamReader sr;
        if (File.Exists(unitPath))
        {
            sr = File.OpenText(unitPath);
        }
        else
        {
            Debug.LogError("读取静态unit数据异常!!!!");
            return;
        }
        string str;
        while ((str = sr.ReadLine()) != null)
        {
            allUnitData.Add(str.Split(','));
        }
        sr.Close();
        sr.Dispose();
    }

    private static void loadSkill()
    {
        StreamReader sr;
        if (File.Exists(skillPath))
        {
            sr = File.OpenText(skillPath);
        }
        else
        {
            Debug.LogError("读取静态skill数据异常!!!!");
            return;
        }
        string str;
        while ((str = sr.ReadLine()) != null)
        {
            allSkillData.Add(str.Split('\t'));
        }
        sr.Close();
        sr.Dispose();
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
        natureName.Add("11","state");
    }
    public static string getEncode(string num)
    {
        return natureName[num];
    }

    public static string[] getUnitData(int i)
    {       //根据编号获取怪物数据
        return allUnitData[i];
    }
    public static string[] getSkillData(int i)
    {       //根据编号获取技能数据
        return allSkillData[i];
    }

}
public class unitMessage
{

}

