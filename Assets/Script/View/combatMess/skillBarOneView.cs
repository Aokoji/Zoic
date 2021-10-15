using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class skillBarOneView : MonoBehaviour
{
    public Text skillName;
    public Text skillCost1;
    public Text skillCost2;
    public GameObject cdShelter;    //cd挡板
    public Text coolText;
    public GameObject enoughShelter;        //技能消耗不足挡板
    public Text enoughText;

    private SkillStaticData skill;
    public Action<SkillStaticData> btnclick;

    public void initData(SkillStaticData skill)
    {
        this.skill = skill;
        skillName.text = skill.name;
        skillCost1.text = skill.expend1.ToString();
        skillCost2.text = skill.expend2.ToString();
        releaseCoolDown();
        initEvent();
    }
    private void initEvent()
    {
        GetComponent<Button>().onClick.AddListener(() =>                             //闭包写法  网上抄的
        {
            btnclick(skill);
        });
    }
    /// <summary>
    /// 外调方法   检查是否处于冷却状态
    /// 由于检查消耗不足是外部判断  所以在冷却时优先冷却显示 隐藏消耗不足，在单纯消耗不足时由外部调用显示
    /// </summary>
    public bool checkCoolDown()
    {
        if (skill.runDown > 0)
        {
            setCoolDown(skill.runDown);
            enoughShelter.SetActive(false);
            return true;
        }
        else
        {
            releaseCoolDown();
            return false;
        }
    }
    //消耗不足显示
    public void setNotEnough(string n)
    {
        enoughShelter.SetActive(true);
        enoughText.text = n + "不足！";
        GetComponent<Button>().enabled = false;
    }
    //消耗足够
    public void setEnough()
    {
        enoughShelter.SetActive(false);
        GetComponent<Button>().enabled = true;
    }
    //进入冷却
    public void setCoolDown(int cool)
    {
        coolText.text = "冷却中  " + cool;
        cdShelter.SetActive(true);
        GetComponent<Button>().enabled = false;
    }
    //释放冷却
    public void releaseCoolDown()
    {
        cdShelter.SetActive(false);
        GetComponent<Button>().enabled = true;
    }

    //获取消耗
    public int getExpend1() { return skill.expend1; }
    public int getExpend2() { return skill.expend2; }

}
