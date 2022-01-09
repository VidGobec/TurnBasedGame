using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class baseenemy
{
    public string name;
    public int level;
    public string race;
    public string specialization;

    public float basehp;
    public float currenthp;

    public int basemp;
    public int currentmp;

    public int VIT; //hp
    public int VIG; //sta
    public int FOC; //mana
    public int PER; //chance to hit
    public int STR; //attack dmg
    public int INT; //spell dmg
    public int AGI; //dodge chance
    public int LCK; //crit chance
    public float SPD; //crit chance

    public List<ability> abilities = new List<ability>();
}
