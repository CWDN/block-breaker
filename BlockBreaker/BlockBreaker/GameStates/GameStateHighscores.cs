using System.Collections.Generic;
using Atom;
using Atom.GameStates;
using Atom.GUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BlockBreaker.GameStates
{
    public class GameStateHighscores : GameState
    {
        private int ScreenWidth;
        private int ScreenHeight;
        private GuiButton _returnButton;
        private GuiScreen _guiScreen;

        public GameStateHighscores(string name)
            : base(name)
        {
            ScreenWidth = GameServices.Graphics.PreferredBackBufferWidth;
            ScreenHeight = GameServices.Graphics.PreferredBackBufferHeight;
        }

        public override void Initialize()
        {
            _returnButton = new GuiButton(new Vector2(0.05F, 0.05F), 0.1F, 0.09F) { BackColour = Color.Transparent, Anchor = Anchor.TopLeft };
            _guiScreen = new GuiScreen(new Point(0, 0), ScreenWidth, ScreenHeight);
        }

        public override void LoadContent(ContentManager contentManager)
        {
            List<int> highScores = HighscoreManager.GetInstance().GetHighScores();

            Texture2D backgroundTexture = contentManager.Load<Texture2D>("highScoresBackground");

            _guiScreen.Texture = backgroundTexture;

            Texture2D buttonTexture = contentManager.Load<Texture2D>("button");
            Texture2D buttonHoverTexture = contentManager.Load<Texture2D>("buttonHover");
            Texture2D buttonClickTexture = contentManager.Load<Texture2D>("buttonClick");

            SpriteFont font = contentManager.Load<SpriteFont>("font");

            _returnButton.Texture = buttonTexture;
            _returnButton.HoverTexture = buttonHoverTexture;
            _returnButton.ClickedTexture = buttonClickTexture;
            _returnButton.Font = font;
            _returnButton.FontColour = Color.White;
            _returnButton.Text = "Return";
            _returnButton.FontScale = 0.7F;
            _returnButton.OnClickHandler += (sender, args) => GameServices.GetService<GameStateManager>().SwapState("MainMenu");

            float posX = -0.4F;
            float posY = -0.5F;

            for (int i = 0; i < 10; i++)
            {
                GuiLabel guiLabel = new GuiLabel(new Vector2(posX, posY), 0.1F, 0.1F) { BackColour = Color.Transparent, Anchor = Anchor.Middle };
                posY += 0.2F;

                guiLabel.Text = (i + 1) + ": " + (highScores.Count > i ? highScores[i].ToString() : "?");
                guiLabel.Font = font;
                guiLabel.FontColour = Color.White;
                guiLabel.FontScale = 1.6F;

                if (i == 4)
                {
                    posX = 0.4F;
                    posY = -0.5F;
                }

                _guiScreen.AddGui(guiLabel);
            }

            
            _guiScreen.AddGui(_returnButton);
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
