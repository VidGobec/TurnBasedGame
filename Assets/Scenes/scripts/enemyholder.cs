using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyholder : MonoBehaviour
{
    public List<myenemy> allenemys = new List<myenemy>();
    private List<myenemy> usableenemys = new List<myenemy>();

    private int position = 1;

    public baseenemy createenemy(int herolvl)
    {
        pickenemys(herolvl);
        myenemy cenemy = chooseenemy();
        baseenemy enemy = Return(cenemy,herolvl);
        return enemy;
    }

    private void pickenemys(int herolvl)
    {
        for (int i = 0; i < allenemys.Count; i++)
        {
            if (allenemys[i].maxlvl >= herolvl && allenemys[i].minlvl <= herolvl)
                Debug.Log(allenemys.Count);
                usableenemys.Add(allenemys[i]);
        }
    }

    private myenemy chooseenemy()
    {
        //for now random
        myenemy pom = usableenemys[Random.Range(0, usableenemys.Count - 1)];
        return pom;
    }

    private baseenemy Return(myenemy myenemy, int herolvl)
    {
        baseenemy enemy = new baseenemy();
        enemy.name = "skeleton" + position.ToString(); //placeholder
        enemy.race = myenemy.race;  //placeholder
        enemy.specialization = myenemy.specialization;
        enemy.level = herolvl + Random.Range(myenemy.leveldown,myenemy.levelup);

        enemy.basehp = 70 + enemy.level * 5;
        enemy.currenthp = enemy.basehp;

        enemy.basemp = 50 + enemy.level * 5;
        enemy.currentmp = enemy.basemp;

        enemy.FOC = Mathf.RoundToInt(2 + enemy.level/2 * myenemy.FOC); //faith
        enemy.PER = Mathf.RoundToInt(2 + enemy.level / 2 * myenemy.PER); //chance to hit
        enemy.STR = Mathf.RoundToInt(2 + enemy.level / 2 * myenemy.STR); //attack dmg
        enemy.INT = Mathf.RoundToInt(2 + enemy.level / 2 * myenemy.INT); //spell dmg
        enemy.AGI = Mathf.RoundToInt(2 + enemy.level / 2 * myenemy.AGI); //dodge chance
        enemy.LCK = Mathf.RoundToInt(2 + enemy.level / 2 * myenemy.LCK); //crit chance
        enemy.SPD = Mathf.RoundToInt(2 + enemy.level / 2 * myenemy.SPD); //crit chance

        for (int i = 0; i < myenemy.abilities.Count; i++)
            enemy.abilities.Add(myenemy.abilities[i]);

        return enemy;
    }

}
