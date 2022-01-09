using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class Syvesystem 
{
    //saving
    public static void saveplayer()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        Debug.Log(Application.persistentDataPath.ToString());
        string path = Application.persistentDataPath + "/ player.ass"; //Active Save Slot
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(); //constructor

        formatter.Serialize(stream, data);
        stream.Close();
    }
    //loading
    public static PlayerData Loadplayer()
    {
        string path = Application.persistentDataPath + "/ player.ass";
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Open);

        PlayerData data = formatter.Deserialize(stream) as PlayerData;

        stream.Close();
        return data;
    }
}
