using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatActorItem : MonoBehaviour
{
    public GameObject arrow;
    public GameObject hpcontrol;
    public Image hpctl;

    public void chooseArrowChange(bool isShow)
    {
        arrow.SetActive(isShow);
    }


}
