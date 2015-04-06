using System.Collections.Generic;
using System.Linq;
using Atom;
using Atom.Entity;
using Atom.Messaging;
using Atom.Physics;
using Atom.World;
using BlockBreaker.Audio;
using BlockBreaker.Entity.Components;
using BlockBreaker.Entity.Messages;
using BlockBreaker.Level;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlockBreaker.Entity.Systems
{
    public class EndGameSystem : BaseSystem, IReceiver
    {
        /// <summary>
        /// Stores the reason to why the game has ended.
        /// </summary>
        public static EndGameReason Finished = EndGameReason.None;

        /// <summary>
        /// The font to use to draw.
        /// </summary>
        private SpriteFont _font;

        /// <summary>
        /// The health component of the paddle.
        /// </summary>
        private HealthComponent _healthComponent;

        /// <summary>
        /// Processes the logic to look for end game situations.
        /// </summary>
        public EndGameSystem()
        {
            ComponentTypeFilter  = new TypeFilter()
                .AddFilter(typeof (PositionComponent))
                .AddFilter(typeof (HealthComponent));
        
            _font = GameServices.Content.Load<SpriteFont>("font");

            PostOffice.Subscribe(this);
        }

        /// <summary>
        /// The update for each entity for this system.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="entityId"></param>
        public override void Update(GameTime gameTime, int entityId)
        {
            BaseEntity entity = World.GetInstance().GetEntity(entityId);

            Paddle paddle = World.GetInstance().GetEntitiesByType<Paddle>().FirstOrDefault();

            if (paddle != null)
            {
                _healthComponent = GetComponentsByEntityId<HealthComponent>(paddle.Id).FirstOrDefault();

                /**
                 * Checks if the ball if below the bottom of the screen.
                 * If it is then a remove health message will be sent.
                 */
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

            /**
             * Checks if there are no blocks in the world.
             * If there are no blocks then it will see if there's another level.
             */
            List<Block> blocks = World.GetInstance().GetEntitiesByType<Block>();

            if (blocks.Count == 0)
            {
                if (World.GetInstance().IsPendingChanges) return;

                if (!GameServices.GetService<LevelBuilder>().LoadNextLevel(World.GetInstance()))
                {
                    EndGame(EndGameReason.AllLevelsBeat);
                }
                else
                {
                    List<Paddle> oldPaddles = World.GetInstance().GetEntitiesByType<Paddle>();
                    oldPaddles.ForEach(oldPaddle => World.GetInstance().RemoveEntity(oldPaddle.Id));
                    List<Ball> oldBalls = World.GetInstance().GetEntitiesByType<Ball>();
                    oldBalls.ForEach(ball => World.GetInstance().RemoveEntity(ball.Id));
                    List<PowerUp> powerUps = World.GetInstance().GetEntitiesByType<PowerUp>();
                    powerUps.ForEach(powerUp => World.GetInstance().RemoveEntity(powerUp.Id));

                    Paddle newPaddle = EntityFactory.GetInstance().Construct<Paddle>();
                    Ball newBall = EntityFactory.GetInstance().Construct<Ball>();

                    World.GetInstance().AddEntity(newPaddle, newPaddle.GetDefaultComponents());
                    World.GetInstance().AddEntity(newBall, newBall.GetDefaultComponents());
                }
            }
        }

        /// <summary>
        /// Draws the lives counter to the top left.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="entityId"></param>
        public override void Draw(SpriteBatch spriteBatch, int entityId)
        {
            if (_healthComponent != null)
            {
                spriteBatch.DrawString(_font, "Lives: " + _healthComponent.Health, new Vector2(10, 5), Color.White);    
            }

            base.Draw(spriteBatch, entityId);
        }

        /// <summary>
        /// Called when the PostOffice receieves a DeadEntityMessage.
        /// </summary>
        /// <param name="message"></param>
        public void OnMessage(IMessage message)
        {
            /**
             * If the dead entity is a paddle then it will call the end game.
             */
            DeadEntityMessage deadEntityMessage = message as DeadEntityMessage;

            BaseEntity entity = World.GetInstance().GetEntity(deadEntityMessage.GetEntityId());

            if (entity is Paddle)
            {
                EndGame(EndGameReason.RanOutOfLives);
            }
        }

        /// <summary>
        /// Helper function to end the game when called.
        /// </summary>
        /// <param name="reason"></param>
        private void EndGame(EndGameReason reason)
        {
            Finished = reason;
            World.GetInstance().Paused = true;
            HighscoreManager.GetInstance().AddHighScore(ScoreSystem.Score);
        }

        /// <summary>
        /// Returns the types of messages it wants to receieve.
        /// </summary>
        /// <returns></returns>
        public TypeFilter GetMessageTypeFilter()
        {
            return new TypeFilter()
                .AddFilter(typeof (DeadEntityMessage));
        }
    }
}
