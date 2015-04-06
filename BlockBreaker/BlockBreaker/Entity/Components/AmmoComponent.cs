using Atom;

namespace BlockBreaker.Entity.Components
{
    public class AmmoComponent : Component
    {
        /// <summary>
        /// The amount of ammo the entity has.
        /// </summary>
        public int AmmoCapacity { get; set; }
    }
}
