using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class characterbuff
{

    public int FOC = 0; //healing
    public int PER = 0; //chance to hit
    public int STR = 0; //attack dmg
    public int INT = 0; //spell dmg
    public int AGI = 0; //dodge chance
    public int LCK = 0; //crit chance
    public float SPD = 0;

    public int lenght = 0;

    public characterbuff(Buff buff)
    {

        FOC = buff.FOC;
        PER = buff.PER;
        STR = buff.STR;
        INT = buff.INT;
        AGI = buff.AGI;
        LCK = buff.LCK;
        SPD = buff.SPD;
        lenght = buff.lenght;
    }
}
