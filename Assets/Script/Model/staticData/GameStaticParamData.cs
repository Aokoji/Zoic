﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStaticParamData 
{
    public static void initData()
    {

    }
    private static int lowGive = 1;
    private static int midGive = 2;
    private static int highGive = 2;
    /// <summary>
    /// 玩家升级给与点数
    /// </summary>
    public static int getAbilityPoint(int level)
    {
        if (level <= 10 || level > 15) return 1;
        if (level > 10 && level <= 15) return 2;
        return 0;
    }

    //战斗动画名称
    public static CombatAnimNameList combatAnimNameList = new CombatAnimNameList();

}

public class CombatAnimNameList
{
    public string attackNormalName = "playerActionAttack";    //普攻
    public string behitNormalName = ""; //受击
    public string hitMagicName = "";        //伤害魔法
    public string chantMagicName = "";  //吟唱魔法
    public string buffsName = "";  //增幅
    public string debuffsName = "";  //减益
    public string dodgeName = "";  //闪避
    public string deadName = "";  //死亡
}