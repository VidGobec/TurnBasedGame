using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    /*
    public battlestate bts;
    public int speed = 30;
    public Rigidbody2D rb;
    public float xpos;
    public GameObject flyto;
    void Start()
    {
        bts = GameObject.Find("manager").GetComponent<battlestate>();
        flyto = gettarget();
        rb.velocity = transform.right * speed;
    }

    private void Update()
    {
        if (this.transform.rotation.y <= 0)
            if (this.transform.position.x <= flyto.transform.position.x)
            {
                //dmg enemy and destroy object
                enemycombatstate.numproj++;
                Destroy(this.gameObject);
            }
        else
            if (this.transform.position.x >=flyto.transform.position.x)
            {
                enemycombatstate.numproj++;
                Destroy(this.gameObject);
            }
    }

    private GameObject gettarget()
    {
        return bts.list[0].target;
    }
    */
}
