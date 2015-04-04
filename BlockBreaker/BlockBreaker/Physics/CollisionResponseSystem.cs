using System.Linq;
using Atom;
using Atom.Entity;
using Atom.Messaging;
using Atom.Physics;
using Atom.Physics.Collision;
using Atom.Physics.Collision.BoundingBox;
using Atom.World;
using BlockBreaker.Entity;
using BlockBreaker.Entity.Messages;
using Microsoft.Xna.Framework;

namespace BlockBreaker.Physics
{
    public class CollisionResponseSystem : BaseSystem, IReceiver
    {
        public CollisionResponseSystem()
        {
            ComponentTypeFilter = new TypeFilter()
                .AddFilter(typeof (VelocityComponent));

            PostOffice.Subscribe(this);
        }

        public void OnMessage(IMessage message)
        {
            CollisionResponseMessage collisionMessage = (CollisionResponseMessage) message;

            BaseEntity entity = World.GetInstance().GetEntity(collisionMessage.GetEntityId());

            CollisionFace face = collisionMessage.GetCollisionFace();

            if (entity is Paddle)
            {
                VelocityComponent velocityComponent = GetComponentsByEntityId<VelocityComponent>(entity.Id).First();
                velocityComponent.Velocity = Vector2.Zero;
            }

            if (entity is Ball)
            {
                VelocityComponent velocityComponent = GetComponentsByEntityId<VelocityComponent>(entity.Id).First();
                if (face == CollisionFace.Top || face == CollisionFace.Bottom)
                {
                    velocityComponent.X *= 1;
                    velocityComponent.Y *= -1;
                }
                else if (face == CollisionFace.Right || face == CollisionFace.Left)
                {
                    velocityComponent.X *= -1.01F;
                }
            }

            if (entity is Block)
            {
                PostOffice.SendMessage(new RemoveHealthMessage(entity.Id, 1));
            }
            
        }

        public TypeFilter GetMessageTypeFilter()
        {
            return new TypeFilter()
                .AddFilter(typeof(CollisionResponseMessage));
        }
    }
}
