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
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BlockBreaker.Level
{
    public class LevelBuilder
    {
        /// <summary>
        /// All the levels that are loaded.
        /// </summary>
        protected List<Level> Levels = new List<Level>();
        protected int currentLevel;

        /// <summary>
        /// Builds levels from a texture.
        /// </summary>
        /// <param name="content"></param>
        public LevelBuilder(ContentManager content)
        {
            Texture2D levelTexture0 = content.Load<Texture2D>("Level00");
            BuildFromTexture2D(levelTexture0);

            Texture2D levelTexture1 = content.Load<Texture2D>("Level01");
            BuildFromTexture2D(levelTexture1);

            Texture2D levelTexture2 = content.Load<Texture2D>("Level02");
            BuildFromTexture2D(levelTexture2);
        }

        /// <summary>
        /// Builds a level object from a Texture2D and add it to the Levels.
        /// </summary>
        /// <param name="texture"></param>
        public void BuildFromTexture2D(Texture2D texture)
        {
            Level level = new Level();

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

                BuildBlock(level, startPosition + position, colour, health);
            }

            Levels.Add(level);
        }

        /// <summary>
        /// Builds a block entity from the given the parameters.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="position"></param>
        /// <param name="tint"></param>
        /// <param name="health"></param>
        private void BuildBlock(Level level, Vector2 position, Color tint, int health)
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

            level.Entities.Add(block);
            level.Components.Add(components);
        }

        /// <summary>
        /// Loads the next level in the list.
        /// If there are no more levels then it will return false.
        /// </summary>
        /// <param name="world"></param>
        /// <returns></returns>
        public bool LoadNextLevel(World world)
        {
            if (currentLevel >= Levels.Count) return false;
            Levels[currentLevel].LoadIntoWorld(world);
            currentLevel++;
            return true;
        }
    }
}
