using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[System.Serializable]
public class leaderboardsave
{
    [System.Serializable]
    public struct scorestats
    {
        public int floor;
        public float score;
    }

    List<scorestats> scoretosave = new List<scorestats>();

    public leaderboardsave()
    {
        scorestats toadd = new scorestats();
        toadd.floor = gameinfo.floor;

        //calc score
        toadd.score = gameinfo.floor; //add more stuff if ever added
        //
        if (File.Exists(Application.persistentDataPath + "/ leaderboard.ass"))
        {
            Debug.Log("adding to list");
            leaderboardsave pom = leaderboardsavesystem.Loadscore();
            scoretosave = pom.scoretosave;

            //get position to insert
            int i = 0;
            for (int j = scoretosave.Count-1; i < j;)
            {
                Debug.Log(j);
                if (scoretosave[(j + i) / 2].score == toadd.score)
                {
                    i = (j + i) / 2;
                    break;
                }
                else if (scoretosave[(j + i) / 2].score <= toadd.score)
                    j = (j / 2) - 1;
                else
                    i = (j / 2) + 1;
            }
            Debug.Log(i);
            if (scoretosave[i].score < toadd.score) scoretosave.Insert(i, toadd);
            else if (i + 1 >= scoretosave.Count) scoretosave.Add(toadd);
            else scoretosave.Insert(i + 1, toadd);

            //if (i + 1 >= scoretosave.Count && toadd.score<scoretosave[i].score || scoretosave.Count == 0) scoretosave.Add(toadd);
            //else scoretosave.Insert(i, toadd);
        }
        else
        {
            Debug.Log("adding to empty list");
            scoretosave.Add(toadd);
        }
    }

    public scorestats returnscore(int i)
    {
        return scoretosave[i];
    }

    public int returnsize()
    {
        return scoretosave.Count;
    }
}
