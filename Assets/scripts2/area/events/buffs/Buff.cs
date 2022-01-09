using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "new buff", menuName = "new buff")]
public class Buff : ScriptableObject
{
    public bool all = true;
    public bool good = true;

    public int health = 0;
    public int mana = 0;
    public int FOC = 0; //healing
    public int PER = 0; //chance to hit
    public int STR = 0; //attack dmg
    public int INT = 0; //spell dmg
    public int AGI = 0; //dodge chance
    public int LCK = 0; //crit chance
    public float SPD = 0;

    public int lenght=0;

    public int money = 0;
    public int food = 0;

    public string afterevent = "";
}


