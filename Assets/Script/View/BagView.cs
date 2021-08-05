using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BagView : MonoBehaviour
{
    public GameObject bagListView;
    private List<List<GameObject>> bagListData=new List<List<GameObject>>();

    public void showBag(PlayerMessage playerdata,Action action)
    {
        //加载背包格子
        loadBagGrid();
        //加载人物装备
        //按钮功能添加
    }

    

    private void loadBagGrid()
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
