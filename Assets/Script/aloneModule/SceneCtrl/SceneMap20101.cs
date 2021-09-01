using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//场景编号01
//场景控制器  需要挂在场景住控上  并手动赋值
public class SceneMap20101 : MonoBehaviour
{
    private int mapid = 20101; //地图编号
    private int curModuleId;    //当前激活资源点编号（玩家进入）   -1为没有
    //-------------------------------------------------------------资源点------------------------------------
    //资源点数据都要单独配置
    public GameObject[] moduleList;        //资源点实体（需要手动赋值）
    private ModuleType allDataList;

    
    public int Mapid { get => mapid; set => mapid = value; }
    public int CurModuleId { get => curModuleId; set => curModuleId = value; }

    //由主控制器调用  并记录
    public void initData()
    {
        if (GameData.Data.DataPlaymessage.resourceItemData.ContainsKey(Mapid))
        {
            allDataList = GameData.Data.DataPlaymessage.resourceItemData[Mapid];
        }
        else
        {
            allDataList = new ModuleType();
        }
        initSceneCollection();
    }
    //初始化场景可互动资源
    private void initSceneCollection()
    {
        int count = 0;
        if (moduleList.Length != allDataList.resourceMessage.Count)
        {//特殊情况   如果手动变动场景导致数据不一致  则全部场景资源初始化
            allDataList = new ModuleType(); //置空数据集
            //初始化资源
            foreach (GameObject m in moduleList)
            {
                showContext script = m.GetComponent<showContext>();
                int type = script.nullType;
                if (type == 0)
                {
                    //无默认值，初始化
                    type = 1;
                }
                //读取到该类型 的默认配置
                //ModuleOneCollect mod=AllUnitData.getModuleCollectionType(type.ToString());
                ModuleOneCollect mod=AllUnitData.Data.getJsonData<ModuleOneCollect>("allCollectData",type);
                mod.mapId = Mapid;
                mod.resourceId = count;
                count++;
                if (script.isFirstCoolDown)
                {
                    mod.isCatch = false;
                    mod.lastCatchTime = DateTime.Now.ToLocalTime();
                }
                script.initData();
                script.setModule(mod);
                setActionToChildModule(script);   //设置事件
            }
        }
        else
        {
            foreach (ModuleOneCollect m in allDataList.resourceMessage)
            {
                showContext script = moduleList[count].GetComponent<showContext>();
                script.initData();
                script.setModule(m);
                count++;
                setActionToChildModule(script);   //设置事件
            }
        }
    }
    //给子组件设置回调事件
    private void setActionToChildModule(showContext module)
    {
        module.collected += collectedModule;
        module.entrance += changeCurEnter;
    }
    //----------------------------------------派发调用的监听事件
    //切换进入事件
    private void changeCurEnter(int id)
    {
        curModuleId = id;
    }
    //收集事件
    private void collectedModule(int id)
    {
        Debug.Log("收集成功！");
    }

}
