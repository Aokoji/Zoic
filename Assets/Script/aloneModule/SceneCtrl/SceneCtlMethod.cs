using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCtlMethod : MonoBehaviour, SceneInterface
{
    public int curModuleId;    //当前激活资源点编号（玩家进入）   -1为没有
    //资源点数据都要单独配置
    public GameObject[] moduleList;        //资源点实体（需要手动赋值）
    public ModuleType allDataList;

    public int mapid;
    //根据现有编号开始加载地图
    public void loadScene()
    {
        //只加载地图组件  不初始化  保持隐藏状态
    }
    //初始化数据     决定要显示的时候再初始化
    public void initData(int id)
    {
        setID(id);
        //从记录中读取已存用的地图信息
        if (GameData.Data.SceneData.ContainsKey(mapid))
        {
            allDataList = GameData.Data.SceneData[mapid];
            initSceneCollection();
        }
        else
        {
            //新建
            initCreateModule();
        }
        gameObject.SetActive(true);
    }
    //初始化地图编号
    public void setID(int id) { mapid = id; }
    //获得地图编号
    public int getSceneID(){ return mapid;}

    //检索初始化场景可互动资源
    private void initSceneCollection()
    {
        if (moduleList.Length != allDataList.resourceMessage.Count)
        {//特殊情况   如果手动变动场景导致数据不一致  则全部场景资源初始化   （根据场景赋值组件计算)
            initCreateModule();
        }
        else
        {//正常情况
            int count = 0;
            foreach (ModuleOneCollect m in allDataList.resourceMessage)
            {
                showContext script = moduleList[count].GetComponent<showContext>();
                script.initData(m);
                count++;
                setActionToChildModule(script);   //设置事件
            }
        }
    }
    //创建初始化
    private void initCreateModule()
    {
        int count = 0;
        allDataList = new ModuleType(); //置空数据集
        allDataList.mapid = mapid;
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
            ModuleOneCollect mod = AllUnitData.Data.getJsonData<ModuleOneCollect>("allCollectData", type);
            mod.mapId = mapid;
            mod.resourceId = count;
            count++;
            if (script.isFirstCoolDown)
            {
                mod.isCatch = false;
                mod.lastCatchTime = DateTime.Now.ToLocalTime();
            }
            script.initData(mod);
            setActionToChildModule(script);   //设置事件
        }
        GameData.Data.SceneData[mapid] = allDataList;
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

    /// <summary>
    /// 检查场景交互，隐藏屏幕外资源
    /// </summary>
    public void checkSceneInteractions()
    {

    }
}
