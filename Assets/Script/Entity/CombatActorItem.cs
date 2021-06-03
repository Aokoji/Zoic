using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatActorItem : MonoBehaviour
{
    public GameObject arrow;

    public void chooseArrowChange(bool isShow)
    {
        arrow.SetActive(isShow);
    }
}
