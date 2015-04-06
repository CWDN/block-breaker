using Atom;
using Atom.Messaging;
using BlockBreaker.Entity.Messages;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlockBreaker.Entity.Systems
{
    public class ScoreSystem : BaseSystem, IReceiver
    {
        /// <summary>
        /// Stores the current score of the player.
        /// </summary>
        public static int Score { get; set; }

        private SpriteFont _font;

        /// <summary>
        /// Manages the scores of the game
        /// </summary>
        public ScoreSystem()
        {
            ComponentTypeFilter = new TypeFilter();
            _font = GameServices.Content.Load<SpriteFont>("font");
            PostOffice.Subscribe(this);
        }

        /// <summary>
        /// Draws the current score on screen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="entityId"></param>
        public override void Draw(SpriteBatch spriteBatch, int entityId)
        {
            string text = "Score: " + Score;

            float textWidth = _font.MeasureString(text).X;

            spriteBatch.DrawString(_font, text, new Vector2(GameServices.Graphics.PreferredBackBufferWidth - textWidth - 10, 5), Color.White, 0, Vector2.Zero, 1F, SpriteEffects.None, 0.01F);    
        }

        /// <summary>
        /// Updates the score when the score message is receieved.
        /// </summary>
        /// <param name="message"></param>
        public void OnMessage(IMessage message)
        {
            ScoreMessage scoreMessage = message as ScoreMessage;

            Score += scoreMessage.GetScore();
        }

        /// <summary>
        /// Returns the types of messages this system wants to receive.
        /// </summary>
        /// <returns></returns>
        public TypeFilter GetMessageTypeFilter()
        {
            return new TypeFilter()
                .AddFilter(typeof (ScoreMessage));
        }
    }
}
