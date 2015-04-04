using System.Linq;
using Atom;
using Atom.Entity;
using Atom.Physics;
using Atom.World;
using BlockBreaker.Entity;
using Microsoft.Xna.Framework;

namespace BlockBreaker.Physics
{
    public class BallMovementSystem : BaseSystem
    {
        public BallMovementSystem()
        {
            ComponentTypeFilter = new TypeFilter()
                .AddFilter(typeof (VelocityComponent))
                .AddFilter(typeof (PositionComponent));
        }

        public override void Update(GameTime gameTime, int entityId)
        {
            BaseEntity entity = World.GetInstance().GetEntity(entityId);

            if (entity is Ball)
            {
                VelocityComponent velocityComponent = GetComponentsByEntityId<VelocityComponent>(entityId).First();
                PositionComponent positionComponent = GetComponentsByEntityId<PositionComponent>(entityId).First();

                positionComponent.Position += velocityComponent.Velocity;

            }
        }
    }
}
