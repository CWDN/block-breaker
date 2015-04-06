using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atom.Messaging;
using Microsoft.Xna.Framework;

namespace BlockBreaker.Entity.Messages
{
    class SpawnLaserMessage : IMessage
    {
        /// <summary>
        /// The data to be stored in the message.
        /// </summary>
        public string[] Data { get; set; }

        /// <summary>
        /// Message sent when a laser is wanted to be spawned.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public SpawnLaserMessage(int x, int y)
        {
            Data = new string[2];
            SetX(x);
            SetY(y);
        }

        /// <summary>
        /// Sets the X data.
        /// </summary>
        /// <param name="x"></param>
        public void SetX(int x)
        {
            Data[0] = x.ToString();
        }

        /// <summary>
        /// Sets the Y data.
        /// </summary>
        /// <param name="y"></param>
        public void SetY(int y)
        {
            Data[1] = y.ToString();
        }

        /// <summary>
        /// Gets the X and Y data as a Vector2.
        /// </summary>
        /// <returns></returns>
        public Vector2 GetXAndY()
        {
            return new Vector2(Convert.ToInt32(Data[0]), Convert.ToInt32(Data[1]));
        }
    }
}
