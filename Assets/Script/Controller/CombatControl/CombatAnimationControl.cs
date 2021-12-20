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
        //移动表现判断        有frontall标识代表一定有移动
        if (result.isfrontAll)
        {
            //先判断移动
            moveCalculate(combat, result, actorList);
        }
        //判断逃跑
        if (result.isrun)
        {
            //显示行动内容提示板
            PubTool.Instance.addAnimStep(delegate (Action callback)
            {
                combat.showTips1Second(actorList[result.sourceActor].Name1+"逃跑了", callback);
            });
            //+++逃跑动画
        }
        else if (!result.isOnlyRun)
        {
            //显示行动内容提示板
            PubTool.Instance.addAnimStep(delegate (Action callback)
            {
                combat.showTips1Second("", callback);
            });
            //播放攻击方动画  
            PubTool.Instance.addAnimStep(delegate (Action callback)
            {
                actorList[result.sourceActor].PrefabCtrl.playAction(result.animTypeSource, callback);
            });
            //播放受击方动画
            PubTool.Instance.addAnimStep(delegate (Action callback)
            {
                int count = result.takenActor.Count;
                foreach (var item in result.takenActor)
                {
                    if (result.isHit)
                    {   //判断命中
                        if (item.israte)
                        {
                            actorList[item.index].showPhysicalChange(true);
                        }
                    }
                    else if (result.iscure)
                    {
                        actorList[item.index].showPhysicalChange(true);
                    }
                    actorList[item.index].PrefabCtrl.playAction(item.animTypeTaken, delegate () {
                        count--;
                        if (count <= 0)
                            callback();
                    });
                    //+++播放的同时显示数值
                }
            });
            //移动表现判断  (攻击后的移动)
            if (!result.isfrontAll&&result.isMoveInstruct)
            {
                //动作后移动动画
                moveCalculate(combat,result,actorList);
            }
        }
        //全部播放完成后  播放死亡
        if (result.willDeadActor.Count != 0)
        {
            PubTool.Instance.addAnimStep(delegate (Action callback)
            {
                int count = result.willDeadActor.Count;
                //挨个播死亡动画
                foreach (int id in result.willDeadActor)
                {
                    actorList[id].PrefabCtrl.playDead(delegate () {
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

    //移动测算
    private void moveCalculate(CombatView combat, AttackResultData result, List<CombatMessage> actorList)
    {
        PubTool.Instance.addAnimStep(delegate (Action callback)
        {//显示提示
            if (actorList[result.sourceActor].IsPlayer)
            {
                //如果是玩家 则是普通提示      加称为
                combat.showTips1Second(actorList[result.sourceActor].Name1 + DataTransTool.combatMoveActionDescribeTrans(result.moveDistance), callback);
                //移动表现
                foreach (var it in actorList)
                {
                    if (it.IsPlayer) continue;
                    it.refreshDistance();  //玩家移动所有enemy都要动 (移动表现也在里边)
                }
            }
            else
            {//++++++是敌方  则根据状态修饰移动动作
                combat.showTips1Second(actorList[result.sourceActor].Name1 + DataTransTool.combatMoveActionDescribeTrans(result.moveDistance), callback);
                //移动表现
                actorList[result.sourceActor].refreshDistance();
            }
        });
    }

    /// <summary>
    /// 回合结算动画
    /// </summary>
    public void playCombatRoundSettle(CombatView combat, wholeRoundData rounddata, CombatMessage source, Action action)
    {
        PubTool.dumpString("【统计结算】", rounddata);
        //显示行动内容提示板
        PubTool.Instance.addAnimStep(delegate (Action callback)
        {
            combat.showTips1Second("回合结束", callback);
        });
        int count = 0;
        //计算结算伤害(自己)
        PubTool.Instance.addAnimStep(delegate (Action callback)
        {
            foreach (var it in rounddata.specialNumber)
            {
                source.PrefabCtrl.showHitNumber();
                //弹数字（伤害或治疗）
                //rounddata.specialType[count]; //这是数字类型
                count++;
            }
        });
        //刷新计算buff显示
        PubTool.Instance.addAnimStep(delegate (Action callback)
        {
            source.PrefabCtrl.refreshAbnormalIcon(rounddata.settleBuffExist);
            //结算属性
            source.Abnormal = rounddata.settleBuffExist;
            source.paddingData();
        });
        //判断死亡
        if(rounddata.isRoundDead)
            PubTool.Instance.addAnimStep(delegate (Action callback)
            {
                source.PrefabCtrl.playDead(callback);
            });
        //结束
        PubTool.Instance.addAnimStep(delegate (Action callback)
        {
            callback();
            action();
        });
    }
}
