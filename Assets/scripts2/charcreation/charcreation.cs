using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class charcreation : MonoBehaviour
{

    public int heroi;
    public string type;
    public List<ability> basics; //strike, shoot, fireball, heal
    public InputField neki;
    public area startingarea;

    public List<character> heroes = new List<character>();
    // Start is called before the first frame update
    void Start()
    {
        //set gameinfo
        gameinfo.floor = 0;
        gameinfo.gold = 100;
        gameinfo.food = 10;
        gameinfo.hppotion = 1;

        heroi = 1;
        type = "warrior";
        for (int i = 0; i < 3; i++)
        {
            heroes.Add(warrior(randomname()));
        }


        GameObject canvas = GameObject.Find("stat panel");
        Transform child = canvas.transform.Find("InputField");
        neki = child.GetComponent<InputField>();

        gameinfo.currarea = startingarea;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject canvas = GameObject.Find("stat panel");
        Transform child = canvas.transform.Find("health");
        Text t = child.GetComponent<Text>();
        t.text = heroes[heroi - 1].basehp.ToString();

        child = canvas.transform.Find("mana");
        t = child.GetComponent<Text>();
        t.text = heroes[heroi - 1].basemp.ToString();


        child = canvas.transform.Find("InputField");
        InputField neki = child.GetComponent<InputField>();
        if (neki.isFocused == false)
            neki.text = heroes[heroi - 1].name;

        /*
        child = canvas.transform.Find("InputField
        child = child.Find("Text");
        t = child.GetComponent<Text>();
        t.text = heroes[heroi - 1].name;
        */

        child = canvas.transform.Find("class");
        t = child.GetComponent<Text>();
        t.text = heroes[heroi - 1].specialization;
    }

    public void charselection()
    {
        string name = EventSystem.current.currentSelectedGameObject.name;
        heroi = int.Parse(name);
        //input.clear();
        Debug.Log(heroi);

    }

    public void classelection()
    {
        string name = EventSystem.current.currentSelectedGameObject.name;
        type = name;
        switch (type)
        {
            case "warrior":
                heroes[heroi - 1] = warrior(heroes[heroi - 1].name);
                break;
            case "mage":
                heroes[heroi - 1] = mage(heroes[heroi - 1].name);
                break;
            case "priest":
                heroes[heroi - 1] = priest(heroes[heroi - 1].name);
                break;
            case "ranger":
                heroes[heroi - 1] = ranger(heroes[heroi - 1].name);
                break;
        }
        Debug.Log(type);
    }

    public character warrior(string name)
    {
        character hero = new character();

        hero.name = name;
        hero.basehp = 125;
        hero.currenthp = hero.basehp;
        hero.basemp = 50;
        hero.currentmp = hero.basemp;
        hero.specialization = "warrior";
        hero.race = "warrior";
        hero.exp = 0;

        hero.FOC = 1; //healing
        hero.PER = 5; //chance to hit
        hero.STR = 6; //attack dmg
        hero.INT = 0; //spell dmg
        hero.AGI = 3; //dodge chance
        hero.LCK = 2; //crit chance
        hero.SPD = 3; //speed
        hero.abilities.Add(basics[0]); //20 
        hero.level = 1;
        hero.exptoadd = 0;


        hero.ai = false;
        //heroes[heroi - 1] = hero;
        return hero;
    }

    public character mage(string name)
    {
        character hero = new character();

        hero.name = name;
        hero.basehp = 80;
        hero.currenthp = hero.basehp;
        hero.basemp = 25; //120
        hero.currentmp = hero.basemp;
        hero.specialization = "mage";
        hero.race = "warrior";
        hero.exp = 0;

        hero.FOC = 2; //healing
        hero.PER = 4; //chance to hit 
        hero.STR = 2; //attack dmg
        hero.INT = 6; //spell dmg
        hero.AGI = 1; //dodge chance
        hero.LCK = 1; //crit chance
        hero.SPD = 4; //speed
        hero.abilities.Add(basics[0]);
        hero.abilities.Add(basics[2]); //20
        hero.level = 1;
        hero.exptoadd = 0;


        hero.ai = false;
        //heroes[heroi - 1] = hero;
        return hero;
    }

    public character priest(string name)
    {
        character hero = new character();

        hero.name = name;
        hero.basehp = 90;
        hero.currenthp = hero.basehp;
        hero.basemp = 100;
        hero.currentmp = hero.basemp;
        hero.specialization = "priest";
        hero.race = "warrior";
        hero.exp = 0;
        hero.exptoadd = 0;

        hero.FOC = 7; //healing
        hero.PER = 2; //chance to hit 
        hero.STR = 1; //attack dmg
        hero.INT = 2; //spell dmg
        hero.AGI = 2; //dodge chance
        hero.LCK = 1; //crit chance
        hero.SPD = 4; //speed
        hero.abilities.Add(basics[0]);
        hero.abilities.Add(basics[3]);
        GameObject pom = GameObject.Find("hero" + hero);
        //pom.GetComponent<SpriteRenderer>().sprite = 
        hero.level = 1;


        hero.ai = false;
        //heroes[heroi - 1] = hero;
        return hero;
    }

    public character ranger(string name)
    {
        character hero = new character();

        hero.name = name;
        hero.basehp = 100;
        hero.currenthp = hero.basehp;
        hero.basemp = 75;
        hero.currentmp = hero.basemp;
        hero.specialization = "ranger";
        hero.race = "warrior";
        hero.exp = 0;
        hero.exptoadd = 0;

        hero.FOC = 2; //healing
        hero.PER = 6; //chance to hit
        hero.STR = 3; //attack dmg
        hero.INT = 1; //spell dmg
        hero.AGI = 3; //dodge chance
        hero.LCK = 2; //crit chance
        hero.SPD = 5; //speed
        hero.abilities.Add(basics[2]); //21
        hero.level = 1;


        hero.ai = false;
        //heroes[heroi - 1] = hero;
        return hero;
    }



    public void rename()
    {
        character hero = new character();

        GameObject canvas = GameObject.Find("stat panel");
        Transform child = canvas.transform.Find("InputField");
        child = child.Find("Text");
        Text text = child.GetComponent<Text>();
        string test = text.text;
        Debug.Log(test);
        if (test.Length > 0)
            switch (type)
            {
                case "warrior":
                    heroes[heroi - 1] = warrior(test);
                    break;
                case "mage":
                    heroes[heroi - 1] = mage(test);
                    break;
                case "priest":
                    heroes[heroi - 1] = priest(test);
                    break;
                case "ranger":
                    heroes[heroi - 1] = ranger(test);
                    break;
            }
    }

    public string randomname()
    {
        string[] names = new string[] { "Greg", "Guts", "Ajax", "Tom", "Bob", "tim", "murmur", "teiruto" };
        return names[Random.Range(0, names.Length)];
    }

    public void play()
    {
        for (int i = 0; i < 3; i++)
        {
            gameinfo.hero[i] = heroes[i];
            gameinfo.hero[i].position = i + 1;
        }

        /*
        Debug.Log(gameinfo.hero[0].specialization);
        Debug.Log(gameinfo.hero[0].position);
        Debug.Log(gameinfo.hero[1].specialization);
        Debug.Log(gameinfo.hero[1].position);          //position test
        Debug.Log(gameinfo.hero[2].specialization);
        Debug.Log(gameinfo.hero[2].position);*/

        SceneManager.LoadScene("Exploration");
    }

    public void back()
    {
        SceneManager.LoadScene("Menu");
    }
}
