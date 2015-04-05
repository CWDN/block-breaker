using System.Linq;
using Atom;
using Atom.Messaging;
using Atom.World;
using BlockBreaker.Entity.Components;
using BlockBreaker.Entity.Messages;

namespace BlockBreaker.Entity.Systems
{
    public class HealthSystem : BaseSystem, IReceiver
    {
        public HealthSystem()
        {
            ComponentTypeFilter = new TypeFilter()
                .AddFilter(typeof (HealthComponent));

            PostOffice.Subscribe(this);
        }


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

        public TypeFilter GetMessageTypeFilter()
        {
            return new TypeFilter()
                .AddFilter(typeof (RemoveHealthMessage))
                .AddFilter(typeof (DeadEntityMessage));
        }
    }
}
