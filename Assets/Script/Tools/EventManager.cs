using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void OperationEvent();
    public event OperationEvent operate = null;


    public delegate void CombatEvent();
    public event CombatEvent combat = null;
    public event CombatEvent combatStart = null;
    public event CombatEvent combatEnd = null;
    //战斗事件监听
    public void doCombat()
    {
        combatStart();
        enterCombat();
        //告知场景控制器
        combat();   //加载战斗数据
        combatEnd();
    }
    public void enterCombat()
    {
        Debug.Log("进入战斗场景加载动画");
    }
}
