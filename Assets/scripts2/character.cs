using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new character", menuName = "new character")]
[System.Serializable]
public class character : ScriptableObject
{
    public string name;
    public int exp;
    public int exptoadd;
    public int level;
    public string specialization; 
    public string race; 


    public float basehp;
    public float currenthp;

    public int basemp;
    public int currentmp;

    public int FOC; //healing
    public int PER; //chance to hit and scout
    public int STR; //attack dmg
    public int INT; //spell dmg
    public int AGI; //dodge chance
    public int LCK; //crit chance
    public float SPD; //speed

    public int position; //possition in battlefield int 1-3 (3 2 1   1 2 3)

    public List<ability> abilities = new List<ability>();

    public bool ai; //human control or ai : on1 0off

    //buffs
    public List<characterbuff> buffs = new List<characterbuff>();
}
