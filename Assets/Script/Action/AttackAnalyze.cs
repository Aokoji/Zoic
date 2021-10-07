using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
//战斗分析器（新）+++待完成
public class AttackAnalyze : CombatAdapter
{
    public AttackAnalyze(List<CombatMessage> data)
    {
        initData(data);
    }




}
//返回类  存储类   当局战利品结算
public class spoilsResult
{
    public List<int> spoils = new List<int>();    //战利品id表
    public int coins;       //结算货币
    public int coinType;    //货币种类
}