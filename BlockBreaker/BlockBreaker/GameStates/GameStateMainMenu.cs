using Atom;
using Atom.GameStates;
using Atom.GUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BlockBreaker.GameStates
{
    public class GameStateMainMenu : GameState
    {
        private int ScreenWidth;
        private int ScreenHeight;
        private GuiButton _playButton;
        private GuiButton _highScoreButton;
        private GuiButton _quitButton;
        private GuiScreen _guiScreen;

        public GameStateMainMenu(string name) : base(name)
        {
            ScreenWidth = GameServices.Graphics.PreferredBackBufferWidth;
            ScreenHeight = GameServices.Graphics.PreferredBackBufferHeight;
        }

        public override void Initialize()
        {
            _playButton = new GuiButton(new Vector2(0F, 0.2F), 0.3F, 0.2F) { BackColour = Color.Transparent, Anchor = Anchor.TopMiddle };
            _highScoreButton = new GuiButton(new Vector2(0F, 0.45F), 0.3F, 0.2F) { BackColour = Color.Transparent, Anchor = Anchor.TopMiddle };
            _quitButton = new GuiButton(new Vector2(0F, 0.7F), 0.3F, 0.2F) { BackColour = Color.Transparent, Anchor = Anchor.TopMiddle };
            _guiScreen = new GuiScreen(new Point(0, 0), ScreenWidth, ScreenHeight);
        }

        public override void LoadContent(ContentManager contentManager)
        {
            Texture2D backgroundTexture = contentManager.Load<Texture2D>("mainMenuBackground");

            _guiScreen.Texture = backgroundTexture;

            Texture2D buttonTexture = contentManager.Load<Texture2D>("button");
            Texture2D buttonHoverTexture = contentManager.Load<Texture2D>("buttonHover");
            Texture2D buttonClickTexture = contentManager.Load<Texture2D>("buttonClick");

            SpriteFont font = contentManager.Load<SpriteFont>("font");

            _playButton.Texture = buttonTexture;
            _highScoreButton.Texture = buttonTexture;
            _quitButton.Texture = buttonTexture;

            _playButton.HoverTexture = buttonHoverTexture;
            _highScoreButton.HoverTexture = buttonHoverTexture;
            _quitButton.HoverTexture = buttonHoverTexture;

            _playButton.ClickedTexture = buttonClickTexture;
            _highScoreButton.ClickedTexture = buttonClickTexture;
            _quitButton.ClickedTexture = buttonClickTexture;

            _playButton.Font = font;
            _highScoreButton.Font = font;
            _quitButton.Font = font;

            _playButton.FontColour = Color.White;
            _highScoreButton.FontColour = Color.White;
            _quitButton.FontColour = Color.White;

            _playButton.Text = "Play";
            _highScoreButton.Text = "High scores";
            _quitButton.Text = "Quit";

            _playButton.FontScale = 1.8F;
            _highScoreButton.FontScale = 1.3F;
            _quitButton.FontScale = 1.8F;

            _playButton.OnClickHandler += (sender, args) => GameServices.GetService<GameStateManager>().SwapState("InGame");
            _highScoreButton.OnClickHandler += (sender, args) =>
            {
                GameServices.GetService<GameStateManager>().Remove("Highscores");
                GameStateHighscores highscoresGamestate = new GameStateHighscores("Highscores");
                highscoresGamestate.Initialize();
                highscoresGamestate.LoadContent(contentManager);
                GameServices.GetService<GameStateManager>().Add(highscoresGamestate);
                GameServices.GetService<GameStateManager>().SwapState("Highscores");
            };
            _quitButton.OnClickHandler += (sender, args) => BlockBreaker.GetInstance().Exit();

            _guiScreen.AddGui(_playButton);
            _guiScreen.AddGui(_highScoreButton);
            _guiScreen.AddGui(_quitButton);
        }

        public override void Update(GameTime gameTime)
        {
            _guiScreen.Update();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            _guiScreen.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
