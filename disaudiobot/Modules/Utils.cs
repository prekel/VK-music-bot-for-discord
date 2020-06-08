using System;

namespace disaudiobot.Modules
{
    internal class Utils
    {
        public static Config _cfg;

        /// <summary>
        ///     Fixing an URL if its corrupted
        /// </summary>
        /// <param name="url">Corrupted URL</param>
        /// <returns></returns>
        public static Uri FixUrl(Uri url)
        {
            var uri = url.AbsoluteUri;

            uri = uri.Replace("/index.m3u8", ".mp3");

            var fi = 0;
            var li = 0;

            var count = 0;

            for (var i = 0; i < uri.Length; ++i)
            {
                if (uri[i] == '/')
                {
                    ++count;
                }

                if (count == 4 && fi == 0)
                {
                    fi = i;
                }

                if (count == 5)
                {
                    li = i;
                    break;
                }
            }

            uri = uri.Remove(fi, li - fi);
            return new Uri(uri);
        }
    }
}
