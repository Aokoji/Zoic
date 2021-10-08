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
    /// <summary>
    /// 外部调用 播放组件动画
    /// </summary>
    /// <param name="obj">组件gameobj</param>
    /// <param name="aniName">动画名</param>
    /// <param name="loop">循环</param>
    /// <param name="callBack">回调</param>
    public void playAnimation(GameObject obj, string aniName,bool loop,Action callBack)
    {
        Animator anim = obj.GetComponent<Animator>();
        float time=getAnimTime(anim, aniName);
        if (time == 0)
        {
            Debug.LogError("animation  time  is  null  !");
            return;
        }
        anim.Play(aniName,0,0f);
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


    public void playCombatSceneTransform(CombatView combat,int type)
    {//加载入场动画  普通切换  动画为 0
        //PubTool.Instance.addStep(loadAnim);

    }


}
