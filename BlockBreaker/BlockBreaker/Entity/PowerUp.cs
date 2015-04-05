using System.Collections.Generic;
using Atom;
using Atom.Entity;
using Atom.Graphics.Rendering;
using Atom.Physics;
using Atom.Physics.Collision;
using Atom.Physics.Collision.BoundingBox;
using Atom.Physics.Gravity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlockBreaker.Entity
{
    public class PowerUp : BaseEntity
    {
        protected override List<Component> CreateDefaultComponents()
        {
            List<Component> components = new List<Component>
            {
                new PositionComponent() {X = 0, Y = 0},
                new VelocityComponent(),
                new AccelerationComponent(),
                new MassComponent() {Mass = 1F},
                new BoundingBoxComponent()
                {
                    Active = true,
                    Width = 42,
                    Height = 42,
                },
                new SpriteComponent()
                {
                    Image = Content.Load<Texture2D>("powerUpSpritesheet"),
                    Location = new Point(0, 0),
                    FrameHeight = 84,
                    FrameWidth = 84,
                    Scale = 0.5F,
                    LayerDepth = 0.4F
                },
                new CollisionExclusionComponent()
                {
                    Exclusions = new TypeFilter()
                    .AddFilter(typeof (PowerUp))
                    .AddFilter(typeof (Ball))
                    .AddFilter(typeof (Block))
                },
                new GravityComponent()
                {
                    Gravity = 5
                }
            };

            return components;
        }
    }
}
