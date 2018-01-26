using System.Collections.Generic;
using System.IO;


namespace LeaderBoard
{
    [System.Serializable]
    public class ScoreData
    {
        public List<string> names = new List<string>();
        public List<int> values = new List<int>();
        public int maxAmount = 20;
        public int index = 0;

        public ScoreData(string _fileName)
        {
            if (File.Exists(_fileName))
            {
                ScoreData s = SaveFile.Load<ScoreData>(_fileName);
                names = s.names;
                values = s.values;
                index = names.Count;
            }
        }
        public void Add(string _name, int _val)
        {
            names.Add(_name);
            values.Add(_val);
            Sort();
            Trim();
            index = names.Count;
        }
        public KeyValuePair<string, int> GetData(string _name)
        {
            KeyValuePair<string, int> pair = new KeyValuePair<string, int>("error", -1);
            for (int i = 0; i < names.Count; i++)
            {
                if (names[i] == _name)
                {
                    pair = new KeyValuePair<string, int>(_name, values[i]);
                }
            }
            return pair;
        }

        void Sort()
        {
            for (int i = 0; i < values.Count; i++)
            {
                for (int j = 0; j < values.Count - 1; j++)
                {
                    if (values[j] < values[j + 1])
                    {
                        string tempN = names[j];
                        int tempv = values[j];
                        names[j] = names[j + 1];
                        values[j] = values[j + 1];
                        names[j + 1] = tempN;
                        values[j + i] = tempv;

                    }
                }
            }
        }

        void Trim()
        {
            if (values.Count > maxAmount)
            {
                names.RemoveRange(maxAmount, values.Count - maxAmount);
                values.RemoveRange(maxAmount, values.Count - maxAmount);
            }
        }

        public KeyValuePair<string, int> GetData(int _index)
        {
            KeyValuePair<string, int> pair = new KeyValuePair<string, int>("error", -1);
            if (_index >= names.Count)
                return pair;

            return new KeyValuePair<string, int>(names[_index], values[_index]);
        }
        public void Clear()
        {
            names.Clear();
            values.Clear();
        }

        public void Save(string _fileName)
        {
            SaveFile.Save(this, _fileName);
        }
    }
}