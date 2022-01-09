using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading.Tasks;

public class victoryscript : MonoBehaviour
{
    bool[] leveled = new bool[] { false, false, false };
    // Start is called before the first frame update
    void Start()
    {

        //PRE EXPBAR
        
        for (int i = 0; i < gameinfo.hero.Length; i++)
        {
            
            if (gameinfo.hero[i].currenthp <= 0) GameObject.Find("hero" + i).SetActive(false);
            else
            {
                Text myText = GameObject.Find("Canvas/heropanel/hero" + i + "/herostats/INT/stat").GetComponent<Text>();
                myText.text = myText.text + gameinfo.hero[i].INT.ToString();
                myText = GameObject.Find("Canvas/heropanel/hero" + i + "/herostats/STR/stat").GetComponent<Text>();
                myText.text = myText.text + gameinfo.hero[i].STR.ToString();
                myText = GameObject.Find("Canvas/heropanel/hero" + i + "/herostats/FOC/stat").GetComponent<Text>();
                myText.text = myText.text + gameinfo.hero[i].FOC.ToString();
                myText = GameObject.Find("Canvas/heropanel/hero" + i + "/herostats/PER/stat").GetComponent<Text>();
                myText.text = myText.text + gameinfo.hero[i].PER.ToString();
                myText = GameObject.Find("Canvas/heropanel/hero" + i + "/herostats/SPD/stat").GetComponent<Text>();
                myText.text = myText.text + gameinfo.hero[i].SPD.ToString();
                myText = GameObject.Find("Canvas/heropanel/hero" + i + "/herostats/AGI/stat").GetComponent<Text>();
                myText.text = myText.text + gameinfo.hero[i].AGI.ToString();
                myText = GameObject.Find("Canvas/heropanel/hero" + i + "/herostats/LCK/stat").GetComponent<Text>();
                myText.text = myText.text + gameinfo.hero[i].LCK.ToString();
                myText = GameObject.Find("Canvas/heropanel/hero" + i + "/herostats/level/stat").GetComponent<Text>();
                myText.text = myText.text + gameinfo.hero[i].level.ToString();
                myText = GameObject.Find("Canvas/heropanel/hero" + i + "/herostats/health/stat").GetComponent<Text>();
                myText.text = myText.text + gameinfo.hero[i].currenthp.ToString() + " / " + gameinfo.hero[i].basehp.ToString();
                myText = GameObject.Find("Canvas/heropanel/hero" + i + "/herostats/mana/stat").GetComponent<Text>();
                myText.text = myText.text + gameinfo.hero[i].currentmp.ToString() + " / " + gameinfo.hero[i].basemp.ToString();


                myText = GameObject.Find("Canvas/heropanel/hero" + i + "/heroname/heroname").GetComponent<Text>();
                myText.text = myText.text + gameinfo.hero[i].name + " the " + gameinfo.hero[i].specialization;


            }
            Debug.Log(gameinfo.hero[i].exp);
            Slider slider = GameObject.Find("Canvas/heropanel/hero" + i + "/heroname/expbar").GetComponent<Slider>();
            slider.maxValue = gameinfo.hero[i].level * 100;
            slider.value = gameinfo.hero[i].exp;
        }

        //CALC EXP
        expbar();

        //POST EXPBAR
        
    }
    
    


    public async void expbar()
    {
        float curexp = 0;
        
        while (gameinfo.hero[0].exptoadd > 0 || gameinfo.hero[1].exptoadd > 0 || gameinfo.hero[2].exptoadd > 0)
        {
            for (int i = 0; i < gameinfo.hero.Length; i++)
            {
                Slider expbar = GameObject.Find("Canvas/heropanel/hero" + i + "/heroname/expbar").GetComponent<Slider>();
                if (expbar.value >= expbar.maxValue)
                {
                    expbar.value = 0;
                    gameinfo.hero[i].level++;
                    lvlup(i);
                    //Debug.Log("heroj se je lvlou");
                }
                //curexp = curexp + Time.deltaTime * (1 + 2 / 10);
                gameinfo.hero[i].exptoadd = gameinfo.hero[i].exptoadd - 1;
                gameinfo.hero[i].exp = gameinfo.hero[i].exp + 1; // really gotta mekt hsi better
                expbar.value = expbar.value + 1;
                //Debug.Log(gameinfo.hero[i].exptoadd);
                await Task.Yield();
            }
        }

    }

