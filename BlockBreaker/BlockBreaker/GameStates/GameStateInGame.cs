using System;
using System.Collections.Generic;
using Atom;
using Atom.Entity;
using Atom.GameStates;
using Atom.Graphics.Rendering.Animated;
using Atom.Graphics.Rendering.Static;
using Atom.GUI;
using Atom.Input;
using Atom.Messaging;
using Atom.Physics;
using Atom.Physics.Collision.BoundingBox;
using Atom.Physics.Gravity;
using Atom.Physics.Movement;
using Atom.World;
using BlockBreaker.Audio;
using BlockBreaker.Entity;
using BlockBreaker.Entity.Systems;
using BlockBreaker.Input;
using BlockBreaker.Level;
using BlockBreaker.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BlockBreaker.GameStates
{
    public class GameStateInGame : GameState
    {
        private Texture2D _background;
        private Texture2D _levelTexture;
        private LevelBuilder _levelBuilder;
        private World _world;
        private GraphicsDeviceManager _graphics;
        private int ScreenWidth;
        private int ScreenHeight;

        private GuiScreen _guiScreen;
        private GuiButton _returnButton;
        private GuiContainer _container;
        private GuiContainer _window;
        private GuiLabel _scoreTitle;
        private GuiLabel _scoreValue;
        private GuiLabel _highScoreTitle;
        private GuiLabel _highScoreValue;
        private GuiLabel _lostLabel;
        private SpriteFont _font;

        public GameStateInGame(string name) : base(name)
        {
            _graphics = GameServices.Graphics;
            ScreenWidth = GameServices.Graphics.PreferredBackBufferWidth;
            ScreenHeight = GameServices.Graphics.PreferredBackBufferHeight;
        }


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        public override void Initialize()
        {
            _world = new World();

            EntityFactory.GetInstance().Register<Paddle>();
            EntityFactory.GetInstance().Register<Wall>();
            EntityFactory.GetInstance().Register<Ball>();
            EntityFactory.GetInstance().Register<Block>();
            EntityFactory.GetInstance().Register<PowerUp>();
            EntityFactory.GetInstance().Register<Laser>();

            _returnButton = new GuiButton(new Vector2(0F, -0.1F), 0.3F, 0.2F) { BackColour = Color.Transparent, Anchor = Anchor.BottomMiddle };
            _container = new GuiContainer(new Vector2(0F, 0F), 1F, 1F) { BackColour = Color.Black, Opacity = 0.5F };
            _window = new GuiContainer(new Vector2(0F, 0F), 0.5F, 0.7F) { BackColour = Color.Transparent, Anchor = Anchor.Middle };
            _scoreTitle = new GuiLabel(new Vector2(0,0.1F), 0.1F, 0.1F) { BackColour = Color.Transparent, Anchor = Anchor.TopMiddle };
            _scoreValue = new GuiLabel(new Vector2(0,0.25F), 0.1F, 0.1F) { BackColour = Color.Transparent, Anchor = Anchor.TopMiddle };
            _highScoreTitle = new GuiLabel(new Vector2(0, 0.1F), 0.1F, 0.1F) { BackColour = Color.Transparent, Anchor = Anchor.Middle };
            _highScoreValue = new GuiLabel(new Vector2(0, 0.4F), 0.1F, 0.1F) { BackColour = Color.Transparent, Anchor = Anchor.Middle };
            _lostLabel = new GuiLabel(new Vector2(0, 0.01F), 0.1F, 0.1F) { BackColour = Color.Transparent, Anchor = Anchor.TopMiddle };
            _guiScreen = new GuiScreen(new Point(0, 0), ScreenWidth, ScreenHeight);

            _levelBuilder = new LevelBuilder();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        public override void LoadContent(ContentManager content)
        {
            _world = BuildWorld(content, _world);

            _font = content.Load<SpriteFont>("font");
            _returnButton.Font = _font;
            _returnButton.FontColour = Color.CornflowerBlue;
            _returnButton.Text = "Return";
            _returnButton.FontScale = 1.3F;
            _returnButton.OnClickHandler += (sender, args) =>
            {
                World.GetInstance().Destroy();
                GC.Collect();
                World world = new World();
                world = BuildWorld(content, world);
                _world = world;
                GameServices.GetService<GameStateManager>().SwapState("MainMenu");
            };

            _scoreTitle.Font = _font;
            _scoreTitle.FontColour = Color.White;
            _scoreTitle.Text = "Score";
            _scoreTitle.FontScale = 1.7F;

            _scoreValue.Font = _font;
            _scoreValue.FontColour = Color.White;
            _scoreValue.Text = ScoreSystem.Score.ToString();
            _scoreValue.FontScale = 1.4F;

            _highScoreTitle.Font = _font;
            _highScoreTitle.FontColour = Color.White;
            _highScoreTitle.Text = "High score";
            _highScoreTitle.FontScale = 1.7F;

            _highScoreValue.Font = _font;
            _highScoreValue.FontColour = Color.White;
            _highScoreValue.Text = "100";
            _highScoreValue.FontScale = 1.4F;

            _lostLabel.Font = _font;
            _lostLabel.FontColour = Color.Red;
            _lostLabel.Text = "You ran out of lives";
            _lostLabel.FontScale = 1.9F;

            Texture2D windowTexture = content.Load<Texture2D>("windowBackground");
            _window.Texture = windowTexture;

            _window.AddGui(_returnButton);
            _window.AddGui(_scoreValue);
            _window.AddGui(_scoreTitle);
            _window.AddGui(_highScoreTitle);
            _window.AddGui(_highScoreValue);
            _container.AddGui(_window);   
            _container.AddGui(_lostLabel);
            _guiScreen.AddGui(_container);
        }

        public World BuildWorld(ContentManager content, World world)
        {
            _levelBuilder = new LevelBuilder();
            PostOffice.NewInstance();
            _background = content.Load<Texture2D>("Background");
            _levelTexture = content.Load<Texture2D>("Level01");

            _levelBuilder.BuildFromTexture2D(_levelTexture);

            Paddle paddle = EntityFactory.GetInstance().Construct<Paddle>();
            Wall leftWall = EntityFactory.GetInstance().Construct<Wall>();
            Wall rightWall = EntityFactory.GetInstance().Construct<Wall>();
            Wall topWall = EntityFactory.GetInstance().Construct<Wall>();
            Ball ball = EntityFactory.GetInstance().Construct<Ball>();

            List<Component> rightWallComponents = rightWall.GetDefaultComponents();

            PositionComponent rightWallPositionComponent = (PositionComponent)rightWallComponents.Find(component =>
                component.GetType() == typeof(PositionComponent));

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

            world.AddSystem(new StaticRenderSystem());
            world.AddSystem(new AnimatedRenderSystem());
            world.AddSystem(new MovementSystem());
            world.AddSystem(new BoundingBoxSystem());
            world.AddSystem(new BoundingBoxCollisionResolveSystem());
            world.AddSystem(new CollisionResponseSystem());
            world.AddSystem(new StandardKeyboardSystem());
            world.AddSystem(new KeyboardInputSystem());
            world.AddSystem(new BasicMovementSystem());
            world.AddSystem(new HealthSystem());
            world.AddSystem(new BlockHealthSystem());
            world.AddSystem(new SpawnPowerUpSystem());
            world.AddSystem(new GravitySystem());
            world.AddSystem(new PowerUpSystem());
            world.AddSystem(new SpawnLaserSystem());
            world.AddSystem(new LifeCheckerSystem());
            world.AddSystem(new ScoreSystem());
            world.AddSystem(new AudioSystem());

            world.AddEntity(ball, ball.GetDefaultComponents());
            world.AddEntity(paddle, paddle.GetDefaultComponents());
            world.AddEntity(leftWall, leftWall.GetDefaultComponents());
            world.AddEntity(rightWall, rightWallComponents);
            world.AddEntity(topWall, topWallComponents);
            _levelBuilder.LoadIntoWorld(world);

            ScoreSystem.Score = 0;
            LifeCheckerSystem.Finished = false;

            return world;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            _world.Update(gameTime);

            if (LifeCheckerSystem.Finished)
            {
                _guiScreen.Update();
                _scoreValue.Text = ScoreSystem.Score.ToString();
                _highScoreValue.Text = HighscoreManager.GetInstance().GetHighestScore().ToString();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <param name="spriteBatch"></param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            spriteBatch.Draw(_background, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);

            _world.Draw(spriteBatch);

            spriteBatch.End();

            spriteBatch.Begin();

            if (LifeCheckerSystem.Finished)
            {
                _guiScreen.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime, spriteBatch);
        }
    }
}
