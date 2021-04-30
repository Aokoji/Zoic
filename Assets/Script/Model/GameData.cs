using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    private static GameData gamedata = null;
    public static GameData Data
    {
        get
        {
            if (gamedata == null)
            {
                gamedata = new GameData();
                gamedata.initData();
            }
            return gamedata;
        }
    }

    private Vector2 lastBornPos;
    private float coinMovement = 0.2f;

    private void initData()
    {
        LastBornPos =new Vector2(PlayerPrefs.GetFloat("lastBornPosX", 0), PlayerPrefs.GetFloat("lastBornPosY", 0));

    }





    public Vector2 LastBornPos { get => lastBornPos; set => lastBornPos = value; }
    public float CoinMovement { get => coinMovement; set => coinMovement = value; }

    //外部初始化调用方法
    public static void initGameData() { gamedata = null; }
}
