using System;
using Atom;
using Atom.Messaging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace BlockBreaker.Audio
{
    public class AudioSystem : BaseSystem, IReceiver
    {
        /// <summary>
        /// Whether the background music is playing.
        /// </summary>
        private bool _backgroundMusicPlaying;

        /// <summary>
        /// Plays soungs and sound effects when sent a AudioMessage via the PostOffice.
        /// </summary>
        public AudioSystem()
        {
            ComponentTypeFilter = new TypeFilter();
            PostOffice.Subscribe(this);
        }

        /// <summary>
        /// Update function
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="entityId">The entity to be updating.</param>
        public override void Update(GameTime gameTime, int entityId)
        {
            if (_backgroundMusicPlaying) return;

            PostOffice.SendMessage(new AudioMessage("backgroundMusic", AudioTypes.Song));
        }

        /// <summary>
        /// Called when a message is sent from the PostOffice.
        /// </summary>
        /// <param name="message"></param>
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

        /// <summary>
        /// Filters only the types of message this system wants to receive.
        /// </summary>
        /// <returns></returns>
        public TypeFilter GetMessageTypeFilter()
        {
            return new TypeFilter()
                .AddFilter(typeof (AudioMessage));
        }
    }
}
