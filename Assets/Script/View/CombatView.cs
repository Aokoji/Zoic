using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatView : MonoBehaviour
{
    public GameObject startPos;         //头像条起点
    public GameObject endPos;           //头像条终点
    public GameObject line;                 //头像条
    public Transform[] enemySlots;   //敌对槽位  (实体立牌)
    public Transform playSlots;        //玩家槽位
    public Button setbtn;   //设置按钮
    public GameObject[] actorIcon;      //头像存储集合
    public GameObject playermessBoard;      //玩家信息框（hpmp）(右下角)
    public GameObject messageTips;      //消息提示框
    //---------基础四按钮窗
    public GameObject baseControl;
    public Button attack;
    public Button skill;
    public Button bag;
    public Button run;
    //遮罩
    public GameObject mask;
    //---------确认窗口 final
    public GameObject tanban;
    public Button attackConfirm;        //攻击确认  假设使用统一的确认取消按钮   不同层级之间要隔离  深层的子集打开时外层点击不再生效(也就是说 加panel)
    public Button attackCancel;         //攻击取消

    //---------技能窗口
    public GameObject messageFather;    //技能弹窗父级
    public GameObject messageContext;   //二级技能弹窗        (它的关闭按钮是现找的childbutton赋值的)
    public GameObject skillThirdFather;     //技能三级父级

    //----------移动窗口--------
    public GameObject runBoard;     //移动选择弹窗
    public Button runForward;   //三个选项
    public Button runStay;
    public Button runAway;
    public Button runClose;
    //---event
    public delegate void confirmEvent();
    public event confirmEvent chooseConfirmBtn;
    //private List<GameObject> icons = null;              //速度条数据

    //-------------配置数据------------------------
    private List<GameObject> actorBody = null;              //显示人物（3敌人1玩家） 目标体
    private List<CombatMessage> _Data = null;       //全部数据
    public CombatMessage playerActor = null;        //玩家本体
    private List<skillBarOneView> skillBars;     //存储技能bar的实体*  用于刷新cd等操作
    public CombatConfigMessage config;                //记录距离等配置

    //------------动态数据---------------------------------------------------------------------------------------------  动态数据------------------
    public List<int> takeActor = new List<int>();//在选择攻击目标时  或 释放环境技能确定窗时  提前赋值
    public SkillStaticData chooseSkill;                 //选择的技能   目标(仅限单体) 其他类型有自动识别
    public bool isrun;  //是否逃跑
    public bool ismove;     //是否移动
    public int moveDistance;
    public bool isprop;    //是否道具
    public bool isforward;  //是否可前进  前进按钮判断位
    private int cacheSkillId = 0;   //缓存id（防止技能重复点击）

    private int panelSkillStateRank = 0; //当前页面层级（技能）
    private bool haveChooseLock;        //确认按钮 锁 判断是否可点击确认按钮
    //----------固定参数--------------------
    private float lineDistance; //总的进度条长度
    private int ENEMY_NUM = 3;
    private int sceneShowType;

    //=========================================           初始化         ================
    //创建的初始化方法
    public void init(List<CombatMessage> data)
    {
        _Data = data;
        initUI();               //初始化组件显示
        initBaseButtonEvent();      //初始化按钮事件   （保证是被新创建出来的  否则会事件重复）
        initCreateIconAndActor();   //初始化创建头像图标和人物模型
        initLayout();       //初始化详细布局
    }
    public void initConfig(CombatConfigMessage dis)
    {
        config = dis;
        initDistance();
    }
    //初始化距离
    private void initDistance()
    {
        foreach(var it in _Data)
        {
            it.distance = config.initialDistance;
        }
    }
    private void initUI()
    {
        //icons = new List<GameObject>();
        actorBody = new List<GameObject>();
        skillBars = new List<skillBarOneView>();
        lineDistance = startPos.transform.position.x - endPos.transform.position.x;
        mask.gameObject.SetActive(false);
        baseControl.SetActive(false);
        tanban.SetActive(false);
        messageFather.SetActive(false);
        skillThirdFather.SetActive(false);
        messageTips.SetActive(false);
    }
    private void initBaseButtonEvent()
    {
        attack.onClick.AddListener(attackButtonClick);
        skill.onClick.AddListener(skillButtonClick);
        bag.onClick.AddListener(propButtonClick);

        attackConfirm.onClick.AddListener(enterConfirmPanel);
        attackCancel.onClick.AddListener(cancelConfirmPanel);    //攻击二级分窗显示
        messageFather.GetComponentInChildren<Button>().onClick.AddListener(closeSkillScene);
        //移动界面
        run.onClick.AddListener(fleeButtonClick);
        runForward.onClick.AddListener(runForwardClick);
        runStay.onClick.AddListener(runStayClick);
        runAway.onClick.AddListener(runAwayClick);
        runClose.onClick.AddListener(runCloseClick);
    }
    //初始化头像和人物模型
    private void initCreateIconAndActor()
    {
        int count = 0;
        foreach (var item in _Data)
        {
            if (count > 3) break;
            actorIcon[count].transform.position = startPos.transform.position;
            actorIcon[count].SetActive(true);
            /*
            GameObject actor = Resources.Load<GameObject>("Entity/combat/actorIcon");
            GameObject loadactor = Instantiate(actor);
            loadactor.name = item.Name;
            //loadactor.transform.SetParent(line.transform);
            loadactor.transform.SetParent(transform);
            loadactor.transform.position = startPos.transform.position;
            loadactor.transform.lossyScale.Set(1, 1,1);
            loadactor.SetActive(true);       //todo  待修改  目前为提前创建好
            */
            item.IconActor = actorIcon[count];

            count++;
            //加载单位
            GameObject actorbody = Resources.Load<GameObject>("Entity/combat/combatActor");
            GameObject loadactorBody = Instantiate(actorbody);
            //SpriteRenderer spr = loadactorBody.GetComponentInChildren<SpriteRenderer>();
            //spr.sprite = Resources.Load("Picture/load/" + item.IconName,typeof(Sprite)) as Sprite;  //换图
            item.ShowActor = loadactorBody.GetComponentInChildren<ComponentScript1>().gameObject;
            item.ShowActor.GetComponent<Image>().sprite = Resources.Load("Picture/load/" + item.IconName, typeof(Sprite)) as Sprite;
            loadactorBody.name = item.Name1;
            loadactorBody.SetActive(false);
            item.Prefab = loadactorBody;
            item.Prefab.name = "createPrefab";
            item.PrefabCtrl = item.Prefab.GetComponent<CombatActorItem>();
            item.PrefabCtrl.numId = item.NumID;     //记录是哪个id
            actorBody.Add(loadactorBody);
            item.PrefabCtrl.chooseArrowChange(false);

            if (item.IsPlayer)
            {
                playerActor = item;
                //给状态栏赋值
                playermessBoard.GetComponent<combatPlayMessView>().initPlayerData(playerActor.Data);
            }
            else
            {
                loadactorBody.AddComponent<Button>().onClick.AddListener(clickChoose);      //+++添加点击事件
            }
        }
    }
    //初始化界面布局数据及内容
    private void initLayout()
    {
        clearContext();     //清理技能列表
        foreach(var skill in playerActor.SkillData.skillHold)
        {
            skill.runDown = 0;
            GameObject bar = addContext();
            //给每个bar整一个脚本管理子项就可以了
            var com= bar.GetComponent<skillBarOneView>();
            //赋值
            com.initData(skill);
            com.btnclick = chooseSkillMessage;      //给回调赋值
            /*      在子项脚本实现了  先不用了
            bar.GetComponent<Button>().onClick.AddListener(()=>                             //闭包写法  网上抄的
            {
                chooseSkillMessage(skill);
            });*/
            skillBars.Add(com); //存起来
        }
        panelSkillStateRank = 0;
    }
    //==========================================================初始化结束================================
    //回合初始化
    public void roundInitFunc()
    {
        takeActor.Clear();
        chooseSkill = null;
        isrun = false;
        ismove = false;
        moveDistance = 0;
        isprop = false;
        panelSkillStateRank = 0;
    }
    //外部调用  布置场景 
    public void setSceneLayout(int type)
    {
        // 加载人物
        int num = 0;
        //目前敌对单位最多三个
        foreach(var item in _Data)
        {
            if (item.IsPlayer)
            {
                item.Prefab.transform.SetParent(playSlots.transform);       //设置父级
                item.Prefab.transform.position=playSlots.transform.position;    //设置位置
                item.ShowActor.transform.localScale = new Vector3(1, 1, 1);     //设置内图片换图大小
                item.ShowActor.GetComponent<Image>().SetNativeSize();       //设置图片自适应
                item.Prefab.transform.localScale = new Vector3(1, 1, 1);        //区分
            }
            else
            {
                item.Prefab.transform.SetParent(enemySlots[num].transform);
                item.Prefab.transform.position=enemySlots[num].transform.position;
                item.ShowActor.transform.localScale = new Vector3(1, 1, 1);
                item.ShowActor.GetComponent<Image>().SetNativeSize();
                item.Prefab.transform.localScale = new Vector3(-1, 1, 1);
                num++;
            }
            item.Prefab.SetActive(true);
        }
        sceneShowType = type;
        //切换场景方法
        PubTool.Instance.addStep(playEnterScene);
        //人物出场短暂动画
        PubTool.Instance.addStep(playActorFirstStage);
        //面板出现短暂动画
        PubTool.Instance.addStep(playPanelFirstStage);
    }
    
    ///--------------------------------------------------------------显示提示板的方式------------------------------
    ///
    //显示提示板（比如距离）
    private void showTips1Second(Action action)
    {
        //播动画
        AnimationController.Instance.playAnimation(messageTips, "tipsShow", false, action);
    }
    //设置内容
    private void setTipsContext(string text)
    {
        messageTips.GetComponentInChildren<Text>().text = text;
    }
    /// <summary>
    /// 显示标签1秒  需要加入序列
    /// </summary>
    public void showTips1Second(string context, Action action)
    {
        setTipsContext(context);
        showTips1Second(action);
    }
    /// <summary>
    /// 显示提示，但不加入序列
    /// </summary>
    public void showTips1Second(string context)
    {
        setTipsContext(context);
        AnimationController.Instance.playAnimation(messageTips, "tipsShow", false);
    }
    //---------------------------------------------出场动画--------------------------------
    private void playEnterScene(Action callback)
    {
        //+++调用animation控制器的播放动画
        //sceneShowType
        callback();
    }
    private void playActorFirstStage(Action callback)
    {
        callback();
    }
    private void playPanelFirstStage(Action callback)
    {
        callback();
    }
    //-------------------------------------------出场动画end------------------------------

    //----------------------------------------数据设置----------------------------------
    public GameObject addContext()    //测试
    {
        GameObject obj = Resources.Load<GameObject>("Entity/combat/itembar");
        GameObject content = Instantiate(obj);
        var items=messageContext.GetComponentsInChildren<ComponentScript1>();
        content.transform.SetParent(messageContext.transform);
        content.transform.localScale=new Vector3(1, 1, 1);
        content.transform.position = items[items.Length - 1].transform.position-new Vector3(0,0.8f,0);
        return content;
    }
    public void deduceContextLast()
    {

    }
    public void clearContext()
    {
        var list = messageContext.GetComponentsInChildren<ComponentScript1>();
        for (int i = list.Length-1; i >= 1; i--)
        {
            if (list[i] != null)
            {
                Destroy(list[i]);
            }
        }
    }
    public void hideContext()
    {
        messageFather.SetActive(false);
    }
    public void showContext()
    {
        messageFather.SetActive(true);
    }

    //----------------------------------------数据end---------------------------------

    //------------------------------------------结算动画-------------------------------------
    public void playSettleAnim(bool result,Action callback)
    {
        Debug.Log("【战斗结束】");
        if (result)
        {//玩家胜利
            playPlayerSettle();
            Debug.Log("【玩家胜利】");
            //动画播放完后
            callback();
        }
        else
        {//gameover 也不是  其实就是送回家
            playGameOverAnim();
            Debug.Log("【战斗失败】");
        }
    }
    private void playGameOverAnim()
    {
        //播放胜利动作
        //出结算面板
    }
    private void playPlayerSettle()
    {
        //播放失败动作
        //出结算面板和回家提示
    }
    //------------------------------------------结算动画end---------------------------------

    //-----------------------------------按钮 面板控制---------------------------------
    //外部调用  玩家回合  出现基础面板
    public void playerRound()
    {
        baseControl.SetActive(true);
        mask.SetActive(false);
        tanban.SetActive(false);
    }

    private void showConfirmPanel()
    {//显示确认面板
        //显示攻击面板的话会替代掉原先控制按钮的位置
        mask.transform.SetAsLastSibling();
        tanban.transform.SetAsLastSibling();
        mask.SetActive(false);
        tanban.SetActive(true);
        //开放目标点击控制  实时改变点击缓存内容
    }

    //点击确认按钮      确认按钮肩负多项职责
    private void enterConfirmPanel()
    {
        //关闭二级技能界面
        if (panelSkillStateRank == 3)
        {//选择技能才会进入三级技能界面记录
            bool iscanNext = false;
            //检查距离（选择否目标）
            foreach(var it in _Data)
            {
                if (!it.PrefabCtrl.getGray())
                    iscanNext = true;
            }
            if (!iscanNext)
            {
                showTips1Second("攻击范围内没有目标!");
                return;
            }
            hideContext();
            skillThirdFather.SetActive(false);
        }
        chooseConfirmBtn();
        //确认按钮点击后
        clearArrowState();      //清理箭头
        //清理当前页面 1 攻击清理     
        tanban.SetActive(false);
        lockBaseButton(false);


    }
    //-------------------------------------------------------------------------------关闭攻击弹板-------------------------------
    //关闭弹板会清理掉一切其他的赋值状态
    private void cancelConfirmPanel()
    {//关闭攻击面板
        takeActor.Clear();
        baseControl.SetActive(true);
        tanban.SetActive(false);
        skillThirdFather.SetActive(false);
        //重置状态
        clearArrowState();
        panelSkillStateRank = 0;
        isrun = false;
        isprop = false;
        chooseSkill = null;
    }
    private void attackButtonClick()
    {
        //进入二级界面
        chooseSkill = AllUnitData.Data.getSkillStaticData(playerActor.AttackID);
        //无效化 基础四个按钮  
        baseControl.SetActive(false);
        //刷新箭头指向状态（就是待选目标的状态  重置为可选目标）
        resetArrowState();
        //弹出攻击面板
        showConfirmPanel();
        panelSkillStateRank = 1;
    }
    private void propButtonClick()
    {
        //+++出二级页面的时候  isprop置true
    }
    private void skillButtonClick()
    {
        lockBaseButton(true);
        cacheSkillId = 0;
        refreshSkillBoard();    //刷新cd显示
        showContext();
        panelSkillStateRank = 2;
    }
    private void fleeButtonClick()
    {
        showFleeBoard();
    }
    private void runForwardClick()
    {
        if (isforward)
        {
            ismove = true;
            moveDistance = 1;
            chooseConfirmBtn();
        }
        else
        {
            //显示提示 不可前进
            showTips1Second("被挡住了去路");
        }
    }
    private void runStayClick()
    {
        ismove = true;
        moveDistance = 0;
        chooseConfirmBtn();
    }
    private void runAwayClick()
    {
        isrun = true;
        ismove = true;
        moveDistance = -1;
        chooseConfirmBtn();
    }
    private void runCloseClick()
    {
        runBoard.SetActive(false);
    }
    //------------------------------------------------------------调用界面-------------------------

    //显示移动面板
    private void showFleeBoard()
    {
        //附带刷新 可用run  针对前进
        isforward = true;
        foreach(var it in _Data)
        {
            if (!it.IsPlayer && !it.IsDead && it.distance <= 0)
                isforward = false;
        }
        runBoard.SetActive(true);
    }
    //选择技能  显示三级 技能详情面板
    private void chooseSkillMessage(SkillStaticData skill)
    {
        if (cacheSkillId > 0 && cacheSkillId == skill.id) return;   //防止重复点击
        cacheSkillId = skill.id;
        skillThirdFather.SetActive(true);
        string cont;
        cont="技能详情\n"+ skill.name+"：\n    "+skill.describe;
        skillThirdFather.GetComponentInChildren<Text>().text = cont;
        //var item = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        baseControl.SetActive(false);
        //记录技能选择
        chooseSkill = skill;
        //刷新箭头指向状态（就是待选目标的状态  重置为可选目标）
        if (panelSkillStateRank!=3)
            resetArrowState();
        //显示确认弹板
        tanban.SetActive(true);
        panelSkillStateRank = 3;
    }
    //关闭二级  技能页面
    private void closeSkillScene()
    {
        lockBaseButton(false);
        cancelConfirmPanel();
        hideContext();
    }
    //刷新人物数值面板
    public void refreshMessBoard()
    {
        playermessBoard.GetComponent<combatPlayMessView>().refreshNumbers();
    }
    //刷新技能冷却信息
    public void refreshSkillBoard()
    {
        foreach(var it in skillBars)
        {
            if (!it.checkCoolDown())    //检查冷却
            {
                //不在冷却状态  判断消耗
                if (playerActor.checkPhysical(it.getExpend1()))
                    if (playerActor.checkVigor(it.getExpend2()))
                        it.setEnough();
                    else
                        it.setNotEnough("精力");
                else
                    it.setNotEnough("体力");
            }
        }
    }
    //锁基础按钮
    private void lockBaseButton(bool islock)
    {
        attack.enabled = !islock;
        skill.enabled = !islock;
        bag.enabled = !islock;
        run.enabled = !islock;
    }
    //选择目标的点击事件
    private void clickChoose()
    {
        var item=UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        var script = item.GetComponent<CombatActorItem>();
        script.onclickCustom();
        if (!script.getLock())
        {
            takeActor.Clear();
            takeActor.Add(script.numId);
        }
    }
    //根据当前攻击距离  重置可点击目标
    private void resetArrowState()
    {
        takeActor.Clear();
        //选择刷新的时候  chooseSkill必须有值
        if (chooseSkill == null) Debug.LogError("测试部分  刷新技能攻击距离内箭头技能为空！！！");
        int count = 0;

        bool isall = false; //全体
        bool isalone = false; //敌单体
        bool isplayer = false;  //指向玩家
        //先判断技能指向类型
        switch (chooseSkill.effectType)
        {
            case 310:
            case 311:
            case 312: isplayer = true; break;
            case 313: isalone = true; break;     //敌方单体
            case 314: isall = true; break;     //敌方全体
            case 315:break;     //全体
            case 316:break;     //除自己
            default: Debug.LogError("测试部分  人物箭头判断技能类型错误！！！");break;
        }
        //判断选择箭头（默认）
        foreach (var item in _Data)
        {
            item.PrefabCtrl.chooseArrowChange(false);
            item.PrefabCtrl.setGray(false);
            //目标是玩家自己
            if (isplayer)
            {
                //指玩家的箭头（非全体  不能控制  只显示玩家）
                if (item.IsPlayer)
                {
                    //玩家加到目标表
                    takeActor.Add(item.NumID);
                }
                else if(!item.IsDead)
                {
                    //敌人设置不可点击
                    item.PrefabCtrl.chooseArrowChange(false);
                    item.PrefabCtrl.changeLock(true);
                    item.PrefabCtrl.setGray(true);
                }
            }
            //目标是敌人状态
            else
            {
                //跳过玩家
                if (item.IsPlayer)
                {
                    item.PrefabCtrl.setGray(true);
                    continue;
                }
                if (item.IsDead) continue;      //跳过死人

                //根据距离设置箭头显不显示
                if (item.distance <= chooseSkill.takeLength)
                {
                    //单体
                    if (isalone)
                    {
                        item.PrefabCtrl.changeLock(false);
                        //判断距离最小值
                        if (takeActor.Count > 0)
                        {
                            if (item.distance < _Data[takeActor[0]].distance)
                                takeActor[0] = item.NumID;
                        }
                        else
                        {
                            takeActor.Add(item.NumID);
                        }
                    }
                    //全体
                    else
                    {
                        //锁
                        item.PrefabCtrl.changeLock(true);
                        item.PrefabCtrl.chooseArrowChange(true);
                        takeActor.Add(item.NumID);
                    }
                }
                else
                {
                    item.PrefabCtrl.changeLock(true);
                    //actor置灰
                    item.PrefabCtrl.setGray(true);
                }
            }
        }//foreach

    }
    //清理选择目标箭头
    private void clearArrowState()
    {
        foreach(var item in _Data)
        {
            item.PrefabCtrl.chooseArrowChange(false);
            item.PrefabCtrl.changeLock(false);
            item.PrefabCtrl.setGray(false);
        }
    }


    //实时设置各个图标跑的进度
    public void setRelative(CombatMessage icon)
    {
        float dis = lineDistance / 100 * icon.CurSpeed;
        icon.IconActor.transform.position = new Vector2(startPos.transform.position.x - dis, icon.IconActor.transform.position.y);
    }
    //动画调用
    public void componentChanges()      
    {//组件变动  面板变动
        //获取面板组件  分析结果进行加减
    }
    //  todo
    //  人物动画  入场poss 攻击 技能 胜利 停止  待机  over
    //  敌人动画  入场poss  攻击 技能 胜利 停止  待机 over
    //  场景入场动画切换场景
    //  面板弹出动画  子窗口界面打开弹出
    //  over结算  胜利结算  窗口弹出
    //  
}
