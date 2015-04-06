using System;
using Atom.Messaging;

namespace BlockBreaker.Entity.Messages
{
    public class ScoreMessage : IMessage
    {
        /// <summary>
        /// The data to be stored in the message.
        /// </summary>
        public string[] Data { get; set; }

        /// <summary>
        /// Message sent when a score change is needed.
        /// </summary>
        /// <param name="score"></param>
        public ScoreMessage(int score)
        {
            Data = new string[1];
            SetScore(score);
        }

        /// <summary>
        /// Sets the score data.
        /// </summary>
        /// <param name="score"></param>
        public void SetScore(int score)
        {
            Data[0] = score.ToString();
        }

        /// <summary>
        /// Gets the score data.
        /// </summary>
        /// <returns></returns>
        public int GetScore()
        {
            return Convert.ToInt32(Data[0]);
        }
    }
}
