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
                Debug.LogError("error way to create player data!");
            }
            return gamedata;
        }
    }
    private string baseDataLoadPath = "/Resources/Data/actorDataSave.txt";
    private Vector2 lastBornPos;
    private float coinMovement = 0.2f;          //金币奖励浮动倍率  80%-120%
    public string PLAYER = "player";
    private PlayerMessage playermessage;

    //外部初始化调用方法
    public static void initGameData()
    {
        gamedata = new GameData();
        gamedata.initData();
    }
    //data公共量初始化
    private void initData() {
        LastBornPos = new Vector2(PlayerPrefs.GetFloat("lastBornPosX", 0), PlayerPrefs.GetFloat("lastBornPosY", 0));
        loadPlayerData();
    }

    public Vector2 LastBornPos { get => lastBornPos; set => lastBornPos = value; }
    public float CoinMovement { get => coinMovement; set => coinMovement = value; }
    public PlayerMessage Playermessage { get => playermessage; set => playermessage = value; }


    private void loadPlayerData()  //加载数据1
    {
        BinaryFormatter bin = new BinaryFormatter();
        if (!File.Exists(Application.dataPath + baseDataLoadPath)) firstInitData();             //没有存档则创建存档
        else
        {
            FileStream file = File.Open(Application.dataPath + baseDataLoadPath,FileMode.Open);
            playermessage = (PlayerMessage)bin.Deserialize(file);
            file.Close();
        }
    }
    //存档
    private void saveLoad() 
    {
        BinaryFormatter bin = new BinaryFormatter();
        FileStream file = File.Create(Application.dataPath + baseDataLoadPath);
        bin.Serialize(file, playermessage);
        file.Close();
        if (File.Exists(Application.dataPath + baseDataLoadPath))
        {
            Debug.Log("save success!");
        }
        else
        {
            Debug.Log("save fail!");
        }
    }

    private void firstInitData()
    {
        playermessage = new PlayerMessage();
        playermessage.isFirstIn = true;
        playermessage.level = 1;
        playermessage.hpmax = 150;
        playermessage.hpcur = 150;
        playermessage.mpmax = 30;
        playermessage.mpcur = 30;
        playermessage.expcur = 0;
        playermessage.expmax = 50;
        playermessage.atk = 12;
        playermessage.def = 2;
        playermessage.strike = 0;
        playermessage.dodge = 0;
        playermessage.speed = 30;
        playermessage.adPat = 0;
        playermessage.apPat = 0;
        BinaryFormatter bin = new BinaryFormatter();
        FileStream file = File.Create(Application.dataPath + baseDataLoadPath);
        bin.Serialize(file, playermessage);
        file.Close();
        Debug.Log("save create success!");
    }


}