using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class herocombatstate : MonoBehaviour
{
    public static int stevec=0;
    public string type = "hero";
    public basehero hero;
    public GameObject selector;
    public enum battlestates
    {
        start,
        waiting,
        waiting2,
        enablebuttons,
        addtolist,
        disablebuttons,
        chooseaction,
        anime,
        status,
        calcdamage,
        win,
        die,
    }

    
    private float maxcd=5;
    private float curcd=0;

    bool pressedbutton = false;
    public Image progressbar;
    public Image hp;
    public battlestates currstate;
    // Start is called before the first frame update

    private battlestate bts;
    public GameObject[] mybutton;
    //4 animations
    private Vector3 startpos;
    private bool actionstarted = false;
    public static GameObject theattacked;
    private float sped = 100; //how fast they move //35 prej
    public Animator animator;
    public static bool animationdone=true;

    public static List<bool> animations = new List<bool>();
    void Start()
    {
        //selector.SetActive(false);
        //naredi ozadje

        //naredi nasprotnika


        //hero = gameinfo.hero[stevec];
        stevec++;
        //turn order

        //test
        /*gameinfo.basehp = 20;
        gameinfo.currenthp = 20;
        hero.basehp = gameinfo.basehp;
        hero.currenthp = gameinfo.currenthp;*/


        
        currstate = battlestates.waiting;


        mybutton = GameObject.FindGameObjectsWithTag("buttons");
        bts = GameObject.Find("manager").GetComponent<battlestate>();
        
        startpos = transform.position;

        hp.transform.localScale = new Vector3(Mathf.Clamp(hero.currenthp / hero.basehp, 0, 1), hp.transform.localScale.y, hp.transform.localScale.z);

    }

    // Update is called once per frame
    void Update()
    {
       
            //updatebar();

        switch (currstate)
        {
            case (battlestates.start):
                


                break;
            case (battlestates.waiting):
                updateprogressbar();
                break;
            case (battlestates.addtolist):
                bts.heroestomanage.Add(this.gameObject);
                currstate = battlestates.waiting;
                //enablebuttons();
                break;
            case (battlestates.waiting2):

                break;
            case (battlestates.chooseaction):
                curcd = 0;
                //currstate = battlestates.waiting;
                break;
            case (battlestates.anime):
                StartCoroutine(actiontimem());
                currstate = battlestates.waiting;
                break;
            case (battlestates.win):
                StartCoroutine(win());
                //add exp
                break;
            case (battlestates.die):
                StartCoroutine(die());
                break;
        }
    }

    public IEnumerator win()
    {
        animator.SetBool("victory", true);
        yield return new WaitForSeconds(1);
    }


    public IEnumerator die()
    {
        animator.SetBool("dead", true);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length + animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        this.gameObject.SetActive(false);
    }

    public IEnumerator reaction()
    {
        animator.SetBool("react", true);
        yield return new WaitForSeconds(1/2);
        animator.SetBool("react",false);
    }

    private IEnumerator actiontimer()
    {
        if (bts.list[0].ability.dmgtype.ToString() == "magic")
        {
            animator.SetBool("magic", true);
            yield return new WaitForSeconds(1);
        }
        else
        {

        }

    }

        private IEnumerator actiontimem()
    {
        updateprogressbar();
        //test
        Debug.Log("added hero");
        bool a = true;
        animations.Add(a);
        //test
        animationdone = false;
        enemycombatstate.animationdone = false;
        if (actionstarted)
        {
            yield break;
        }
        actionstarted = true;
        Debug.Log(theattacked);
        //move
        Vector3 heroposition = new Vector3(theattacked.transform.position.x - 15, theattacked.transform.position.y, theattacked.transform.position.z);
        animator.SetFloat("sped", sped);
        while (move(heroposition))
        {
            yield return null;
        }
        animator.SetFloat("sped", 1 / 100); //stop moving
        //move
        //attack 
        for (int i = 0; i < bts.list[0].ability.amount; i++)
        {
            if (i % 2 == 0)
            {
                animator.SetBool("attack", true);
            }
            else
            {
                animator.SetBool("attack1", true);
            }
            enemycombatstate est = theattacked.GetComponent<enemycombatstate>();
            est.updatebar(); //update hp bar
            yield return new WaitForSeconds(1);
        }
        //attack

        transform.Rotate(Vector3.up, 180);
        animator.SetFloat("sped", sped);
        while (move(startpos))
        {
            yield return null;
        }
        animator.SetFloat("sped", 1 / 100);
        transform.Rotate(Vector3.up, 180);
        bts.list.RemoveAt(0);
        //bts.battlestates = battlestate.action.wait;
        currstate = battlestates.waiting;
        animator.SetBool("attack", false);
        actionstarted = false;
        animationdone = true;
        enemycombatstate.animationdone = true;

        //test
        animations.RemoveAt(0);
    }



    /*
    public void attack()
    {
        pressedbutton = true;
        int damage = hero.STR + 10;


    }*/

    
    public void updateprogressbar() {
        if (bts.heroestomanage.Count <= 0 && bts.list.Count <= 0 && animationdone == true && enemycombatstate.animationdone==true)
            curcd = curcd + Time.deltaTime*(1+2/10);
        float calccd = curcd / maxcd;
        progressbar.transform.localScale = new Vector3(Mathf.Clamp(calccd, 0, 1), progressbar.transform.localScale.y, progressbar.transform.localScale.z);

        if (curcd >= maxcd) {
            currstate = battlestates.addtolist;
            curcd = 0;
        }
    }


    public void chooseaction()
    {
        turnhandler attack = new turnhandler();
        attack.atacker = this.gameObject;
        attack.attackername = hero.name;
        //attack.type = "hero";
        attack.target =theattacked;
        bts.getaction(attack);
    }
    /*


    public GameObject gettheattacked() {
        return theattacked;
    }*/


    public void updatebar()
    {

        float calchp = hero.currenthp / hero.basehp;
        hp.transform.localScale = new Vector3(Mathf.Clamp(calchp, 0, 1), hp.transform.localScale.y, hp.transform.localScale.z);
        StartCoroutine(reaction());

        if (hero.currenthp <= 0)
        {
            currstate = battlestates.die;
        }

        /*
        float pom = gameinfo.currenthp;
        float calchp = pom / gameinfo.basehp;
        //hp.transform.localScale = new Vector3(Mathf.Clamp(calchp, 0, 1), hp.transform.localScale.y, hp.transform.localScale.z);
        Vector3 temp = hp.transform.localScale;
        if (gameinfo.currenthp <= 0) { temp.x = 0; }
        else temp.x = calchp;

        hp.transform.localScale = temp;

        if (gameinfo.currenthp <= 0)
        {
            currstate = battlestates.lose;
        }*/
    }

    private bool move(Vector3 target)
    {


        return target != (transform.position = Vector3.MoveTowards(transform.position, target, sped * Time.deltaTime));

    }

    public void endattack()
    {
        animator.SetBool("attack", false);
    }

    public void calcdamage(float damage)
    {
        if (Random.Range(0, 50) < hero.AGI)
        {
            damage = 0; // block
        }
        animator.SetFloat("dmg", damage);
        hero.currenthp = hero.currenthp - damage;
        
        
    }

    public void enablebuttons()
    {
        GameObject[] actionbuttons = GameObject.FindGameObjectsWithTag("buttons");
        foreach (GameObject button in actionbuttons)
        {
            button.GetComponent<UnityEngine.UI.Button>().interactable = true;
        }
    }

 

    public void changepossition()  //middle= x=0 y=18 z=5 //x differance= 20
    {
        switch (hero.position)
        {
            case 1://prvi
                transform.position = new Vector3(-20, 16, 5);
                break;
            case 2:
                transform.position = new Vector3(-40, 16, 5);
                break;
            case 3:
                transform.position = new Vector3(-60, 16, 5);
                break;
        }
    }


    public void disableobject() {
        this.gameObject.SetActive(false);
    }


}





/*  foreach (GameObject button in mybutton)
                {
                    if(button.GetComponent<Button>().interactable == false)
                    button.GetComponent<Button>().interactable = true;
                }*/
