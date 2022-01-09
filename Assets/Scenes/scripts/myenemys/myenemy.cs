using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "new enemy", menuName = "new enemy")]
public class myenemy : ScriptableObject
{
    public string name;
    public int levelup; //+ playerlvl
    public int leveldown;//+ playerlvl
    public int maxlvl;//+ playerlvl
    public int minlvl;//+ playerlvl
    public string race; // idk
    public string specialization; //class

    public float basehp;  //default hp

    public int basemp; //default mp

    //modifiers
    public float FOC=1; 
    public float PER=1; 
    public float STR=1; 
    public float INT=1; 
    public float AGI=1; 
    public float LCK=1; 
    public float SPD=1; 

    public List<ability> abilities = new List<ability>();
}