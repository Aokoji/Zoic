using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SceneCtlMethod : MonoBehaviour, SceneInterface
{
    public int mapid;
    //根据现有编号开始加载地图
    public void loadScene()
    {
        //只加载地图组件  不初始化  保持隐藏状态
    }
    //初始化数据     决定要显示的时候再初始化
    public virtual void initData(){}
    //初始化地图编号（需要重写） 默认为20101号 序章地图
    public void setID(int id) { mapid = id; }
    //获得地图编号
    public int getSceneID(){ return mapid;}
}
