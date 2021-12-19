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
        back2.color = new Color(0.125f, 0.125f, 0.125f, 1);
        back1.color = new Color(1,1,1, 1);
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
        refreshLayout();
        plot.initData();
        //+++ 刷新所有组件的内容（置空）
    }
    public void refreshLayout()
    {
        hideUnderBar();
    }

    //==============            外接入口    开始剧情            ==================
    public void doplot(int plotid,Action callback)
    {
        Debug.Log(plotid + "---id---plot");
        showInterface();
        refreshVariate();
        plot.readConfig(plotid);                 //读数据
        PubTool.dumpString(plot.plotdata);
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
        Debug.Log("当前进度序号 : " + curplotShedule);
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
        {
            //触发延时
            float num = float.Parse(time1);
            StartCoroutine(timmerLock(num));
        }
        else
            StartCoroutine(timmerLock(fixTime));
    }
    IEnumerator timmerLock(float time)
    {
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
            if (dialogIsShow)
                playAnim(undertip, "tipHide", false);
            dialogIsShow = false;
            return;
        }
        else
            showUnderBar();
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
        Debug.Log("改bg");
        //渐变 显隐  处理
        bool fade = false;
        bool ishide = false;
        if (curplotData[plot.bgshowtype].Equals("渐隐"))
        {
            Debug.Log("目前没有渐隐处理===========");
            fade = true;
            ishide =true;
        }
        else if (curplotData[plot.bgshowtype].Equals("渐显"))
        {
            fade = true;
            ishide = false;
        }
        Debug.Log(curplotData[plot.bgname]+ bgfront+fade);
        if (fade)
        {
            if (bgfront)
            {//前图渐显
                bgstartNum = 0;
                back1.color = new Color(back1.color.r, back1.color.g, back1.color.b, 1);
                if (curplotData[plot.bgname].Equals("0"))
                {
                    back2.sprite = null;
                    back2.color = new Color(0.125f,0.125f,0.125f, bgstartNum);
                }
                else
                {
                    back2.color = new Color(1,1,1, bgstartNum);
                    back2.sprite = Resources.Load(plot.resPath + plot.plotid + "/" + curplotData[plot.bgname], typeof(Sprite)) as Sprite;
                }
            }
            else
            {//后图渐显
                bgstartNum = 2;
                if (curplotData[plot.bgname].Equals("0"))
                {
                    back2.sprite = null;
                    back2.color = new Color(0.125f, 0.125f, 0.125f, bgstartNum);
                    Debug.Log("zhihei");
                }
                else
                    back2.sprite = Resources.Load(plot.resPath + plot.plotid + "/" + curplotData[plot.bgname], typeof(Sprite)) as Sprite;
                back1.color = new Color(back1.color.r, back1.color.g, back1.color.b, bgstartNum);
                back2.color = new Color(back2.color.r, back2.color.g, back2.color.b, bgstartNum);
            }
            isfadeing = true;
            StartCoroutine(bgrunFade(bgfront, ishide));
        }
        //其他渐变处理

    }
    private void bgChangeComplete()
    {
        Debug.Log("bgChangeComplete");
        bgfront = !bgfront;
    }
    /// <summary>
    /// isfront  将要换的是否前景，是否渐隐
    /// </summary>
    private bool isfadeing = false;
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
                isfadeing = false;
            }
            else
                back2.color = new Color(back2.color.r, back2.color.g, back2.color.b, bgstartNum);
        }
        else
        {//后图渐显
            bgstartNum -= Time.deltaTime;
            if (bgstartNum < 0)
            {
                bgstartNum = 0;
                back2.color = new Color(back2.color.r, back2.color.g, back2.color.b, bgstartNum);
                bgChangeComplete();
                isfadeing = false;
            }
            else
                back2.color = new Color(back2.color.r, back2.color.g, back2.color.b, bgstartNum);
        }
        yield return null;
        if (isfadeing)
            StartCoroutine(bgrunFade(isfront, ishide));
    }

    //=================================     工具部分        ==============

    private void playAnim(GameObject obj, string animname,bool isloop)
    {
        //AnimationController.Instance.playAnimation(obj, animname, isloop);
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
    private void showUnderBar() { undertip.SetActive(true); }
    private void hideUnderBar() { undertip.SetActive(false); }
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
