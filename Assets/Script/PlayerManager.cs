using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : DDOLController<PlayerManager>
{
    public void loadPlayerManager()
    {//初始化方法    我也不知道为啥起这个名

    }
    //玩家升级
    public void levelUp()
    {
        EventTransfer.instance.levelUpAction(); //派发升级事件
    }
    //升级技能
    public void upgradeSkill(int skillID)
    {

    }
    //获得技能点
    public void getSkillPoint()
    {
        GameData.Data.skillPointGot();
        //EventTransfer.instance.getSkillAction();    //派发获得事件
    }
    /// <summary>
    ///     计算经验:
    ///     经验获得参数 :  ||
    ///      ±1级 1.0   ||
    ///      - 1-5级  0.8  ||  + 1-5级 1.25   ||
    ///     - 6- 9级  0.5  ||  + 6-8级 1.5    ||
    ///     -10级以下   0.2  ||  +9以上 1.8   ||
    /// </summary>
    public int differExp(int playLv,int enemyLv,int exp)
    {
        int minus = enemyLv - playLv;
        float multi = 0;
        if (minus >= 9)
            multi = 1.8f;
        else if (minus >= 6)
            multi = 1.5f;
        else if (minus > 1)
            multi = 1.25f;
        else if (minus >= -1)
            multi = 1f;
        else if (minus >= -5)
            multi = 0.8f;
        else if (minus >= -9)
            multi = 0.5f;
        else
            multi = 0.2f;
        exp =(int)Mathf.Floor(exp * multi);
        return exp;
    }
    //获得经验
    public void addExp(int num)
    {
        PubTool.instance.addLogger("获得经验：" + num);
        int level = GameData.Data.addExp(num);
        if (level > 0)
            for(int i = 0; i < level; i++)
            {
                EventTransfer.instance.levelUpAction();
                getSkillPoint();
            }
    }
    //获得物品
    public void addItem(List<int> spoils)
    {

    }
    //获得货币
    public void addCoin(int type,int num)
    {

    }
}
