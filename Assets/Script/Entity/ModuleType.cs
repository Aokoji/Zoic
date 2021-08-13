using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ModuleType 
{
    public int moduleId;    //地图编号      格式200101   20前缀 01 类型（序章）01 地图编号
    public List<ModuleOneCollect> resourceMessage = new List<ModuleOneCollect>();       //当前场景所有采集点信息
}
[System.Serializable]
public class ModuleOneCollect
{
    //场景资源用
    public int mapId;     //地图编号
    public int resourceId;  //资源点编号
    public int resourceName;  //资源点名称
    public int resourceType;    //资源类型
    public DateTime lastCatchTime;  //最后采集时间
    public bool isCatch;    //是否采集
    public int catchInterval;   //采集间隔
}
