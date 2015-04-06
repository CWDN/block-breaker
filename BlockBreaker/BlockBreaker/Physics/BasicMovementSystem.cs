using System.Linq;
using Atom;
using Atom.Entity;
using Atom.Physics;
using Atom.World;
using BlockBreaker.Entity;
using Microsoft.Xna.Framework;

namespace BlockBreaker.Physics
{
    public class BasicMovementSystem : BaseSystem
    {
        /// <summary>
        /// Manages the movement of the entities.
        /// This is a simple version of the movement system of Atom.
        /// </summary>
        public BasicMovementSystem()
        {
            ComponentTypeFilter = new TypeFilter()
                .AddFilter(typeof (VelocityComponent))
                .AddFilter(typeof (PositionComponent));
        }

        /// <summary>
        /// Updates the position of the ball and laser entities.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="entityId"></param>
        public override void Update(GameTime gameTime, int entityId)
        {
            BaseEntity entity = World.GetInstance().GetEntity(entityId);

            if (entity is Ball || entity is Laser)
            {
                VelocityComponent velocityComponent = GetComponentsByEntityId<VelocityComponent>(entityId).FirstOrDefault();
                PositionComponent positionComponent = GetComponentsByEntityId<PositionComponent>(entityId).FirstOrDefault();

                if (velocityComponent == null || positionComponent == null) return;

                positionComponent.Position += velocityComponent.Velocity;

            }
        }
    }
}
