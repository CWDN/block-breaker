using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private double _upgradeDuration;

        public PowerUpSystem()
        {
            ComponentTypeFilter = new TypeFilter()
                .AddFilter(typeof (PositionComponent))
                .AddFilter(typeof (SpriteComponent))
                .AddFilter(typeof (BoundingBoxComponent))
                .AddFilter(typeof (PowerUpComponent));
            PostOffice.Subscribe(this);
        }

        public override void Update(GameTime gameTime, int entityId)
        {
            if (_upgradeDuration > 0)
            {
                _upgradeDuration -= gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                BaseEntity entity = World.GetInstance().GetEntity(entityId);

                if (entity is Paddle)
                {
                    PowerUpComponent powerUpComponent =
                        GetComponentsByEntityId<PowerUpComponent>(entityId).FirstOrDefault();

                    if (powerUpComponent == null) return;

                    if (powerUpComponent.PowerUp == PowerUps.LargerPaddle)
                    {
                        PositionComponent positionComponent =
                            GetComponentsByEntityId<PositionComponent>(entityId).FirstOrDefault();

                        Paddle paddle = EntityFactory.GetInstance().Construct<Paddle>();

                        List<Component> components = paddle.GetDefaultComponents();

                        components.RemoveAll(component => component.GetType() ==  typeof(PositionComponent));

                        positionComponent.EntityId = paddle.Id;

                        components.Add(positionComponent);

                        World.GetInstance().AddEntity(paddle, components);
                        World.GetInstance().RemoveEntity(entityId);
                    }
                }
            }
        }

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

            PowerUpComponent powerUpComponent = new PowerUpComponent
            {
                EntityId = paddleEntity.Id,
                PowerUp = powerUpMessage.GetPowerUp()
            };

            World.GetInstance().AddEntityComponents(
                powerUpMessage.GetPaddleId(), 
                new List<Component>
                {
                    powerUpComponent
                }
            );

            Random random = new Random();

            switch (powerUpMessage.GetPowerUp())
            {
                case PowerUps.Shooter:
                    spriteComponent.Location = new Point(spriteComponent.Location.X, 40);
                    spriteComponent.FrameWidth = 274;
                    spriteComponent.FrameHeight = 60;
                    boundingBoxComponent.RelativeX = 0;
                    boundingBoxComponent.RelativeY = 20;
                    boundingBoxComponent.Width = 274;
                    boundingBoxComponent.Height = 20;
                    positionComponent.Y = GameServices.Graphics.PreferredBackBufferHeight - 60;
                    AmmoComponent ammoComponent = new AmmoComponent
                    {
                        AmmoCapacity = 60
                    };
                    World.GetInstance().AddEntityComponents(paddleEntity.Id, new List<Component>
                    {
                        ammoComponent
                    });

                    break;
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

        public TypeFilter GetMessageTypeFilter()
        {
            return new TypeFilter()
                .AddFilter(typeof(PowerUpMessage));
        }
    }
}
