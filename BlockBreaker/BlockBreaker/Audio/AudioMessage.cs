using System;
using Atom.Messaging;

namespace BlockBreaker.Audio
{
    public class AudioMessage : IMessage
    {
        public string[] Data { get; set; }

        public AudioMessage(string audio, AudioTypes type)
        {
            Data = new string[2];
            SetAudio(audio, type);
        }

        public void SetAudio(string audio, AudioTypes type)
        {
            Data[0] = audio;
            Data[1] = type.ToString();
        }

        public string GetAudio()
        {
            return Data[0];
        }

        public AudioTypes GetAudioType()
        {
            AudioTypes type;
            Enum.TryParse(Data[1], out type);
            return type;
        }
    }
}
