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


    private GameObject bagItemPerfeb;   //创建一个小型的复用实例
    private List<BagOneRowView> bagListPage=new List<BagOneRowView>();    //二维的当前页数组
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
        gameObject.SetActive(false);
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
        //初始化背包状态
        initBagState();
        //加载背包格子
        //loadBagGrid(playerdata.items.goods);
        //根据当前页刷新页面
        refreshBag();
        //+++待修改  加载完成
        gameObject.SetActive(true);
    }

    private void initBagState()
    {
        curShowBag = 1;
    }
    /// <summary>
    /// 加载背包网格
    /// </summary>
    /// <param name="items"></param>
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
        refreshActorBoard();
        refreshEquip();
        refreshCurPage();
    }
    //-------------------------------------------------------内置功能---------------------------
    /// <summary>
    /// 用于当前页切换
    /// </summary>
    private void refreshCurPage()
    {
        refreshGridBtn();
        refreshPageItemIcon();
    }
    //刷新背包格按钮
    private void refreshGridBtn()
    {
        //根据当前显示 进行刷新

    }
    //
    private void refreshPageItemIcon()
    {
        var items = getPageTransGoods();
        //先对比长度  动态添加背包长度
        int lengthMix = items.Count-bagListPage.Count*5;
        if (lengthMix > 0)
        {
            lengthMix = Mathf.CeilToInt(lengthMix / 5);
            for(int i = 0; i < lengthMix; i++)
            {
                addBagGrid();
            }
        }
        int rowId = 0;
        foreach(var good in items)
        {
            //执行添加操作
            if (!bagListPage[rowId].addOneIcon(good))
            {
                //添加失败 意味着需要换行
                rowId++;
                if (!bagListPage[rowId].addOneIcon(good))
                {
                    //添加失败  报错
                    Debug.LogError("物品列表初始化失败！失败行号：：" + rowId);
                }
            }
        }
    }
    //刷新角色面板
    private void refreshActorBoard()
    {
        
    }
    //刷新装备格
    private void refreshEquip()
    {

    }
    private void clearBagItemIcon()
    {
        foreach(var obj in bagListPage)
        {
            if (obj.gameObject.activeSelf)
            {
                //+++待修改  调用父级单排网格  清除其中图表显示
                obj.clearChildIcon();
                obj.gameObject.SetActive(false);
            }
        }
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
        refreshCurPage();
    }
    //道具按钮
    private void clickProp()
    {
        if (curShowBag == 2) return;
        refreshCurPage();
    }
    //材料按钮
    private void clickMaterial()
    {
        if (curShowBag == 3) return;
        refreshCurPage();
    }
    //任务按钮
    private void clickTask()
    {
        if (curShowBag == 4) return;
        refreshCurPage();
    }
    //关闭
    private void clickClose()
    {
        gameObject.SetActive(false);
    }

    private List<oneGood> getPageTransGoods()
    {
        switch (curShowBag)
        {
            case 1:return bag_equip;
            case 2:return bag_prop;
            case 3:return bag_material;
            case 4:return bag_task;
            default:
                Debug.LogError("背包数据类型读取错误！");return null;
        }
    }

    /// <summary>
    /// 初始化小型的单位背包图标实例
    /// </summary>
    private void initItemIconPerfeb()
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
        bagListPage.Add(content.GetComponent<BagOneRowView>());
        content.SetActive(false);
        return content;
    }



}
