using System.Collections;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Text;

public class GameData: DDOLData<GameData>
{/*
    private static GameData data = null;
    public static GameData Data
    {
        get
        {
            if (data == null)
            {
                data = new GameData();
                data.initData();
                Debug.LogError("error way to create player data!");
            }
            return data;
        }
    }*/
    private string baseDataLoadPath = "/Resources/Data/game/actorDataSave.txt";
    private string gameDataLoadPath = "/Resources/Data/game/gameMessageSave.json";
    private string gameDataLoadSign = "/Resources/Data/sign.dll";
    private int expBase = 40;                           //等级经验基数
    private int hpBase = 150;
    private int mpBase = 30;
    private int atkBase = 12;
    private int defBase = 2;
    private int phyRecover = 3;
    private float phyRecMulti = 0.2f;
    private int vigRecover = 2;
    private float vigRecMulti = 0.2f;
    public string PLAYER = "player";



    private DataPlayMessage dataplaymessage;           //游戏配置数据
    private PlayerMessage playermessage;        //-----------------------------------------------玩家数据----------
    public PlayerMessageBridge playerBridge;        //玩家数据桥接类

    private bool useTestData = true;    //使用测试数据

    //外部初始化调用方法
    public void initGameData()
    {
        //data = new GameData();
        initData();
    }
    //data公共量初始化
    private void initData() {
        //LastBornPos = new Vector2(PlayerPrefs.GetFloat("lastBornPosX", 0), PlayerPrefs.GetFloat("lastBornPosY", 0));
        loadGameMessageData();
        loadPlayerData();
    }

    //public Vector2 LastBornPos { get => lastBornPos; set => lastBornPos = value; }
    public DataPlayMessage DataPlaymessage { get => dataplaymessage; set => dataplaymessage = value; }

