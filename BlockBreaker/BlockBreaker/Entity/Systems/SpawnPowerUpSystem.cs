using Atom;
using Atom.Messaging;

namespace BlockBreaker.Entity.Systems
{
    public class SpawnPowerUpSystem : BaseSystem, IReceiver
    {
        public void OnMessage(IMessage message)
        {
            
        }

        public TypeFilter GetMessageTypeFilter()
        {
            return new TypeFilter()
                .AddFilter(typeof ());
        }
    }
}
