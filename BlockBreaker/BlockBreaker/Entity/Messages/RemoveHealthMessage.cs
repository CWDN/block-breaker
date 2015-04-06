using System;
using Atom.Messaging;
using BlockBreaker.Entity.Systems;

namespace BlockBreaker.Entity.Messages
{
    public class RemoveHealthMessage : IMessage
    {
        /// <summary>
        /// The data to be stored in the message.
        /// </summary>
        public string[] Data { get; set; }

        /// <summary>
        /// Message sent when health is to be removed from an entity.
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="health"></param>
        public RemoveHealthMessage(int entityId, int health)
        {
            Data = new string[2];
            SetEntityId(entityId);
            SetHealth(health);
        }

        /// <summary>
        /// Sets the entity id data.
        /// </summary>
        /// <param name="entityId"></param>
        public void SetEntityId(int entityId)
        {
            Data[0] = entityId.ToString();
        }

        /// <summary>
        /// Set the health delta data.
        /// </summary>
        /// <param name="health"></param>
        public void SetHealth(int health)
        {
            Data[1] = health.ToString();
        }

        /// <summary>
        /// Gets the entity id data from the message.
        /// </summary>
        /// <returns></returns>
        public int GetEntityId()
        {
            return Convert.ToInt32(Data[0]);
        }
        
        /// <summary>
        /// Gets the health delta data from the message.
        /// </summary>
        /// <returns></returns>
        public int GetHealth()
        {
            return Convert.ToInt32(Data[1]);
        }
    }
}
