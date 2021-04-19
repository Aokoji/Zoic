using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatView : MonoBehaviour
{
    public GameObject startPos;
    public GameObject endPos;
    public GameObject line;
    public Button attack;
    public Button skill;
    public Button bag;
    public Button run;

    public Button attackConfirm;        //攻击确认
    public Button attackCancel;         //攻击取消

    private List<GameObject> icons = null;              //速度条数据
    private List<CombatMessage> _Data = null;       //全部数据
    public CombatMessage chooseActor = null;       //选择中的目标
    public CombatMessage playerActor = null;        //玩家本体
    private float distance; //总的进度条长度

    public void initMethod()
    {
        initUI();
        initBaseButtonEvent();
    }
    private void initUI()
    {
        icons = new List<GameObject>();
        distance = startPos.transform.position.x - endPos.transform.position.x;
    }
    private void initBaseButtonEvent()
    {//初始化自身基础按钮的功能   不包含最终的二级或深级界面功能按钮
        attack.onClick.AddListener(showAttackPanel);    //攻击二级分窗显示
        attack.onClick.AddListener(cancelAttackPanel);    //攻击二级分窗消失
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
            if (item.Name == "player") playerActor = item;
        }
    }
    private void showAttackPanel()
    {//显示攻击面板
        //给chooseActor赋值
    }
    private void cancelAttackPanel()
    {//关闭攻击面板
        chooseActor = null;
    }
    public void setRelative(CombatMessage icon)
    {//实时设置各个图标跑的进度
        float dis = distance / 100 * icon.CurSpeed;
        icon.IconActor.transform.position = new Vector2(startPos.transform.position.x - dis, icon.IconActor.transform.position.y);
    }

}
