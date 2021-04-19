using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : DDOLController<MainController>
{
    public EventManager eventManager;
    public void initController()
    {
        
    }

    public void openCombat()
    {
        List<CombatMessage> list=new List<CombatMessage>();
        CombatController.instance.openCombat(list);
    }





}
