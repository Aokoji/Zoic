using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecycleObject : MonoBehaviour
{
    private List<Irecycle> recycleComponent;

    private void Awake()
    {
        var components = GetComponents<MonoBehaviour>();
        recycleComponent = new List<Irecycle>();
        //初始化自身更改重启内容对象
        foreach(var r in components)
        {
            if(r is Irecycle)
            {
                recycleComponent.Add(r as Irecycle);
            }
        }
    }

    //重启
    public void restart()
    {
        gameObject.SetActive(true);
        foreach(var i in recycleComponent)
        {
            i.restart();
        }
    }
    //回收
    public void shutdown()
    {
        foreach(var i in recycleComponent)
        {
            i.shutdown();
        }
        gameObject.SetActive(false);
    }
}
public interface Irecycle
{
    void restart();
    void shutdown();
}
