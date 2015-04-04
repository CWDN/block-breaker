using System;
using Atom.Messaging;

namespace BlockBreaker.Entity.Messages
{
    public class DeadEntityMessage : IMessage
    {
        public string[] Data { get; set; }

        public DeadEntityMessage(int entityId)
        {
            Data = new string[1];
            SetEntityId(entityId);
        }
        
        public void SetEntityId(int entityId)
        {
            Data[0] = entityId.ToString();
        }

        public int GetEntityId()
        {
            return Convert.ToInt32(Data[0]);
        }
    }
}
