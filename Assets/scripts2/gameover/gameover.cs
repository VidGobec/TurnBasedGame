using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class gameover : MonoBehaviour
{
    [SerializeField] private GameObject bestscore;
    private int score;
    // Start is called before the first frame update
    void Start()
    {
        score = gameinfo.floor;
        float p = 0;

        if (File.Exists(Application.persistentDataPath + "/ leaderboard.ass") == true)
        {
            leaderboardsave pom = leaderboardsavesystem.Loadscore();
            p = pom.returnscore(0).score;
        }
        else p = 0;
 
        Text text = GameObject.Find("Canvas/Panel/highscore/text").GetComponent<Text>();
        text.text = p.ToString();
        text = GameObject.Find("Canvas/Panel/score/text").GetComponent<Text>();
        text.text = score.ToString();


        if (score > p) bestscore.SetActive(true);

        leaderboardsavesystem.savescore();
        File.Delete(Application.persistentDataPath + "/ player.ass");
    }

    public void tomenu()
    {
        SceneManager.LoadScene("Menu");
    }

    
}
