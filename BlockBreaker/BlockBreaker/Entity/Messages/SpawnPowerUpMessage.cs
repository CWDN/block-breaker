using System;
using Atom.Messaging;
using Microsoft.Xna.Framework;

namespace BlockBreaker.Entity.Messages
{
    public class SpawnPowerUpMessage : IMessage
    {
        /// <summary>
        /// The data to be stored in the message.
        /// </summary>
        public string[] Data { get; set; }

        /// <summary>
        /// Message sent when a power is wanted to be spawned.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public SpawnPowerUpMessage(int x, int y) : this(new Vector2(x, y))
        {
            Data = new string[2];
            SetXAndY(x, y);
        }

        /// <summary>
        /// Message sent when a power is wanted to be spawned.
        /// </summary>
        /// <param name="position"></param>
        public SpawnPowerUpMessage(Vector2 position)
        {
            Data = new string[2];
            SetXAndY((int) position.X, (int) position.Y);
        }

        /// <summary>
        /// Sets the X and Y data.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetXAndY(int x, int y)
        {
            Data[0] = x.ToString();
            Data[1] = y.ToString();
        }

        /// <summary>
        /// Gets the X and Y data in the message.
        /// </summary>
        /// <returns></returns>
        public Vector2 GetXAndY()
        {
            return new Vector2(Convert.ToInt32(Data[0]), Convert.ToInt32(Data[1]));
        }
    }
}
