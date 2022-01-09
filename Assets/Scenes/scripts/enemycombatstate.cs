using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemycombatstate : MonoBehaviour
{

    public GameObject firepoint;
    private battlestate bts;
    public baseenemy enemy;
    public enum battlestates
    {
        start,
        waiting,
        waiting2,
        chooseaction,
        anime,
        status,
        calcdamage,
        win,
        die
    }

    
    private static int position=1;
    private float maxcd = 5;
    private float curcd = 0;
    

    //4 animations
    private Vector3 startpos;
    private bool actionstarted = false;
    public GameObject theattacked;
    private float sped=35;
    
    public Animator animator;
    //

    public Image progressbar;
    public Image hp;
    public battlestates currstate;
    public static bool animationdone = true;

    //test
    public string type = "enemy";

    public static List<bool> animations = new List<bool>();

    public static int numproj = 0;

    // Start is called before the first frame update
    void Start()
    {
        setup();
        currstate = battlestates.waiting;
        bts = GameObject.Find("manager").GetComponent<battlestate>();
        startpos = transform.position;
        hp.transform.localScale = new Vector3(Mathf.Clamp(enemy.currenthp / enemy.basehp, 0, 1), hp.transform.localScale.y, hp.transform.localScale.z);
    }

    // Update is called once per frame
    void Update()
    {

       
           // updatebar();
        switch (currstate)
        {
            case (battlestates.start):



                break;
            case (battlestates.waiting):
                updateprogressbar();
                break;
            case (battlestates.waiting2):
                break;
            case (battlestates.chooseaction):
                chooseaction();
                updateprogressbar();
                currstate = battlestates.waiting;
                break;
            case (battlestates.anime):
                //Debug.Log(bts.list[0].ability.closefar.ToString());
                if(bts.list[0].ability.closefar.ToString() == "ranged")
                    StartCoroutine(actiontimer());
                else
                    StartCoroutine(actiontimem());
                currstate = battlestates.waiting;
                Debug.Log("it works works");
                battlestate.pomi++;
                break;
            case (battlestates.win):
                animator.SetBool("victory", true);
                break;
            case (battlestates.die):
                StartCoroutine(die());
                //this.gameObject.SetActive(false);
                break;
        }
    }

    public IEnumerator die()
    {
        animator.SetBool("dead", true);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length + animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        this.gameObject.SetActive(false);
    }

    public void updateprogressbar()
    {
        if (bts.heroestomanage.Count<=0 && bts.list.Count<= 0 && animationdone == true && herocombatstate.animationdone == true)
            curcd = curcd + Time.deltaTime*(0); //1+enemy.SPD/10
        float calccd = curcd / maxcd;
        progressbar.transform.localScale = new Vector3(Mathf.Clamp(calccd, 0, 1), progressbar.transform.localScale.y, progressbar.transform.localScale.z);

        if (curcd >= maxcd)
        {
            curcd = 0;
            bts.heroestomanage.Add(this.gameObject);
        }
    }
    /*
    public void chooseaction() {
        turnhandler attack = new turnhandler();
        attack.atacker = this.gameObject;
        attack.attackername = enemy.name;
        //attack.type = "enemy";
        attack.damage = 7;
        attack.target = bts.heroes[Random.Range(0, bts.heroes.Count)];
        bts.getaction(attack);
    }*/



    private IEnumerator actiontimer()
    {
        //dekleracije
        //numproj = 0;
        //currstate = battlestates.waiting2;

        
        //for (int i = 0; i < bts.list[0].ability.amount; i++)
        //{
            // casting animation and make projectile
            if (bts.list[0].ability.dmgtype.ToString() == "magical")
            {
                animator.SetBool("magic", true);
                Debug.Log("spell is magik");

            }
            else
            {
                Debug.Log("spell is arrow");
            }
            //yield return new WaitForSeconds(1);
            //create proj
            
            GameObject projectile = Instantiate(bts.list[0].ability.projectileprefab, firepoint.transform.position, firepoint.transform.rotation);

            //ability types
            /*
            if (bts.list[0].ability.type.ToString() == "random")
            {
                bts.list[0].target = bts.getrandomtarget("hero");
            }
            else if (bts.list[0].ability.type.ToString() == "chaos")
            {
                bts.list[0].target = bts.getrandomtarget("chaos");
            }
            */
            //move to target
            Vector3 heroposition = new Vector3(theattacked.transform.position.x /*+ 15*/, theattacked.transform.position.y, theattacked.transform.position.z); //might gotta change the number slightlly
            while (move(heroposition, projectile, sped*2))
            {
                yield return null;
            }

            //do dmg
            herocombatstate hst = theattacked.GetComponent<herocombatstate>();
            hst.updatebar(); //update hp bar
            yield return new WaitForSeconds(1/3); //how fast the projectile disappears

            Destroy(projectile);
        //}

        /*
        do
        {
            
        } while (numproj< bts.list[0].ability.amount);*/
        //animator.SetBool("magic", false);
        //bts.list.RemoveAt(0);
        //currstate = battlestates.waiting;
        currstate = battlestates.waiting2;
    }

    public IEnumerator reaction()
    {
        animator.SetBool("react", true);
        yield return new WaitForSeconds(1 / 2);
        animator.SetBool("react", false);
    }

    private IEnumerator actiontimem()
    {
        //test
        bool a = true;
        animations.Add(a);
        //test

        animationdone = false;
        herocombatstate.animationdone = false;
        if (actionstarted) {
            yield break;
        }
        actionstarted = true;

        //move to target
        Vector3 heroposition= new Vector3(theattacked.transform.position.x+15, theattacked.transform.position.y, theattacked.transform.position.z);
        animator.SetFloat("sped", sped);
        while (move(heroposition)) {
            yield return null;
        }
        animator.SetFloat("sped", 1 / 100); //stop moving
        //move to target

        //attack animation
        for (int i=0;i<bts.list[0].ability.amount;i++)
        {
            if (i % 2 == 0)
            {
                animator.SetBool("attack", true);
            }
            else
            {
                animator.SetBool("attack1", true);
            }
            herocombatstate hst = theattacked.GetComponent<herocombatstate>();
            hst.updatebar(); //update hp bar
            yield return new WaitForSeconds(1);
        }
        //attack animation

        //move back
        transform.Rotate(Vector3.up, 180);
        animator.SetFloat("sped", sped);
        while (move(startpos))
        {
            yield return null;
        }
        
        animator.SetFloat("sped", 1 / 100);
        transform.Rotate(Vector3.up, 180);
        //move back

        bts.list.RemoveAt(0);
        //bts.battlestates = battlestate.action.wait;
        currstate = battlestates.waiting;
        actionstarted = false;
        animationdone = true;
        herocombatstate.animationdone = true;

        //test
        animations.RemoveAt(0);
    }


    private bool move(Vector3 target) {

        
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, sped*Time.deltaTime));
        
    }

    private bool move(Vector3 target, GameObject objecc)
    {
        return target != (objecc.transform.position = Vector3.MoveTowards(objecc.transform.position, target, sped * Time.deltaTime));
    }

    private bool move(Vector3 target, GameObject objecc, float speeed)
    {
        return target != (objecc.transform.position = Vector3.MoveTowards(objecc.transform.position, target, speeed * Time.deltaTime));
    }

    public void endattack() {
        animator.SetBool("attack", false);
        animator.SetBool("attack1", false);
    }

    public void calcdamage(float damage) {
        if (Random.Range(0, 50) < enemy.AGI)
        {
            damage = 0; // block
        }
        animator.SetFloat("dmg", damage);
        enemy.currenthp = enemy.currenthp - damage;

    }


    public void updatebar()
    {
        StartCoroutine(reaction());

        float calchp = enemy.currenthp / enemy.basehp;
        hp.transform.localScale = new Vector3(Mathf.Clamp(calchp, 0, 1), hp.transform.localScale.y, hp.transform.localScale.z);

        if (enemy.currenthp <= 0)
        {
           currstate = battlestates.die;
        }
    }

    private void setup()
    {
        //enemyholder pom;
        enemyholder pom = GameObject.Find("manager").GetComponent<enemyholder>();
        int povlvl = Mathf.RoundToInt((gameinfo.hero[0].level + gameinfo.hero[1].level + gameinfo.hero[2].level)/3);
        enemy = pom.createenemy(povlvl);
    }


    public void chooseaction()
    {
        turnhandler action = new turnhandler();
        action.target = enemytarget();
        action.atacker = this.gameObject;
        action.attackername = this.name;
        action.ability = chooseability();
        action.damage = bts.attackvalueenemy(action.ability, enemy);
        bts.getaction(action);
        Debug.Log("the enemy target is:" + action.target.ToString());
    }

    public GameObject enemytarget()
    {
        int p = Random.RandomRange(0, bts.heroes.Count);
        theattacked = bts.heroes[p];
        return bts.heroes[p];
    }

    public ability chooseability()
    {
        List<ability> available = new List<ability>();
        for (int i = 0; i < enemy.abilities.Count; i++)
            if (enemy.abilities[i].cost < enemy.currentmp)
                available.Add(enemy.abilities[i]);
        if (Random.Range(0, 1) == 0)
        {
            if(available.Count>1)
                return available[Random.Range(1,available.Count)];
        }
        return enemy.abilities[0];
    }


    
}