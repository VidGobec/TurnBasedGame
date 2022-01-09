using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new item", menuName = "item")]
public class item : ScriptableObject
{
    public new string name;

    public Sprite artwork;

    public string type; //
    public int lvl;

    public int atk;
    public int def;

    public int itemmod1;
    public string itemmode1type;
    public int itemmod2;
    public string itemmode2type;
    public int itemmod3;
    public string itemmode3type;
}
