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
        public string[] Data { get; set; }

        public SpawnLaserMessage(int x, int y)
        {
            Data = new string[2];
            SetX(x);
            SetY(y);
        }

        public void SetX(int x)
        {
            Data[0] = x.ToString();
        }

        public void SetY(int y)
        {
            Data[1] = y.ToString();
        }

        public Vector2 GetXAndY()
        {
            return new Vector2(Convert.ToInt32(Data[0]), Convert.ToInt32(Data[1]));
        }
    }
}
