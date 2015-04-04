using System.Collections.Generic;
using Atom;
using Atom.Entity;
using Atom.Graphics.Rendering;
using Atom.Physics;
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
                    Active = false,
                    Width = 84,
                    Height = 84,
                },
                new SpriteComponent()
                {
                    Image = Content.Load<Texture2D>("powerUpSpritesheet"),
                    Location = new Point(0, 0),
                    FrameHeight = 84,
                    FrameWidth = 84,
                    Scale = 1F,
                    LayerDepth = 1F
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
