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
    //外调方法   检查是否处于冷却状态
    public void checkCoolDown()
    {
        if (skill.runDown > 0)
            setCoolDown(skill.runDown);
        else
            releaseCoolDown();
    }
    //进入冷却
    public void setCoolDown(int cool)
    {
        coolText.text = cool+"";
        cdShelter.SetActive(true);
        GetComponent<Button>().enabled = false;
    }
    //释放冷却
    public void releaseCoolDown()
    {
        cdShelter.SetActive(false);
        GetComponent<Button>().enabled = true;
    }



}
