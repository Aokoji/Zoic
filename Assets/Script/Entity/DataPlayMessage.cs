using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class DataPlayMessage 
{

    //场景
    //public int sceneIdleNum;        //当前场景

    //所有场景可收集资源数据  场景编号，场景数列
    ///无需赋初始值  进入场景加载后资源不匹配会自动初始化  并保存结果
    public Dictionary<int, ModuleType> resourceItemData; 

    public int combatIDCount;   //战斗记录编号
}
