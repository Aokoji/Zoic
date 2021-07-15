using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO.Compression;
using System.Runtime.Serialization;
using System;
using System.Text;

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
    private string gameDataLoadPath = "/Resources/Data/game/gameMessageSave.json";
    private string gameDataLoadSign = "/Resources/Data/sign.dll";
    private float coinMovement = 0.2f;          //金币奖励浮动倍率  80%-120%
    private int expBase = 40;                           //等级经验基数
    private int hpBase = 150;
    private int mpBase = 30;
    private int atkBase = 12;
    private int defBase = 2;
    public string PLAYER = "player";



    private DataPlayMessage dataplaymessage;           //游戏配置数据
    private PlayerMessage playermessage;        //-----------------------------------------------玩家数据----------

    //外部初始化调用方法
    public static void initGameData()
    {
        gamedata = new GameData();
        gamedata.initData();
    }
    //data公共量初始化
    private void initData() {
        //LastBornPos = new Vector2(PlayerPrefs.GetFloat("lastBornPosX", 0), PlayerPrefs.GetFloat("lastBornPosY", 0));
        loadGameMessageData();
        loadPlayerData();
    }

    //public Vector2 LastBornPos { get => lastBornPos; set => lastBornPos = value; }
    public float CoinMovement { get => coinMovement; set => coinMovement = value; }
    public PlayerMessage Playermessage { get => playermessage; set => playermessage = value; }
    public DataPlayMessage DataPlaymessage { get => dataplaymessage; set => dataplaymessage = value; }

    //事件调用  升级
    public void levelUp()
    {
        actorGrowthCurve(playermessage.level);
        playermessage.level++;
        PubTool.Instance.addLogger("人物升级!" + playermessage.level);
    }
    //事件调用  获得技能点
    public void skillPointGot()
    {
        playermessage.skillPoint++;
    }
    //外部调用 获得经验
    public int addExp(int num)
    {
        int curExp = playermessage.expcur + num;
        int finalLevel = 0;
        while (curExp >= playermessage.expmax)
        {
            curExp -= playermessage.expmax;
            levelUp();  //升级
            finalLevel++;
            levelExp(playermessage.level);  //加经验上限
        }
        playermessage.expcur = curExp;
        return finalLevel;
    }









    //---------------------------------------------------------------------升级配置-----------------------
    //人物成长曲线
    private void actorGrowthCurve(int level)
    {
        //暂时不用    log(1+x^2+2*x)+2^(x/20)+x^2/1000-1    半s型曲线
        //人物成长曲线公式1-100   0-50multi     2^(x/20)+x/5-1    持续增长型
        //生命曲线  30multi        2^(x/24)+x/8-1    
        //精力曲线  20multi        x/6+log(x)+1/2
        //攻击曲线  50multi        2^(x/20)+x/5-1
        //防御曲线  25multi        x/4
        //经验曲线  125multi      2^(x/15)+x/4+x^(1/4)-1          +0.1*等级

        //体力提升  额外回复+20%
        int numPer = (int)Math.Floor((Math.Pow(2, (level + 1) / 24) + (level + 1) / 8 - 1) - (Math.Pow(2, level / 24) + level / 8 - 1)) * hpBase;
        playermessage.hpmax += numPer;
        playermessage.hpcur += numPer + (int)(playermessage.hpmax * 0.2);
        if (playermessage.hpcur > playermessage.hpmax)
            playermessage.hpcur = playermessage.hpmax;
        //精力提升  额外恢复+30%
        numPer = (int)Math.Floor((Math.Log(level + 1) + (level + 1) / 6 + 1 / 2) - (Math.Log(level) + level / 6 + 1 / 2)) * mpBase;
        playermessage.mpmax += numPer;
        playermessage.mpcur += numPer + (int)(playermessage.hpmax * 0.3);
        if (playermessage.mpcur > playermessage.mpmax)
            playermessage.mpcur = playermessage.mpmax;
        //攻防提升
        numPer = (int)Math.Floor((Math.Pow(2, (level + 1) / 20) + (level + 1) / 5 - 1) - (Math.Pow(2, level / 20) + level / 5 - 1)) * atkBase;
        playermessage.atk += numPer;
        numPer = (int)((level + 1.0) / 4 - level / 4) * defBase;
        playermessage.def += numPer;
    }
    //加经验上限
    private void levelExp(int level)
    {
        double maxexp = expBase * (Math.Pow(2, level / 15) + level / 4 + Math.Pow(level, 1 / 4) - 1 + level / 10);
        playermessage.expmax = (int)Math.Floor(maxexp);
    }
    //-----------------------------------------------------------------------------存储配置-----------------------------------------
    // 加载游戏部分配置数据       （自动加载）
    private void loadGameMessageData()
    {
        PubTool.Instance.addLogger("读取游戏配置");
        //BinaryFormatter bin = new BinaryFormatter();
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
            PubTool.Instance.addLogger("创建游戏配置");
            initGameDataMessage1();             //没有档则创建档
        }
        else
        {
            byte[] jsbt = File.ReadAllBytes(Application.dataPath + gameDataLoadPath);
            string read = Encoding.ASCII.GetString(jsbt);
            dataplaymessage= JsonUtility.FromJson<DataPlayMessage>(read);
        }
    }
    //  记录游戏部分配置数据      （自动记录）
    public void saveGameMessageData()
    {
        PubTool.Instance.addLogger("保存配置");
        string json = JsonUtility.ToJson(dataplaymessage);
        byte[] js = Encoding.ASCII.GetBytes(json.ToCharArray());
        File.WriteAllBytes(Application.dataPath + gameDataLoadPath, js);
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
        playermessage.hpmax = hpBase;
        playermessage.hpcur = hpBase;
        playermessage.mpmax = mpBase;
        playermessage.mpcur = mpBase;
        playermessage.expcur = 0;
        playermessage.expmax = expBase;
        playermessage.atk = atkBase;
        playermessage.def = defBase;
        playermessage.strike = 0;
        playermessage.dodge = 0;
        playermessage.speed = 30;
        playermessage.adPat = 0;
        playermessage.apPat = 0;

        playermessage.skillPoint = 1;
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
    private void initGameDataMessage1()
    {
        dataplaymessage = new DataPlayMessage();
        dataplaymessage.combatIDCount = 0;
        saveGameMessageData();
    }


}