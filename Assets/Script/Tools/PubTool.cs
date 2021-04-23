using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;




public class PubTool : DDOLController<PubTool>
{
    private bool stepLock = false;
    private bool stepAllow = true;
    private List<Func<bool>> stepList = new List<Func<bool>>();

    private void Update()
    {
        updateStep();
    }

    //延时方法  待测试
    public void laterDo(float time,Action action)
    {
        StartCoroutine(lateraction(time, action));
    }
    IEnumerator lateraction(float time,Action action)
    {
        yield return new WaitForSeconds(time);
        action();
    }

    //序列方法函数    待测试
    private void updateStep()
    {
        stepAllow = stepList.Count > 0;
        if (!stepLock)
        {
            if (stepAllow)
            {
                stepLock = true;
                Func<bool> callback = stepList[0];
                stepList.Remove(stepList[0]);
                stepLock = callback();
            }
        }
    }
    public void addStep(Func<bool> action)
    {
        stepList.Add(action);
    }

}
