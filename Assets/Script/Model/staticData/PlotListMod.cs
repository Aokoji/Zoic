﻿using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//针对配置型  简单版对话剧情的控制mod
public class PlotListMod 
{
    public string plotPath = "Assets/Resources/Data/plot/plot";       //路径
    public string resPath = "Data/plot/plot";       //路径
    public List<string[]> plotdata;       //当前plot数据集
    public int plotid;

    public int isDialog = 1;    //文本
    public int dialog = 2;
    public int isbg = 3;
    public int bgname = 4;
    public int bgshowtype = 5;
    public int delay = 6;   //强制延时

    public void initData()
    {
        plotdata = new List<string[]>();
    }
    //=======================================           剧情配置            ===========
    public int startStaticNum = 101;
    public bool[] needLoadConfig =
    {
        true,//101
        false,//102
    };
    //=======================================       配置end       =================
    public void setID(int plotid)
    {
        this.plotid = plotid;
    }
    //读csv数据
    public void readConfig(int plotid)
    {
        string path = plotPath + plotid + "//" + plotid + ".csv";
        StreamReader sr = null;
        if (File.Exists(path))
        {
            sr = File.OpenText(path);
        }
        else
        {
            Debug.LogError("读取剧情数据异常!!!!" + path);
            return;
        }
        string str;
        while ((str = sr.ReadLine()) != null)
        {
            plotdata.Add(str.Split(','));
        }
        sr.Close();
        sr.Dispose();
        if (plotdata == null || plotdata.Count <= 0)
            Debug.LogError("读取错误" + path);
    }

}