    public void lvlup(int i)
    {
        int FOC = 1;
        int STR = 1;
        int INT = 1;
        int PER = 1;
        int LCK = 1;
        float SPD = 0;
        int AGI = 1;
        int level = 1;
        int health = 5;
        int mana = 5;
        
        switch (gameinfo.hero[i].specialization)
        {
            case ("warrior"):
                STR = 3;
                LCK = 2;
                INT = 2;
                PER = 2;
                health = 20;
                SPD = 0.05f;
                break;
            case ("priest"):
                mana = 15;
                health = 10;
                AGI = 2;
                FOC = 3;
                INT = 2;
                SPD = 0.1f;
                break;
            case ("mage"):
                INT = 3;
                health = 5;
                mana = 20;
                SPD = 0.2f;
                PER = 2;
                break;
            case ("ranger"):
                health = 5;
                mana = 10;
                LCK = 3;
                PER = 3;
                SPD = 0.3f;
                AGI = 3;
                break;
        }
        
        gameinfo.hero[i].PER = gameinfo.hero[i].PER + PER;
        gameinfo.hero[i].FOC = gameinfo.hero[i].FOC + FOC;
        gameinfo.hero[i].STR = gameinfo.hero[i].STR + STR;
        gameinfo.hero[i].INT = gameinfo.hero[i].INT + INT;
        gameinfo.hero[i].LCK = gameinfo.hero[i].LCK + LCK;
        gameinfo.hero[i].SPD = gameinfo.hero[i].SPD + SPD;
        gameinfo.hero[i].AGI = gameinfo.hero[i].AGI + AGI;
        gameinfo.hero[i].basehp = gameinfo.hero[i].basehp + health;
        gameinfo.hero[i].currenthp = gameinfo.hero[i].currenthp + health;
        gameinfo.hero[i].basemp = gameinfo.hero[i].basemp + health;
        gameinfo.hero[i].currentmp = gameinfo.hero[i].currentmp + health;
        gameinfo.hero[i].level = gameinfo.hero[i].level + level;



        Text myText = GameObject.Find("Canvas/heropanel/hero" + i + "/herostats/INT/add").GetComponent<Text>();
        myText.text = " + " + INT.ToString();
        myText = GameObject.Find("Canvas/heropanel/hero" + i + "/herostats/STR/add").GetComponent<Text>();
        myText.text = " + " + STR.ToString();
        myText = GameObject.Find("Canvas/heropanel/hero" + i + "/herostats/FOC/add").GetComponent<Text>();
        myText.text = " + " + FOC.ToString();
        myText = GameObject.Find("Canvas/heropanel/hero" + i + "/herostats/PER/add").GetComponent<Text>();
        myText.text = " + " + PER.ToString();
        myText = GameObject.Find("Canvas/heropanel/hero" + i + "/herostats/SPD/add").GetComponent<Text>();
        myText.text = " + " + SPD.ToString();
        myText = GameObject.Find("Canvas/heropanel/hero" + i + "/herostats/AGI/add").GetComponent<Text>();
        myText.text = " + " + AGI.ToString();
        myText = GameObject.Find("Canvas/heropanel/hero" + i + "/herostats/LCK/add").GetComponent<Text>();
        myText.text = " + " + LCK.ToString();
        myText = GameObject.Find("Canvas/heropanel/hero" + i + "/herostats/level/add").GetComponent<Text>();
        myText.text = " + " + level.ToString();
        myText = GameObject.Find("Canvas/heropanel/hero" + i + "/herostats/health/add").GetComponent<Text>();
        myText.text = " + " + health.ToString();
        myText = GameObject.Find("Canvas/heropanel/hero" + i + "/herostats/mana/add").GetComponent<Text>();
        myText.text = " + " + mana.ToString();
    }



    public void continuebutton()
    {
        SceneManager.LoadScene("Exploration");
    }
}
