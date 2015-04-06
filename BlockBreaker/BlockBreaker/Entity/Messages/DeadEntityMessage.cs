using System;
using Atom.Messaging;

namespace BlockBreaker.Entity.Messages
{
    public class DeadEntityMessage : IMessage
    {
        /// <summary>
        /// The data to be stored in the message.
        /// </summary>
        public string[] Data { get; set; }

        /// <summary>
        /// Message is sent when an entity is out of health or dead.
        /// </summary>
        /// <param name="entityId"></param>
        public DeadEntityMessage(int entityId)
        {
            Data = new string[1];
            SetEntityId(entityId);
        }
        
        /// <summary>
        /// Sets the data for the message.
        /// </summary>
        /// <param name="entityId"></param>
        public void SetEntityId(int entityId)
        {
            Data[0] = entityId.ToString();
        }

        /// <summary>
        /// Gets the entity id set within the message.
        /// </summary>
        /// <returns></returns>
        public int GetEntityId()
        {
            return Convert.ToInt32(Data[0]);
        }
    }
}
