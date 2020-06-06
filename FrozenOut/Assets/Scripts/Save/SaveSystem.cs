using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Scripts.Save
{
    public class SaveSystem
    {

        public static void SaveGame(Game game)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/Save.sv";
            FileStream stream = new FileStream(path, FileMode.Create);

            SaveData data = new SaveData(game);

            formatter.Serialize(stream, data);
            stream.Close();
        }

        public static SaveData LoadGame()
        {

            string path = Application.persistentDataPath + "/Save.sv";
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                SaveData data = formatter.Deserialize(stream) as SaveData;
                stream.Close();
                return data;

            }
            else
            {
                Debug.LogError("Save file not found in " + path);
                return null;
            }


        }

    }
}