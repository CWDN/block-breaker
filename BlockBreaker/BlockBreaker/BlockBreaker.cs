using Atom;
using Atom.GameStates;
using BlockBreaker.GameStates;
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
        private GameStateManager _gameStateManager;

        private static BlockBreaker instance;

        public BlockBreaker()
        {
            _graphics = new GraphicsDeviceManager(this);
            _gameStateManager = new GameStateManager();
            Content.RootDirectory = "Content";
            instance = this;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.ApplyChanges();

            GameServices.Initialize(Content, _graphics);
            GameServices.AddService(_gameStateManager);

            GameStateInGame gameStateInGame = new GameStateInGame("InGame");
            GameStateMainMenu gameStateMainMenu = new GameStateMainMenu("MainMenu");
            GameStateHighscores gameStateHighscores = new GameStateHighscores("Highscores");
            _gameStateManager.Add(gameStateInGame);
            _gameStateManager.Add(gameStateMainMenu);
            _gameStateManager.Add(gameStateHighscores);

            IsMouseVisible = true;

            _gameStateManager.Initialize();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _gameStateManager.LoadContent(Content);

            _gameStateManager.SwapState("MainMenu");
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
            _gameStateManager.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            _gameStateManager.Draw(gameTime, _spriteBatch);

            base.Draw(gameTime);
        }

        public static BlockBreaker GetInstance()
        {
            return instance;
        }
    }
}
