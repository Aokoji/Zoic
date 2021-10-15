using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatConfigMessage 
{
    public string combatLogName;        //战斗日志编号
    public string combatActorsDescribe;     //战斗怪物群描述
    public string originalState;        //初始怪物状态（移动倾向）
    public string sceneName;    //场景名称
    public int initialDistance;        //初始距离
}
