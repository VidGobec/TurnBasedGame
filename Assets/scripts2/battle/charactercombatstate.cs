using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using UnityEditor;

public class charactercombatstate : MonoBehaviour
{
    public Animator animator;
    float sped = 90; //kok hitr se characterji premikajo
    private float x = 5.12f;
    private float y = 2.56f;
    private bool ishero;
    public enum battlestates
    {
        waiting,
        taketurn,
        win,
        die
    }
    public battlestates currstate;

    public int setupst; //0-2 hero 3-5enemy
    public character me; //to test change to private later
    //progressbar
    private float maxcd = 5;
    private float curcd = 0;
    public Image progressbar;
    public Image hp;
    public Image manabar;

    //load order play1-3 enemy-3-6
    public static int loadorder=0;


    //combatstate
    private combatmanager cmanager;

    //
    public GameObject target; //to see if it works right set to private later
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("adding character");
        currstate = battlestates.waiting;
        
        cmanager = GameObject.Find("manager").GetComponent<combatmanager>();

      

        //get information
        //vsem kerakterjem sem dal stevilko 1-3 za igralce in 4-6 za neprijazne tako pravi gameobject dobi prave informacije
        if (setupst <= 3)
        {
            ishero = true;
            //if (gameinfo.hero[setupst - 1].currenthp > 0)
            //{
                me = gameinfo.hero[setupst - 1];
                this.transform.position = new Vector3(-1 * 20 * me.position, 18, 110);
                transform.Rotate(Vector3.up, 180);
                //add to cmanager list
                cmanager.addhero(this.gameObject);
            /*}
            else
            {
                Debug.Log("disabling hero");//disable ui and hero
                cmanager.deadhero(this.gameObject);
            }*/
        }
        else
        {
            Debug.Log("enemyspawned");
            ishero = false;
            //perhaps decide on how meny enemies
            me = gameinfo.currarea.enemy[Random.Range(0, gameinfo.currarea.enemy.Count-1)];
            this.transform.position = new Vector3(1 * 20 * (setupst-3), 18, 110);
            me.currenthp = me.basehp;
            //add to cmanager list
            cmanager.addenemy(this.gameObject);
            cmanager.playertarget = this.gameObject;
        }


        //cmanager = GameObject.Find("manager").GetComponent<combatmanager>();
        float calchp = me.currenthp / me.basehp;
        hp.transform.localScale = new Vector3(Mathf.Clamp(calchp, 0, 1), hp.transform.localScale.y, hp.transform.localScale.z);

        //test - delete later
        //me.ai = true;

        


