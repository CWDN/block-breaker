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

        /// <summary>
        /// Manages the high scores of the game.
        /// </summary>
        public HighscoreManager()
        {
            _instance = this;
        }
        
        /// <summary>
        /// Checks the high score file exists.
        /// </summary>
        /// <param name="filePath"></param>
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

        /// <summary>
        /// Adds a new high score to the list.
        /// </summary>
        /// <param name="score"></param>
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

        /// <summary>
        /// Gets all the high scores.
        /// </summary>
        /// <returns></returns>
        public List<int> GetHighScores()
        {
            CheckHighScoreFile();
            string[] scores = File.ReadAllLines(_filePath);
            return scores.ToList().ConvertAll(Convert.ToInt32);
        }

        /// <summary>
        /// Does an insertion sort on the high score list.
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the number one high score.
        /// </summary>
        /// <returns></returns>
        public int GetHighestScore()
        {
            return GetHighScores().FirstOrDefault();
        }

        /// <summary>
        /// Returns the instance of the high score manager.
        /// </summary>
        /// <returns></returns>
        public static HighscoreManager GetInstance()
        {
            return _instance ?? new HighscoreManager();
        }


    }
}
