using System;
using Atom;
using Atom.Messaging;

namespace BlockBreaker.Entity.Messages
{
    public class PowerUpMessage : IMessage
    {
        /// <summary>
        /// The data to be stored in the message.
        /// </summary>
        public string[] Data { get; set; }

        /// <summary>
        /// Message sent when the paddle gets a power up.
        /// </summary>
        /// <param name="paddleId"></param>
        /// <param name="powerUp"></param>
        public PowerUpMessage(int paddleId, PowerUps powerUp)
        {
            Data = new string[2];

            SetPaddleId(paddleId);
            SetPowerUp(powerUp);
        }
        
        /// <summary>
        /// Sets the paddle id for the message.
        /// </summary>
        /// <param name="entityId"></param>
        public void SetPaddleId(int paddleId)
        {
            Data[0] = paddleId.ToString();
        }

        /// <summary>
        /// Sets the power up for the message.
        /// </summary>
        /// <param name="entityId"></param>
        public void SetPowerUp(PowerUps powerUp)
        {
            Data[1] = powerUp.ToString();
        }

        /// <summary>
        /// Gets the entity id.
        /// </summary>
        /// <returns></returns>
        public int GetPaddleId()
        {
            return Convert.ToInt32(Data[0]);
        }

        /// <summary>
        /// Gets the power up from the message.
        /// </summary>
        /// <returns></returns>
        public PowerUps GetPowerUp()
        {
            PowerUps powerUp;

            Enum.TryParse(Data[1], out powerUp);

            return powerUp;
        }
    }
}
