using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "battlearea", menuName = "battlearea")]
[System.Serializable]
public class battlearea : ScriptableObject
{
    public string name;
    public List<character> enemy = new List<character>();

}
