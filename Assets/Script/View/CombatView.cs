using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatView : MonoBehaviour
{
    public GameObject startPos;         //头像条起点
    public GameObject endPos;           //头像条终点
    public GameObject line;                 //头像条
    public Transform[] enemySlots;   //敌对槽位
    public GameObject playSlots;        //玩家槽位
    public Button attack;
    public Button skill;
    public Button bag;
    public Button run;

    public Button attackConfirm;        //攻击确认  假设使用统一的确认取消按钮   不同层级之间要隔离  深层的子集打开时外层点击不再生效(也就是说 加panel)
    public Button attackCancel;         //攻击取消

    private List<GameObject> icons = null;              //速度条数据
    private List<GameObject> actorBody = null;
    private List<CombatMessage> _Data = null;       //全部数据
    public CombatMessage playerActor = null;        //玩家本体
    public int chooseSkill;                 //选择的技能   目标(仅限单体) 其他类型有自动识别
    public int chooseActor;             //在选择攻击目标时  或 释放环境技能确定窗时  提前赋值

    private float distance; //总的进度条长度
    private int ENEMY_NUM = 3;

    public void initMethod()
    {
        initUI();
        initBaseButtonEvent();
    }
    private void initUI()
    {
        icons = new List<GameObject>();
        actorBody = new List<GameObject>();
        distance = startPos.transform.position.x - endPos.transform.position.x;
        enemySlots = GetComponentsInChildren<Transform>();
    }
    private void initBaseButtonEvent()
    {//初始化自身基础按钮的功能   不包含最终的二级或深级界面功能按钮
        attack.onClick.AddListener(attackButtonClick);
        skill.onClick.AddListener(skillButtonClick);
        bag.onClick.AddListener(propButtonClick);
        run.onClick.AddListener(fleeButtonClick);
        attackCancel.onClick.AddListener(cancelAttackPanel);    //攻击二级分窗显示
    }
    public void initItemData(List<CombatMessage> data)
    {//+++需要创建头像图标    单位实体  并存储
        _Data = data;
        foreach (var item in data)
        {
            GameObject actor = Resources.Load<GameObject>("Entity/actorIcon");
            GameObject loadactor = Instantiate(actor);
            loadactor.name = item.Name;
            loadactor.transform.SetParent(line.transform);
            loadactor.transform.position = startPos.transform.position;
            loadactor.SetActive(true);       //todo  待修改
            item.IconActor = loadactor;
            icons.Add(loadactor);
            if (item.Name == "player") {
                item.IsPlayer = true;
                playerActor = item;
            }
            //加载单位
            GameObject actorbody= Resources.Load<GameObject>("Entity/combatActor");
            GameObject loadactorBody = Instantiate(actorbody);
            loadactorBody.name = item.Name;
            loadactorBody.SetActive(false);
            item.Prefab = loadactorBody;
            actorBody.Add(loadactorBody);
        }
    }
    //外部调用  布置场景 
    public void setSceneLayout()
    {
        // 加载人物
        int num = 1;
        //目前敌对单位最多三个
        foreach(var item in actorBody)
        {
            if (item.name == "player")
            {
                item.transform.SetParent(playSlots.transform);
            }
            else
            {
                item.transform.SetParent(enemySlots[num].transform);
            }
            item.SetActive(true);
        }
    }
    public bool playEnterScene()
    {
        return true;
    }

    private void showAttackPanel()
    {//显示攻击面板
        //给chooseActor赋值
        //开放目标点击控制  实时改变点击缓存内容
    }
    private void cancelAttackPanel()
    {//关闭攻击面板
        chooseActor = -1;
    }
    private void attackButtonClick()
    {
        //进入二级界面
        chooseActor = 0;
        chooseSkill = playerActor.AttackID;
        //无效化 基础四个按钮  
        //弹出攻击面板
        showAttackPanel();
    }
    private void propButtonClick()
    {

    }
    private void skillButtonClick()
    {

    }
    private void fleeButtonClick()
    {

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

}
