using System.Collections;
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
        if (combatNextStep == null) return;
        Delegate[] delArray = combatNextStep.GetInvocationList();
        for(int i = 0; i < delArray.Length; i++)
        {
            combatNextStep -= delArray[i] as CombatAction;
        }
    }
    //外部调用 播放组件动画
    public void playAnimation(GameObject obj, string aniName,Action callBack)
    {
        Animator anim = obj.GetComponent<Animator>();
        float time=getAnimTime(anim, aniName);
        if (time == 0)
        {
            Debug.LogError("animation  time  is  null  !");
            return;
        }
        anim.Play(aniName);
        PubTool.Instance.laterDo(time, callBack);
    }
    //工具  获取动画时长
    private float getAnimTime(Animator anim, string aniName)
    {
        if (null == anim)
        {
            Debug.LogError("animation  is  null  !");
            return 0;
        }
        var clips = anim.runtimeAnimatorController.animationClips;
        if (null==clips|| clips.Length <= 0)
        {
            Debug.LogError("animation  clips  is  wrong  !");
            return 0;
        }
        AnimationClip clip;
        for(int i = 0, len = clips.Length; i < len; ++i)
        {
            clip = clips[i];
            if (null != clip && clip.name == aniName)
                return clip.length;
        }
        return 0;
    }

    public void playCombatBeHit(CombatView combat,AttackResult result)
    {//播放被击动画  并调用合适的视图面板变动
        Debug.Log("【播放攻击动画】攻击方:"+result.sourceActor.Name+"    受击方:"+result.takenActor[0].Name+"   伤害结算:"+result.hitCount);
        Debug.Log("【战斗数据】"+ result.sourceActor.Name + "："+result.sourceActor.UnitData["curHp"]+"    "+ result.takenActor[0].Name + "："+result.takenActor[0].UnitData["curHp"]);
        //全部播放完成后
        combatNextStep();
    }

    public void playCombatSceneTransform(CombatView combat,int type)
    {//加载入场动画  普通切换  动画为 0
        //PubTool.Instance.addStep(loadAnim);

    }


}
