using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatActorItem : MonoBehaviour
{
    public int numId;
    public GameObject arrow;
    public GameObject hpcontrol;
    public Image hpctl;
    public Image hpaux; //hp辅助条  显示掉血的虚血格
    public Text distanceText;       //与玩家距离
    public GameObject abnormalIconBoard;        //显示异常状态的图标父级

    private bool isgray=false;
    private bool clickLock = false;

    /// <summary>
    /// 被点击 外调方法
    /// </summary>
    public void onclickCustom()
    {
        if (!clickLock)
            chooseArrowChange(true);
    }
    //设置头顶箭头
    public void chooseArrowChange(bool isShow)
    {
        arrow.SetActive(isShow);
    }
    public void setGray(bool isgray)
    {
        this.isgray = isgray;
        //+++变灰
    }
    public bool getGray() { return isgray; }

    //设置血条可见
    public void setHpVisible(bool isshow)
    {
        hpcontrol.SetActive(isshow);
    }
    //设置血条长度
    public void setHpLine(float num)
    {
        hpctl.fillAmount = num;
        hpaux.fillAmount = 0;
    }
    //================================ 动作 ================
    public void playAttack()
    {

    }
    public void playDead(Action callback)
    {
        //+++播放死亡动画
        callback();
    }

    //================================  飞伤害=============
    public void showHitNumber()
    {

    }
    //------刷新异常状态
    public void refreshAbnormalIcon(List<abnormalState> abnormals)
    {

    }

    //=====================================   掉血缓动效果参数  =================
    private float timephyDec=1f;     //掉血时间参数
    private float timephyPer=GameStaticParamData.timePer20; //一秒20次刷新
    private float timephyRun;       //时间记录变量
    private float changtoPhy;       //改变量
    //掉血/加血 血条变动效果
    public void changePhysicalLine(bool ishit,float changeTo)
    {
        //初始化计时参数
        timephyRun = 0f;
        changtoPhy = changeTo;
        //初始化虚血
        hpaux.fillAmount = hpctl.fillAmount;
        //计算递减量
        float per=(changeTo - hpctl.fillAmount) / (timephyDec / timephyPer);
        //掉血动画
        StartCoroutine(runPhysicalChange(ishit, per, ishit));
    }
    IEnumerator runPhysicalChange(bool waitfirst,float per,bool ishit)
    {
        if (waitfirst)
        {
            //延时0.25秒掉真血 展示0.5s
            yield return new WaitForSeconds(0.25f);
            hpctl.fillAmount = changtoPhy;
            yield return new WaitForSeconds(0.5f);
            waitfirst = false;
            if(ishit)
                StartCoroutine(runPhysicalChange(waitfirst, per,ishit));
            else
                yield return false;
        }
        else
        {
            //逐渐掉血
            hpaux.fillAmount += per;
            timephyRun += timephyPer;
            //判断时间是否到1s
            if (timephyRun >= timephyDec)
            {
                if(ishit)
                    yield return false;
                else
                {
                    waitfirst = true;
                    StartCoroutine(runPhysicalChange(waitfirst, per, ishit));
                }

            }
            else
            {
                //没到
                yield return new WaitForSeconds(timephyPer);
                StartCoroutine(runPhysicalChange(waitfirst, per,ishit));
            }
        }
    }
    public bool getLock() { return clickLock; }
    public void changeLock(bool clicklock)
    {
        clickLock = clicklock;
    }
    /// <summary>
    /// 改变距离显示
    /// </summary>
    public void changeDistance(int dis)
    {
        distanceText.text = dis+"";
    }

}
