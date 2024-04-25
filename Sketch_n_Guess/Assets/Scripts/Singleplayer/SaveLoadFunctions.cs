//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveLoadFunctions {
    // Instead of saving to .txt, .xml, etc.
    // that can be easily modified,
    // we used BinaryFormatter for encryption of data
    public static void SavePlayerStats(Player player) {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/PlayerStats.dat";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    // Same as in saving the data,
    // we just do the opposite and decrypt the data
    public static PlayerData LoadPlayerStats() {
        string path = Application.persistentDataPath + "/PlayerStats.dat";
        
        if(File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            
            return data;
        } else {
            Debug.LogError("Save file is NOT FOUND in " + path);
            return null;
        }
    }
}
