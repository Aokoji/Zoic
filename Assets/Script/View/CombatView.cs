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
    public Transform[] enemySlots;   //敌对槽位
    public Transform playSlots;        //玩家槽位
    public Button attack;
    public Button skill;
    public Button bag;
    public Button run;
    public GameObject[] actorIcon;
    public GameObject playermessBoard;      //玩家信息框（hpmp）

    public GameObject baseControl;
    public GameObject mask;
    public GameObject tanban;
    public GameObject messageFather;    //技能弹窗父级
    public GameObject messageContext;   //二级技能弹窗
    public GameObject skillThirdFather;     //技能三级父级

    public Button attackConfirm;        //攻击确认  假设使用统一的确认取消按钮   不同层级之间要隔离  深层的子集打开时外层点击不再生效(也就是说 加panel)
    public Button attackCancel;         //攻击取消

    //---event
    public delegate void confirmEvent();
    public event confirmEvent chooseConfirmBtn;

    //private List<GameObject> icons = null;              //速度条数据
    private List<GameObject> actorBody = null;              //显示人物（3敌人1玩家） 目标体
    private List<CombatMessage> _Data = null;       //全部数据
    public CombatMessage playerActor = null;        //玩家本体
    public SkillStaticData chooseSkill;                 //选择的技能   目标(仅限单体) 其他类型有自动识别
    public int chooseActor;             //在选择攻击目标时  或 释放环境技能确定窗时  提前赋值

    public bool isrun;  //是否逃跑
    private int panelSkillStateRank = 0; //当前页面层级（技能）

    private float distance; //总的进度条长度
    private int ENEMY_NUM = 3;
    private int sceneShowType;

    private bool isChooseOneActor = false;      //决定选择单个目标的显示箭头 点击是否生效

    //===============================================================           初始化         ================
    //创建的初始化方法
    public void init(List<CombatMessage> data)
    {
        _Data = data;
        initUI();               //初始化组件显示
        initBaseButtonEvent();      //初始化按钮事件   （保证是被新创建出来的  否则会事件重复）
        initCreateIconAndActor();   //初始化创建头像图标和人物模型
        initLayout();       //初始化详细布局
    }
    private void initUI()
    {
        //icons = new List<GameObject>();
        actorBody = new List<GameObject>();
        distance = startPos.transform.position.x - endPos.transform.position.x;
        mask.gameObject.SetActive(false);
        baseControl.SetActive(false);
        tanban.SetActive(false);
        messageFather.SetActive(false);
        skillThirdFather.SetActive(false);
    }
    private void initBaseButtonEvent()
    {//初始化自身基础按钮的功能   不包含最终的二级或深级界面功能按钮
        attack.onClick.AddListener(attackButtonClick);
        skill.onClick.AddListener(skillButtonClick);
        bag.onClick.AddListener(propButtonClick);
        run.onClick.AddListener(fleeButtonClick);
        attackConfirm.onClick.AddListener(enterConfirmPanel);
        attackCancel.onClick.AddListener(cancelConfirmPanel);    //攻击二级分窗显示
        messageFather.GetComponentInChildren<Button>().onClick.AddListener(closeSkillScene);
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
            actorBody.Add(loadactorBody);
            loadactorBody.GetComponent<CombatActorItem>().chooseArrowChange(false);

            if (item.IsPlayer)
            {
                playerActor = item;
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
        foreach(var skId in playerActor.SkillData.skillHold)
        {
            var skill=AllUnitData.Data.getSkillStaticData(skId);
            GameObject bar = addContext();
            Text[] conts=bar.GetComponentsInChildren<Text>();
            conts[0].text = skill.name;     //技能名称
            conts[1].text = skill.expend1 + "";     //体力消耗
            conts[2].text = skill.expend2 + "";     //精力消耗
            bar.GetComponent<Button>().onClick.AddListener(()=>                             //闭包写法  网上抄的
            {
                chooseSkillMessage(skill);
            });
        }
        panelSkillStateRank = 0;
    }
    //==========================================================初始化结束================================

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

    //显示提示板（比如距离）
    public void showTips1Second(Action action)
    {
        //提示板0.25渐显
        //提示板1秒展示
        //提示板0.25渐隐
        //渐隐结束
        action();
    }
    //设置内容
    public void setTipsContext(string text)
    {

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
        //给chooseActor赋值
        setRefreshChooseActor();
        //开放目标点击控制  实时改变点击缓存内容
    }

    //点击确认按钮      确认按钮肩负多项职责
    private void enterConfirmPanel()
    {
        chooseConfirmBtn();
        //确认按钮点击后
        clearArrowState();      //清理箭头
        //清理当前页面 1 攻击清理     
        tanban.SetActive(false);
        clearArrowState();
        lockBaseButton(false);
        //关闭二级技能界面
        if (panelSkillStateRank == 3)
        {
            hideContext();
            skillThirdFather.SetActive(false);
        }

    }
    //----------------------------------------------------------------------------------------------关闭攻击弹板-------------------------------
    //关闭弹板会清理掉一切其他的赋值状态
    private void cancelConfirmPanel()
    {//关闭攻击面板
        chooseActor = -1;
        isChooseOneActor = false;
        baseControl.SetActive(true);
        tanban.SetActive(false);
        skillThirdFather.SetActive(false);
        //重置状态
        clearArrowState();
        panelSkillStateRank = 0;
        isrun = false;
        chooseSkill = null;
    }
    private void attackButtonClick()
    {
        isChooseOneActor = true;    //允许出现选择箭头
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

    }
    private void skillButtonClick()
    {
        lockBaseButton(true);
        showContext();
        panelSkillStateRank = 2;
    }
    private void fleeButtonClick()
    {
        isrun = true;
    }
    //选择技能  显示三级 技能详情面板
    private void chooseSkillMessage(SkillStaticData skill)
    {
        skillThirdFather.SetActive(true);
        string cont;
        cont="技能详情\n"+ skill.name+"：\n    "+skill.describe;
        skillThirdFather.GetComponentInChildren<Text>().text = cont;
        //var item = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        baseControl.SetActive(false);
        //刷新箭头指向状态（就是待选目标的状态  重置为可选目标）
        if(panelSkillStateRank!=3)
            resetArrowState();
        //记录技能选择
        chooseSkill = skill;
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
        if (!isChooseOneActor) return;
        Debug.Log("choose");
        var item=UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        item.GetComponent<CombatActorItem>().chooseArrowChange(true);
    }
    private void resetArrowState()
    {
        bool isreset = false;
        int count = 0;
        foreach(var item in _Data)
        {
            item.Prefab.GetComponent<CombatActorItem>().chooseArrowChange(false);
            if (!isreset)
            {
                if(!item.IsDead && !item.IsPlayer)
                {
                    isreset = true;
                    chooseActor = count;
                    item.Prefab.GetComponent<CombatActorItem>().chooseArrowChange(true);
                }
            }
            count++;
        }
    }
    //清理选择目标箭头
    private void clearArrowState()
    {
        foreach(var item in _Data)
        {
            item.Prefab.GetComponent<CombatActorItem>().chooseArrowChange(false);
        }
    }
    //初始化  开放选择时的 默认初始目标
    private void setRefreshChooseActor()
    {
        foreach(var i in _Data)
        {
            if (!i.IsPlayer && !i.IsDead)
                chooseActor = i.NumID;
        }
    }


    //实时设置各个图标跑的进度
    public void setRelative(CombatMessage icon)
    {
        float dis = distance / 100 * icon.CurSpeed;
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
