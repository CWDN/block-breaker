using System;
using Atom.Messaging;

namespace BlockBreaker.Entity.Messages
{
    public class ScoreMessage : IMessage
    {
        public string[] Data { get; set; }

        public ScoreMessage(int score)
        {
            Data = new string[1];
            SetScore(score);
        }

        public void SetScore(int score)
        {
            Data[0] = score.ToString();
        }

        public int GetScore()
        {
            return Convert.ToInt32(Data[0]);
        }
    }
}
