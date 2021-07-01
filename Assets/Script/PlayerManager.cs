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
        EventTransfer.instance.getSkillAction();    //派发获得事件
    }
    //获得经验
    public void addExp(int num)
    {

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
