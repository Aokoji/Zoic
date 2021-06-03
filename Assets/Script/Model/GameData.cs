using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO.Compression;
using System.Runtime.Serialization;
using System;

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
    private string baseDataLoadPath = "/Resources/Data/game/actorDataSave.txt";
    private string gameDataLoadPath = "/Resources/Data/game/gameMessageSave.txt";
    private string gameDataLoadSign = "/Resources/Data/sign.dll";
    private Vector2 lastBornPos;
    private float coinMovement = 0.2f;          //金币奖励浮动倍率  80%-120%
    public string PLAYER = "player";
    private PlayerMessage playermessage;        //玩家数据
    private DataPlayMessage dataplaymessage;           //游戏配置数据


    //外部初始化调用方法
    public static void initGameData()
    {
        gamedata = new GameData();
        gamedata.initData();
    }
    //data公共量初始化
    private void initData() {
        LastBornPos = new Vector2(PlayerPrefs.GetFloat("lastBornPosX", 0), PlayerPrefs.GetFloat("lastBornPosY", 0));
        //loadGameMessageData();
        dataplaymessage = new DataPlayMessage();
        dataplaymessage.combatIDCount = 1;
        loadPlayerData();
    }

    public Vector2 LastBornPos { get => lastBornPos; set => lastBornPos = value; }
    public float CoinMovement { get => coinMovement; set => coinMovement = value; }
    public PlayerMessage Playermessage { get => playermessage; set => playermessage = value; }
    public DataPlayMessage DataPlaymessage { get => dataplaymessage; set => dataplaymessage = value; }

    // 加载游戏部分配置数据       （自动加载）
    private void loadGameMessageData()
    {
        PubTool.Instance.addLogger("读取游戏配置");
        BinaryFormatter bin = new BinaryFormatter();
        if (!Directory.Exists("Assets/Resources/Data/game")) Directory.CreateDirectory("Assets/Resources/Data/game");
        if (!File.Exists(Application.dataPath + gameDataLoadPath))
        {
            /*
            if (File.Exists(Application.dataPath + gameDataLoadSign))
            {
                Debug.LogError("游戏配置错误!");
                PubTool.Instance.addLogger("游戏配置错误!");
                Application.Quit();
            }*/
            initGameDataMessage();             //没有档则创建档
            PubTool.Instance.addLogger("创建游戏配置");
        }
        else
        {
            FileStream file = File.Open(Application.dataPath + gameDataLoadPath, FileMode.Open);
            dataplaymessage = (DataPlayMessage)bin.Deserialize(file);
            file.Close();
        }
    }
    //  记录游戏部分配置数据      （自动记录）
    public void saveGameMessageData()
    {
        PubTool.Instance.addLogger("保存配置");
        BinaryFormatter bin = new BinaryFormatter();
        FileStream file = File.Create(Application.dataPath + gameDataLoadPath);
        bin.Serialize(file, playermessage);
        file.Close();
    }
    //加载玩家数据       (手动加载 + 自动加载)
    public void loadPlayerData()  
    {
        PubTool.Instance.addLogger( "读取游戏存档");
        BinaryFormatter bin = new BinaryFormatter();
        if (!File.Exists(Application.dataPath + baseDataLoadPath))
        {
            firstInitData();             //没有存档则创建存档
            PubTool.Instance.addLogger( "创建存档");
        }
        else
        {
            FileStream file = File.Open(Application.dataPath + baseDataLoadPath, FileMode.Open);
            //file.Seek(0, SeekOrigin.Begin);
            playermessage = (PlayerMessage)bin.Deserialize(file);
            if(playermessage!=null) PubTool.Instance.addLogger( "读取成功");
            else PubTool.Instance.addLogger( "读取失败");
            file.Close();
        }
    }
    //  玩家数据存档      （手动存档）
    public void saveLoad() 
    {
        PubTool.Instance.addLogger( "保存游戏");
        BinaryFormatter bin = new BinaryFormatter();
        FileStream file = File.Create(Application.dataPath + baseDataLoadPath);
        bin.Serialize(file, playermessage);
        file.Close();
        if (File.Exists(Application.dataPath + baseDataLoadPath))
        {
            PubTool.Instance.addLogger( "存储成功");
            Debug.Log("save success!");
        }
        else
        {
            PubTool.Instance.addLogger( "存储失败");
            Debug.Log("save fail!");
        }
    }
    //第一次游戏 玩家数据初始化
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
        skillSave skill1 = new skillSave();
        skill1.skillID = 4;
        skill1.skillLevel = 1;
        playermessage.skills.Add(skill1);
        BinaryFormatter bin = new BinaryFormatter();
        FileStream file = File.Create(Application.dataPath + baseDataLoadPath);
        bin.Serialize(file, playermessage);
        file.Close();
        Debug.Log("save create success!");
    }
    //初始化游戏配置
    private void initGameDataMessage()
    {
        dataplaymessage = new DataPlayMessage();
        dataplaymessage.combatIDCount = 0;

        BinaryFormatter bin = new BinaryFormatter();
        FileStream file = File.Create(Application.dataPath + gameDataLoadPath);
        File.Create(Application.dataPath + gameDataLoadSign);
        bin.Serialize(file, dataplaymessage);
        file.Close();
        Debug.Log("data create success!");
    }


}