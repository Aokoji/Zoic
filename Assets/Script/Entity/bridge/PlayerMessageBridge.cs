using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMessageBridge : MessageBridgeInterface
{
    private PlayerMessage playermessage;
    public PlayerMessageBridge(PlayerMessage playermessage)
    {
        this.playermessage = playermessage;
    }
    public void paddingData()
    {
        playermessage.paddingData();
    }

    public PlayerMessage getInstance() { return playermessage; }
    /// <summary>
    /// 按减法计算当前体力
    /// </summary>
    public void subCurPhysical(int hit)
    {
        playermessage.subCurPhysical(hit);
    }
    /// <summary>
    /// 按减法计算当前精力
    /// </summary>
    public void subCurVigor(int hit)
    {
        playermessage.subCurVigor(hit);
    }
    public playerParam getPlayerParamData()
    {
        return playermessage.data;
    }

    public string getPlayerName()
    {
        return playermessage.name;
    }
    public bool getFirstIn()
    {
        return playermessage.isFirstIn;
    }
    public int getplotCount()
    {
        return playermessage.plotCount;
    }
}
