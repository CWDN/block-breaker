using Atom;

namespace BlockBreaker.Entity.Components
{
    public class HealthComponent : Component
    {
        public int MaxHealth { get; set; }
        public int Health { get; set; }
    }
}
