using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace LeaderBoard
{
    public static class SaveFile
    {

        public static FileStream file = null;
        public static string fileName = Application.persistentDataPath + "save.sav";

        public static void SetFilePath(string _path)
        {
            fileName = _path;
        }
        public static void Save<T>(T _toSave, string _path) where T : class
        {
            BinaryFormatter binFo = new BinaryFormatter();
            string toSave = JsonUtility.ToJson(_toSave);
            file = File.Exists(_path) ? File.Open(_path, FileMode.Open, FileAccess.ReadWrite) : File.Create(_path);
            binFo.Serialize(file, toSave);
            file.Close();
        }

        public static T Load<T>(string _path) where T : class
        {
            T t = null;
            BinaryFormatter binFo = new BinaryFormatter();

            if (File.Exists(_path))
            {
                FileStream file = File.Open(_path, FileMode.Open, FileAccess.ReadWrite);
                t = JsonUtility.FromJson<T>((string)binFo.Deserialize(file));
                file.Close();
            }
            return t;
        }
    }
}