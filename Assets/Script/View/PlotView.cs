using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class PlotView : MonoBehaviour
{

    public GameObject backimg1;      //背景图  底
    public GameObject backimg2;      //背景图  上
    private Image back1;
    private Image back2;

    public GameObject undertip;     //底框
    public Text underContext;       //底框文字

    public Button topperClick;      //全屏点击区域

    private PlotListMod plot;       //plot数据集
    //----静态数据
    private float fixTime = 0.5f;
    //------当局变量数据集  需要刷-------
    private bool allowClick;        //允许点击(当前是否允许点击进行下一步)
    private Action nextstep;
    private int curplotShedule;     //当前进度
    private int maxplotShedule;     //总进度
    private bool islocking;     //计时器锁
    private bool dialogIsShow;
    private string[] curplotData;       //当次内容
    private bool bgfront;   //是否前底图

    //显隐变量
    private float bgstartNum;
    private float bgFadeFinal = 1;

    public void initData()
    {
        plot = new PlotListMod();
        initLayout();
        initEvent();
        refreshVariate();
    }
    private void initLayout()
    {
        hideInterface();
        back1 = backimg1.GetComponent<Image>();
        back2 = backimg2.GetComponent<Image>();
        back1.color = new Color(0, 0, 0, 1);
        back2.color = new Color(0, 0, 0, 1);
        //这俩背景都显示，黑底
    }
    private void initEvent()
    {
        topperClick.onClick.AddListener(nextPlotDialog);
    }

    //刷新变量
    private void refreshVariate()
    {
        nextstep = null;
        curplotShedule = 0;
        maxplotShedule = 0;
        allowClick = false;
        islocking = false;
        dialogIsShow = false;
        bgfront = false;
        plot.initData();
        //+++ 刷新所有组件的内容（置空）
    }
    //==============            外接入口    开始剧情            ==================
    public void doplot(int plotid,Action callback)
    {
        refreshVariate();
        plot.readConfig(plotid);                 //读数据
        maxplotShedule = plot.plotdata.Count;
        nextstep = callback;
        allowClick = true;
        nextPlotDialog();
    }

    //下一段对话
    private void nextPlotDialog()
    {
        if (!allowClick) return;
        allowClick = false;
        curplotShedule++;
        //判断结束
        if (curplotShedule >= maxplotShedule)  
            curPlotEnded();
        curplotData = plot.plotdata[curplotShedule];
        //分析锁
        analyzeLock();
        //分析操作
        analyzeDialog();
        analyzeBackGround();
    }
    //剧情结束
    private void curPlotEnded()
    {
        nextstep?.Invoke();
    }
    //===============================       分析部分        ====================
    //锁 默认存在最低延时，保证流程通畅
    private void analyzeLock()
    {
        string time1 = curplotData[plot.delay];
        if (!time1.Equals("0"))
            //触发延时
            StartCoroutine(timmerLock(int.Parse(time1)));
        else
            StartCoroutine(timmerLock(fixTime));
    }
    IEnumerator timmerLock(float time)
    {
        if (!islocking) Debug.Log("锁延迟时间调用存在错误！！！！！当前编号："+plot.plotid+"----步骤 "+curplotShedule);
        islocking = true;
        yield return new WaitForSeconds(time);
        islocking = false;
        allowClick = true;
    }

    //分析对话
    public void analyzeDialog()
    {
        //判断是否文本
        if (curplotData[plot.isDialog].Equals("0"))
        {
            if(dialogIsShow)
                playAnim(undertip, "tipHide", false);
            dialogIsShow = false;
            return;
        } 
        if (!dialogIsShow)
        {
            playAnim(undertip, "tipShow", false);
            dialogIsShow = true;
        }
        underContext.text = curplotData[plot.dialog];
    }
    /// <summary>
    /// 渐变思路      
    /// 渐显（后图状态）  后换图不变  前渐隐
    /// 渐显（前图状态）后不变     前换图渐显
    /// </summary>
    private void analyzeBackGround()
    {
        if (curplotData[plot.isbg].Equals("0")) return;
        //渐变 显隐  处理
        bool fade = false;
        bool ishide = false;
        if (curplotData[plot.isbg].Equals("渐隐"))
        {
            Debug.Log("目前没有渐隐处理===========");
            fade = true;
            ishide =true;
        }
        else if (curplotData[plot.isbg].Equals("渐显"))
        {
            fade = true;
            ishide = false;
        }
        if (fade)
        {
            if (bgfront)
            {//前图渐显
                bgstartNum = 0;
                back1.color = new Color(back2.color.r, back2.color.g, back2.color.b, 1);
                back2.color = new Color(back2.color.r, back2.color.g, back2.color.b, bgstartNum);
                back2.sprite = Resources.Load(plot.plotPath + plot.plotid + "//" + curplotData[plot.bgname], typeof(Sprite)) as Sprite;
            }
            else
            {//后图渐显
                bgstartNum = 1;
                back1.sprite= Resources.Load(plot.plotPath+plot.plotid+"//"+ curplotData[plot.bgname], typeof(Sprite)) as Sprite;
                back1.color = new Color(back2.color.r, back2.color.g, back2.color.b, bgstartNum);
                back2.color = new Color(back2.color.r, back2.color.g, back2.color.b, bgstartNum);
            }
            StartCoroutine(bgrunFade(bgfront, ishide));
        }
        //其他渐变处理

    }
    private void bgChangeComplete()
    {
        bgfront = !bgfront;
    }
    /// <summary>
    /// isfront  将要换的是否前景，是否渐隐
    /// </summary>
    IEnumerator bgrunFade(bool isfront,bool ishide)
    {
        //渐显处理
        if (isfront)
        {//前图渐显
            bgstartNum += Time.deltaTime;
            if (bgstartNum > bgFadeFinal)
            {
                bgstartNum = 1;
                back2.color = new Color(back2.color.r, back2.color.g, back2.color.b, bgstartNum);
                bgChangeComplete();
                yield return null;
            }
            else
                back2.color = new Color(back2.color.r, back2.color.g, back2.color.b, bgstartNum);
        }
        else
        {//后图渐显
            bgstartNum -= Time.deltaTime;
            if (bgstartNum < bgFadeFinal)
            {
                bgstartNum = 0;
                back2.color = new Color(back2.color.r, back2.color.g, back2.color.b, bgstartNum);
                bgChangeComplete();
                yield return null;
            }
            else
                back2.color = new Color(back2.color.r, back2.color.g, back2.color.b, bgstartNum);
        }
        StartCoroutine(bgrunFade(isfront, ishide));
        yield return null;
    }

    //=================================     工具部分        ==============

    private void playAnim(GameObject obj, string animname,bool isloop)
    {
        AnimationController.Instance.playAnimation(obj, animname, isloop);
    }

    private void showInterface()
    {//显示界面  内部用
        gameObject.SetActive(true);
    }
    //隐藏界面
    private void hideInterface()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    ///加载全屏的剧情  || 剧情编号
    /// </summary>
    public void loadToFillScenePlot(int num)
    {

    }
    ///<summary>
    ///加载场景的动作或动画     （需要单独的配置，不能统一配置）
    /// </summary>
    public void loadToActiveScenePlot(int num)  //配置编号(特殊配置)
    {

    }

}
