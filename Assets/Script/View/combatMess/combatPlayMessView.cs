using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class combatPlayMessView : MonoBehaviour
{
    public GameObject playerIconImg;        //左下角玩家头像
    public Text hptxt;
    public Text mptxt;
    public Image hpline;
    public Image mpline;

    private combatUnitProperty _Data;       //记录的玩家数据

    public void initPlayerData(combatUnitProperty data)
    {
        _Data = data;
        refreshNumbers();
    }

    public void refreshNumbers()
    {
        if(_Data==null)
        {
            Debug.LogError("加载战斗面板数据错误！！");
            return;
        }
        //设置体力
        hpline.fillAmount= (float)_Data.curHp/_Data.physical_last;
        hptxt.text = _Data.curHp + "/" + _Data.physical_last;
        //精力
        mpline.fillAmount = (float)_Data.curMp / _Data.vigor_last;
        mptxt.text = _Data.curMp + "/" + _Data.vigor_last;
    }
}
