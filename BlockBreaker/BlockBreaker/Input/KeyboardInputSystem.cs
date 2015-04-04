using System;
using System.Collections.Generic;
using System.Linq;
using Atom;
using Atom.Entity;
using Atom.Input;
using Atom.Physics;
using Atom.World;
using BlockBreaker.Entity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BlockBreaker.Input
{
    public class KeyboardInputSystem : BaseSystem
    {
        public KeyboardInputSystem()
        {
            ComponentTypeFilter = new TypeFilter()
                .AddFilter(typeof (StandardKeyComponent))
                .AddFilter(typeof (VelocityComponent));
        }

        public override void Update(GameTime gameTime, int entityId)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            List<StandardKeyComponent> keyComponents = GetComponentsByEntityId<StandardKeyComponent>(entityId);

            foreach (StandardKeyComponent keyComponent in keyComponents)
            {
                if (keyboardState.IsKeyUp(keyComponent.Key)) continue;

                BaseEntity entity = World.GetInstance().GetEntity(entityId);

                StandardInputActions actions = keyComponent.Action;

                Random random = new Random();

                if (entity is Ball)
                {
                    switch (actions)
                    {
                        case StandardInputActions.Fire:
                            VelocityComponent velocityComponent = GetComponentsByEntityId<VelocityComponent>(entityId).First();
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
            }
        }
    }
}
