using System;
using System.Collections.Generic;
using System.Linq;
using Atom;
using Atom.Entity;
using Atom.Graphics.Rendering;
using Atom.Messaging;
using Atom.Physics;
using Atom.Physics.Collision.BoundingBox;
using Atom.World;
using BlockBreaker.Entity.Components;
using BlockBreaker.Entity.Messages;
using Microsoft.Xna.Framework;

namespace BlockBreaker.Entity.Systems
{
    public class PowerUpSystem : BaseSystem, IReceiver
    {
        /// <summary>
        /// Stores how long the longer paddle will last.
        /// </summary>
        private double _upgradeDuration;

        /// <summary>
        /// Manages the power ups that can be picked up.
        /// </summary>
        public PowerUpSystem()
        {
            ComponentTypeFilter = new TypeFilter()
                .AddFilter(typeof (PositionComponent))
                .AddFilter(typeof (SpriteComponent))
                .AddFilter(typeof (BoundingBoxComponent))
                .AddFilter(typeof (PowerUpComponent))
                .AddFilter(typeof (AmmoComponent));
            PostOffice.Subscribe(this);
        }

        /// <summary>
        /// Processes the update for the entities.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="entityId"></param>
        public override void Update(GameTime gameTime, int entityId)
        {
            BaseEntity entity = World.GetInstance().GetEntity(entityId);

            if (entity is Paddle)
            {
                PowerUpComponent powerUpComponent =
                    GetComponentsByEntityId<PowerUpComponent>(entityId).FirstOrDefault();

                if (powerUpComponent == null) return;

                /**
                 * If the larger paddle power up is set then reduce the upgrade duration until 0.
                 * The it will reset the paddle.
                 */
                if (powerUpComponent.PowerUp == PowerUps.LargerPaddle)
                {

                    if (_upgradeDuration > 0)
                    {
                        _upgradeDuration -= gameTime.ElapsedGameTime.TotalSeconds;
                    }
                    else
                    {
                        ResetPaddle(entityId);
                    }
                }
                /**
                 * If a shooter power up is applied then check to see if the ammo has been depleted.
                 * If so then the paddle will be reset.
                 */
                else if (powerUpComponent.PowerUp == PowerUps.Shooter)
                {
                    AmmoComponent ammoComponent = GetComponentsByEntityId<AmmoComponent>(entityId).FirstOrDefault();

                    if (ammoComponent == null) return;

                    if (ammoComponent.AmmoCapacity <= 0)
                    {
                        ResetPaddle(entityId);
                    }
                }
            }

        }

        /// <summary>
        /// Helper function to reset the paddle.
        /// Creates a new paddle and removes the old paddle.
        /// Uses the same Y position as the current paddle Y position.
        /// </summary>
        /// <param name="entityId"></param>
        public void ResetPaddle(int entityId)
        {
            PositionComponent positionComponent =
                        GetComponentsByEntityId<PositionComponent>(entityId).FirstOrDefault();

            Paddle paddle = EntityFactory.GetInstance().Construct<Paddle>();

            List<Component> components = paddle.GetDefaultComponents();

            PositionComponent originalPositionComponent =
                (PositionComponent)components.FindAll(component => component.GetType() == typeof (PositionComponent)).FirstOrDefault();

            positionComponent.Y = originalPositionComponent.Y;

            components.RemoveAll(component => component.GetType() == typeof(PositionComponent));

            positionComponent.EntityId = paddle.Id;

            components.Add(positionComponent);

            World.GetInstance().AddEntity(paddle, components);
            World.GetInstance().RemoveEntity(entityId);
        }

