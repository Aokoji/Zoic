using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : DDOLController<PlayerManager>
{
    PlayerControl player = PlayerControl.Instance;
    GameData data = GameData.Data;
    public void loadPlayerManager()
    {

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
    ///     获得经验:
    ///     经验获得参数 :  ||
    ///      ±1级 1.0   ||
    ///      - 1-5级  0.8  ||  + 1-5级 1.25   ||
    ///     - 6-10级  0.5  ||  + 6-8级 1.5    ||
    ///     -10级以下   0.2  ||  +9以上 1.8   ||
    /// </summary>
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
