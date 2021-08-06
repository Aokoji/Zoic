using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BagView : MonoBehaviour
{
    public GameObject bagListView;
    public Button equipBtn;         //四个背包类型按钮
    public Button propBtn;
    public Button materialBtn;
    public Button taskBtn;
    public Button closeBtn; //关闭按钮

    private List<GameObject> bagListPage=new List<GameObject>();    //二维的当前页数组
    private int curShowBag = 0;     //背包当前页             1装备 2道具 3材料 4任务
    private Dictionary<int, List<oneGood>> bagListData = new Dictionary<int, List<oneGood>>();
    private List<oneGood> bag_equip = new List<oneGood>();      //装备
    private List<oneGood> bag_prop = new List<oneGood>();       //道具
    private List<oneGood> bag_material = new List<oneGood>();   //材料
    private List<oneGood> bag_task = new List<oneGood>();       //任务

    public void initBagView()
    {
        bagListData.Add(1, bag_equip);
        bagListData.Add(2, bag_prop);
        bagListData.Add(3, bag_material);
        bagListData.Add(4, bag_task);
        initEvent();
    }
    //赋予按钮功能
    private void initEvent()
    {
        equipBtn.onClick.AddListener(clickEquip);
        propBtn.onClick.AddListener(clickProp);
        materialBtn.onClick.AddListener(clickMaterial);
        taskBtn.onClick.AddListener(clickTask);
        closeBtn.onClick.AddListener(clickClose);
    }
    /// <summary>
    /// 打开背包
    /// </summary>
    public void openBag(PlayerMessage playerdata,Action action)
    {
        curShowBag = 1;
        //加载背包格子
        loadBagGrid(playerdata.items.goods);
        //加载人物装备
        //按钮功能添加
    }

    

    private void loadBagGrid(List<oneGood> items)
    {
        foreach(oneGood one in items)
        {
            if (bagListData.ContainsKey(one.bagType))
            {
                bagListData[one.bagType].Add(one);
            }
            else
            {
                Debug.LogError("不存在的物品类型！！");
                break;
            }
        }
        foreach(oneGood one in bagListData[curShowBag])
        {
            GameObject bar = addBagGrid();
            //每个创建的物体上应该有设置文件view  获取该文件
            //设置图标  数量  记录id 去文档中找对应描述
            bar.SetActive(true);
        }

    }
    /// <summary>
    /// 刷新界面（所有）
    /// </summary>
    public void refreshBag()
    {
        refreshGrid();
        refreshActorBoard();
        refreshEquip();
    }
    //-------------------------------------------------------内置功能---------------------------

    //刷新背包格
    private void refreshGrid()
    {
        //根据当前显示 进行刷新
    }
    //刷新角色面板
    private void refreshActorBoard()
    {
        
    }
    //刷新装备格
    private void refreshEquip()
    {

    }
    //--------------------------------------------------按钮功能-------------------------------------
    //装备按钮
    private void clickEquip()
    {
        if (curShowBag == 1) return;
        int count = 0;  //累计计数
        int point = 1;  //指针
        foreach(oneGood one in bag_equip)
        {
            if(count!=0 && count % 5 == 0)
            {
                //换行
                GameObject bar = addBagGrid();
            }
            count++;
           
        }
    }
    //道具按钮
    private void clickProp()
    {
        if (curShowBag == 2) return;
    }
    //材料按钮
    private void clickMaterial()
    {
        if (curShowBag == 3) return;
    }
    //任务按钮
    private void clickTask()
    {
        if (curShowBag == 4) return;
    }
    //关闭
    private void clickClose()
    {

    }

    // 加一个背包网格
    private GameObject addBagGrid()
    {
        GameObject obj = Resources.Load<GameObject>("Entity/combat/bagbar");
        GameObject content = Instantiate(obj);
        var items = bagListView.GetComponentsInChildren<ComponentScript1>();
        content.transform.SetParent(bagListView.transform);
        content.transform.localScale = new Vector3(1, 1, 1);
        //+++  content.transform.position = items[items.Length - 1].transform.position - new Vector3(0, 0.8f, 0);
        //判断好位置  分行列添加格子
        return content;
    }



}
