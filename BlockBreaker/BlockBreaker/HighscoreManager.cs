using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BlockBreaker
{
    public class HighscoreManager
    {
        private static HighscoreManager _instance;

        private string _filePath;

        public HighscoreManager()
        {
            _instance = this;
        }

        public void CheckHighScoreFile(string filePath = "")
        {
            string file;

            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                string directory = Directory.GetCurrentDirectory() + @"\Highscores";
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                file = directory + @"\highscores.txt";
                if (!File.Exists(file))
                {
                    File.Create(file).Dispose();
                }

                _filePath = file;
            }
            else
            {
                _filePath = filePath;
            }
        }

        public void AddHighScore(int score)
        {
            CheckHighScoreFile();

            List<int> scores = GetHighScores();

            scores.Add(score);
            scores = Sort(scores);
            scores.Reverse();

            using (StreamWriter writer = new StreamWriter(_filePath))
            {
                scores.ForEach(writer.WriteLine);
            }
        }

        public List<int> GetHighScores()
        {
            CheckHighScoreFile();
            string[] scores = File.ReadAllLines(_filePath);
            return scores.ToList().ConvertAll(Convert.ToInt32);
        }

        public List<int> Sort(List<int> items)
        {
            for (int index = 1; index < items.Count; index++)
            {
                int value = items[index];
                int sortIndex = index - 1;

                while (sortIndex >= 0 && items[sortIndex] > value)
                {
                    items[sortIndex + 1] = items[sortIndex];
                    sortIndex--;
                }
                items[sortIndex + 1] = value;
            }
            return items;
        }

        public int GetHighestScore()
        {
            return GetHighScores().FirstOrDefault();
        }

        public static HighscoreManager GetInstance()
        {
            return _instance ?? new HighscoreManager();
        }


    }
}
