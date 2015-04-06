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
        /// <summary>
        /// Manages the response from the collision system.
        /// </summary>
        public CollisionResponseSystem()
        {
            ComponentTypeFilter = new TypeFilter()
                .AddFilter(typeof (VelocityComponent))
                .AddFilter(typeof (PowerUpComponent));

            PostOffice.Subscribe(this);
        }

        /// <summary>
        /// Called when the PostOffice receives a collision response message.
        /// </summary>
        /// <param name="message"></param>
        public void OnMessage(IMessage message)
        {
            CollisionResponseMessage collisionMessage = (CollisionResponseMessage) message;

            BaseEntity entity = World.GetInstance().GetEntity(collisionMessage.GetEntityId());
            BaseEntity targetEntity = World.GetInstance().GetEntity(collisionMessage.GetTargetEntityId());

            CollisionFace face = collisionMessage.GetCollisionFace();

            /**
             * Manages the power ups that collided with the paddle.
             */
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
            /**
             * Manages the rebound bounce of the ball.
             */
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
            /**
             * Removes one health from the block when it collides with another entity.
             */
            if (entity is Block)
            {
                PostOffice.SendMessage(new RemoveHealthMessage(entity.Id, 1));
            }

            /**
             * Removes the laser when it collides with something.
             */
            if (entity is Laser)
            {
                World.GetInstance().RemoveEntity(entity.Id);
            }
            
        }

        /// <summary>
        /// Returns a filter of all the messages this systems wants to receive.
        /// </summary>
        /// <returns></returns>
        public TypeFilter GetMessageTypeFilter()
        {
            return new TypeFilter()
                .AddFilter(typeof(CollisionResponseMessage));
        }
    }
}
