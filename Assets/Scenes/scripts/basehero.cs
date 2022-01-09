using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class basehero
{
    public string name;
    //private string race;
    public int level;
    public string specialization; //class/sublass


    public float basehp;
    public float currenthp;

    public int basemp;
    public int currentmp;

    public int VIT; //hp
    public int VIG; //sta
    public int FOC; //healing
    public int PER; //chance to hit
    public int STR; //attack dmg
    public int INT; //spell dmg
    public int AGI; //dodge chance
    public int LCK; //crit chance
    public float SPD; //speed

    public int position;

    public List<ability> abilities = new List<ability>();

    /*public int Level {
        get { return level; }
        set { level = value; }
    }*/

}
