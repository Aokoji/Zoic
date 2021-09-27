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

public abstract class DDOLData<T> : MonoBehaviour where T : DDOLData<T>
{
    protected static T data = null;
    public static T Data
    {
        get {
            if (null == data)
            {
                GameObject dataprefeb = GameObject.FindGameObjectWithTag("DDOLDATA");
                if (null == dataprefeb)
                {
                    GameObject manage = new GameObject("DDOLData");
                    manage.tag = "DDOLDATA";
                    data = manage.AddComponent<T>();
                    DontDestroyOnLoad(manage);
                }
                else
                {
                    data = dataprefeb.AddComponent<T>();
                }
            }
            return data;
        }
    }
}
