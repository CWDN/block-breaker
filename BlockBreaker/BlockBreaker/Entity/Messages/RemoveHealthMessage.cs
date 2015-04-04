using System;
using Atom.Messaging;
using BlockBreaker.Entity.Systems;

namespace BlockBreaker.Entity.Messages
{
    public class RemoveHealthMessage : IMessage
    {
        public string[] Data { get; set; }

        public RemoveHealthMessage(int entityId, int health)
        {
            Data = new string[2];
            SetEntityId(entityId);
            SetHealth(health);
        }

        public void SetEntityId(int entityId)
        {
            Data[0] = entityId.ToString();
        }

        public void SetHealth(int health)
        {
            Data[1] = health.ToString();
        }

        public int GetEntityId()
        {
            return Convert.ToInt32(Data[0]);
        }

        public int GetHealth()
        {
            return Convert.ToInt32(Data[1]);
        }
    }
}
