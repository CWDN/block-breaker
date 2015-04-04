using System.Collections.Generic;
using Atom;
using Atom.Entity;
using Atom.Graphics.Rendering.Static;
using Atom.Input;
using Atom.Physics;
using Atom.Physics.Collision.BoundingBox;
using Atom.Physics.Movement;
using Atom.World;
using BlockBreaker.Entity;
using BlockBreaker.Entity.Systems;
using BlockBreaker.Input;
using BlockBreaker.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlockBreaker
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class BlockBreaker : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        private Texture2D _background;
        private World _world;

        public BlockBreaker()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            _world = new World();

            EntityFactory.GetInstance().Register<Paddle>();
            EntityFactory.GetInstance().Register<Wall>();
            EntityFactory.GetInstance().Register<Ball>();
            EntityFactory.GetInstance().Register<Block>();

            GameServices.Initialize(Content, _graphics);

            _graphics.PreferredBackBufferHeight = 720;
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.ApplyChanges();


            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _background = Content.Load<Texture2D>("Background");

            Paddle paddle = EntityFactory.GetInstance().Construct<Paddle>();
            Wall leftWall = EntityFactory.GetInstance().Construct<Wall>();
            Wall rightWall = EntityFactory.GetInstance().Construct<Wall>();
            Wall topWall = EntityFactory.GetInstance().Construct<Wall>();
            Ball ball = EntityFactory.GetInstance().Construct<Ball>();
            Block block = EntityFactory.GetInstance().Construct<Block>();

            List<Component> rightWallComponents = rightWall.GetDefaultComponents();

            PositionComponent rightWallPositionComponent = (PositionComponent)rightWallComponents.Find(component =>
                component.GetType() == typeof (PositionComponent));

            BoundingBoxComponent rightWallBoundingBoxComponent = (BoundingBoxComponent)rightWallComponents.Find(component =>
                component.GetType() == typeof(BoundingBoxComponent));

            rightWallPositionComponent.X += rightWallBoundingBoxComponent.Width + _graphics.PreferredBackBufferWidth;

            List<Component> topWallComponents = topWall.GetDefaultComponents();

            PositionComponent topWallPositionComponent = (PositionComponent)topWallComponents.Find(component =>
                component.GetType() == typeof(PositionComponent));

            BoundingBoxComponent topWallBoundingBoxComponent = (BoundingBoxComponent)topWallComponents.Find(component =>
                component.GetType() == typeof(BoundingBoxComponent));

            topWallPositionComponent.X = 0;
            topWallBoundingBoxComponent.Height = topWallBoundingBoxComponent.Width;
            topWallPositionComponent.Y -= topWallBoundingBoxComponent.Height;
            topWallBoundingBoxComponent.Width = _graphics.PreferredBackBufferWidth;

            _world.AddSystem(new StaticRenderSystem());
            _world.AddSystem(new MovementSystem());
            _world.AddSystem(new BoundingBoxSystem());
            _world.AddSystem(new BoundingBoxCollisionResolveSystem());
            _world.AddSystem(new CollisionResponseSystem());
            _world.AddSystem(new StandardKeyboardSystem());
            _world.AddSystem(new KeyboardInputSystem());
            _world.AddSystem(new BallMovementSystem());
            _world.AddSystem(new HealthSystem());
            _world.AddSystem(new BlockHealthSystem());
            
            _world.AddEntity(ball, ball.GetDefaultComponents());
            _world.AddEntity(paddle, paddle.GetDefaultComponents());
            _world.AddEntity(leftWall, leftWall.GetDefaultComponents());
            _world.AddEntity(rightWall, rightWallComponents);
            _world.AddEntity(topWall, topWallComponents);
            _world.AddEntity(block, block.GetDefaultComponents());
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            _world.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            _spriteBatch.Draw(_background, new Rectangle(0,0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);


            _world.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
