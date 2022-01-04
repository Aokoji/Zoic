using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface PlotInterface 
{
    void initData();
    void startPlot(Action action);
    void endPlot();
}
//剧情组件适配器  封装部分公用播放方法
//该适配器最好只重写  不要写新的外调方法（因为未知方法调不到）
public class PlotAdapter : MonoBehaviour, PlotInterface
{
    //公共组件
    public GameObject bgimg;
    public GameObject bgimg2;
    public Button topperClick;
    public GameObject talkBar;   //对话板
    public Text talkText;
    //============      参数      =============
    private string resPath = "Data/plot/plot";
    public Action endAction;
    public float minLockTime = 1f;  //最小剧情间隔
    public int plotid;

    private int curstep;
    private bool isCanNext;
    private GameObject curbgImg;  //当前可执行渐变图引用      //这个东西需要外接一个适配提成公共方法
    private bool isbg1;     //辅助判断
    private bool isdialogIdle;  //对话板是否在

    //=====     重写方法    =======
    public void initData()
    {
        curbgImg = bgimg;
        isbg1 = true;
        isCanNext = false;
        curstep = 0;
        init();
        topperClick.onClick.AddListener(nextstep);
    }
    public virtual void init() {}

    //外调开始
    public void startPlot(Action action)
    {
        endAction = action;
        isCanNext = true;
        startsteps();
        nextstep();
    }
    public virtual void startsteps() { }
    //外调结束
    public void endPlot()
    {
        isCanNext = false;  //点击失效
        gameObject.SetActive(false);
        endAction();
        endAction = null;
    }

    //=====================     内部方法        ===============
    /// <summary>
    /// 渐变方法
    /// 组件必须带有image     组件，是否渐显  ps  目前只执行渐显  渐显图强制抬高层级
    /// </summary>
    private Image changePic;    //图引用
    private float runnum;       //变化值
    private float multiCoef=1;    //速率
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
        yield return null;
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
    }
    //      ======================================      各种set       ===============
    /// <summary>
    /// 设置图片
    /// </summary>
    public void setImage(GameObject obj,string picname)
    {
        Debug.Log(resPath + plotid + "/" + picname);
        obj.GetComponent<Image>().sprite = Resources.Load(resPath + plotid + "/" + picname, typeof(Sprite)) as Sprite;
    }
    //改变背景
    public void changebgImage(string sname)
    {
        curbgImg.transform.SetAsLastSibling();     //提层级
        setImage(curbgImg, sname);      //换图
        gradualChange(curbgImg, true);      //渐变
        curbgImg = isbg1 ? bgimg2 : bgimg;
        isbg1 = isbg1 ? false : true;
    }
    /// <summary>
    /// 改变渐变速率  默认为1  越小越慢
    /// </summary>
    public void setFadeSpeed(float time)
    {
        multiCoef = time;
    }
    /// <summary>
    /// 设置最小间隔锁（按钮）
    /// </summary>
    public void setMinLock(float time)
    {
        minLockTime = time;
    }
    //  =================================       下一步和按钮固定时间锁     =========
    private float runlock;
    public void nextstep()
    {
        if (!isCanNext) return;
        isCanNext = false;
        runlock = 0;
        StartCoroutine(locknextRun());
        //分析进度
        analyzeStep();
        curstep++;
    }
    /// <summary>
    /// 获取当前步骤
    /// </summary>
    public int getCurStep() { return curstep; }
    public virtual void analyzeStep() { }
    IEnumerator locknextRun()
    {
        runlock += Time.deltaTime;
        yield return null;
        if (runlock >= minLockTime)
        {
            Debug.Log("reset click");
            isCanNext = true;
        }
        else
            StartCoroutine(locknextRun());
    }

    /// <summary>
    /// 对话框显隐
    /// </summary>
    public void showcontext(string text)
    {
        talkText.text = text;
        if (isdialogIdle)
            AnimationController.Instance.playAnimation(talkBar, "showtip", false);
        isdialogIdle = true;
    }
    public void hidecontext()
    {
        isdialogIdle = false;
        AnimationController.Instance.playAnimation(talkBar, "hidetip", false);
    }
}