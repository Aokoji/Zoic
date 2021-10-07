using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagOneRowView : MonoBehaviour
{
    public int iconPoint = 0;   //远程设置下标

    public void clearChildIcon()
    {
        iconPoint = 0;
    }
    public bool addOneIcon(oneGood good)
    {
        return true;
    }
    
}
