﻿using System.Linq;
using Atom;
using Atom.Messaging;
using BlockBreaker.Entity.Components;
using BlockBreaker.Entity.Messages;

namespace BlockBreaker.Entity.Systems
{
    public class HealthSystem : BaseSystem, IReceiver
    {
        /// <summary>
        /// Manages the health of the entities.
        /// </summary>
        public HealthSystem()
        {
            ComponentTypeFilter = new TypeFilter()
                .AddFilter(typeof (HealthComponent));

            PostOffice.Subscribe(this);
        }

        /// <summary>
        /// When a message is receive it will remove the health set in the message.
        /// If the health reaches 0 then it will send a dead entity message.
        /// </summary>
        /// <param name="message"></param>
        public void OnMessage(IMessage message)
        {
            if (message is RemoveHealthMessage)
            {
                RemoveHealthMessage removeMessage = message as RemoveHealthMessage;
                HealthComponent healthComponent = GetComponentsByEntityId<HealthComponent>(removeMessage.GetEntityId()).FirstOrDefault();

                if (healthComponent == null) return;

                if (healthComponent.Health - removeMessage.GetHealth() > 0)
                {
                    healthComponent.Health -= removeMessage.GetHealth();
                }
                else
                {
                    healthComponent.Health = 0;
                    PostOffice.SendMessage(new DeadEntityMessage(healthComponent.EntityId));
                }
            }

            if (message is DeadEntityMessage)
            {
                DeadEntityMessage deadEntityMessage = message as DeadEntityMessage;
            }
        }

        /// <summary>
        /// Returns the types of messages this system wants to receive.
        /// </summary>
        /// <returns></returns>
        public TypeFilter GetMessageTypeFilter()
        {
            return new TypeFilter()
                .AddFilter(typeof (RemoveHealthMessage))
                .AddFilter(typeof (DeadEntityMessage));
        }
    }
}
