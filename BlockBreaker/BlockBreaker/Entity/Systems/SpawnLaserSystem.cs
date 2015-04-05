using System.Collections.Generic;
using Atom;
using Atom.Entity;
using Atom.Messaging;
using Atom.Physics;
using Atom.World;
using BlockBreaker.Audio;
using BlockBreaker.Entity.Messages;

namespace BlockBreaker.Entity.Systems
{
    public class SpawnLaserSystem : BaseSystem, IReceiver
    {
        public SpawnLaserSystem()
        {
            ComponentTypeFilter = new TypeFilter()
                .AddFilter(typeof (PositionComponent))
                .AddFilter(typeof (VelocityComponent));

            PostOffice.Subscribe(this);
        }

        public void OnMessage(IMessage message)
        {
            SpawnLaserMessage spawnLaserMessage = message as SpawnLaserMessage;

            Laser laser = EntityFactory.GetInstance().Construct<Laser>();

            List<Component> components = laser.GetDefaultComponents();

            PositionComponent positionComponent =
                (PositionComponent) components.Find(component => component.GetType() == typeof (PositionComponent));

            VelocityComponent velocityComponent =
                (VelocityComponent)components.Find(component => component.GetType() == typeof(VelocityComponent));

            positionComponent.Position = spawnLaserMessage.GetXAndY();
            velocityComponent.Y = -3;

            PostOffice.SendMessage(new AudioMessage("laser", AudioTypes.SoundEffect));

            World.GetInstance().AddEntity(laser, components);

        }

        public TypeFilter GetMessageTypeFilter()
        {
            return new TypeFilter()
                .AddFilter(typeof (SpawnLaserMessage));
        }
    }
}
