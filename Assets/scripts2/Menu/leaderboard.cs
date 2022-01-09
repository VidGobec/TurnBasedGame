using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class leaderboard : MonoBehaviour
{
    public GameObject entry;
    public GameObject parent;

    // Start is called before the first frame update
    void Start()
    {

        if (File.Exists(Application.persistentDataPath + "/ leaderboard.ass")==true)
        {
            leaderboardsave data = leaderboardsavesystem.Loadscore();

            for (int i = 0; i < data.returnsize(); i++)
            {
                Debug.Log(i + ":");
                Debug.Log(data.returnscore(i).score);
                GameObject pom = Instantiate(entry, parent.transform);


                Text text;
                text = pom.transform.Find("score/Text").GetComponent<Text>();
                text.text = data.returnscore(i).score.ToString();
                text = pom.transform.Find("floor/Text").GetComponent<Text>();
                text.text = data.returnscore(i).floor.ToString();


                Debug.Log(data.returnsize());
                //break;//test

            }
        }
    }

    public void exit()
    {
        SceneManager.LoadScene("Menu");
    }
}
