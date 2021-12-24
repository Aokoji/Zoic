using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//场景编号01
//场景控制器  需要挂在场景住控上(父)  并手动赋值       这种一般是view
//目前计划改装场景通用view层 
public class SceneMap20101 : SceneCtlMethod
{
    private int curModuleId;    //当前激活资源点编号（玩家进入）   -1为没有
    //-------------------------------------------------------------资源点------------------------------------
    
    public SceneMap20101() { setID(10101); }   //默认 10101编号
    public SceneMap20101(int id)
    {
        if (id > 0) setID(id);
    }


}
