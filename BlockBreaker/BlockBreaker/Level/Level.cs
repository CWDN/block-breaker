using System.Collections.Generic;
using Atom;
using Atom.Entity;
using Atom.World;

namespace BlockBreaker.Level
{
    public class Level
    {
        public List<BaseEntity> Entities { get; set; }
        public List<List<Component>> Components { get; set; }

        /// <summary>
        /// A level that can be loaded into a world.
        /// </summary>
        public Level()
        {
            Entities =  new List<BaseEntity>();
            Components = new List<List<Component>>();
        }

        /// <summary>
        /// Loads the entities and components into the world.
        /// </summary>
        /// <param name="world"></param>
        public void LoadIntoWorld(World world)
        {
            for (int index = 0; index < Entities.Count; index++)
            {
                BaseEntity entity = Entities[index];
                List<Component> components = Components[index];

                World.GetInstance().AddEntity(entity, components);
            }
        }

    }
}
