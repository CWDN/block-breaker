using System.Collections.Generic;
using Atom;
using Atom.Entity;
using Atom.Graphics.Rendering;
using Atom.Input;
using Atom.Physics;
using Atom.Physics.Collision;
using Atom.Physics.Collision.BoundingBox;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BlockBreaker.Entity
{
    public class Ball : BaseEntity
    {
        protected override List<Component> CreateDefaultComponents()
        {
            float middleOfScreen = (float)GameServices.Graphics.PreferredBackBufferWidth / 2;
            int ballHeight = 100;
            int ballWidth = 100;

            float scale = 0.5F;

            List<Component> components = new List<Component>
            {
                new PositionComponent() {X = middleOfScreen - ballWidth * scale / 2, Y = GameServices.Graphics.PreferredBackBufferHeight - (ballHeight * scale) - 70 },
                new VelocityComponent(),
                new BoundingBoxComponent()
                {
                    Active = true,
                    Width = (int) (ballWidth * scale),
                    Height = (int) (ballHeight * scale),
                },
                new StandardKeyComponent()
                {
                    Action = StandardInputActions.Fire,
                    Key = Keys.Space
                },
                new SpriteComponent()
                {
                    Image = Content.Load<Texture2D>("ball"),
                    Location = new Point(0, 0),
                    FrameHeight = ballHeight,
                    FrameWidth = ballWidth,
                    Scale = scale,
                    LayerDepth = 0.5F
                },
                new CollisionExclusionComponent()
                {
                    Exclusions = new TypeFilter()
                    .AddFilter(typeof (PowerUp))
                }
            };

            return components;
        }
    }
}
