using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotView : MonoBehaviour
{




    private void showInterface()
    {//显示界面  内部用
        gameObject.SetActive(true);
    }

    //隐藏界面
    public void hideInterface()
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
