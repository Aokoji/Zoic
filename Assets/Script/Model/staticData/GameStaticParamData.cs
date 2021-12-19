using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using toolEntity;

public static class GameStaticParamData 
{
    public static void initData()
    {

    }
    //玩家速度
    public static float playerSpeed= 2.5f;

    public static float timePer20=0.05f;
    private static int lowGive = 1;
    private static int midGive = 2;
    private static int highGive = 2;
    /// <summary>
    /// 玩家升级给与点数
    /// </summary>
    public static int getAbilityPoint(int level)
    {
        if (level <= 10) return midGive;        //18p
        if (level > 10 && level <= 15) return lowGive;      //5p
        if (level > 15 && level<=18) return highGive;       //9p
        if (level > 18) return lowGive;         //82p
                                                                //agg   114p
        return 0;
    }

    //战斗动画名称
    public static CombatAnimNameList combatAnimNameList = new CombatAnimNameList();
    //unit类调用名称
    public static UnitTypeNameList unitName = new UnitTypeNameList();
    //动画序号转化
    public static string combatAnimIDTrans(int id)
    {
        switch (id)
        {
            case 1:return combatAnimNameList.attackNormalName;
            case 2:return combatAnimNameList.behitNormalName;
            case 3:return combatAnimNameList.hitPhysicsName;
            case 4:return combatAnimNameList.hitMagicName;
            case 5:return combatAnimNameList.chantMagicName;
            case 6:return combatAnimNameList.buffsName;
            case 7:return combatAnimNameList.debuffsName;
            case 8:return combatAnimNameList.dodgeName;
            case 9:return combatAnimNameList.deadName;
            default:return "";
        }
    }

    //巡逻状态数列            //+++距离要根据实际情况调整
    ///进攻性10级
    private static int[,] fieldDataList = {
        {10,0,10,1},
        {10,0,10,1},
        {10,0,10,1},
        {10,0,10,1},
        {10,0,10,1},
        {10,0,10,1},
        {10,0,10,1},
        {10,0,10,1},
        {10,0,10,1},
    };
    public static int runAwayTime = 3;      //逃跑时间
    public static float patrolTime = 2.5f;      //idle时间
    public static FieldTypeList getFieldEnemyTypeData(int id)
    {
        FieldTypeList field = new FieldTypeList();
        field.alertLength = fieldDataList[id, 0];
        field.isAlert = fieldDataList[id, 1]==1;
        field.alertMinLength = fieldDataList[id, 2];
        field.isrunAway = fieldDataList[id, 3]==1;
        return field;
    }

}

//巡逻类型变量
public class FieldTypeList
{
    public int alertLength;     //警戒距离
    public bool isAlert;     //是否警惕
    public int alertMinLength;  //最小行动距离（有警戒的话）
    public bool isrunAway;      //（不警戒或超最小距离）是否逃跑
}

namespace toolEntity
{
    public class CombatAnimNameList
    {
        public string attackNormalName = "playerActionAttack";    //普攻
        public string behitNormalName = "playerActionAttack"; //受击
        public string hitPhysicsName = "playerActionAttack"; //物理技能
        public string hitMagicName = "playerActionAttack";        //伤害魔法
        public string chantMagicName = "playerActionAttack";  //吟唱魔法
        public string buffsName = "playerActionAttack";  //增幅
        public string debuffsName = "playerActionAttack";  //减益
        public string dodgeName = "playerActionAttack";  //闪避
        public string deadName = "playerActionAttack";  //死亡
    }
    //取数据 json字典的名字
    public class UnitTypeNameList
    {
        public string unit = "allUnitData";
        public string skill = "allSkillData";
        public string unitskill = "allUnitSkillData";
        public string spoil = "allSpoilData";
        public string goods = "allGoodData";
        public string collect = "allCollcetData";
        public string abnormal = "allAbnormalData";
        public string equip = "allEquipData";
    }
}
