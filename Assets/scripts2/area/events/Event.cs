using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(fileName = "new event", menuName = "new event")]
public class Event : ScriptableObject
{
    public string title;
    public string description;
    public string options1;
    public string options2;
    public string options3;
    //public Eventbehaviour[] eventbehaviour= new Eventbehaviour[3];

    //stuffs


    public Buff[] buff = new Buff[3];


}