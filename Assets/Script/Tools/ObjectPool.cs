using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//对象池
public class ObjectPool : MonoBehaviour
{
    public RecycleObject prefab;
    public List<RecycleObject> poolInstance = new List<RecycleObject>();

    private RecycleObject createItem(Vector2 pos)
    {
        var clone = GameObject.Instantiate(prefab);
        clone.transform.position = pos;
        clone.transform.parent = transform;
        poolInstance.Add(clone);
        return clone;
    }
    public RecycleObject nextObject(Vector2 pos)
    {
        RecycleObject obj = null;
        foreach(var item in poolInstance)
        {
            if (!obj.gameObject.activeSelf)
            {
                obj = item;
                obj.transform.position = pos;
                break;
            }
        }
        if (obj == null) obj = createItem(pos);
        obj.restart();
        return obj;
    }
    public List<RecycleObject> getObjectPool()
    {
        return poolInstance;
    }
}
