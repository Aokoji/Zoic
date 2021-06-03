using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : DDOLController<CombatController>
{
    public EventManager eventManager;           //注册一个事件处理器
    private AttackAction attackAction;              //战斗缓存器
    public CombatView combat;
    private GameObject combatScene;
    private float pubPer = 0.05f;
    private bool iswait;
    private string logname;

    private List<CombatMessage> messageActor = null;         //跑进度全部单位数据
    private CombatMessage willActionActor = null;      //待操作的单位

    //manager调用  初始化并自动创建单例
    public void initController()
    {
        combat = null;
        combatScene = null;
        iswait = false;
        messageActor = new List<CombatMessage>();
        attackAction = new AttackAction();
        //willActionActor = new List<CombatMessage>();
    }
    //外部调用  打开界面
    public void openCombat(List<CombatMessage> data,string logName)    //处理传进来的数据  敌人 玩家 战斗类型（野怪 boss或精英剧情等） 战斗场景等配置
    {
        logname = logName;
        messageActor = data;
        attackAction.initData(messageActor);
        if (combat == null) { initCombat(messageActor); }
        AnimationController.Instance.cleanNextStepAction(); //清空动画控制器事件
        AnimationController.Instance.combatNextStep += nextStep;
        combat.transform.SetAsLastSibling();            //置顶
        initEvent();
        ViewController.instance.setCameraVisible("combatcam", true);        //强制显示战斗场景相机（单显示）
        //ViewController.instance.setCameraVisible("uicam", false);               //补充添加战斗ui相机
        eventManager = new EventManager();      //战斗触发器
        eventManager.combatStart += combatStart;
        eventManager.combat += arrangeScence;      //赋予布置场景方法   可多个
        eventManager.combatEnd += combatEnd;
        eventManager.doCombat();            //打开界面
        PubTool.Instance.addStep(startPrograss);    //加入序列
    }
    //内部调用          创建UI
    public void initCombat(List<CombatMessage> data)
    {
        GameObject baseain = Resources.Load<GameObject>("Entity/CombatUI");
        var baseMain = Instantiate(baseain);
        baseMain.name = "CombatView";
        //baseMain.transform.SetParent(CanvasLoad.canvasui.transform, false);
        baseMain.transform.position=CanvasLoad.canvasui.transform.position;
        var mainview = baseMain.GetComponent<CombatView>();
        mainview.gameObject.SetActive(true);       //todo  待修改
        combat = mainview;

        combat.initMethod();
        combat.initItemData(data);          //初始化界面数据
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
        //布置场景数据
        combat.setSceneLayout(0);
        //AnimationController.Instance.playCombatSceneTransform(combat, 0);   //  0    默认转场
        Debug.Log("布置场景数据");
    }
    //事件
    public void initEvent()
    {
        //+++各种战斗确认界面通用一个确认和取消  在点击方法中根据view层 选择状态区分触发攻击的操作事件
        combat.attackConfirm.onClick.AddListener(playerDoAttack);    //攻击并触发下一步
    }
    //-------------------------------------------------内部逻辑-----------------------------
    private void combatStart()
    {
        //ViewController.instance.setMainUIActive(false);
    }
    private void combatEnd()
    {
        Debug.Log("结束加载场景 ");//+++其实改先播放一会等待的展示 或播一个回合开始的小动画
    }
    //开始跑进度  （速度条）
    private void startPrograss(Action callback)
    {
        PubTool.instance.addCombatLogger(logname, "开始游戏进度");
        StartCoroutine(doLoadPrograss());
        callback();
    }

    IEnumerator doLoadPrograss()
    {
        if (iswait) yield break;
        else
        {
            foreach (var item in messageActor)
            {
                //暂定总长100  标速 40为1s
                float per = pubPer * item.UnitData["speed"];
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
            if (!willActionActor.IsPlayer)//敌人攻击
            {
                Debug.Log("【敌人攻击】");
                //轮到敌人攻击  拿到一个攻击数据组
                AnalyzeResult aiAction = willActionActor.Analyse.analyseCombatAttack(messageActor, willActionActor,combat.playerActor);
                //获取一个分析后数据   调用战斗数据缓存器attackAction存储缓存数据
                AttackResult animData=attackAction.normalAction(aiAction);
                //根据计算结果  调用动画播放器   播放完动画后进行下一步
                AnimationController.Instance.playCombatBeHit(combat,animData);
            }
            else
            {
                //+++dosomething  轮到玩家操作
                combat.playerRound();
                Debug.Log("【玩家攻击】");
            }
        }
    }
    //进行完操作  接着跑进度
    public void nextStep()
    {
        Debug.Log("开始计算速度..");
        if (!checkCombatResult())
        {
            combatSettle();
            //+++如果结束  执行结算判断  胜利或失败  继续结算动画
            return;
        }
        iswait = false;
        willActionActor = null;
        StartCoroutine(doLoadPrograss());
    }
    //检查战斗是否结束      包括玩家与敌人
    public bool checkCombatResult()
    {
        return attackAction.checkCombatContinue();
    }
    //战斗结算
    private void combatSettle()
    {
        combat.playSettleAnim(attackAction.checkCombatResult(),exitCombat);
    }
    //退出
    public void exitCombat()
    {//先关相机并且删掉  再打开主相机  否则相机冲突
        ObjectUtil.Destroy(combat.gameObject);
        ObjectUtil.Destroy(combatScene);
        ViewController.Instance.removeCameraDictionary("combatcam");
        //返回主视角
        ViewController.instance.showMainCam();      //切主相机
        initController();
    }
    //////////////////-----------------------------------------EVENT----------------------
    
    public void playerDoAttack()
    {
        AnalyzeResult aiAction = new AnalyzeResult();//+++模拟一个ai动作数据
        aiAction.selfNum = combat.playerActor.NumID;
        aiAction.skillID =combat.chooseSkill;
        aiAction.skillType =int.Parse( AllUnitData.getSkillData(combat.chooseSkill)[3]);
        aiAction.takeNum = combat.chooseActor;
        //获取一个分析后数据   调用战斗数据缓存器attackAction存储缓存数据
        AttackResult animData = attackAction.normalAction(aiAction);
        //根据计算结果  调用动画播放器   播放完动画后进行下一步
        AnimationController.Instance.playCombatBeHit(combat, animData);
        //eventManager.doattackNext(combat.playerActor, combat.chooseActor);
    }
    public void playerDoAttack1()
    {
        AnalyzeResult aiAction = new AnalyzeResult();//+++模拟一个ai动作数据
        aiAction.selfNum = 0;
        aiAction.skillID = 2;
        aiAction.skillType = 103;
        aiAction.takeNum = 1;
        //获取一个分析后数据   调用战斗数据缓存器attackAction存储缓存数据
        AttackResult animData = attackAction.normalAction(aiAction);
        //根据计算结果  调用动画播放器   播放完动画后进行下一步
        AnimationController.Instance.playCombatBeHit(combat, animData);
        //eventManager.doattackNext(combat.playerActor, combat.chooseActor);
    }
    public void playerDoAttack2()
    {
        AnalyzeResult aiAction = new AnalyzeResult();//+++模拟一个ai动作数据
        aiAction.selfNum = 0;
        aiAction.skillID = 4;
        aiAction.skillType = 103;
        aiAction.takeNum = 1;
        AttackResult animData = attackAction.normalAction(aiAction);
        AnimationController.Instance.playCombatBeHit(combat, animData);
    }



}
