using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAnimationControl : MonoBehaviour
{
    private Action nextRunStep;
    public void initData(Action action)
    {
        nextRunStep = action;
    }

    /// <summary>
    /// 调用动画播放器 播放攻击回合动画
    /// </summary>
    /// <param name="combat">控制场景</param>
    /// <param name="result">结果</param>
    public void playCombatBeHit(CombatView combat, AttackResultData result, List<CombatMessage> actorList, Action action)
    {//播放被击动画  并调用合适的视图面板变动
        PubTool.dumpString("【战斗数据】", result);
        //播放攻击方动画
        PubTool.Instance.addAnimStep(delegate (Action callback)
        {
            AnimationController.Instance.playAnimation(actorList[result.sourceActor].Prefab, result.animTypeSource, false, callback);
        });
        //播放受击方动画
        PubTool.Instance.addAnimStep(delegate (Action callback)
        {
            int count = result.animTypeTaken.Count;
            foreach(var id in result.takenActor)
            {
                if (result.isHit)
                {   //判断命中
                    if (result.isHitRare[id])
                    {
                        actorList[id].showPhysicalChange(true);
                    }
                }
                else if(result.iscure)
                {
                    actorList[id].showPhysicalChange(true);
                }
                AnimationController.Instance.playAnimation(actorList[id].Prefab, result.animTypeTaken[id], false, delegate () {
                    count--;
                    if (count <= 0)
                        callback();
                });
                //+++播放的同时显示数值
            }
        });
        //全部播放完成后  播放死亡
        if (result.willDeadActor.Count != 0)
        {
            PubTool.Instance.addAnimStep(delegate (Action callback)
            {
                int count = result.willDeadActor.Count;
                //挨个播死亡动画
                foreach (int id in result.willDeadActor)
                {
                    AnimationController.Instance.playAnimation(actorList[id].Prefab, GameStaticParamData.combatAnimNameList.deadName, false, delegate () {
                        count--;
                        if (count <= 0)
                            callback();
                    });
                }
            });
        }
        //攻击播放结束
        PubTool.Instance.addAnimStep(delegate (Action callback)
        {
            callback();
            action();
        });
    }

    /// <summary>
    /// 回合结算动画
    /// </summary>
    public void playCombarRoundSettle(wholeRoundData rounddata, List<CombatMessage> actorList, Action action)
    {
        PubTool.dumpString("【统计结算】", rounddata);
        action();
    }
}
