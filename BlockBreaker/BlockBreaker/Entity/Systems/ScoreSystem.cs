using Atom;
using Atom.Messaging;
using BlockBreaker.Entity.Messages;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlockBreaker.Entity.Systems
{
    public class ScoreSystem : BaseSystem, IReceiver
    {
        public static int Score { get; set; }

        private SpriteFont _font;

        public ScoreSystem()
        {
            ComponentTypeFilter = new TypeFilter();
            _font = GameServices.Content.Load<SpriteFont>("font");
            PostOffice.Subscribe(this);
        }

        public override void Draw(SpriteBatch spriteBatch, int entityId)
        {
            string text = "Score: " + Score;

            float textWidth = _font.MeasureString(text).X;

            spriteBatch.DrawString(_font, text, new Vector2(GameServices.Graphics.PreferredBackBufferWidth - textWidth - 10, 5), Color.White, 0, Vector2.Zero, 1F, SpriteEffects.None, 0.01F);    
        }

        public void OnMessage(IMessage message)
        {
            ScoreMessage scoreMessage = message as ScoreMessage;

            Score += scoreMessage.GetScore();
        }

        public TypeFilter GetMessageTypeFilter()
        {
            return new TypeFilter()
                .AddFilter(typeof (ScoreMessage));
        }
    }
}
