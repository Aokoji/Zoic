﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//动画控制器
public class AnimationController : DDOLController<AnimationController>
{
    //manager调用  初始化并自动创建单例
    public void initController()
    {

    }

    //外部调用
    public delegate void CombatAction();
    public event CombatAction combatNextStep = null;
    public void cleanNextStepAction()
    {//清空下一步事件
        Delegate[] delArray = combatNextStep.GetInvocationList();
        for(int i = 0; i < delArray.Length; i++)
        {
            combatNextStep -= delArray[i] as CombatAction;
        }
    }
    public void playCombatBeHit(CombatView combat,AttackResult result)
    {//播放被击动画  并调用合适的视图面板变动

        //全部播放完成后
        combatNextStep();
    }

    public void playCombatSceneTransform(CombatView combat,int type)
    {//加载入场动画  普通切换  动画为 0
        Func<bool> loadAnim = combat.playEnterScene;
        PubTool.Instance.addStep(loadAnim);

    }


}
