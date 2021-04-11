using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatView : MonoBehaviour
{
    public GameObject startPos;
    public GameObject endPos;
    public GameObject line;
    public Button attack;
    public Button skill;
    public Button bag;
    public Button run;

    private List<GameObject> icons = null;
    private float distance;

    public void initUI()
    {
        icons = new List<GameObject>();
        distance = startPos.transform.position.x - endPos.transform.position.x;
    }
    public void initItemData(List<CombatMessage> data)
    {
        foreach(var item in data)
        {
            GameObject actor = Resources.Load<GameObject>("Entity/actorIcon");
            GameObject loadactor = Instantiate(actor);
            loadactor.name = item.Name;
            loadactor.transform.SetParent(line.transform);
            loadactor.transform.position = startPos.transform.position;
            loadactor.SetActive(true);       //todo  待修改
            item.IconActor = loadactor;
            icons.Add(loadactor);
        }
    }
    public void setRelative(CombatMessage icon)
    {
        float dis = distance / 100 * icon.CurSpeed;
        icon.IconActor.transform.position = new Vector2(startPos.transform.position.x - dis, icon.IconActor.transform.position.y); 
    }
}
