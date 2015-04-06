using System;
using Atom.Messaging;

namespace BlockBreaker.Audio
{
    public class AudioMessage : IMessage
    {
        /// <summary>
        /// Stores all the data about the message
        /// </summary>
        public string[] Data { get; set; }

        /// <summary>
        /// Sends a message to systems saying which audio file to play.
        /// </summary>
        /// <param name="audio">The name of the audio file you want to play</param>
        /// <param name="type">The type of audio that will be played.</param>
        public AudioMessage(string audio, AudioTypes type)
        {
            Data = new string[2];
            SetAudio(audio, type);
        }

        /// <summary>
        /// Sets the data for the message.
        /// Called in the constructor.
        /// </summary>
        /// <param name="audio"></param>
        /// <param name="type"></param>
        public void SetAudio(string audio, AudioTypes type)
        {
            Data[0] = audio;
            Data[1] = type.ToString();
        }

        /// <summary>
        /// Returns the audio file name.
        /// </summary>
        /// <returns>string</returns>
        public string GetAudio()
        {
            return Data[0];
        }

        /// <summary>
        /// Returns the audio type.
        /// </summary>
        /// <returns>AudioTypes</returns>
        public AudioTypes GetAudioType()
        {
            AudioTypes type;
            Enum.TryParse(Data[1], out type);
            return type;
        }
    }
}
