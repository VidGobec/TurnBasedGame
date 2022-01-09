using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using UnityEditor;



public class StartMenu : MonoBehaviour
{
    public Button button;
    
    public void Start()
    {
        Debug.Log("runnin");
        if (File.Exists(Application.persistentDataPath + "/ player.ass"))
            button.interactable = true;
        else
            button.interactable = false;
    }

    public void play() {
        SceneManager.LoadScene("team creation");
    }

    public void load()
    {
        PlayerData data = Syvesystem.Loadplayer();


        //load area info from prefab
        gameinfo.currarea = new area();
        gameinfo.currarea.name = data.area;
        Debug.Log(data.area);
        Debug.Log("Assets/scripts2/area/areas/" + data.area + ".asset");
        //area pomarea = AssetDatabase.LoadAssetAtPath("Assets/scripts2/area/areas/" + data.area + ".asset", typeof(area)) as area;
        area pomarea = Resources.Load<area>(data.area);
        Debug.Log(pomarea);
        for (int i = 0; i < pomarea.enemy.Count; i++)
            gameinfo.currarea.enemy.Add(pomarea.enemy[i]);
        for (int i = 0; i < pomarea.events.Count; i++)
            gameinfo.currarea.events.Add(pomarea.events[i]);

        Debug.Log(gameinfo.currarea.enemy[0]);
        gameinfo.floor = data.floor;
        gameinfo.gold = data.gold;
        gameinfo.food = data.food;
        gameinfo.hppotion = data.hppotion;

        for (int i = 0; i < 3; i++)
        {
            gameinfo.hero[i] = data.returnchar(i);
        }

        gameinfo.floor--; //just so cant abuse
        SceneManager.LoadScene("Exploration");
    }

    public void leaderboard()
    {
        SceneManager.LoadScene("highscore");
    }

    public void exit()
    {
        Application.Quit();
    }


}
