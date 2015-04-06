using Atom;

namespace BlockBreaker.Entity.Components
{
    public class PowerUpComponent : Component
    {
        /// <summary>
        /// The type of power up the entity has.
        /// </summary>
        public PowerUps PowerUp { get; set; }
    }
}
