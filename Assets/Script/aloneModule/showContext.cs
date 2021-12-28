using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class showContext : MonoBehaviour
{
    public GameObject showObj;      //外挂自身控制的显示板
    public Button collect;
    /// <summary>
    /// 特殊参数  配置时手动添加   ## 资源点类型 ##， 如果为空0  则默认赋值。
    /// </summary>
    public int nullType;
    public bool isFirstCoolDown;    //是否初值为冷却（多等一个冷却）

    public ModuleOneCollect module;       //资源点类型

    private bool isEnterCollect = false;    //是否能收集（进入范围）
    private bool isLockCollect = false;     //是否能收集（数据记录）

    public delegate void collectEvent(int a);        //收集事件（传递 资源点编号）
    public event collectEvent collected;            //   （收集）                        初始化时会由父级赋给响应方法
    public event collectEvent entrance;             //  （玩家进入范围）

    private void Start()
    {
        if (showObj != null) showObj.SetActive(false);
    }
    //初始化两步走  initdata初始化  setmodule设置数据
    public void initData(ModuleOneCollect data)
    {
        module = data;
        initEvent();
    }

    private void initEvent()
    {
        collect.onClick.AddListener(clickCollect);
    }
    //点击收集
    private void clickCollect()
    {
        if (isLockCollect) return;
        //可收集情况  发送收集事件
        collected(module.resourceId);
    }
    //检查时间是否可收集
    private void checkTime()
    {
        if (module.isCatch)
        {
            isLockCollect = false;
            return;
        }
        DateTime data = DateTime.Now.ToLocalTime();
        DateTime load = module.lastCatchTime;
        int num = DateTime.Compare(data, load);
        if (num >= module.catchInterval)
        {
            //大于时间间隔  代表可收集
            module.isCatch = true;
            isLockCollect = false;
        }
        else
        {
            //小于时间间隔  代表不可收集
            module.isCatch = false;
            isLockCollect = true;
        }
    }
    //记录进入状态  发给场景主控制器  告知进入采集范围编号
    public void callState(bool isin)
    {
        if (isin)
        {
            entrance(module.resourceId);
        }
        else
        {
            entrance(-1);
        }

    }
    //展示可收集提示版
    public void showTips()
    {
        showObj.SetActive(true);
        checkTime();
        if (isLockCollect)
        {
            //+++不可收集  显示灰色标识
        }
        else
        {
            //+++可收集
        }
        
    }
    public void hideTips()
    {
        showObj.SetActive(false);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isEnterCollect = true;
            //callState(true);
            showTips();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isEnterCollect = false;
            //callState(false);
            hideTips();
        }
    }
}
