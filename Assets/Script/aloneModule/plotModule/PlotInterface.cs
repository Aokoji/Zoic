using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PlotInterface 
{
    void startPlot(Action action);
    void endPlot();
}
//剧情组件适配器  封装部分公用播放方法
//该适配器最好只重写  不要写新的外调方法（因为未知方法调不到）
public class PlotAdapter : MonoBehaviour, PlotInterface
{
    public Action endAction;
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
}