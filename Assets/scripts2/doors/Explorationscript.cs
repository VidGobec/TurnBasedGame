using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Explorationscript : MonoBehaviour
{

    private enum encounter
    {
        battle,
        randomevent,
        shop,
        trainer,
        altar
    }

    public GameObject eventpanel;
    public GameObject shoppanel;

    [SerializeField] private List<area> areahodler = new List<area>();
    private encounter[] enc = new encounter [3];

    private Event currentevent;

    public List<ability> heroability = new List<ability>();
    private int option1 = 0;
    private int option2 = 0;
    private int selectedoption;
    public GameObject trainerpanel;
    public GameObject trainerpanel2;
    public GameObject[] trainerbuttons = new GameObject[3];

    // Start is called before the first frame update
    void Start()
    {
        if(gameinfo.floor%20==0)
        checkarea();

        if (gameinfo.food == 0)
            for (int i = 0; i < gameinfo.hero.Length; i++) gameinfo.hero[i].currenthp = gameinfo.hero[i].currenthp - (gameinfo.hero[i].currenthp / 5);
        else gameinfo.food--;


        gameinfo.floor++;
        Syvesystem.saveplayer();

        for (int j = 0; j < gameinfo.hero.Length; j++)
            reducebuffs(gameinfo.hero[j]);

        for (int i = 0; i < 3; i++)
        {
            int pom = Random.Range(0, 100);
            if (pom <= 45) enc[i] = encounter.battle;
            else if(pom <=80) enc[i] = encounter.randomevent;
            else if (pom <= 90) enc[i] = encounter.shop; 
            else  enc[i] = encounter.trainer;

            float scout=0;
            for(int j=0;j<gameinfo.hero.Length;j++)
                scout +=  gameinfo.hero[j].PER;
            scout = scout - Random.Range(0, 100);
            if (scout > 0) revealenc(i);
        }

        displaystuff();

    }

    // Update is called once per frame
    private void checkarea()
    {
        switch (gameinfo.floor)
        {
            case 20:
                gameinfo.currarea = areahodler[1];
                break;
            case 40:
                gameinfo.currarea = areahodler[2];
                break;
            case 60:
                gameinfo.currarea = areahodler[3];
                break;
            case 80:
                gameinfo.currarea = areahodler[4];
                break;
        }

    }

    public void pickpath(int path)
    {
        switch (enc[path])
        {
            case encounter.battle:
                SceneManager.LoadScene("combat2.0");
                break;
            case encounter.randomevent:
                randomevent(GetEvent());
                break;
            case encounter.shop:
                shop();
                break;
            case encounter.trainer:
                trainer();
                break;
            case encounter.altar:
                altar();
                break;
        }
    }
    
    private Event GetEvent()
    {
        Debug.Log(Random.Range(0, gameinfo.currarea.events.Count));
        return gameinfo.currarea.events[Random.Range(0, gameinfo.currarea.events.Count)];
    }

    private void randomevent(Event thisevent)
    {
        eventpanel.SetActive(true);
        //GameObject activate = GameObject.Find("Canvas/randomevent").GetComponent<GameObject>();
        //Debug.Log(activate);
        //activate.SetActive(true);
        
        Text myText = GameObject.Find("Canvas/randomevent/title").GetComponent<Text>();
        myText.text = thisevent.title;
        myText = GameObject.Find("Canvas/randomevent/description").GetComponent<Text>();
        myText.text = thisevent.description;
        myText = GameObject.Find("Canvas/randomevent/button1/Text").GetComponent<Text>();
        myText.text = thisevent.options1;
        myText = GameObject.Find("Canvas/randomevent/button2/Text").GetComponent<Text>();
        myText.text = thisevent.options2;
        myText = GameObject.Find("Canvas/randomevent/button3/Text").GetComponent<Text>();
        myText.text = thisevent.options3;

        currentevent = thisevent;
    }

    private void shop()
    {
        shoppanel.SetActive(true);
        //GameObject.Find("Canvas/Continue").SetActive(true);
    }

    private void trainer()
    {
        trainerpanel.SetActive(true);
        option1 = Random.Range(0, heroability.Count);
        do {
            option2 = Random.Range(0, heroability.Count);
        } while (option1==option2);

        GameObject.Find("Canvas/Trainerpanel/Button1/Text").GetComponent<Text>().text = heroability[option1].name;
        GameObject.Find("Canvas/Trainerpanel/Button2/Text").GetComponent<Text>().text = heroability[option2].name;
    }

    private void altar()
    {
        SceneManager.LoadScene("combat2.0");
    }

    public void choice(int i)
    {
        Button deac = GameObject.Find("Canvas/randomevent/button1").GetComponent<Button>();
        deac.gameObject.SetActive(false);
        deac = GameObject.Find("Canvas/randomevent/button2").GetComponent<Button>();
        deac.gameObject.SetActive(false);
        deac = GameObject.Find("Canvas/randomevent/button3").GetComponent<Button>();
        deac.gameObject.SetActive(false);
        Text myText = GameObject.Find("Canvas/randomevent/description").GetComponent<Text>();
        myText.text = "";
        GameObject.Find("Canvas/randomevent/Continue").SetActive(true);

        gameinfo.food += currentevent.buff[i].food;
        if (gameinfo.food < 0) gameinfo.food = 0;
        gameinfo.gold += currentevent.buff[i].money;
        if (gameinfo.gold < 0) gameinfo.gold = 0;

        if(currentevent.buff[i].afterevent.Length>=3)
        displaybuff(currentevent.buff[i].afterevent);

        displaychanges(currentevent.buff[i]);

        if (currentevent.buff[i].all == true)
            for (int j = 0; j < 3; j++)
            {
                gameinfo.hero[j].currenthp += currentevent.buff[i].health;
                if (gameinfo.hero[j].currenthp > gameinfo.hero[j].basehp) gameinfo.hero[j].currenthp = gameinfo.hero[j].basehp;
                else if (gameinfo.hero[j].currenthp < 1) gameinfo.hero[j].currenthp = 1;
                gameinfo.hero[j].currentmp += currentevent.buff[i].mana;
                if (gameinfo.hero[j].currentmp > gameinfo.hero[j].basemp) gameinfo.hero[j].currentmp = gameinfo.hero[j].basemp;
                else if (gameinfo.hero[j].currentmp < 0) gameinfo.hero[j].currentmp = 0;
                //
                characterbuff buf = new characterbuff(currentevent.buff[i]);
                gameinfo.hero[j].buffs.Add(buf);
                addbuff(gameinfo.hero[j], buf);
                displaychanges(gameinfo.hero[j], currentevent.buff[i]);
            }
        else
        {
            int j = Random.Range(0, 3);
            characterbuff buf = new characterbuff(currentevent.buff[i]);
            gameinfo.hero[j].buffs.Add(buf);
            addbuff(gameinfo.hero[j], buf);
            displaychanges(gameinfo.hero[j], currentevent.buff[i]);
        }


       

    }

    private void displaybuff(string x)
    {
        Text myText = GameObject.Find("Canvas/randomevent/description").GetComponent<Text>();
        myText.text = myText.text + x + "\n";
    }

    private void displaychanges(character hero, Buff buff) {
        Text myText = GameObject.Find("Canvas/randomevent/description").GetComponent<Text>();
        myText.text = myText.text + hero.name + " the " + hero.specialization + ": \n";
        if (buff.health > 0) myText.text = myText.text + "has recovered by " + buff.health + ". \n";
        else if(buff.health < 0) myText.text = myText.text + "has been damaged for " + buff.health + ". \n"; // if dead add debuff

        if (buff.mana > 0) myText.text = myText.text + "has recovered " + buff.mana + " energy. \n";
        else if (buff.mana < 0) myText.text = myText.text + "has tired himself out for " + buff.mana + ". \n";

        if (isbuff(hero.buffs[hero.buffs.Count - 1]) == true && hero.buffs[hero.buffs.Count - 1].lenght > 0) myText.text = myText.text + "has recieved a buff for " + hero.buffs[hero.buffs.Count - 1].lenght + " turns";
        else if (isbuff(hero.buffs[hero.buffs.Count - 1]) == false && hero.buffs[hero.buffs.Count - 1].lenght > 0) myText.text = myText.text + "has recieved a debuff for " + hero.buffs[hero.buffs.Count - 1].lenght + " turns";

 
        myText.text = myText.text + "\n";

    }
    private void displaychanges(Buff buff)
    {
        Text myText = GameObject.Find("Canvas/randomevent/description").GetComponent<Text>();
        myText.text = myText.text + "\n";

        if (buff.money > 0) myText.text = myText.text + "your party has gained " + buff.money + " gold. \n";
        else if (buff.money < 0) myText.text = myText.text + "your party has lost " + buff.money + " gold. \n";

        if (buff.food > 0) myText.text = myText.text + "your party has gained " + buff.food + " food. \n";
        else if (buff.food < 0) myText.text = myText.text + "your party has lost " + buff.food + " food. \n";
    }

        public bool isbuff(characterbuff buff)
    {
        float i = buff.AGI + buff.FOC + buff.INT + buff.LCK + buff.PER + buff.SPD + buff.STR;
        if (i > 0) return true;
        else return false;
    }

    public void continutonextdoor() {
        SceneManager.LoadScene("Exploration");
    }


    public void reducebuffs(character chr)
    {
        for (int i = 0; i < chr.buffs.Count; i++)
        {
            if (chr.buffs[i].lenght <= 0)
            {
                addbuff(chr, chr.buffs[i], -1);
                chr.buffs.RemoveAt(i);
            }
            else
                chr.buffs[i].lenght--;
        }
    }

    public void addbuff(character chr, characterbuff buff, int p=1)
    {

        Debug.Log("idk if this works so: " + p);
            chr.FOC = chr.FOC + (buff.FOC * p);
            chr.STR = chr.STR + (buff.STR * p);
            chr.PER = chr.PER + (buff.PER * p);
            chr.SPD = chr.SPD + (buff.SPD * p);
            chr.INT = chr.INT + (buff.INT * p);
            chr.AGI = chr.AGI + (buff.AGI * p);
            chr.LCK = chr.LCK + (buff.LCK * p);
    }

  

    private void revealenc(int i)
    {
        Debug.Log("revleaenc");
        int j = i + 1;
        Image pom = GameObject.Find("Canvas/Panel/path" + j + "/Panel/Image").GetComponent<Image>();

        switch (enc[i]) {
            case encounter.battle:
                pom.sprite = Resources.Load<Sprite>("icons/combat");
                break;
            case encounter.randomevent:
                pom.sprite = Resources.Load<Sprite>("icons/event");
                break;
            case encounter.shop:
                pom.sprite = Resources.Load<Sprite>("icons/shop");
                break;
            case encounter.altar:
                pom.sprite = Resources.Load<Sprite>("icons/altar");
                break;
            case encounter.trainer:
                pom.sprite = Resources.Load<Sprite>("icons/altar");
                break;
        }
    }

    public void buy(int i)
    {
        switch (i)
        {
            case 0:
                if (gameinfo.gold>=50)
                {
                    gameinfo.food++;
                    gameinfo.gold = gameinfo.gold - 50;
                }
                break;
            case 1:
                if (gameinfo.gold >= 100)
                {
                    gameinfo.hppotion++;
                    gameinfo.gold = gameinfo.gold - 100;
                }
                break;
            case 2:
                if (gameinfo.gold >= 50)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        gameinfo.hero[j].currenthp += 20;
                        if (gameinfo.hero[j].currenthp > gameinfo.hero[j].basehp) gameinfo.hero[j].currenthp = gameinfo.hero[j].basehp;

                        gameinfo.hero[j].currentmp += 20;
                        if (gameinfo.hero[j].currentmp > gameinfo.hero[j].basemp) gameinfo.hero[j].currentmp = gameinfo.hero[j].basemp;

                    }
                    gameinfo.gold = gameinfo.gold - 50;
                }
                break;
        }
        displaystuff();
    }

    public void displaystuff()
    {
        Text text = GameObject.Find("Canvas/stuffpanel/money/Text").GetComponent<Text>();
        text.text = "" + gameinfo.gold as string;
        text = GameObject.Find("Canvas/stuffpanel/food/Text").GetComponent<Text>();
        text.text = "" + gameinfo.food as string;
    }

    public void trainerability(int i)
    {
        if (gameinfo.gold>=50 && (gameinfo.hero[0].abilities.Count<8 || gameinfo.hero[1].abilities.Count < 8 || gameinfo.hero[2].abilities.Count < 8))
        {
            switch (i)
            {
                case 1:
                    selectedoption = option1;
                    break;
                case 2:
                    selectedoption = option2;
                    break;
            }

            changebuttons();
        }
    }

    public void changebuttons()
    {
        for (int i = 1, j = 4; i <= 3; i++, j++)
        {
            GameObject.Find("Canvas/Trainerpanel/Button" + i).SetActive(false);
            trainerbuttons[i-1].SetActive(true);
            GameObject.Find("Canvas/Trainerpanel/Button" + j + "/Text").GetComponent<Text>().text = gameinfo.hero[i-1].name + " the " + gameinfo.hero[i-1].specialization;
        }
            
    }

    public void skip()
    {
        int i = Random.Range(0, 3);
        int j = Random.Range(0, 9);
        switch(j)
        {
            case 0:
                gameinfo.hero[i].basehp += 5;
                gameinfo.hero[i].currenthp += 5;
                displaytrainingchanges(gameinfo.hero[i].name + " the " + gameinfo.hero[i].specialization + " has increased his health");
                break;
            case 1:
                gameinfo.hero[i].basemp += 5;
                gameinfo.hero[i].currentmp += 5;
                displaytrainingchanges(gameinfo.hero[i].name + " the " + gameinfo.hero[i].specialization + " has increased his mana");
                break;
            case 2:
                gameinfo.hero[i].FOC += 1;
                displaytrainingchanges(gameinfo.hero[i].name + " the " + gameinfo.hero[i].specialization + " has increased his focus");
                break;
            case 3:
                gameinfo.hero[i].AGI += 1;
                displaytrainingchanges(gameinfo.hero[i].name + " the " + gameinfo.hero[i].specialization + " has increased his agiliy");
                break;
            case 4:
                gameinfo.hero[i].STR += 1;
                displaytrainingchanges(gameinfo.hero[i].name + " the " + gameinfo.hero[i].specialization + " has increased his streanght");
                break;
            case 5:
                gameinfo.hero[i].INT += 1;
                displaytrainingchanges(gameinfo.hero[i].name + " the " + gameinfo.hero[i].specialization + " has increased his inteligence");
                break;
            case 6:
                gameinfo.hero[i].SPD += 1;
                displaytrainingchanges(gameinfo.hero[i].name + " the " + gameinfo.hero[i].specialization + " has increased his speed");
                break;
            case 7:
                gameinfo.hero[i].LCK += 1;
                displaytrainingchanges(gameinfo.hero[i].name + " the " + gameinfo.hero[i].specialization + " has increased his luck");
                break;
            case 8:
                gameinfo.hero[i].PER += 1;
                displaytrainingchanges(gameinfo.hero[i].name + " the " + gameinfo.hero[i].specialization + " has increased his perception");
                break;
        }
        
    }

    public void choosehero(int i)
    {
        if (gameinfo.hero[i].abilities.Contains(heroability[selectedoption]))
        {
            gameinfo.gold -= 50;
            for (int j = 0; j < gameinfo.hero[i].abilities.Count; j++)
                if (gameinfo.hero[i].abilities[j] == heroability[selectedoption])
                    gameinfo.hero[i].abilities[j].mod += 0.1f;
            displaytrainingchanges(gameinfo.hero[i].name + " the " + gameinfo.hero[i].specialization + " has learned new things about " + heroability[selectedoption].name);
        }
        else if(gameinfo.hero[i].abilities.Count<8)
        {
            gameinfo.gold -= 50;
            gameinfo.hero[i].abilities.Add(heroability[selectedoption]);
            displaytrainingchanges(gameinfo.hero[i].name + " the " + gameinfo.hero[i].specialization + " has succesfully learned " + heroability[selectedoption].name);
        }
    }

    public void displaytrainingchanges(string pom)
    {
        trainerpanel.SetActive(false);
        trainerpanel2.SetActive(true);
        GameObject.Find("Canvas/Trainerpanel2/Text").GetComponent<Text>().text = pom;
    }



    public void back()
    {
        SceneManager.LoadScene("Menu");
    }

    public void surrender()
    {
        SceneManager.LoadScene("Losing");
    }

}