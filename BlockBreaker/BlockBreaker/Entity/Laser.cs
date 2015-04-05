using System.Collections.Generic;
using Atom;
using Atom.Entity;
using Atom.Graphics.Rendering;
using Atom.Physics;
using Atom.Physics.Collision;
using Atom.Physics.Collision.BoundingBox;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlockBreaker.Entity
{
    public class Laser : BaseEntity
    {
        protected override List<Component> CreateDefaultComponents()
        {
            List<Component> components = new List<Component>
            {
                new PositionComponent
                {
                    Position = new Vector2(0,0)
                },
                new VelocityComponent(),
                new BoundingBoxComponent()
                {
                    Active = true,
                    Width = 8,
                    Height = 26,
                },
                new CollisionExclusionComponent()
                {
                    Exclusions = new TypeFilter()
                    .AddFilter(typeof (PowerUp))
                    .AddFilter(typeof (Laser))
                },
                new AnimatedSpriteComponent()
                {
                    Image = Content.Load<Texture2D>("laserSpritesheet"),
                    Location = new Point(0, 0),
                    FrameHeight = 26,
                    FrameWidth = 8,
                    Scale = 1F,
                    LayerDepth = 0.4F,
                    FrameCount = 3,
                    FramesPerSecond = 21,
                    SequenceStartFrame = 0
                }
            };

            return components;
        }
    }
}
