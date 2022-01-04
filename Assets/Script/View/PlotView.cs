using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlotView : MonoBehaviour
{

    public GameObject backimg1;      //背景图  底
    public GameObject importStage;  //加载组件位置
    private GameObject stagePrefab;     //剧情组件实体
    public PlotInterface stageMod;     //组件脚本

    public GameObject undertip;     //底框
    public Text underContext;       //底框文字
    public Button topperClick;      //全屏点击区域

    private PlotListMod plot;       //plot数据集
    //------当局变量数据集  需要刷-------
    private bool allowClick;        //允许点击(当前是否允许点击进行下一步)
    private Action nextstep;
    private int curplotShedule;     //当前进度
    private int maxplotShedule;     //总进度
    private bool dialogIsShow;
    private string[] curplotData;       //当次内容
    private bool bgfront;   //是否前底图

    //非csv plot ：普通连贯式全屏控制剧情，加载预制体，调用initdata 和 startplot自动执行
    //csv plot：读取csv 仅限于对话系统，非全程控制，到达csv长度上限结束剧情。
    private bool isLoadCSV;     //是否是csv剧情
    //显隐变量
    private float bgstartNum;
    private float bgFadeFinal = 1;
    //常量
    private float waittime = 1f;

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
        refreshLayout();
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
        dialogIsShow = false;
        bgfront = false;
        plot.initData();
        //+++ 刷新所有组件的内容（置空）
    }
    public void refreshLayout()
    {
        backimg1.GetComponent<Image>().color = new Color(0.125f, 0.125f, 0.125f, 0.5f);
        hideUnderBar();
    }

    //==============            外接入口    开始剧情            ==================
    public void doplot(int plotid,Action callback)
    {
        Debug.Log(plotid + "---id---plot");
        plot.setID(plotid);
        showInterface();
        refreshLayout();
        if (readPlotLoadConfig())
        {
            //全屏控制剧情
            isLoadCSV = false;
            importStage.SetActive(true);
            GameObject loadobj = Resources.Load<GameObject>(plot.resPath+plotid+"/mainplot"+plotid);
            var obj = Instantiate(loadobj);
            obj.transform.SetParent(importStage.transform, false);
            //obj.transform.position = new Vector3(0, 0, 0);
            stagePrefab = obj;
            stageMod =obj.GetComponent<PlotInterface>();
            stageMod.initData();
            stageMod.startPlot(callback);
        }
        else
        {
            //界面对话剧情
            isLoadCSV = true;
            importStage.SetActive(false);
            showInterface();
            refreshVariate();
            plot.readConfig(plotid);                 //读数据
            PubTool.dumpString(plot.plotdata);
            maxplotShedule = plot.plotdata.Count;
            nextstep = callback;
            allowClick = true;
            nextPlotDialog();
        }
    }

    public void checkPlotClosed()
    {
        if (!isLoadCSV)
        {
            //预制体状态   stageMod有值
            if (stagePrefab == null) Debug.LogError("剧情组件 删除错误！！  非csv数据没有加载预制体！！！");
            Destroy(stagePrefab);
            stagePrefab = null;
            stageMod = null;
        }
        else
        {

        }
        hideInterface();
    }

    //下一阶段 对话强制剧情   (当前一定要记录剧情)
    public void nextPlotRank()
    {

    }

    //下一段对话
    private void nextPlotDialog()
    {
        if (!allowClick) return;
        allowClick = false;
        curplotShedule++;
        //判断结束
        if (curplotShedule >= maxplotShedule)
        {
            curPlotEnded();
            return;
        }
        curplotData = plot.plotdata[curplotShedule];
        StartCoroutine(timmerLock());
        //分析操作
        analyzeDialog();
    }
    //剧情结束
    private void curPlotEnded()
    {
        nextstep?.Invoke();
        nextstep = null;
    }
    //===============================       分析部分        ====================

    IEnumerator timmerLock()
    {
        float wtime = waittime;
        if (!curplotData[plot.iswait].Equals("0"))
            wtime = float.Parse(curplotData[plot.iswait]);
        yield return new WaitForSeconds(wtime);
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
    //判断是否加载外部预制体
    private bool readPlotLoadConfig()
    {
        int n = plot.plotid - plot.startStaticNum;
        return plot.needLoadConfig[n];
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
