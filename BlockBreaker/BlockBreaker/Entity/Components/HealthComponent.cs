using Atom;

namespace BlockBreaker.Entity.Components
{
    public class HealthComponent : Component
    {
        /// <summary>
        /// The maximum amount of health the entity has.
        /// </summary>
        public int MaxHealth { get; set; }
        
        /// <summary>
        /// The current health of the entity.
        /// </summary>
        public int Health { get; set; }
    }
}
