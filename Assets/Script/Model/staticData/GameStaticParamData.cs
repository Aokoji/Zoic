using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStaticParamData 
{
    public static void initData()
    {

    }
    /// <summary>
    /// 玩家升级给与点数
    /// </summary>
    public static int getAbilityPoint(int level)
    {
        if (level <= 10 || level > 15) return 2;
        if (level > 10 && level <= 15) return 1;
        return 0;
    }

}
