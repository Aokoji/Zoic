using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

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
                Debug.Log("chuangjian--------------");
            }
            return gamedata;
        }
    }
    private string baseDataLoadPath = "/Resources/Data/actorDataSave.txt";
    private Vector2 lastBornPos;
    private float coinMovement = 0.2f;
    public string PLAYER = "player";
    private void initData()
    {
        LastBornPos =new Vector2(PlayerPrefs.GetFloat("lastBornPosX", 0), PlayerPrefs.GetFloat("lastBornPosY", 0));
        //loadPlayerData_BaseData();
    }
    private void loadPlayerData_BaseData()  //加载数据1
    {
        BinaryFormatter bin = new BinaryFormatter();
        FileStream file = File.Create(Application.dataPath + baseDataLoadPath);
    }




    public Vector2 LastBornPos { get => lastBornPos; set => lastBornPos = value; }
    public float CoinMovement { get => coinMovement; set => coinMovement = value; }
    //存档
    private void saveLoad() 
    {
        Debug.Log("write success!");
    }
    //外部初始化调用方法
    public static void initGameData() {
        gamedata = null;
        Debug.Log("qingkong--------------");
    }      //+++存在问题  是先创建还是先清除  需要测试


}