using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace LeaderBoard
{
    public class NameInput : MonoBehaviour
    {
        [SerializeField] private Text[] names;
        private int[] letterIndex;
        private int[] asciiIndex;
        private char[] name;
        [SerializeField] private float maxTime = 0.2f;
        bool allowPress = true;
        //debug
        public Text result;
        private Timer timer;

        private int index = 0;
        private int[] letters = { 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 32, 33, 35, 38, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 63 };
        [SerializeField] private List<char> forbiddenLetters = new List<char>();
        private void Awake()
        {
            name = new char[names.Length];
            letterIndex = new int[names.Length];
            asciiIndex = new int[names.Length];

            for (int i = 0; i < names.Length; i++)
            {
                letterIndex[i] = 0;
                asciiIndex[i] = 65;
                name[i] = (char)65;
            }
            UpdateText();
            timer = new Timer(maxTime);
        }

        private void Update()
        {
            for (int i = 0; i < names.Length; i++)
            {
                if (i == index)
                {
                    names[i].color = Color.red;
                }
                else
                {
                    names[i].color = Color.black;
                }
            }
                IterateThroughLetters(Input.GetKeyDown(KeyCode.UpArrow) ? 1 : Input.GetKeyDown(KeyCode.DownArrow) ? -1 : 0);
                IterateThroughField(Input.GetKeyDown(KeyCode.LeftArrow) ? -1 : Input.GetKeyDown(KeyCode.RightArrow) ? 1 : 0);
                
            
            result.text = GetName();
        }


        private void UpdateText()
        {
            for (int i = 0; i < names.Length; i++)
            {
                names[i].text = name[i].ToString();
            }
        }
        void CheckForForbiddenLetter(int _direction)
        {
            if (forbiddenLetters.Contains((char)asciiIndex[index]))
            {
                asciiIndex[index] += _direction;
                if (_direction == 1)
                {
                    if (asciiIndex[index] > 126)
                    {
                        asciiIndex[index] = 32;
                    }
                }
                else
                {
                    if (asciiIndex[index] < 32)
                    {
                        asciiIndex[index] = 126;
                    }
                }
                CheckForForbiddenLetter(_direction);
            }
            return;
        }
        void IterateThroughAscii(float _direction)
        {
            if (_direction != 0)
            {
                int dir = Mathf.FloorToInt(_direction);
                if (dir > 0)
                {
                    asciiIndex[index]++;
                    if (asciiIndex[index] > 126)
                    {
                        asciiIndex[index] = 32;
                    }
                }
                else
                {
                    asciiIndex[index]--;
                    if (asciiIndex[index] < 32)
                    {
                        asciiIndex[index] = 126;
                    }
                }
                CheckForForbiddenLetter(dir);
                name[index] = (char)asciiIndex[index];
                UpdateText();
            }
        }
        void IterateThroughLetters(float _direction)
        {
            if (_direction != 0)
            {
                if (timer.timeElapsed)
                {
                    if (Mathf.FloorToInt(_direction) > 0)
                    {
                        letterIndex[index]++;
                        if (letterIndex[index] == letters.Length)
                        {
                            letterIndex[index] = 0;
                        }
                    }
                    else
                    {
                        letterIndex[index]--;
                        if (letterIndex[index] < 0)
                        {
                            letterIndex[index] = letters.Length - 1;
                        }
                    }
                    timer.StartTimer();
                }
            }
            name[index] = (char)letters[letterIndex[index]];
            UpdateText();
        }
        void IterateThroughField(float _direction)
        {
            if (_direction != 0)
            {
                if (timer.timeElapsed)
                {
                    if (Mathf.FloorToInt(_direction) > 0)
                    {
                        index++;
                        if (index == names.Length)
                        {
                            index = 0;
                        }
                    }
                    else
                    {
                        index--;
                        if (index < 0)
                        {
                            index = names.Length - 1;
                        }
                    }
                    timer.StartTimer();
                }
            }
        }

        public string GetName()
        {
            return new string(name);
        }
    }
}