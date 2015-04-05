using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using Atom;
using Atom.Entity;
using Atom.Graphics.Rendering;
using Atom.Messaging;
using Atom.Physics;
using Atom.World;
using BlockBreaker.Audio;
using BlockBreaker.Entity.Components;
using BlockBreaker.Entity.Messages;
using Microsoft.Xna.Framework;
using IMessage = Atom.Messaging.IMessage;

namespace BlockBreaker.Entity.Systems
{
    public class BlockHealthSystem : BaseSystem, IReceiver
    {
        public BlockHealthSystem()
        {
            ComponentTypeFilter = new TypeFilter()
                .AddFilter(typeof (HealthComponent))
                .AddFilter(typeof (SpriteComponent))
                .AddFilter(typeof (PositionComponent));

            PostOffice.Subscribe(this);
        }

        public void OnMessage(IMessage message)
        {
            if (message is RemoveHealthMessage)
            {
                RemoveHealthMessage removeMessage = message as RemoveHealthMessage;
                BaseEntity entity = World.GetInstance().GetEntity(removeMessage.GetEntityId());
                
                if (entity is Block)
                {
                    HealthComponent healthComponent = GetComponentsByEntityId<HealthComponent>(removeMessage.GetEntityId()).FirstOrDefault();
                    SpriteComponent spriteComponent = GetComponentsByEntityId<SpriteComponent>(removeMessage.GetEntityId()).FirstOrDefault();

                    if (healthComponent == null || spriteComponent == null) return;

                    if (healthComponent.Health == 0) return;

                    PostOffice.SendMessage(new AudioMessage("crack", AudioTypes.SoundEffect));

                    spriteComponent.Location = new Point(spriteComponent.FrameWidth * (healthComponent.MaxHealth -
                                                 healthComponent.Health), spriteComponent.Location.Y);
                }
            }

            if (message is DeadEntityMessage)
            {
                DeadEntityMessage deadEntityMessage = message as DeadEntityMessage;
                BaseEntity entity = World.GetInstance().GetEntity(deadEntityMessage.GetEntityId());

                if (entity is Block)
                {
                    PositionComponent positionComponent =
                        GetComponentsByEntityId<PositionComponent>(deadEntityMessage.GetEntityId()).FirstOrDefault();

                    HealthComponent healthComponent =
                        GetComponentsByEntityId<HealthComponent>(deadEntityMessage.GetEntityId()).FirstOrDefault();

                    if (healthComponent == null || positionComponent == null) return;

                    PostOffice.SendMessage(new AudioMessage("destroy", AudioTypes.SoundEffect));
                    PostOffice.SendMessage(new ScoreMessage(healthComponent.MaxHealth));
                    PostOffice.SendMessage(new SpawnPowerUpMessage(positionComponent.Position));

                    World.GetInstance().RemoveEntity(deadEntityMessage.GetEntityId());
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
