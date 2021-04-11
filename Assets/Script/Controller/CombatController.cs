using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : DDOLController<CombatController>
{
    public EventManager eventManager;
    private CombatView combat = null;
    private GameObject combatScene = null;
    private int stepNum = 0;        //进度代号
    private float pubPer = 0.05f;
    private bool isstart;
    private bool iswait;

    private List<CombatMessage> messageActor = null;         //跑进度全部单位数据
    private CombatMessage willActionActor = null;      //待操作的单位

    //manager调用  初始化并自动创建单例
    public void initController()
    {
        isstart = false;
        iswait = false;
        messageActor = new List<CombatMessage>();
        //willActionActor = new List<CombatMessage>();
    }
    //外部调用  打开界面
    public void openCombat()
    {
        messageActor = getData();    //获取敌人数据 --测试
        if (combat == null) { initCombat(messageActor); }
        combat.transform.SetAsLastSibling();            //置顶
        initEvent();

        eventManager = new EventManager();      //战斗触发器
        eventManager.combatStart += combatStart;
        eventManager.combat += arrangeScence;      //赋予布置场景方法   可多个
        eventManager.combatEnd += combatEnd;
        eventManager.doCombat();            //打开界面

        ViewController.instance.setCameraVisible("combatcam", true);
        ViewController.instance.setCameraVisible("uicam", false);
    }
    //内部调用
    public void initCombat(List<CombatMessage> data)
    {
        GameObject baseain = Resources.Load<GameObject>("Entity/CombatUI");
        var baseMain = Instantiate(baseain);
        baseMain.name = "CombatView";
        baseMain.transform.SetParent(CanvasLoad.canvasui.transform, false);
        var mainview = baseMain.GetComponent<CombatView>();
        mainview.gameObject.SetActive(true);       //todo  待修改
        combat = mainview;
        combat.initUI();
        combat.initItemData(data);          //初始化数据
        GameObject scece = Resources.Load<GameObject>("Entity/CombatScene");
        var scecemain = Instantiate(scece);
        scecemain.name = "CombatScene";
        var pos = CanvasLoad.canvasui.transform.position + new Vector3(0, 100);
        scecemain.transform.position = pos;
        scecemain.transform.SetParent(CanvasLoad.instance.uiPos.transform);
        //添加相机资源
        var combatcam = scecemain.GetComponentInChildren<Camera>();
        ViewController.instance.addCameraDictionary("combatcam", combatcam);
        combatScene = scecemain;
    }
    //布置场景
    public void arrangeScence()
    {
        //布置场景数据  测试方法
        Debug.Log("布置场景数据");
    }
    //事件
    public void initEvent()
    {
        combat.attack.onClick.AddListener(nextStep);
    }
    //--------------------------------------------------------------------------------测试方法
    public List<CombatMessage> getData()
    {
        List<CombatMessage> actors = new List<CombatMessage>();
        CombatMessage player1 = new CombatMessage();
        player1.Name = "player";
        player1.Speed = 30;
        CombatMessage enemy1 = new CombatMessage();
        string[] data1 = AllUnitData.getUnitData(1);
        enemy1.Name = data1[1];
        enemy1.Attack= int.Parse(data1[4]);
        enemy1.MaxMp= int.Parse(data1[3]);
        enemy1.MaxHP= int.Parse(data1[2]);
        enemy1.Speed =int.Parse(data1[5] );
        actors.Add(player1);
        actors.Add(enemy1);
        return actors;
    }
    //------------------------------------------------------------------------------
    public void combatStart()
    {
        //ViewController.instance.setMainUIActive(false);
    }
    public void combatEnd()
    {
        stepNum = 0;
        Debug.Log("结束加载场景   开始跑速度条");
        startPrograss();
        //nextStep();
    }
    //开始跑进度  （速度条）
    private void startPrograss()
    {
        isstart = true;
        //StartCoroutine(doLoadPrograss());
    }

    IEnumerator doLoadPrograss()
    {
        if (!isstart) yield break;
        if (iswait)
        {
            yield break;
        }
        else
        {
            foreach (var item in messageActor)
            {
                //暂定总长100  标速 40
                float per = pubPer * item.Speed;
                item.CurSpeed += per;
                combat.setRelative(item);
                if (item.CurSpeed >= 100)
                {
                    //willActionActor.Add(item);
                    willActionActor = item;
                    item.CurSpeed = 0;
                    break;  //暂且先这样
                }
            }
            checkAction();
        }
        yield return new WaitForSeconds(0.02f);
        StartCoroutine(doLoadPrograss());
    }

    //检查一次更新过后   未执行的单位
    private void checkAction()
    {
        if (willActionActor != null)
        {
            iswait = true;
            if (willActionActor.Name != "player")
            {
                StartCoroutine(testEnemy());
            }
            Debug.Log("is doing actor       " + willActionActor.Name);
        }
        //willActionActor
    }
    //进行完操作  接着跑进度      --测试  需要修改为private
    public void nextStep()
    {
        iswait = false;
        willActionActor = null;
        StartCoroutine(doLoadPrograss());
    }

    public void enemyAttack()
    {
        Debug.Log("enemy Attack!!");
    }



   //////////////////-----------------------------------------EVENT----------------------
   ///
   private void clickAttack()
    {

    }
    IEnumerator testEnemy()
    {
        yield return new WaitForSeconds(2);
        nextStep();
    }

}
