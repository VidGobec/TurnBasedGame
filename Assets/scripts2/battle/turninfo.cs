using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turninfo : ScriptableObject
{
    public GameObject attacker;
    public List<GameObject> target = new List<GameObject>();
    public ability turnability;
}
