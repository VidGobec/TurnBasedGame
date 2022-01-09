using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class combatmanager : MonoBehaviour
{
    public List<GameObject> heroes = new List<GameObject>();
    public List<GameObject> enemies = new List<GameObject>();


    public List<GameObject> tomanage = new List<GameObject>();

    public GameObject projectileprefab;

    public enum battlestates
    {
        waiting,
        waiting2,
        taketurn,
        end
    }

    public int exp = 0; 
    public battlestates state;

    public GameObject playertarget;

    //player buttons
    public GameObject[] playerbuttons;

    private int goldtoget = 0;
    private bool winlose; //1 0
    // Start is called before the first frame update
    void Start()
    {
        spawncharacter(Random.Range(1,4));
        playerbuttons = GameObject.FindGameObjectsWithTag("buttons");

        //playertarget = enemies[0];

        for (int i = 0; i < gameinfo.hero.Length; i++) gameinfo.hero[i].ai = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            
            if (state == battlestates.end)
            {
                if (winlose == true)
                {
                    for (int j = 0; j < gameinfo.hero.Length; j++)
                    {
                        gameinfo.hero[j].exptoadd = exp + 25;
                        gameinfo.gold += goldtoget;
                        goldtoget = 0;
                        if (gameinfo.hero[j].currenthp <= 0) gameinfo.hero[j].currenthp = 1;
                        Debug.Log(exp);
                    }
                    SceneManager.LoadScene("winning"); //get exp
                }
                else
                    SceneManager.LoadScene("Losing"); //lose scene
            }
            else if (hit.transform != null && hit.transform.gameObject.tag == "enemy")
            {
                playertarget = hit.transform.gameObject;
            }
            
        }



        switch (state)
        {
            case (battlestates.waiting):
                if (checkheroes() == true) //if heroes dead
                {
                    GameObject pom = GameObject.Find("winlosetext");
                    Text text = pom.GetComponent<Text>();
                    text.text = "DEFEAT";
                    state = battlestates.end;
                    winlose = false;
                }
                else if (checkenemies() == true)
                {

                    GameObject pom = GameObject.Find("winlosetext");
                    Text text = pom.GetComponent<Text>();
                    text.text = "VICTORY";
                    state = battlestates.end;
                    winlose = true;
                }
                else if (tomanage.Count > 0)
                    state = battlestates.taketurn;
                break;
            case (battlestates.taketurn):
                charactercombatstate pomchr = tomanage[0].GetComponent<charactercombatstate>();
                if (pomchr.me.ai == true)
                {
                    turninfo tinfo = new turninfo();
                    tinfo.turnability = pomchr.randomaiability();
                    tinfo.attacker = tomanage[0];
                    tinfo = addtarget(tinfo);
                    
                    if (tinfo.turnability.closefar == ability.Closefar.melee) pomchr.meleeturn(tinfo);
                    else if(tinfo.turnability.type == ability.Type.dmg) pomchr.rangedturn(tinfo);
                    else StartCoroutine(pomchr.ranged2turn(tinfo));

                    state = battlestates.waiting2;
                }
                else
                {
                    //shit code
                    if (tomanage[0].GetComponent<charactercombatstate>().me.ai == false)
                    {
                        foreach (GameObject button in playerbuttons)
                        {
                            button.GetComponent<UnityEngine.UI.Button>().interactable = true;
                        }
                    }
                    else
                    {
                        foreach (GameObject button in playerbuttons)
                        {
                            button.GetComponent<UnityEngine.UI.Button>().interactable = false;
                        }
                    }
                }
                break;
            case (battlestates.waiting2):
                if (tomanage.Count < 1)
                    state = battlestates.waiting;
                break;
            case (battlestates.end):
                break;
        }
    }

    public void addtolist(GameObject chr) {
        tomanage.Add(chr);
    }

    public void addenemy(GameObject pom) {
        enemies.Add(pom);
    }

    public void addhero(GameObject pom)
    {
        heroes.Add(pom);
    }

    public void deadhero(GameObject pom)
    {
        heroes.Add(pom);
    }

    public bool checkheroes()
    {
        if (heroes.Count <= 0)
            for (int i = 0; i < enemies.Count; i++)
            {
                charactercombatstate pom = enemies[i].GetComponent<charactercombatstate>();
                pom.reduceprogress();
                tomanage.Clear();
                state = battlestates.end;
                pom.currstate = charactercombatstate.battlestates.win;
                return true;
            }
        return false;

    }

    public bool checkenemies()
    {
        if (enemies.Count <= 0)
            for (int i = 0; i < heroes.Count; i++)
            {
                charactercombatstate pom = heroes[i].GetComponent<charactercombatstate>();
                pom.reduceprogress();
                tomanage.Clear();
                state = battlestates.end;
                pom.currstate = charactercombatstate.battlestates.win;
            }
        else
            return false;

        return true;
        //win panel
    }


    public void remove() {
        tomanage.RemoveAt(0);
        state = battlestates.waiting; // probs bad maybe change later
    }
    //add targets
    /*
    public GameObject getrandomtarget(ability ability) {
        charactercombatstate pomchr = tomanage[0].GetComponent<charactercombatstate>();
        if (ability.target == ability.Target.self) return tomanage[0];
        else if (ability.target ==)
    } idk man*/

    public turninfo addtarget(turninfo tinfo) { //random flaot [inclusive,inclusive] |||| random int [inclusive, exclusive]
        if (tinfo.turnability.target == ability.Target.self) tinfo.target.Add(tomanage[0]);
        else if (enemies.Contains(tinfo.attacker) == true)
        {
            if (tinfo.turnability.target == ability.Target.single) tinfo.target.Add(heroes[Random.Range(0, heroes.Count)]);
            else if (tinfo.turnability.target == ability.Target.fsingle) tinfo.target.Add(enemies[Random.Range(0, enemies.Count)]);
            else if (tinfo.turnability.target == ability.Target.random) for (int i = 0; i < tinfo.turnability.amount; i++) tinfo.target.Add(heroes[Random.Range(0, heroes.Count)]);
            else if (tinfo.turnability.target == ability.Target.aoe) for (int i = 0; i < heroes.Count; i++) tinfo.target.Add(heroes[i]);
            else if (tinfo.turnability.target == ability.Target.faoe) for (int i = 0; i < enemies.Count; i++) tinfo.target.Add(enemies[i]);
            //trueaoe
            //else {
            //for (int i = 0; i < tinfo.turnability.amount; i++) tinfo.target.Add(heroes[Random.Range(0, heroes.Count)]);
            //for (int i = 0; i < tinfo.turnability.amount; i++) tinfo.target.Add(enemies[Random.Range(0, enemies.Count)]);
            //}
        }
        else
        {
            if (tinfo.turnability.target == ability.Target.single) tinfo.target.Add(enemies[Random.Range(0, enemies.Count)]);
            else if (tinfo.turnability.target == ability.Target.fsingle) tinfo.target.Add(heroes[Random.Range(0, heroes.Count)]);
            else if (tinfo.turnability.target == ability.Target.random) for (int i = 0; i < tinfo.turnability.amount; i++) tinfo.target.Add(enemies[Random.Range(0, enemies.Count)]);
            else if (tinfo.turnability.target == ability.Target.aoe) for (int i = 0; i < enemies.Count; i++) tinfo.target.Add(enemies[i]);
            else if (tinfo.turnability.target == ability.Target.faoe) for (int i = 0; i < heroes.Count; i++) tinfo.target.Add(heroes[i]);
            //trueaoe
            //else {
            //for (int i = 0; i < tinfo.turnability.amount; i++) tinfo.target.Add(heroes[Random.Range(0, heroes.Count)]);
            //for (int i = 0; i < tinfo.turnability.amount; i++) tinfo.target.Add(enemies[Random.Range(0, enemies.Count)]);
            //}
        }
        return tinfo;
    }

    //combat infomakers
    public float calculatepdmg(ability ability, charactercombatstate attacker, charactercombatstate defender)
    {
        float dmg;
        dmg = ability.mod * (attacker.me.STR /*- def values someday*/);
        if (Random.Range(1, 100) < attacker.me.LCK)
            dmg = dmg + (dmg * Random.Range(4 / 10, 6 / 10));
        if (Random.Range(1, 100) < defender.me.AGI)
            dmg = 0;
        return dmg;
    }

    public float calculatemdmg(ability ability, charactercombatstate attacker, charactercombatstate defender)
    {
        float dmg;
        dmg = ability.mod * (attacker.me.INT /*- def values someday*/);
        if (Random.Range(1, 100) < attacker.me.LCK)
            dmg = dmg + (dmg * Random.Range(4/10, 6/10));
        if (Random.Range(1, 100) < defender.me.AGI)
            dmg = 0;
        return dmg;
    }

    public float calculatehealing(ability ability, charactercombatstate attacker, charactercombatstate defender)
    {
        float dmg;
        dmg = ability.mod * (attacker.me.FOC) * (-1); //-1 new
        if (Random.Range(1, 100) < attacker.me.LCK)
            dmg = dmg + (dmg * Random.Range(4 / 10, 6 / 10));
        Debug.Log("healing" + dmg);
        return dmg;
    }

    //button fuctions
    public void disablebuttons()
    {
        foreach (GameObject button in playerbuttons)
        {
            button.GetComponent<UnityEngine.UI.Button>().interactable = false;
        }
    }

    public void basicattack()
    {
        turninfo tinfo = new turninfo();
        charactercombatstate pomchr = tomanage[0].GetComponent<charactercombatstate>();
        tinfo.turnability = pomchr.me.abilities[0];
        tinfo.attacker = tomanage[0];
        tinfo.target.Add(playertarget);
        //Debug.Log("ability =" + tinfo.turnability.name + ", attacker" + tinfo.attacker.name);
        //for (int i = 0; i < tinfo.target.Count; i++) Debug.Log(tinfo.target[i].name);
        pomchr.meleeturn(tinfo);

        state = battlestates.waiting2;

        disablebuttons();
    }

    public void endturn()
    {
        charactercombatstate pomchr = tomanage[0].GetComponent<charactercombatstate>();
        pomchr.updatemanabar(-10);
        remove();
        disablebuttons();
        state = battlestates.waiting;
    }

    public void abilitytext()
    {
        //kinda shit
        for (int i = 1; i <= 8; i++)
        {
            GameObject GO = GameObject.Find("Canvas/abilityselection/ability" + i);
            GO.SetActive(false);
        }

        

        charactercombatstate pomchr = tomanage[0].GetComponent<charactercombatstate>();
        for (int i = 1; i < pomchr.me.abilities.Count; i++)
        {
            Debug.Log("abilitys " + i);
            GameObject GO = GameObject.Find("Canvas/abilityselection/ability" + i);
            GO.SetActive(true);
            Text myText = GameObject.Find("Canvas/abilityselection/ability" + i + "/Cost").GetComponent<Text>();
            myText.text = pomchr.me.abilities[i].cost.ToString();
            myText = GameObject.Find("Canvas/abilityselection/ability" + i + "/name").GetComponent<Text>();
            myText.text = pomchr.me.abilities[i].name;
        }
    }

    public void executeability(int abi)
    {
        charactercombatstate pomchr = tomanage[0].GetComponent<charactercombatstate>();
        turninfo tinfo = new turninfo();
        tinfo.turnability = pomchr.me.abilities[abi];
        tinfo.attacker = tomanage[0];

        //Choose target needs idiotproof
        if (tinfo.turnability.target == ability.Target.single)
            tinfo.target.Add(playertarget);
        else
            tinfo = addtarget(tinfo);

        if (tinfo.turnability.closefar == ability.Closefar.melee)
        {
            if (pomchr.me.currentmp - pomchr.me.abilities[abi].cost > 0)
            {
                pomchr.updatemanabar(pomchr.me.abilities[abi].cost);
                pomchr.meleeturn(tinfo);
            }
            else
                ;//debuff attack or idk yet
        }
        else if (tinfo.turnability.type == ability.Type.dmg)
        {
            if (pomchr.me.currentmp - pomchr.me.abilities[abi].cost > 0)
            {
                pomchr.updatemanabar(pomchr.me.abilities[abi].cost);
                pomchr.rangedturn(tinfo);
            }
            else
                switch (chanceoffailure())//explode
                {
                    case 0:
                        pomchr.updatemanabar(tinfo.turnability.cost);
                        pomchr.rangedturn(tinfo);
                        break;
                    case 1:
                        pomchr.failureanim();
                        //make failure animation
                        break;
                    case 2:
                        StartCoroutine(pomchr.explode(pomchr.me.abilities[abi].cost));
                        break;
                }
        }
        else
        {
            if (pomchr.me.currentmp - pomchr.me.abilities[abi].cost > 0)
            {
                pomchr.updatemanabar(pomchr.me.abilities[abi].cost);
                StartCoroutine(pomchr.ranged2turn(tinfo));
            }
            else
                switch (chanceoffailure())//explode
                {
                    case 0:
                        pomchr.updatemanabar(tinfo.turnability.cost);
                        StartCoroutine(pomchr.ranged2turn(tinfo));
                        break;
                    case 1:
                        pomchr.failureanim();//make failure animation
                        break;
                    case 2:
                        StartCoroutine(pomchr.explode(pomchr.me.abilities[abi].cost));
                        break;
                }
        }

        


        GameObject GO = GameObject.Find("Canvas/abilityselection");
        GO.SetActive(false);
    }

    private int chanceoffailure()
    {
        int r = Random.Range(0, 100);
        if (r < 25) return 0; //attack - attack without mana
        else if (r < 50) return 1; //failure - nothing
        else return 2;//critical failure - self harm
        
    }

    public void fleebattle()
    {
        int r = Random.Range(0,3);
        if(r==0)
        {
            for (int i = 0; i < gameinfo.hero.Length; i++)
                gameinfo.hero[i].currenthp = gameinfo.hero[i].currenthp - (gameinfo.hero[i].currenthp / 10); //zadna cifra so procenti.... mogoc adjusti
            SceneManager.LoadScene("Exploration");
        }
        else
        {
            charactercombatstate pomchr = tomanage[0].GetComponent<charactercombatstate>();
            remove();
            pomchr.takedamage(10);
            disablebuttons();
            state = battlestates.waiting;

        }
    }
    public void potion()
    {
        if (gameinfo.hppotion > 0)
        {
            gameinfo.hppotion--;

            charactercombatstate pomchr = tomanage[0].GetComponent<charactercombatstate>();
            pomchr.updatemanabar(-100);
            pomchr.takedamage(-100);
            remove();
            disablebuttons();
            state = battlestates.waiting;
        }
        else {
            //maybe someday
        }
    }

    public void Swaptofromai(int i)
    {
        i++;
        if (gameinfo.hero[i-1].ai == true)
        {
            gameinfo.hero[i-1].ai = false;
            GameObject.Find("Canvas/Panel/playerinfo/hero" + i + "/icon").GetComponent<Image>().color = new Color32(255, 0, 0, 255);
        }
        else
        {
            gameinfo.hero[i-1].ai = true;
            GameObject.Find("Canvas/Panel/playerinfo/hero" + i + "/icon").GetComponent<Image>().color = new Color32(0, 255, 0, 255);
        }
    }

    public void spawncharacter(int j)
    {
        Debug.Log(j + " enemnies");
        exp = exp + j * 10;
        goldtoget += j * 10;
            for (int i = 0; i < (gameinfo.hero.Length + j); i++)
            {
                Debug.Log(i);
                GameObject chr = Resources.Load("character") as GameObject;
                charactercombatstate pom = chr.GetComponent<charactercombatstate>();
                

                if (i < gameinfo.hero.Length)
                {
                    pom.me = gameinfo.hero[i];
                    pom.setupst = gameinfo.hero[i].position;
                Debug.Log("joku bi se:" + pom.setupst);

                    int r = i + 1;
                    pom.progressbar = GameObject.Find("Canvas/Panel/playerinfo/hero" + r + "/progressbar").GetComponent<Image>();
                    pom.hp = GameObject.Find("Canvas/Panel/playerinfo/hero" + r + "/hp").GetComponent<Image>();
                    pom.manabar = GameObject.Find("Canvas/Panel/playerinfo/hero" + r + "/abilitypoints").GetComponent<Image>();
                    chr.tag = "hero";
                }
                else {
                    Debug.Log("adding enemy");
                    int r = i + 1 - gameinfo.hero.Length;
                    pom.me = getenemy();
                    pom.setupst = 4 + i - gameinfo.hero.Length;

                    pom.progressbar = GameObject.Find("Canvas/Panel/enemyinfo/enemy" + r + "/progressbar").GetComponent<Image>();
                    pom.hp = GameObject.Find("Canvas/Panel/enemyinfo/enemy" + r + "/hp").GetComponent<Image>();
                    pom.manabar = GameObject.Find("Canvas/Panel/enemyinfo/enemy" + r + "/abilitypoints").GetComponent<Image>();
                    chr.tag = "enemy";
                }
            Instantiate(chr);
        }
        

    }

    public character getenemy()
    {
        int x = Random.Range(0, gameinfo.currarea.enemy.Count);
        return gameinfo.currarea.enemy[x];
    }
    /*
    public void check()
    {
        if (checkheroes() == true) //if heroes dead
        {
            GameObject pom = GameObject.Find("winlosetext");
            Text text = pom.GetComponent<Text>();
            text.text = "DEFEAT";
            state = battlestates.end;
            winlose = false;
        }
        else if (checkenemies() == true)
        {

            GameObject pom = GameObject.Find("winlosetext");
            Text text = pom.GetComponent<Text>();
            text.text = "VICTORY";
            state = battlestates.end;
            winlose = true;
        }
    }*/
}
