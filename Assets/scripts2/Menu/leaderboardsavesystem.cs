using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class leaderboardsavesystem
{
    //saving
    public static void savescore()
    {
        leaderboardsave data = new leaderboardsave(); //constructor

        BinaryFormatter formatter = new BinaryFormatter();
        Debug.Log(Application.persistentDataPath.ToString());
        string path = Application.persistentDataPath + "/ leaderboard.ass"; //Active Save Slot
        FileStream stream = new FileStream(path, FileMode.Create);

        //leaderboardsave data = new leaderboardsave(); //constructor

        formatter.Serialize(stream, data);
        stream.Close();
    }

    //loading
    public static leaderboardsave Loadscore()
    {
        string path = Application.persistentDataPath + "/ leaderboard.ass";
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Open);

        leaderboardsave data = formatter.Deserialize(stream) as leaderboardsave;

        stream.Close();
        return data;
    }

   
}
