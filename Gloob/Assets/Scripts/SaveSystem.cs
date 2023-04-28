using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public static class SaveSystem
{
    public static void SaveStats(HPBar p)
    {
        BinaryFormatter format = new BinaryFormatter();
        string path = Application.persistentDataPath + "gloobStats.config";
        FileStream stream = new FileStream(path, FileMode.Create);

        //saves the data for health and damage
        SerializeData data = new SerializeData(p);
        format.Serialize(stream, data);
        stream.Close();
    }

    public static SerializeData loadStats (){
        string path = Application.persistentDataPath + "gloobStats.config";
        if (File.Exists(path)) {
            BinaryFormatter format = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SerializeData data = format.Deserialize(stream) as SerializeData;
            return data;
        }
        else {
            Debug.Log("File " + path + " not found.");
            return null;
        }
    }
}
