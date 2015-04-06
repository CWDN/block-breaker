using System.Linq;
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
        /// <summary>
        /// Manages the health of the blocks.
        /// </summary>
        public BlockHealthSystem()
        {
            ComponentTypeFilter = new TypeFilter()
                .AddFilter(typeof (HealthComponent))
                .AddFilter(typeof (SpriteComponent))
                .AddFilter(typeof (PositionComponent));

            PostOffice.Subscribe(this);
        }

        /// <summary>
        /// Called when either a health change has happened or an entity has died.
        /// </summary>
        /// <param name="message"></param>
        public void OnMessage(IMessage message)
        {
            #region Remove Health Message

            if (message is RemoveHealthMessage)
            {
                RemoveHealthMessage removeMessage = message as RemoveHealthMessage;
                BaseEntity entity = World.GetInstance().GetEntity(removeMessage.GetEntityId());
                
                if (entity is Block)
                {
                    /**
                     * Checks the health of the block and if it's greater than 0 
                     * then it will update the sprite location and send an audio message.
                     */
                    HealthComponent healthComponent = GetComponentsByEntityId<HealthComponent>(removeMessage.GetEntityId()).FirstOrDefault();
                    SpriteComponent spriteComponent = GetComponentsByEntityId<SpriteComponent>(removeMessage.GetEntityId()).FirstOrDefault();

                    if (healthComponent == null || spriteComponent == null) return;

                    if (healthComponent.Health == 0) return;

                    PostOffice.SendMessage(new AudioMessage("crack", AudioTypes.SoundEffect));

                    spriteComponent.Location = new Point(spriteComponent.FrameWidth * (healthComponent.MaxHealth -
                                                 healthComponent.Health), spriteComponent.Location.Y);
                }
            }

            #endregion

            #region Dead Entity Message

            if (message is DeadEntityMessage)
            {
                DeadEntityMessage deadEntityMessage = message as DeadEntityMessage;
                BaseEntity entity = World.GetInstance().GetEntity(deadEntityMessage.GetEntityId());

                if (entity is Block)
                {
                    /**
                     * If the entity that is dead is a block, 
                     * then it will remove the block, add score and send a audio message
                     */
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

            #endregion
        }

        /// <summary>
        /// Sets the filter that this system will receieve.
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
