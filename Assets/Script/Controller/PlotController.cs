using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//剧情控制器
public class PlotController : DDOLController<PlotController>
{
    private PlotView plotview;
    private int plotCount;      //主线进度
    private bool ismainPloting = false;
    private bool issidePloting = false;
    public void initData()
    {
        plotCount = GameData.Data.Playermessage.plotCount;
        initPlotView();
    }

    //触发主线!
    public void mainplotTrigger()
    {

    }

    //支线强制剧情
    public void sideplotTrigger()
    {

    }

    //加载剧情父级视图   (加入到主ui的二级  与 图标父级平级)
    private void initPlotView()
    {
        GameObject loadobj = Resources.Load<GameObject>("Entity/PlotUI");
        var uiTarget = Instantiate(loadobj);
        uiTarget.name = "PlotView";
        uiTarget.transform.SetParent(CanvasLoad.canvasui.transform, false);
        uiTarget.transform.position = CanvasLoad.canvasui.transform.position;
        plotview = uiTarget.GetComponent<PlotView>();
        plotview.hideInterface();
    }
}
