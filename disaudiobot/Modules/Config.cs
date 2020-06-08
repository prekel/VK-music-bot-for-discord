using System.Runtime.Serialization;

using Discord;

namespace disaudiobot.Modules
{
    [DataContract]
    internal class Config
    {
        [IgnoreDataMember]
        public Color Color;

        [DataMember]
        public string Login { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string Token { get; set; }

        [DataMember]
        public char Prefix { get; set; }

        [DataMember]
        public int StarsCount { get; set; }

        [DataMember]
        public int GetPlaylistCount { get; set; }


        [DataMember]
        public uint ColorValue { get; set; }
    }
}
