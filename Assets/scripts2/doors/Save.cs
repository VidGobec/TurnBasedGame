using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class PlayerData
{ 
    [System.Serializable]
    struct Characterinfo
    {
        public string name;
        public int exp;
        public int exptoadd;
        public int level;
        public string specialization; //future: class/sublass
        public string race; //future: enemy spawn based on regions, stat bonuses


        public float basehp;
        public float currenthp;

        public int basemp;
        public int currentmp;

        public int FOC; //healing
        public int PER; //chance to hit
        public int STR; //attack dmg
        public int INT; //spell dmg
        public int AGI; //dodge chance
        public int LCK; //crit chance
        public float SPD; //speed

        public int position; //possition in battlefield int 1-3 (3 2 1   1 2 3)

        public string[] abilitynames;

        public bool ai; //human control or ai : on1 0off

        public characterbuff[] buffs;
    }

    public int gold;
    public int food;
    public int hppotion;

    Characterinfo[] chrinfo = new Characterinfo[3];

    public string area; //name of area;
    public int floor;

    public PlayerData()
    {
        gold = gameinfo.gold;
        area = gameinfo.currarea.name;
        floor = gameinfo.floor;
        food = gameinfo.food;
        hppotion = gameinfo.hppotion;

        for (int i = 0; i < 3; i++)
        {
            chrinfo[i].name = gameinfo.hero[i].name;
            chrinfo[i].exp = gameinfo.hero[i].exp;
            chrinfo[i].exptoadd = gameinfo.hero[i].exptoadd;
            chrinfo[i].level = gameinfo.hero[i].level;
            chrinfo[i].specialization = gameinfo.hero[i].specialization;
            chrinfo[i].race = gameinfo.hero[i].race;
            chrinfo[i].basehp = gameinfo.hero[i].basehp;
            chrinfo[i].currenthp = gameinfo.hero[i].currenthp;
            chrinfo[i].basemp = gameinfo.hero[i].basemp;
            chrinfo[i].currentmp = gameinfo.hero[i].currentmp;
            chrinfo[i].FOC = gameinfo.hero[i].FOC;
            chrinfo[i].PER = gameinfo.hero[i].PER;
            chrinfo[i].STR = gameinfo.hero[i].STR;
            chrinfo[i].INT = gameinfo.hero[i].INT;
            chrinfo[i].AGI = gameinfo.hero[i].AGI;
            chrinfo[i].LCK = gameinfo.hero[i].LCK;
            chrinfo[i].SPD = gameinfo.hero[i].SPD;
            chrinfo[i].position = gameinfo.hero[i].position;
            chrinfo[i].ai = gameinfo.hero[i].ai;

            System.Array.Resize(ref chrinfo[i].abilitynames, gameinfo.hero[i].abilities.Count);
            for (int j = 0; j < gameinfo.hero[i].abilities.Count; j++)
                chrinfo[i].abilitynames[j] = gameinfo.hero[i].abilities[j].name;

            System.Array.Resize(ref chrinfo[i].buffs, gameinfo.hero[i].buffs.Count);
            for (int j = 0; j < gameinfo.hero[i].buffs.Count; j++)
                chrinfo[i].buffs[j] = gameinfo.hero[i].buffs[j];

            Debug.Log("writing wroks i guess");
        }
    }

    public character returnchar(int i)
    {
        character pchr = new character();

        pchr.AGI = chrinfo[i].AGI;
        pchr.ai = chrinfo[i].ai;
        pchr.basehp = chrinfo[i].basehp;
        pchr.basemp = chrinfo[i].basemp;
        pchr.currenthp = chrinfo[i].currenthp;
        pchr.currentmp = chrinfo[i].currentmp;
        pchr.exp = chrinfo[i].exp;
        pchr.exptoadd = chrinfo[i].exptoadd;
        pchr.FOC = chrinfo[i].FOC;
        pchr.INT = chrinfo[i].INT;
        pchr.LCK = chrinfo[i].LCK;
        pchr.level = chrinfo[i].level;
        pchr.name = chrinfo[i].name;
        pchr.PER = chrinfo[i].PER;
        pchr.position = chrinfo[i].position;
        pchr.race = chrinfo[i].race;
        pchr.SPD = chrinfo[i].SPD;
        pchr.specialization = chrinfo[i].specialization;
        pchr.STR = chrinfo[i].STR;
        //buffs
        for (int j = 0; j < chrinfo[i].buffs.Length; j++)
            pchr.buffs.Add(chrinfo[i].buffs[j]);

        //abilities
        for (int j = 0; j < chrinfo[i].abilitynames.Length; j++)
        {
            //ability abi = AssetDatabase.LoadAssetAtPath("Assets/scripts2/baseability/Resources/" + chrinfo[i].abilitynames[j] + ".asset", typeof(ability)) as ability;
            ability abi = Resources.Load<ability>(chrinfo[i].abilitynames[j]);
            Debug.Log(chrinfo[i].abilitynames[j] + ".asset");
            Debug.Log(abi);
            pchr.abilities.Add(abi);
            //gameinfo.hero[i].abilities.Add(Resources.Load("Assets/scripts2/baseability" + chrinfo[i].abilitynames[j] + ".asset") as ability);
        }
        return pchr;
    }

}