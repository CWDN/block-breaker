using System;
using System.Collections.Generic;
using System.Linq;
using Atom;
using Atom.Entity;
using Atom.Input;
using Atom.Messaging;
using Atom.Physics;
using Atom.Physics.Collision.BoundingBox;
using Atom.World;
using BlockBreaker.Entity;
using BlockBreaker.Entity.Components;
using BlockBreaker.Entity.Messages;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BlockBreaker.Input
{
    public class KeyboardInputSystem : BaseSystem
    {
        /// <summary>
        /// Stores the previous update keyboar state.
        /// </summary>
        private KeyboardState _previousKeyboardState;
        private double coolDownTime;

        /// <summary>
        /// Manages the keyboard input for the entities.
        /// </summary>
        public KeyboardInputSystem()
        {
            ComponentTypeFilter = new TypeFilter()
                .AddFilter(typeof (StandardKeyComponent))
                .AddFilter(typeof (VelocityComponent))
                .AddFilter(typeof (PowerUpComponent))
                .AddFilter(typeof (PositionComponent))
                .AddFilter(typeof (BoundingBoxComponent))
                .AddFilter(typeof (AmmoComponent));
        }

        /// <summary>
        /// Update function that checks what keys have been pressed.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="entityId"></param>
        public override void Update(GameTime gameTime, int entityId)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (coolDownTime > 0)
            {
                coolDownTime -= gameTime.ElapsedGameTime.TotalSeconds;
            }

            List<StandardKeyComponent> keyComponents = GetComponentsByEntityId<StandardKeyComponent>(entityId);

            foreach (StandardKeyComponent keyComponent in keyComponents)
            {
                if (keyboardState.IsKeyUp(keyComponent.Key)) continue;

                BaseEntity entity = World.GetInstance().GetEntity(entityId);

                StandardInputActions actions = keyComponent.Action;

                Random random = new Random();

                /**
                 * The keyboard inputs for the ball
                 */
                if (entity is Ball)
                {
                    switch (actions)
                    {
                        case StandardInputActions.Fire:
                            VelocityComponent velocityComponent = GetComponentsByEntityId<VelocityComponent>(entityId).FirstOrDefault();

                            if (velocityComponent == null) return;

                            if (velocityComponent != null && velocityComponent.Velocity == Vector2.Zero)
                            {
                                bool isMinus = random.Next(0, 1) == 1;

                                float velocityX = random.Next(5, 10);

                                velocityComponent.X = (isMinus) ? -velocityX : velocityX;
                                velocityComponent.Y = -(random.Next(5, 10));   
                            }
                            break;
                        case StandardInputActions.AltFire:
                            break;
                    }
                }

                /**
                 * Keyboard inputs for the paddle.
                 */
                if (entity is Paddle)
                {
                    PowerUpComponent powerUpComponent = GetComponentsByEntityId<PowerUpComponent>(entity.Id).FirstOrDefault();
                    PositionComponent positionComponent = GetComponentsByEntityId<PositionComponent>(entityId).FirstOrDefault();
                    BoundingBoxComponent boundingBoxComponent = GetComponentsByEntityId<BoundingBoxComponent>(entityId).FirstOrDefault();

                    if (powerUpComponent == null || positionComponent == null || boundingBoxComponent == null) return;

                    switch (powerUpComponent.PowerUp)
                    {
                        case PowerUps.Shooter:

                            if (actions == StandardInputActions.Fire)
                            {
                                AmmoComponent ammoComponent = GetComponentsByEntityId<AmmoComponent>(entity.Id).FirstOrDefault();

                                if (ammoComponent == null) return;

                                if (coolDownTime > 0)
                                {
                                    return;
                                }

                                coolDownTime = 5;

                                if (ammoComponent.AmmoCapacity >= 2)
                                {
                                    ammoComponent.AmmoCapacity -= 2;

                                    PostOffice.SendMessage(new SpawnLaserMessage((int) positionComponent.X,
                                        (int) (positionComponent.Y - 26)));

                                    PostOffice.SendMessage(
                                        new SpawnLaserMessage(
                                            (int) positionComponent.X + boundingBoxComponent.Width - 8,
                                            (int) (positionComponent.Y - 26)));
                                }
                            }

                            break;
                    }
                }
            }

            _previousKeyboardState = keyboardState;
        }
    }
}
