using System;
using Atom;
using Atom.Messaging;
using BlockBreaker.Entity.Messages;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace BlockBreaker.Audio
{
    public class AudioSystem : BaseSystem, IReceiver
    {
        private bool _backgroundMusicPlaying;

        public AudioSystem()
        {
            ComponentTypeFilter = new TypeFilter();
            PostOffice.Subscribe(this);
        }

        public override void Update(GameTime gameTime, int entityId)
        {
            if (_backgroundMusicPlaying) return;

            PostOffice.SendMessage(new AudioMessage("backgroundMusic", AudioTypes.Song));
        }

        public void OnMessage(IMessage message)
        {
            AudioMessage audioMessage = message as AudioMessage;

            switch (audioMessage.GetAudioType())
            {
                case AudioTypes.SoundEffect:
                    SoundEffect soundEffect =
                        GameServices.Content.Load<SoundEffect>(@"Sounds\" + audioMessage.GetAudio());
                    soundEffect.Play(0.2F, 0F, 0F);
                    break;
                case AudioTypes.Song:
                    Song song = GameServices.Content.Load<Song>(@"Sounds\" + audioMessage.GetAudio());
                    MediaPlayer.IsRepeating = true;
                    MediaPlayer.Volume = 0.1F;
                    MediaPlayer.Play(song);
                    _backgroundMusicPlaying = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public TypeFilter GetMessageTypeFilter()
        {
            return new TypeFilter()
                .AddFilter(typeof (AudioMessage));
        }
    }
}
