using System;
using System.Collections.Generic;
using System.Linq;
using Atom;
using Atom.Entity;
using Atom.Graphics.Rendering;
using Atom.Physics;
using Atom.World;
using BlockBreaker.Entity;
using BlockBreaker.Entity.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlockBreaker.Level
{
    public class LevelBuilder
    {
        private List<BaseEntity> _entities = new List<BaseEntity>();
        private List<List<Component>> _components = new List<List<Component>>();

        public void BuildFromTexture2D(Texture2D texture)
        {
            float blockWidth = 73 * 0.75F;
            Color[] colourData = new Color[texture.Width * texture.Height];
            texture.GetData(colourData);

            int halfWindowWidth = GameServices.Graphics.PreferredBackBufferWidth/2;
            int halfLevelWidth = (int) (blockWidth*texture.Width) / 2;

            Vector2 startPosition = new Vector2(halfWindowWidth - halfLevelWidth, blockWidth * 2);

            Random random = new Random();

            for (int index = 0; index < colourData.Length; index++)
            {
                Color colour = colourData[index];

                int health = random.Next(3, 7);

                int positionX =
                (int) ((index % texture.Width) * blockWidth);
                int positionY =
                    (int) ((index / texture.Width) * blockWidth);

                Vector2 position = new Vector2(positionX, positionY);

                BuildBlock(startPosition + position, colour, health);
            }
        }

        private void BuildBlock(Vector2 position, Color tint, int health)
        {
            Block block = EntityFactory.GetInstance().Construct<Block>();

            List<Component> components = block.GetDefaultComponents();

            PositionComponent positionComponent =
                (PositionComponent) components.FindAll(component => component.GetType() == typeof (PositionComponent)).FirstOrDefault();
            positionComponent.Position = position;

            SpriteComponent spriteComponent =
                (SpriteComponent) components.FindAll(component => component.GetType() == typeof (SpriteComponent)).FirstOrDefault();
            spriteComponent.Tint = tint;

            HealthComponent healthComponent =
                (HealthComponent)
                    components.FindAll(component => component.GetType() == typeof (HealthComponent)).FirstOrDefault();
            healthComponent.Health = health;
            healthComponent.MaxHealth = health;

            _entities.Add(block);
            _components.Add(components);
        }

        public void LoadIntoWorld(World world)
        {
            for (int index = 0; index < _entities.Count; index++)
            {
                BaseEntity entity = _entities[index];
                List<Component> components = _components[index];

                World.GetInstance().AddEntity(entity, components);
            }
        }
    }
}
