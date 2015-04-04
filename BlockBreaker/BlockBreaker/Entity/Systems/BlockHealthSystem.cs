using System.Linq;
using Atom;
using Atom.Entity;
using Atom.Graphics.Rendering;
using Atom.Messaging;
using Atom.World;
using BlockBreaker.Entity.Components;
using BlockBreaker.Entity.Messages;
using Microsoft.Xna.Framework;

namespace BlockBreaker.Entity.Systems
{
    public class BlockHealthSystem : BaseSystem, IReceiver
    {
        public BlockHealthSystem()
        {
            ComponentTypeFilter = new TypeFilter()
                .AddFilter(typeof (HealthComponent))
                .AddFilter(typeof (SpriteComponent));

            PostOffice.Subscribe(this);
        }

        public void OnMessage(IMessage message)
        {
            if (message is RemoveHealthMessage)
            {
                RemoveHealthMessage removeMessage = message as RemoveHealthMessage;
                HealthComponent healthComponent = GetComponentsByEntityId<HealthComponent>(removeMessage.GetEntityId()).FirstOrDefault();
                SpriteComponent spriteComponent = GetComponentsByEntityId<SpriteComponent>(removeMessage.GetEntityId()).FirstOrDefault();

                if (healthComponent == null || spriteComponent == null) return;

                if (healthComponent.Health == 0) return;

                spriteComponent.Location = new Point(spriteComponent.FrameWidth*(healthComponent.MaxHealth -
                                             healthComponent.Health), spriteComponent.Location.Y);
            }

            if (message is DeadEntityMessage)
            {
                DeadEntityMessage deadEntityMessage = message as DeadEntityMessage;
                BaseEntity entity = World.GetInstance().GetEntity(deadEntityMessage.GetEntityId());

                if (entity is Block)
                {

                }
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
