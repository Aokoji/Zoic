using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface PlotInterface 
{
    void startPlot(Action action);
    void endPlot();
}
//剧情组件适配器  封装部分公用播放方法
//该适配器最好只重写  不要写新的外调方法（因为未知方法调不到）
public class PlotAdapter : MonoBehaviour, PlotInterface
{
    //============      参数      =============
    private string resPath = "Data/plot/plot";
    public Action endAction;
    private int plotid;

    //=====     重写方法    =======
    public void initData()
    {
        plotid = 101;
    }

    //外调开始
    public void startPlot(Action action)
    {
        endAction = action;
    }
    //外调结束
    public void endPlot()
    {
        endAction();
        endAction = null;
    }

    //=====================     内部方法        ===============
    /// <summary>
    /// 渐变方法
    /// 组件必须带有image     组件，是否渐显
    /// </summary>
    private Image changePic;    //图引用
    private float runnum;       //变化值
    public float multiCoef=1;    //速率
    public void gradualChange(GameObject obj,bool isshow)
    {
        //初始化基本数据
        runnum = isshow ? 0 : 1;
        obj.SetActive(true);
        changePic= obj.GetComponent<Image>();
        changePic.color = new Color(1, 1, 1, runnum);

        StartCoroutine(runFade(isshow));
    }
    //渐变完成
    public void changeComplete()
    {
        Debug.Log("bgChangeComplete");

    }
    IEnumerator runFade(bool isadd)
    {
        if (isadd)
        {
            runnum += Time.deltaTime*multiCoef;
            changePic.color = new Color(1, 1, 1, runnum);
            if (runnum >= 1)
                changeComplete();
            else
                StartCoroutine(runFade(isadd));
        }
        else
        {
            runnum -= Time.deltaTime * multiCoef;
            changePic.color = new Color(1, 1, 1, runnum);
            if (runnum <=0)
                changeComplete();
            else
                StartCoroutine(runFade(isadd));
        }
        yield return null;
    }
    /// <summary>
    /// 设置图片
    /// </summary>
    public void setImage(GameObject obj,string picname)
    {
        obj.GetComponent<Image>().sprite = Resources.Load(resPath + plotid + "/" + picname, typeof(Sprite)) as Sprite;
    }
}