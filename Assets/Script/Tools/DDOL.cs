using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//挂载器  需要优先挂在界面上
public class DDOL : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        //GameManager manager = FindObjectOfType(typeof(GameManager)) as GameManager;
        if (null== GameManager.gameManager) {
            GameManager.initManager();
        }
    }

}