        /// <summary>
        /// Called when the Post Office receieve the power up message.
        /// </summary>
        /// <param name="message"></param>
        public void OnMessage(IMessage message)
        {
            PowerUpMessage powerUpMessage = message as PowerUpMessage;

            BaseEntity paddleEntity = World.GetInstance().GetEntity(powerUpMessage.GetPaddleId());

            SpriteComponent spriteComponent =
                GetComponentsByEntityId<SpriteComponent>(powerUpMessage.GetPaddleId()).FirstOrDefault();
            PositionComponent positionComponent =
                GetComponentsByEntityId<PositionComponent>(powerUpMessage.GetPaddleId()).FirstOrDefault();
            BoundingBoxComponent boundingBoxComponent =
                GetComponentsByEntityId<BoundingBoxComponent>(powerUpMessage.GetPaddleId()).FirstOrDefault();

            if (spriteComponent == null || positionComponent == null || boundingBoxComponent == null) return;

            PowerUpComponent powerUpComponent = GetComponentsByEntityId<PowerUpComponent>(powerUpMessage.GetPaddleId()).FirstOrDefault();

            // If the power up already exists then it will updated the current one.

            PowerUpComponent newPowerUpComponent = new PowerUpComponent
            {
                EntityId = paddleEntity.Id,
                PowerUp = powerUpMessage.GetPowerUp()
            };

            if (powerUpComponent == null)
            {
                World.GetInstance().AddEntityComponents(
                    powerUpMessage.GetPaddleId(),
                    new List<Component>
                    {
                        newPowerUpComponent
                    });
            }
            else
            {
                powerUpComponent.PowerUp = newPowerUpComponent.PowerUp;
            }

            Random random = new Random();

            /**
             * Gets the power up from the message.
             * Depending on the power up set then the paddle will get updated.
             */
            switch (powerUpMessage.GetPowerUp())
            {
                // Sets the shooter paddle.
                case PowerUps.Shooter:
                    spriteComponent.Location = new Point(spriteComponent.Location.X, 40);
                    spriteComponent.FrameWidth = 274;
                    spriteComponent.FrameHeight = 60;
                    boundingBoxComponent.RelativeX = 0;
                    boundingBoxComponent.RelativeY = 20;
                    boundingBoxComponent.Width = 274;
                    boundingBoxComponent.Height = 20;
                    positionComponent.Y = GameServices.Graphics.PreferredBackBufferHeight - 60;

                    AmmoComponent ammoComponent = GetComponentsByEntityId<AmmoComponent>(powerUpMessage.GetPaddleId()).FirstOrDefault();

                    AmmoComponent newAmmoComponent = new AmmoComponent
                    {
                        EntityId = paddleEntity.Id,
                        AmmoCapacity = 60
                    };

                    if (ammoComponent == null)
                    {
                        World.GetInstance().AddEntityComponents(paddleEntity.Id, new List<Component>
                        {
                            newAmmoComponent
                        });
                    }
                    else
                    {
                        ammoComponent = newAmmoComponent;
                    }
                    break;
                // Sets the larger paddle.
                case PowerUps.LargerPaddle:
                    // Offsets bigger paddle so it doesn't clip into the walls.
                    int newPositionX = Math.Abs(398 - boundingBoxComponent.Width);
                    positionComponent.X -= positionComponent.X - newPositionX < 0 ? 0 : newPositionX;
                    spriteComponent.Location = new Point(spriteComponent.Location.X, 20);
                    spriteComponent.FrameWidth = 398;
                    spriteComponent.FrameHeight = 20;
                    boundingBoxComponent.RelativeX = 0;
                    boundingBoxComponent.RelativeY = 0;
                    boundingBoxComponent.Width = 398;
                    boundingBoxComponent.Height = 20;
                    positionComponent.Y = GameServices.Graphics.PreferredBackBufferHeight - 30;
                    _upgradeDuration = 30;
                    break;
                // Adds two more balls to the world.
                case PowerUps.MultiBall:

                    Ball newBall = EntityFactory.GetInstance().Construct<Ball>();
                    Ball newBall2 = EntityFactory.GetInstance().Construct<Ball>();

                    List<Component> components = newBall.GetDefaultComponents();
                    List<Component> components2 = newBall2.GetDefaultComponents();

                    VelocityComponent velocityComponent =
                        (VelocityComponent) components.FirstOrDefault(component => component is VelocityComponent);
                    VelocityComponent velocityComponent2 =
                        (VelocityComponent)components2.FirstOrDefault(component => component is VelocityComponent);

                    bool isMinus = random.Next(0, 1) == 1;
                    float velocityX = random.Next(5, 10);
                    velocityComponent.X = (isMinus) ? -velocityX : velocityX;
                    velocityComponent.Y = -(random.Next(5, 10));   

                    isMinus = random.Next(0, 1) == 1;
                    velocityX = random.Next(5, 10);
                    velocityComponent2.X = (isMinus) ? -velocityX : velocityX;
                    velocityComponent2.Y = -(random.Next(5, 10));

                    World.GetInstance().AddEntity(newBall, components);
                    World.GetInstance().AddEntity(newBall2, components2);
                    break;
            }
        }

        /// <summary>
        /// Returns the types of the message this system wants to receieve.
        /// </summary>
        /// <returns></returns>
        public TypeFilter GetMessageTypeFilter()
        {
            return new TypeFilter()
                .AddFilter(typeof(PowerUpMessage));
        }
    }
}
