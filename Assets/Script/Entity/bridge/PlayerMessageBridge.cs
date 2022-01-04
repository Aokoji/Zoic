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
    //获取玩家name
    public string getPlayerName()
    {
        return playermessage.name;
    }
    //获取第一次进游戏
    public bool getFirstIn() { return true; }// playermessage.isFirstIn; }

    //----------------------    剧情控制------------------
    //获取剧情序号
    public int getplotCount() { return 102; }// playermessage.plotCount;}
    //初始化剧情
    public void initPlotCount() { playermessage.plotCount = 101; }
    //推剧情
    public void goonPlot() { playermessage.plotCount++; }
    //---------------------------------------------------------------

    //获取上次场景
    public int getLastScene() { return playermessage.lastSceneNum; }
    //获取上次位置
    public Vector2 getLastPos(){return new Vector2(playermessage.lastPosX, playermessage.lastPosY);}
}
