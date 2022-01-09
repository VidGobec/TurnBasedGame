using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "new ability", menuName = "baseability")]
public class ability : ScriptableObject{
    public new string name;
    public string desc;
    public float mod;  // damage delt
    public int cost;
    public int amount; //number of attacks

    public enum Target
    {
        single,
        fsingle,
        aoe,
        faoe,
        random,
        self
    }
    public enum Closefar
    {
        melee,
        ranged
    }
    public enum Dmgtype
    {
        magical,
        physical
    }
    public enum Type
    {
        dmg,
        buff
    }

    public Closefar closefar;
    public Dmgtype dmgtype;
    public Type type;
    public Target target;


    public GameObject projectileprefab;
    public float scale = 0;
}






/*
public class ability
{
    private string abilityname;
    private string abilitydesc;
    private int abilitypower;
    private int abilitycost;
    //private list<abilitybehaviours> behaviors;
    //private int cooldown;
    //private GameObject particle
    private abilitytype atype;

    public enum abilitytype
    {
        ranged,
        melee,
        spell,
        aoespell,
        buff,
        aoebuff,
        debuff,
        aoedebuff
    };
    

    public ability(string aname, string adesc, int apow, int acost/*, string type /*,int acd, list<abilitybehaviors> abeh*, GameObject aparticle)
    {
        abilityname = aname;
        abilitydesc = adesc;
        abilitypower = apow;
        abilitycost = acost;
        //atype = type;
        //cooldown = acd;
        //behaviors = new list<abilitybehaviours>();
        //particle=aparticle;
    }
    
    public string aname {
        get { return abilityname; }
    }

    public string adesc
    {
        get { return abilitydesc; }
    }

    public int apow
    {
        get { return abilitypower; }
    }

    public int acost
    {
        get { return abilitycost; }
    }

    

}
*/