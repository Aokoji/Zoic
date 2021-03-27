using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DDOLController <T>: MonoBehaviour where T: DDOLController<T>
{
    protected static T instance = null;
    public static T Instance
    {
        get {
            if (null == instance)
            {
                GameManager manage = FindObjectOfType(typeof(GameManager)) as GameManager;

                if (null == manage)
                {
                    GameManager.initManager();
                    manage = GameManager.gameManager;
                }
                GameObject ctrl = manage.gameObject;
                instance = ctrl.GetComponent<T>();
                if (null == instance)
                {
                    instance = ctrl.AddComponent<T>();
                }
            }
            return instance;
        }
    }
}
