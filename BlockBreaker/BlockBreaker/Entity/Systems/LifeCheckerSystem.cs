using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using Atom;
using Atom.Entity;
using Atom.Messaging;
using Atom.Physics;
using Atom.World;
using BlockBreaker.Audio;
using BlockBreaker.Entity.Components;
using BlockBreaker.Entity.Messages;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlockBreaker.Entity.Systems
{
    public class LifeCheckerSystem : BaseSystem, IReceiver
    {
        public static bool Finished = false;

        private SpriteFont _font;
        private HealthComponent _healthComponent;

        public LifeCheckerSystem()
        {
            ComponentTypeFilter  = new TypeFilter()
                .AddFilter(typeof (PositionComponent))
                .AddFilter(typeof (HealthComponent));
        
            _font = GameServices.Content.Load<SpriteFont>("font");

            PostOffice.Subscribe(this);
        }

        public override void Update(GameTime gameTime, int entityId)
        {
            BaseEntity entity = World.GetInstance().GetEntity(entityId);

            Paddle paddle = World.GetInstance().GetEntitiesByType<Paddle>().FirstOrDefault();

            if (paddle == null) return;

            _healthComponent = GetComponentsByEntityId<HealthComponent>(paddle.Id).FirstOrDefault();

            if (entity is Ball)
            {
                PositionComponent positionComponent = GetComponentsByEntityId<PositionComponent>(entityId).FirstOrDefault();

                if (positionComponent.Y > GameServices.Graphics.PreferredBackBufferHeight)
                {
                    PostOffice.SendMessage(new RemoveHealthMessage(paddle.Id, 1));
                    PostOffice.SendMessage(new AudioMessage("lifeLost", AudioTypes.SoundEffect));

                    World.GetInstance().RemoveEntity(entityId);

                    List<Ball> balls = World.GetInstance().GetEntitiesByType<Ball>();

                    if (balls.Count <= 1)
                    {
                        Ball ball = EntityFactory.GetInstance().Construct<Ball>();
                        World.GetInstance().AddEntity(ball, ball.GetDefaultComponents());
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, int entityId)
        {
            if (_healthComponent != null)
            {
                spriteBatch.DrawString(_font, "Lives: " + _healthComponent.Health, new Vector2(10, 5), Color.White);    
            }

            base.Draw(spriteBatch, entityId);
        }

        public void OnMessage(IMessage message)
        {
            DeadEntityMessage deadEntityMessage = message as DeadEntityMessage;

            BaseEntity entity = World.GetInstance().GetEntity(deadEntityMessage.GetEntityId());

            if (entity is Paddle)
            {
                Finished = true;
                World.GetInstance().Paused = true;
                HighscoreManager.GetInstance().AddHighScore(ScoreSystem.Score);
            }
        }

        public TypeFilter GetMessageTypeFilter()
        {
            return new TypeFilter()
                .AddFilter(typeof (DeadEntityMessage));
        }
    }
}
