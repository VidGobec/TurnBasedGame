using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new area", menuName = "new area")]
[System.Serializable]
public class area : ScriptableObject
{
    public string name;
    public List<character> enemy = new List<character>();
    public List<Event> events = new List<Event>();
}

