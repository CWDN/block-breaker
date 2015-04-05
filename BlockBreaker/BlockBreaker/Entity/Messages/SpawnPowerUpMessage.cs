using System;
using Atom.Messaging;
using Microsoft.Xna.Framework;

namespace BlockBreaker.Entity.Messages
{
    public class SpawnPowerUpMessage : IMessage
    {
        public string[] Data { get; set; }

        public SpawnPowerUpMessage(int x, int y) : this(new Vector2(x, y))
        {
            Data = new string[2];
            SetXAndY(x, y);
        }

        public SpawnPowerUpMessage(Vector2 position)
        {
            Data = new string[2];
            SetXAndY((int) position.X, (int) position.Y);
        }

        public void SetXAndY(int x, int y)
        {
            Data[0] = x.ToString();
            Data[1] = y.ToString();
        }

        public Vector2 GetXAndY()
        {
            return new Vector2(Convert.ToInt32(Data[0]), Convert.ToInt32(Data[1]));
        }
    }
}
