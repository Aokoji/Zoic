using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//环境管理器
public class EnvironmentManager : DDOLController<EnvironmentManager>
{
    public void initData()
    {

    }

    private void loadEnvironment()
    {
        if (GameData.Data.playerBridge.getLastScene() == 0)
            initalGameEnvironment();
    }
    //初始化最初场景
    ///没有场景数据  则为第一次进入  需要先加载 等待剧情结束再调用显示
    private void initalGameEnvironment()
    {

    }
}
