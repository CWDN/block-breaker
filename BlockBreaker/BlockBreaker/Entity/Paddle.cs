using System.Collections.Generic;
using Atom;
using Atom.Entity;
using Atom.Graphics.Rendering;
using Atom.Input;
using Atom.Physics;
using Atom.Physics.Collision.BoundingBox;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BlockBreaker.Entity
{
    public class Paddle : BaseEntity
    {
        protected override List<Component> CreateDefaultComponents()
        {
            float middleOfScreen = (float)GameServices.Graphics.PreferredBackBufferWidth/2;
            int paddleHeight = 20;
            int paddleWidth = 274;

            List<Component> components = new List<Component>
            {
                new PositionComponent() {X = middleOfScreen - (float)paddleWidth / 2, Y = GameServices.Graphics.PreferredBackBufferHeight - 30 },
                new VelocityComponent(),
                new AccelerationComponent(),
                new MassComponent() {Mass = 0.1F},
                new StandardKeyComponent()
                {
                    Action = StandardInputActions.Left,
                    Key = Keys.Left
                },
                new StandardKeyComponent()
                {
                    Action = StandardInputActions.Right,
                    Key = Keys.Right
                },
                new StandardKeyComponent()
                {
                    Action = StandardInputActions.Left,
                    Key = Keys.A
                },
                new StandardKeyComponent()
                {
                    Action = StandardInputActions.Right,
                    Key = Keys.D
                },
                new StandardKeyComponent()
                {
                    Action = StandardInputActions.Fire,
                    Key = Keys.Space
                },
                new BoundingBoxComponent()
                {
                    Active = true,
                    Width = paddleWidth,
                    Height = paddleHeight,
                },
                new SpriteComponent()
                {
                    Image = Content.Load<Texture2D>("paddleSpritesheet"),
                    Location = new Point(0, 0),
                    FrameHeight = paddleHeight,
                    FrameWidth = paddleWidth,
                    Scale = 1,
                    LayerDepth = 1F
                }
            };

            return components;
        }
    }
}