        //set animator clips
        //Debug.Log(this.gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size.x);
        //Debug.Log(this.gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size.y);
        changeclips(animator, me.race);
    }

    // Update is called once per frame
    void Update()
    {
        switch (currstate) {
            case (battlestates.waiting):
                updateprogressbar();
                break;
            case (battlestates.taketurn):
                if (this.me.ai == true)
                {

                }
                break;
            case (battlestates.win):
                animator.SetBool("victory", true);
                break;
            case (battlestates.die):
                //cmanager.check();
                
                StartCoroutine(death());
                
                break;
        }

    }

    public void endattack()
    {
        animator.SetBool("attack", false);
        animator.SetBool("attack1", false);
        Debug.Log(this.me.name + "attack ended");
    }

    public void reduceprogress()
    {
        this.curcd = 0;
    }

    public void reduceprogress(float red)
    {
        curcd = curcd - red;
    }

    public void updateprogressbar()
    {
        if(cmanager.tomanage.Count<1)
            curcd = curcd + Time.deltaTime * (1 + this.me.SPD / 10);
        float calccd = curcd / maxcd;
        progressbar.transform.localScale = new Vector3(Mathf.Clamp(calccd, 0, 1), progressbar.transform.localScale.y, progressbar.transform.localScale.z);

        if (curcd >= maxcd)
        {
            cmanager.addtolist(this.gameObject);
            curcd = 0;
        }
    }

    public void aidecides() {
        if (ishero == true) target = cmanager.enemies[Random.Range(0, cmanager.enemies.Count)];
        else target = cmanager.heroes[Random.Range(0, cmanager.heroes.Count)];
    }

    public ability randomaiability() {
        return me.abilities[Random.Range(0,me.abilities.Count)];
    }

    //take damage
    public void takedamage(float dmg) {
        me.currenthp = me.currenthp - dmg; // add def here if want

        if (me.currenthp > me.basehp) me.currenthp = me.basehp;

        if(dmg>0)
            animator.SetFloat("dmg", dmg);
        updatebar();
        //hurtanimation
    }

    //change hp

    public void updatebar()
    {

        float calchp = me.currenthp / me.basehp;
        hp.transform.localScale = new Vector3(Mathf.Clamp(calchp, 0, 1), hp.transform.localScale.y, hp.transform.localScale.z);
        StartCoroutine(reaction());

        if (this.me.currenthp <= 0)
        {
            currstate = battlestates.die;
        }
    }


    //mana
    public void updatemanabar(int mana)
    {
        Debug.Log(me.currentmp);
        me.currentmp = me.currentmp - mana;
        Debug.Log(me.currentmp);
        float pom = me.currentmp;
        float calmana = pom / me.basemp;
        if (me.currentmp > me.basemp) me.currentmp = me.basemp;

        Debug.Log(calmana);
        Debug.Log(Mathf.Clamp(calmana, 0, 1));
        manabar.transform.localScale = new Vector3(Mathf.Clamp(calmana, 0, 1), manabar.transform.localScale.y, manabar.transform.localScale.z);
    }

    //take turn function
    public async void meleeturn(turninfo tinfo)
    {
        //move
        for (int i = 0; i < tinfo.target.Count; i++)
        {
            //move to attack
            Vector3 startingposition = transform.position;
            int a = 15; //distance so you moveinfront of enemy
            if (tinfo.attacker.transform.position.x > 0) a = -1 * a;
            Vector3 heroposition = new Vector3(tinfo.target[i].transform.position.x - a, tinfo.target[i].transform.position.y, tinfo.target[i].transform.position.z);
            animator.SetFloat("sped", sped);
            while (move(heroposition))
            {
                await Task.Yield();
            }
            animator.SetFloat("sped", 0);

            //ATTACK 

            for (int j = 0; j < tinfo.turnability.amount; j++)
            {
                Debug.Log("beginning attack");
                if (j % 2 == 0)
                {
                    animator.SetBool("attack", true);
                    while (animator.GetBool("attack")==true)
                    {
                        await Task.Yield();
                        Debug.Log("attacking1");
                    }
                }
                else
                {
                    animator.SetBool("attack1", true);
                    while (animator.GetBool("attack1") == true)
                    {
                        await Task.Yield();
                        Debug.Log("attacking2");
                    }
                }
                Debug.Log("finishedhitting");
                charactercombatstate pom = tinfo.target[i].GetComponent<charactercombatstate>();
                pom.takedamage(cmanager.calculatepdmg(tinfo.turnability, this, pom));
            }

            //move back
            transform.Rotate(Vector3.up, 180);
            animator.SetFloat("sped", sped);
            while (move(startingposition))
            {
                await Task.Yield();
            }
            animator.SetFloat("sped", 0);
            transform.Rotate(Vector3.up, 180);
        }
        cmanager.remove();

    }



    public async void rangedturn(turninfo tinfo)
    {
        //start of attack
        Vector3 startingposition = transform.position;
        int a = -5; //distance for hero
        if (tinfo.attacker.transform.position.x > 0)
        {
            a = -1 * a;
        }
        startingposition.x = startingposition.x + a;
        for (int i = 0; i < tinfo.target.Count; i++)
        {
            animator.SetBool("magic", true);
            GameObject bullet = Instantiate(tinfo.turnability.projectileprefab);
            bullet.transform.position = startingposition;
            if (tinfo.attacker.transform.position.x > 0) bullet.transform.Rotate(Vector3.up, 180);
            

            while (move(tinfo.target[i].transform.position, bullet))
            {
                await Task.Yield();
            }

            charactercombatstate pom = tinfo.target[i].GetComponent<charactercombatstate>();
            pom.takedamage(cmanager.calculatepdmg(tinfo.turnability, this, pom));

            Destroy(bullet);    
            animator.SetBool("magic", false);
        }
        cmanager.remove();
    }

    public IEnumerator ranged2turn(turninfo tinfo)
    {

        animator.SetBool("magic", true);
        List<GameObject> bullet = new List<GameObject>();
        for (int i = 0; i < tinfo.target.Count; i++)
        {
            bullet.Add(Instantiate(tinfo.turnability.projectileprefab));
            bullet[i].transform.position = tinfo.target[i].transform.position;
        


        charactercombatstate pom = tinfo.target[i].GetComponent<charactercombatstate>();
            if(tinfo.turnability.type == ability.Type.dmg) //new
                pom.takedamage(cmanager.calculatepdmg(tinfo.turnability, this, pom));
            else pom.takedamage(cmanager.calculatehealing(tinfo.turnability, this, pom)); //new
        }
        yield return new WaitForSeconds(1);

        for (int i = 0; i < tinfo.target.Count; i++)
            Destroy(bullet[i]);
            animator.SetBool("magic", false);
        
        cmanager.remove();
    }



    //animeations
    public IEnumerator reaction()
    {
        animator.SetBool("react", true);
        yield return new WaitForSeconds(1 / 2);
        animator.SetBool("react", false);
    }

    private bool move(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, sped * Time.deltaTime));
    }

    private bool move(Vector3 target, GameObject x)
    {
        return target != (x.transform.position = Vector3.MoveTowards(x.transform.position, target, sped * Time.deltaTime));
    }

    
    public IEnumerator death() //bad
    {
        animator.SetBool("dead", true);
        if (cmanager.heroes.Contains(this.gameObject) == true)
        {
            cmanager.heroes.Remove(this.gameObject);

            //cmanager.checkheroes();
        }
        else
        {
            
            cmanager.enemies.Remove(this.gameObject);
            //cmanager.checkenemies();
        }
        //cmanager.check();
        yield return new WaitForSeconds(1);
        
        
        //mabe
        this.gameObject.SetActive(false);
    }



    public IEnumerator explode(int mana)
    {
        //GameObject explos = AssetDatabase.LoadAssetAtPath("Assets/Resources/Prefabs/Combustion.prefab", typeof(GameObject)) as GameObject;
        GameObject explos = Resources.Load<GameObject>("Prefabs/Combustion");
        GameObject pom;
        pom = Instantiate(explos, this.transform.position,this.transform.rotation);
        reaction();
        yield return new WaitForSeconds(1);
        Destroy(pom);
        me.currenthp = me.currenthp - Random.Range(1, mana);
        updatebar();
        cmanager.remove();
    }

    public void failureanim()
    {
        //animations here
        cmanager.remove();
    }
    //animator fuctions

    public void changeclips(Animator animator, string name)
    {
        AnimatorOverrideController overrideController = new AnimatorOverrideController();
        overrideController.runtimeAnimatorController = animator.runtimeAnimatorController;

        int i = 0;
        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            //GameObject pomo = Resources.Load<GameObject>("character/" + name + "/" + clip.name);
            
            ResourceRequest request = Resources.LoadAsync("character/" + name + "/" + clip.name);
            AnimationClip animclip = request.asset as AnimationClip;
            
            /*
            Debug.Log("---------------------");
            Debug.Log("character/" + name + "/" + clip.name);
            Debug.Log(request);
            Debug.Log(clip.name);
            Debug.Log(animclip);
            Debug.Log("---------------------");
            */

            overrideController[clip.name] = animclip;

        }

        animator.runtimeAnimatorController = overrideController;
    }

  

    public AnimationClip FindAnimation(Animator animator, string name)
    {
        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == name)
            {
                return clip;
            }
        }

        return null;
    }

}