    //事件调用  升级
    public void levelUp()
    {
        actorGrowthCurve(playermessage.level,playermessage.jobOrder);
        playermessage.level++;
        PubTool.Instance.addLogger("人物升级!" + playermessage.level);
        //获得能力点  1-10  2   10-15   1  15+  2
        int point = GameStaticParamData.getAbilityPoint(playermessage.level);
        playermessage.data.talentPoint+=point;
        playermessage.data.maxTalentPoint += point;
    }
    //事件调用  获得技能点
    public void skillPointGot()
    {
        playermessage.skills.skillPoint++;
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
    private void actorGrowthCurve(int level,int job)
    {
        //暂时不用    log(1+x^2+2*x)+2^(x/20)+x^2/1000-1    半s型曲线
        //人物成长曲线公式1-100   0-50multi     2^(x/20)+x/5-1    持续增长型
        //生命曲线  30multi        2^(x/24)+x/8-1    
        //精力曲线  20multi        x/6+log(x)+1/2
        //攻击曲线  50multi        2^(x/20)+x/5-1
        //防御曲线  25multi        x/4
        //经验曲线  125multi      2^(x/15)+x/4+x^(1/4)-1          +0.1*等级
        ///        防御减伤效果曲线    -2^((150-x)/30)+32+x/9                     当前防御效果差值 取百分之0-40      防/攻=系数x 不能负值  

        //体力提升  额外回复+20%
        int numPer = (int)Math.Floor((Math.Pow(2, (level + 1) / 24) + (level + 1) / 8 - 1) - (Math.Pow(2, level / 24) + level / 8 - 1)) * hpBase;
        playermessage.data.physical_base += numPer;
        //精力提升  额外恢复+30%
        int numPer1 = (int)Math.Floor((Math.Log(level + 1) + (level + 1) / 6 + 1 / 2) - (Math.Log(level) + level / 6 + 1 / 2)) * mpBase;
        playermessage.data.vigor_base += numPer1;
        //攻防提升
        int numPer2 = (int)Math.Floor((Math.Pow(2, (level + 1) / 20) + (level + 1) / 5 - 1) - (Math.Pow(2, level / 20) + level / 5 - 1)) * atkBase;
        playermessage.data.attack_base += numPer2;
        numPer2 = (int)((level + 1.0) / 4 - level / 4) * defBase;
        playermessage.data.defence_base += numPer;
        //刷新
        playermessage.paddingData();
        //额外恢复
        playermessage.subCurPhysical(numPer + (int)(playermessage.data.physical_base * 0.2));
        playermessage.subCurVigor(numPer + (int)(playermessage.data.vigor_base * 0.3));
        //常态恢复
        playermessage.data.physical_recBase = (int)Mathf.Floor(phyRecover * (1 + (level + 1) * phyRecMulti));
        if (job == 12)      //祭祀会自动回精力
            playermessage.data.vigor_recBase = (int)Mathf.Floor(vigRecover * (1 + (level + 1) * vigRecMulti));
        else
            playermessage.data.vigor_recBase = 0;
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
            if (File.Exists(Application.dataPath + gameDataLoadSign))
            {
                PubTool.Instance.gameError("游戏配置错误!",true);
            }
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
    //----------------------------------------*******玩家数据******--------------------------------------
    //加载玩家数据       (手动加载 + 自动加载)
    public void loadPlayerData()  
    {
        StartCoroutine(loadLoading());
    }
    IEnumerator loadLoading()
    {
        PubTool.Instance.addLogger("读取游戏存档");
        BinaryFormatter bin = new BinaryFormatter();
        if (!File.Exists(Application.dataPath + baseDataLoadPath))
        {
            PubTool.Instance.addLogger("创建存档");
            firstInitData();             //没有存档则创建存档
        }
        FileStream file = File.Open(Application.dataPath + baseDataLoadPath, FileMode.Open);
        //file.Seek(0, SeekOrigin.Begin);
        playermessage = (PlayerMessage)bin.Deserialize(file);
        playerBridge = new PlayerMessageBridge(playermessage);
        playerBridge.paddingData();
        if (useTestData) testDataInterface.Data.testDataAdd(ref playermessage); //测试数据
        if (playermessage != null)
        {
            PubTool.Instance.addLogger("读取成功");
            //action?.Invoke(true);
        }
        else
        {
            PubTool.Instance.addLogger("读取失败");
            //action?.Invoke(false);
        }
        file.Close();
        yield return null;
    }
    //----------------------------------------------------------------------------************存档********----------------------
    //  玩家数据存档      （手动存档）
    //这个应该是ui事件  加载等待条  保存成功后再放开控制 并且触发全局save事件
    public void saveLoad(Action<bool> action) 
    {
        StartCoroutine(saveLoading(action));
    }
    IEnumerator saveLoading(Action<bool> action)
    {
        PubTool.Instance.addLogger("保存游戏");
        BinaryFormatter bin = new BinaryFormatter();
        FileStream file = File.Create(Application.dataPath + baseDataLoadPath);
        bin.Serialize(file, playermessage);
        file.Close();
        if (File.Exists(Application.dataPath + baseDataLoadPath))
        {
            PubTool.Instance.addLogger("存储成功");
            Debug.Log("save success!");
            EventTransfer.Instance.doSaveLoad();    //派发事件
            action?.Invoke(true);
        }
        else
        {
            PubTool.Instance.addLogger("存储失败");
            Debug.Log("save fail!");
            action?.Invoke(false);
        }
        yield return null;
    }
    //第一次游戏 玩家数据初始化
    private void firstInitData()
    {
        playermessage = new PlayerMessage();
        playermessage.isFirstIn = true;
        playermessage.level = 1;
        playermessage.data.physical_base = hpBase;
        playermessage.data.hpcur = hpBase;
        playermessage.data.vigor_base = mpBase;
        playermessage.data.mpcur = mpBase;
        playermessage.expcur = 0;
        playermessage.expmax = expBase;
        playermessage.data.attack_base = atkBase;
        playermessage.data.defence_base = defBase;
        playermessage.data.strike_base = 0;
        playermessage.data.dodge_base = 0;
        playermessage.data.speed_base = 30;
        playermessage.data.hitRate_base = 100;
        playermessage.data.adPat_base = 0;
        playermessage.data.apPat_base = 0;
        playermessage.data.agility_base = 2;
        playermessage.data.wisdom_base = 2;
        playermessage.data.force_base = 2;

        playermessage.data.physical_recBase = phyRecover;   //自然恢复
        playermessage.data.vigor_recBase = 0;

        playermessage.attackID = 2;
        playermessage.skills = new skillSave();
        playermessage.skills.skillPoint = 1;

        playermessage.skills.skillHold.Add(AllUnitData.data.getSkillStaticData(4));  //初始一个技能
        playermessage.skills.skillHold.Add(AllUnitData.data.getSkillStaticData(5));  //初始一个技能

        playermessage.paddingData();
        Debug.Log("save create success!");
        saveLoad(null);
    }
    //初始化游戏配置
    private void initGameDataMessage1()
    {
        dataplaymessage = new DataPlayMessage();
        dataplaymessage.combatIDCount = 0;
        saveGameMessageData();
    }

}