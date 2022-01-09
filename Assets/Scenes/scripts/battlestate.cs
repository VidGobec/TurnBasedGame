using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class battlestate : MonoBehaviour
{

    public enum action {
        wait,
        enemyturn,
        heroturn,
        performturn
    }

    public action battlestates;

    public List<turnhandler> list = new List<turnhandler>();
    public List<GameObject> heroes = new List<GameObject>();
    public List<GameObject> enemys = new List<GameObject>();

    //hero move
    public enum herogui {
        activate,
        waiting,
        input,
        attack,
        select,
        done
    }
    GameObject attacker;

    public herogui hgui;

    public GameObject[] actionbuttons;
    public turnhandler herochoice;
    public List<GameObject> heroestomanage = new List<GameObject>();
    //hero
    public static GameObject theattacked; //used to know who to move to

    //test
    public static int pomi=0;
    //public float dmg;
    // Start is called before the first frame update
    void Start()
    {
        //hgui = herogui.activate;
        battlestates = action.wait;
        heroes.AddRange(GameObject.FindGameObjectsWithTag("hero"));
        enemys.AddRange(GameObject.FindGameObjectsWithTag("enemy"));
        theattacked = enemys[0];
        herocombatstate.theattacked = enemys[0];

        actionbuttons = GameObject.FindGameObjectsWithTag("buttons");
        disablebuttons();
    }

    // Update is called once per frame
    void Update() {


        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.transform != null && hit.transform.gameObject.tag == "enemy")
            {
                theattacked = hit.transform.gameObject;
                herocombatstate.theattacked = hit.transform.gameObject;
                Debug.Log(herocombatstate.theattacked.name);
            }
        }

        switch (battlestates) {
            case (action.wait):
                if (heroestomanage.Count > 0)
                {
                    Debug.Log(heroestomanage[0].name);
                    if (heroestomanage[0].tag == "enemy" /*&& list.Count <= 0*/)
                    {
                        enemycombatstate est = heroestomanage[0].GetComponent<enemycombatstate>();
                        est.currstate = enemycombatstate.battlestates.chooseaction;
                        battlestates = action.enemyturn;
                    }
                    else if (heroestomanage[0].tag == "hero" /*&& list.Count <= 0*/)
                    {
                        battlestates = action.heroturn;
                    }
                   // else if (heroestomanage[0].tag == "enemy" && list.Count > 0)
                        //battlestates = action.enemyturn;
                }
                break;

            case (action.enemyturn):
                enemycombatstate pest = list[0].atacker.GetComponent<enemycombatstate>();
                herocombatstate phst = list[0].target.GetComponent<herocombatstate>();
                while(pomi < list[0].ability.amount)
                {  
                    if (list[0].ability.type.ToString() == "random")
                    {
                        list[0].target = getrandomtarget("hero");
                    }
                    else if (list[0].ability.type.ToString() == "chaos")
                    {
                        list[0].target = getrandomtarget("chaos");
                    }
                    phst.calcdamage(list[0].damage);
                    pest.currstate = enemycombatstate.battlestates.anime;
                    Debug.Log("battlestates: " + pomi);
                }
                pest.animator.SetBool("magic", false); //so he stayes in casting form until this point
                pest.currstate = enemycombatstate.battlestates.waiting;
                battlestates = action.performturn;
                break;
            case (action.heroturn):
                if (herocombatstate.animationdone == true && enemycombatstate.animationdone == true)
                {
                    enablebuttons();
                }

                //GameObject attacker = GameObject.Find(list[0].attackername);
                //herocombatstate hst = attacker.GetComponent<herocombatstate>();
                //list[0].target.GetComponent<enemycombatstate>().calcdamage(list[0].damage);
                //hst.currstate = herocombatstate.battlestates.anime;
                break;
            case (action.performturn):
                pomi = 0;
                list.RemoveAt(0);
                heroestomanage.RemoveAt(0);
                disablebuttons();
                battlestates = action.wait;
                break;
        }
        /*
        switch (hgui) {
            case (herogui.activate):
                if (heroestomanage.Count > 0)
                {
                    //heroestomanage[0].transform.FindChild("selector").gameObject.SetActive(true);
                    foreach (GameObject button in actionbuttons)
                    {
                        button.GetComponent<UnityEngine.UI.Button>().interactable = true;
                        
                    }
                    hgui = herogui.waiting;
                    herochoice = new turnhandler();
                }
                break;
            case (herogui.attack):

                break;
            case (herogui.done):

                break;
            case (herogui.input):

                break;
            case (herogui.waiting):

                break;

        }*/
                }

    public void disablebuttons()
    {
        foreach (GameObject button in actionbuttons)
        {
            button.GetComponent<UnityEngine.UI.Button>().interactable = false;
        }
    }

    public void enablebuttons()
    {
        foreach (GameObject button in actionbuttons)
        {
            button.GetComponent<UnityEngine.UI.Button>().interactable = true;
        }
    }

    public float attackvalueenemy(ability ability, baseenemy enemy)
    {

        int crit = 1;
        float dmg;

        if (Random.Range(1, 100) <enemy.LCK) crit = 2;
        if (ability.dmgtype.ToString() == "magic")
        {
            dmg = enemy.INT * ability.mod * crit;
        }
        else
        {
            dmg = enemy.STR * ability.mod * crit;
        }
        Debug.Log(dmg);
        return dmg;

    }

    public float attackvaluehero(ability ability, herocombatstate hst)
    {

        int crit = 1;
        float dmg;
        
        if (Random.Range(1, 100) < hst.hero.LCK) crit = 2;
        if (ability.dmgtype.ToString() == "magic")
        {
            dmg = hst.hero.INT * ability.mod * crit;
        }
        else
        {
            dmg = hst.hero.STR * ability.mod * crit;
        }
        return dmg;

    }

    public void getaction(turnhandler input) {
        list.Add(input);
    }

    public void attack()
    {
        herochoice.attackername = heroestomanage[0].name;
        herochoice.atacker = heroestomanage[0];
        //herochoice.type = "hero";
        herochoice.damage = 5;
        herochoice.target = herocombatstate.theattacked;

        disablebuttons();
    
        list.Add(herochoice);
        //heroestomanage[0].transform.FindChild("selector").gameObject.SetActive(false);
        heroestomanage.RemoveAt(0);
        hgui = herogui.activate;


    }

    public void basicattack()
    {
        
        herocombatstate hst = heroestomanage[0].GetComponent<herocombatstate>();
        herochoice.ability = hst.hero.abilities[0];
        herochoice.damage = attackvaluehero(herochoice.ability, hst);
        herochoice.atacker = heroestomanage[0];
        herochoice.attackername = hst.hero.name;
        herochoice.target = theattacked;
        //herochoice.target.GetComponent<enemycombatstate>().calcdamage(herochoice.damage);
        hst.currstate = herocombatstate.battlestates.anime;
        getaction(herochoice);

        enemycombatstate est = theattacked.GetComponent<enemycombatstate>();
        est.calcdamage(list[0].damage);

        //list.RemoveAt(0);
        battlestates = action.performturn;
        disablebuttons();
    }

    public void endturn()
    {
        foreach (GameObject button in actionbuttons)
        {
            button.GetComponent<UnityEngine.UI.Button>().interactable = false;

        }
        GameObject attacker = GameObject.Find(list[0].attackername);
        herocombatstate hst = attacker.GetComponent<herocombatstate>();
        list[0].target.GetComponent<enemycombatstate>().calcdamage(list[0].damage);
        hst.currstate = herocombatstate.battlestates.anime;
        Debug.Log("crashes here");
        heroestomanage.RemoveAt(0);
    }

    public GameObject getrandomtarget(string who) {
        GameObject rantarget;
        if (who == "hero")
        {
            rantarget = heroes[Random.Range(0,heroes.Count)];
        }
        else if (who == "enemy")
        {
            rantarget = enemys[Random.Range(0, heroes.Count)];
        }
        else
        {
            List<GameObject> all = new List<GameObject>();
            all.AddRange(heroes);
            all.AddRange(enemys);
            rantarget = all[Random.Range(0, all.Count)];
        }
        return rantarget;
    }

   

    /*
    public void enemytarget(enemycombatstate est)
    {
        int p = Random.RandomRange(0, heroes.Count);
        list[0].target= heroes[p];
        est.theattacked = heroes[p];
    }*/
        
    }
