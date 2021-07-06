using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//剧情控制器
public class PlotController : DDOLController<PlotController>
{
    private int plotCount;
    private bool ismainPloting = false;
    private bool issidePloting = false;
    public void initData()
    {
        plotCount = GameData.Data.Playermessage.plotCount;
    }

    //触发主线!
    public void mainplotTrigger()
    {

    }

    //支线强制剧情
    public void sideplotTrigger()
    {

    }
}
