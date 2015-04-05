using System;
using System.Linq;
using Atom;
using Atom.Entity;
using Atom.Messaging;
using Atom.Physics;
using Atom.Physics.Collision;
using Atom.Physics.Collision.BoundingBox;
using Atom.World;
using BlockBreaker.Audio;
using BlockBreaker.Entity;
using BlockBreaker.Entity.Components;
using BlockBreaker.Entity.Messages;
using Microsoft.Xna.Framework;

namespace BlockBreaker.Physics
{
    public class CollisionResponseSystem : BaseSystem, IReceiver
    {
        public CollisionResponseSystem()
        {
            ComponentTypeFilter = new TypeFilter()
                .AddFilter(typeof (VelocityComponent))
                .AddFilter(typeof (PowerUpComponent));

            PostOffice.Subscribe(this);
        }

        public void OnMessage(IMessage message)
        {
            CollisionResponseMessage collisionMessage = (CollisionResponseMessage) message;

            BaseEntity entity = World.GetInstance().GetEntity(collisionMessage.GetEntityId());
            BaseEntity targetEntity = World.GetInstance().GetEntity(collisionMessage.GetTargetEntityId());

            CollisionFace face = collisionMessage.GetCollisionFace();

            if (entity is Paddle)
            {
                VelocityComponent velocityComponent = GetComponentsByEntityId<VelocityComponent>(entity.Id).FirstOrDefault();

                if (velocityComponent == null) return;

                velocityComponent.Velocity = Vector2.Zero;

                if (targetEntity is PowerUp)
                {
                    PowerUpComponent powerUpComponent = GetComponentsByEntityId<PowerUpComponent>(targetEntity.Id).FirstOrDefault();

                    if (powerUpComponent == null) return;

                    PostOffice.SendMessage(new PowerUpMessage(entity.Id, powerUpComponent.PowerUp));

                    World.GetInstance().RemoveEntity(targetEntity.Id);

                    PostOffice.SendMessage(new AudioMessage("pickUp", AudioTypes.SoundEffect));
                }
                
            }

            if (entity is Ball)
            {
                VelocityComponent velocityComponent = GetComponentsByEntityId<VelocityComponent>(entity.Id).FirstOrDefault();

                if (velocityComponent == null) return;

                if (face == CollisionFace.Top || face == CollisionFace.Bottom)
                {
                    velocityComponent.X *= 1;
                    velocityComponent.Y *= -1;
                }
                else if (face == CollisionFace.Right || face == CollisionFace.Left)
                {
                    velocityComponent.X *= -1.01F;
                }

                PostOffice.SendMessage(new AudioMessage("bounce", AudioTypes.SoundEffect));
            }

            if (entity is Block)
            {
                PostOffice.SendMessage(new RemoveHealthMessage(entity.Id, 1));
            }

            if (entity is Laser)
            {
                World.GetInstance().RemoveEntity(entity.Id);
            }
            
        }

        public TypeFilter GetMessageTypeFilter()
        {
            return new TypeFilter()
                .AddFilter(typeof(CollisionResponseMessage));
        }
    }
}
