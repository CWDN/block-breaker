using System;
using Atom;
using Atom.Messaging;

namespace BlockBreaker.Entity.Messages
{
    public class PowerUpMessage : IMessage
    {
        public string[] Data { get; set; }

        public PowerUpMessage(int paddleId, PowerUps powerUp)
        {
            Data = new string[2];

            SetPaddleId(paddleId);
            SetPowerUp(powerUp);
        }

        public void SetPaddleId(int paddleId)
        {
            Data[0] = paddleId.ToString();
        }

        public void SetPowerUp(PowerUps powerUp)
        {
            Data[1] = powerUp.ToString();
        }

        public int GetPaddleId()
        {
            return Convert.ToInt32(Data[0]);
        }

        public PowerUps GetPowerUp()
        {
            PowerUps powerUp;

            Enum.TryParse(Data[1], out powerUp);

            return powerUp;
        }
    }
}
