using Atom;
using Atom.GameStates;
using BlockBreaker.GameStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlockBreaker
{
    /// <summary>
    /// The start of the Block breaker game.
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
        /// Initialises the game.
        /// </summary>
        protected override void Initialize()
        {
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.ApplyChanges();

            GameServices.Initialize(Content, _graphics);
            GameServices.AddService(_gameStateManager);

            // Initialises the 3 games tates and adds them to the game state manager.
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
        /// Loads the content of the game.
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _gameStateManager.LoadContent(Content);

            _gameStateManager.SwapState("MainMenu");
        }

        /// <summary>
        /// Updates the game state manager, which in turn which update which ever is the current game state.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            _gameStateManager.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the game
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            _gameStateManager.Draw(gameTime, _spriteBatch);

            base.Draw(gameTime);
        }

        /// <summary>
        /// Returns an instance of the Block Breaker game.
        /// </summary>
        /// <returns></returns>
        public static BlockBreaker GetInstance()
        {
            return instance;
        }
    }
}
