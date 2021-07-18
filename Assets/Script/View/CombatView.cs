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

    public GameObject baseControl;
    public GameObject mask;
    public GameObject tanban;
    public GameObject messageFather;    //技能弹窗父级
    public GameObject messageContext;   //二级技能弹窗
    public GameObject skillThirdFather;     //技能三级父级

    public Button attackConfirm;        //攻击确认  假设使用统一的确认取消按钮   不同层级之间要隔离  深层的子集打开时外层点击不再生效(也就是说 加panel)
    public Button attackCancel;         //攻击取消

    //private List<GameObject> icons = null;              //速度条数据
    private List<GameObject> actorBody = null;
    private List<CombatMessage> _Data = null;       //全部数据
    public CombatMessage playerActor = null;        //玩家本体
    public int chooseSkill;                 //选择的技能   目标(仅限单体) 其他类型有自动识别
    public int chooseActor;             //在选择攻击目标时  或 释放环境技能确定窗时  提前赋值

    private float distance; //总的进度条长度
    private int ENEMY_NUM = 3;
    private int sceneShowType;

    private bool isChooseOneActor = false;      //决定选择单个目标的显示箭头 点击是否生效


    public void initMethod()
    {
        initUI();
        initBaseButtonEvent();
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
        attackCancel.onClick.AddListener(cancelConfirmPanel);    //攻击二级分窗显示
        messageFather.GetComponentInChildren<Button>().onClick.AddListener(closeSkillScene);
    }
    public void initItemData(List<CombatMessage> data)
    {//创建头像图标    单位实体  并存储
        _Data = data;
        int count = 0;
        foreach (var item in data)
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
            GameObject actorbody= Resources.Load<GameObject>("Entity/combat/combatActor");
            GameObject loadactorBody = Instantiate(actorbody);
            SpriteRenderer spr = loadactorBody.GetComponentInChildren<SpriteRenderer>();
            spr.sprite = Resources.Load("Picture/load/" + item.IconName,typeof(Sprite)) as Sprite;  //换图
            loadactorBody.name = item.Name;
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
        initLayout();
    }
    //初始化界面布局数据及内容
    private void initLayout()
    {
        clearContext();     //清理技能列表
        foreach(var skill in playerActor.SkillData)
        {
            GameObject bar = addContext();
            Text[] conts=bar.GetComponentsInChildren<Text>();
            conts[0].text = AllUnitData.getSkillData(skill.skillID)[1];     //技能名称
            conts[1].text = skill.skillLevel+"";     //等级
            conts[2].text = AllUnitData.getSkillData(skill.skillID)[29];     //体力消耗
            conts[3].text = AllUnitData.getSkillData(skill.skillID)[30];     //精力消耗
            bar.GetComponent<Button>().onClick.AddListener(()=>                             //闭包写法  网上抄的
            {
                int id = skill.skillID;
                chooseSkillMessage(id);
            });
        }
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
                item.Prefab.transform.SetParent(playSlots.transform);
                item.Prefab.transform.position=playSlots.transform.position;
            }
            else
            {
                item.Prefab.transform.SetParent(enemySlots[num].transform);
                item.Prefab.transform.position=enemySlots[num].transform.position;
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
    {//显示攻击面板
        //显示攻击面板的话会替代掉原先控制按钮的位置
        mask.transform.SetAsLastSibling();
        tanban.transform.SetAsLastSibling();
        mask.SetActive(false);
        tanban.SetActive(true);
        //给chooseActor赋值
        //开放目标点击控制  实时改变点击缓存内容
    }
    private void cancelConfirmPanel()
    {//关闭攻击面板
        chooseActor = -1;
        isChooseOneActor = false;
        baseControl.SetActive(true);
        tanban.SetActive(false);
        skillThirdFather.SetActive(false);
        clearArrowState();
    }
    private void attackButtonClick()
    {
        isChooseOneActor = true;    //允许出现选择箭头
        //进入二级界面
        chooseSkill = playerActor.AttackID;
        //无效化 基础四个按钮  
        baseControl.SetActive(false);
        //刷新箭头指向状态（就是待选目标的状态  重置为可选目标）
        resetArrowState();
        //弹出攻击面板
        showConfirmPanel();
    }
    private void propButtonClick()
    {

    }
    private void skillButtonClick()
    {
        lockBaseButton(true);
        showContext();
    }
    private void fleeButtonClick()
    {

    }
    //选择技能  显示三级 技能详情面板
    private void chooseSkillMessage(int id)
    {
        skillThirdFather.SetActive(true);
        string cont;
        cont="技能详情\n"+ AllUnitData.getSkillData(id)[1]+"\n    "+ AllUnitData.getSkillData(id)[2];
        skillThirdFather.GetComponentInChildren<Text>().text = cont;
        //var item = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        baseControl.SetActive(false);
        //刷新箭头指向状态（就是待选目标的状态  重置为可选目标）
        resetArrowState();
        tanban.SetActive(true);
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
