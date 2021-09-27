﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testDataInterface 
{
    private static testDataInterface gamedata = new testDataInterface();
    public static testDataInterface Data
    {
        get
        {
            return gamedata;
        }
    }
    //升至x级
    public int levelAddTo = 0; 
    //加攻击
    public int attackAdd = 0; 
    //加血上线
    public int hpAdd = 0; 
    //加钱
    public int coinsAdd = 0;

    public void testDataAdd(ref PlayerMessage playermessage)
    {
        playermessage.data.attack_base += attackAdd;
        playermessage.items.coins += coinsAdd;
        playermessage.data.physical_base += hpAdd;
        playermessage.data.hpcur += hpAdd;
        if (levelAddTo > 0)
            playermessage.level = levelAddTo;


    }
}
