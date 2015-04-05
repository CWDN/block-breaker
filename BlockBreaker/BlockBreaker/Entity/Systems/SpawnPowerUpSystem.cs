using System;
using System.Collections.Generic;
using Atom;
using Atom.Entity;
using Atom.Graphics.Rendering;
using Atom.Messaging;
using Atom.Physics;
using Atom.World;
using BlockBreaker.Entity.Components;
using BlockBreaker.Entity.Messages;
using Microsoft.Xna.Framework;

namespace BlockBreaker.Entity.Systems
{
    public class SpawnPowerUpSystem : BaseSystem, IReceiver
    {

        public SpawnPowerUpSystem()
        {
            ComponentTypeFilter = new TypeFilter()
                .AddFilter(typeof(SpriteComponent))
                .AddFilter(typeof(PositionComponent))
                .AddFilter(typeof(PowerUpComponent));

            PostOffice.Subscribe(this);
        }

        public void OnMessage(IMessage message)
        {
            if (message is SpawnPowerUpMessage)
            {
                SpawnPowerUpMessage spawnPowerUpMessage = message as SpawnPowerUpMessage;

                Random random = new Random();

                int powerUpChoice = random.Next(0, 0);

                if (powerUpChoice > 2) return;

                PowerUp powerUp = EntityFactory.GetInstance().Construct<PowerUp>();

                List<Component> components = powerUp.GetDefaultComponents();

                PowerUps power = (PowerUps) powerUpChoice;

                SpriteComponent spriteComponent =
                    (SpriteComponent) components.Find(component => component.GetType() == typeof (SpriteComponent));

                PositionComponent positionComponent =
                    (PositionComponent)components.Find(component => component.GetType() == typeof(PositionComponent));

                spriteComponent.Location =  new Point(powerUpChoice * spriteComponent.FrameWidth, spriteComponent.Location.Y);

                PowerUpComponent powerUpComponent = new PowerUpComponent
                {
                    EntityId = spriteComponent.EntityId,
                    PowerUp = power
                };

                positionComponent.Position = spawnPowerUpMessage.GetXAndY();

                components.Add(powerUpComponent);

                World.GetInstance().AddEntity(powerUp, components);
            }
        }

        public TypeFilter GetMessageTypeFilter()
        {
            return new TypeFilter()
                .AddFilter(typeof (SpawnPowerUpMessage));
        }
    }
}
