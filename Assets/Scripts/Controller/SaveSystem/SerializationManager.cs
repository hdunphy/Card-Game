using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Assets.Scripts.Controller.SaveSystem
{
    public class SerializationManager
    {
        public static bool Save(string saveName, object saveData)
        {
            BinaryFormatter formatter = GetBinaryFormatter();

            if (!Directory.Exists(Application.persistentDataPath + "/saves"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/saves");
            }

            string savePath = Application.persistentDataPath + "/saves/" + saveName + ".save";

            FileStream file = File.Create(savePath);

            formatter.Serialize(file, saveData);

            file.Close();

            return true;
        }

        public static object Load(string path)
        {
            if (!File.Exists(path))
            {
                return null;
            }

            BinaryFormatter formatter = GetBinaryFormatter();

            FileStream file = File.Open(path, FileMode.Open);

            try
            {
                object save = formatter.Deserialize(file);
                file.Close();
                return save;
            }
            catch
            {
                Debug.LogError($"Failed to load file at {path}");
                file.Close();
                return null;
            }
        }

        private static BinaryFormatter GetBinaryFormatter()
        {
            BinaryFormatter formatter = new BinaryFormatter();

            SurrogateSelector selector = new SurrogateSelector();

            Vector3SerializationSurrogate vector3Surrogate = new Vector3SerializationSurrogate();

            selector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), vector3Surrogate);

            formatter.SurrogateSelector = selector;

            return formatter;
        }
    }
}