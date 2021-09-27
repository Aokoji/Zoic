using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//战斗接口  实现战斗的基本流程
public interface CombatInterface 
{
    void normalAttack();
    void specialAttack();
}


public abstract class combatAna : CombatInterface
{
    public void normalAttack()
    {
        
    }

    public void specialAttack()
    {
        
    }
}