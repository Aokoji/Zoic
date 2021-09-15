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
    private string NONE = "none";
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
    public void playAnimation(GameObject obj, string aniName,bool loop,Action callBack)
    {
        Animator anim = obj.GetComponent<Animator>();
        float time=getAnimTime(anim, aniName);
        if (time == 0)
        {
            Debug.LogError("animation  time  is  null  !");
            return;
        }
        anim.Play(aniName);
        void action()
        {
            if(!loop) anim.Play(NONE);
            callBack();
        }
        PubTool.Instance.laterDo(time, action);
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

    /// <summary>
    /// 调用动画播放器 播放攻击回合动画
    /// </summary>
    /// <param name="combat">控制场景</param>
    /// <param name="result">结果</param>
    public void playCombatBeHit(CombatView combat, AttackResultData result,Action action)
    {//播放被击动画  并调用合适的视图面板变动
        Debug.Log("【播放动画】攻击方:"+result.sourceActor.Name+"    受击方:"+result.takenActor[0].Name);
        Debug.Log("【参数】异常状态"+result.takenActor[0].Abnormal.Count+"个【附加异常】"+result.inflictionID.Count);
        //if (result.sourceActor.AtkExtra.Count != 0) { Debug.Log(result.sourceActor.AtkExtra[0].id); }
        //if (result.extraHit!=null && result.extraHit.Length != 0) { Debug.Log(result.extraHit[0]); }
        Debug.Log("【战斗数据】"+ result.sourceActor.Name + "："+result.sourceActor.UnitData["curHp"]+"    "+ result.takenActor[0].Name + "："+result.takenActor[0].UnitData["curHp"]);
        //全部播放完成后
        if (result.willDeadActor.Count != 0)
        {

        }
        playAnimation(result.sourceActor.Prefab.GetComponentInChildren<Animator>().gameObject, "testAction",false, action);

    }

    public void playCombarRoundSettle(wholeRoundData rounddata,Action action)
    {

    }




    public void playCombatSceneTransform(CombatView combat,int type)
    {//加载入场动画  普通切换  动画为 0
        //PubTool.Instance.addStep(loadAnim);

    }


}
