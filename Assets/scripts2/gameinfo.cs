using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameinfo : MonoBehaviour
{
    void Awake() {
        DontDestroyOnLoad(transform.gameObject);
    }
    public static int gold;
    public static int food;
    public static int hppotion;
    public static character[] hero = new character[3];
    public static area currarea;
    public static int floor;
}

