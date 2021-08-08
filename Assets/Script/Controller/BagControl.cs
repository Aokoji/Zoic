using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BagControl : DDOLController<BagControl>
{
    private BagView bag;
    public void initData()
    {
        if (bag != null)
        {
            Destroy(bag);
        }
        GameObject mode = Resources.Load<GameObject>("Entity/BagUI");
        var baseMain = Instantiate(mode);
        baseMain.name = "BagView";
        baseMain.transform.SetParent(CanvasLoad.canvasui.transform, false);
        baseMain.transform.position = CanvasLoad.canvasui.transform.position;
        var view = baseMain.GetComponent<BagView>();
        view.initBagView();
        bag = view;
    }

    /// <summary>
    /// 打开背包操作
    /// 背包打开完成后会进行回调
    /// </summary>
    public void openBag(Action action)
    {
        //获取物品数据 传递给view
        bag.openBag(GameData.Data.Playermessage,action);
    }







}
