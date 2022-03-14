using System;
using System.Collections.Generic;
using System.Text;
using LibVLCSharp.Shared;
using System.Threading;
using System.IO;

namespace PotatoEngine.Audio
{
    public class AudioPlayer
    {
        public static void PlayFile(string path)
        {
            Core.Initialize();

            using var libVLC = new LibVLC(enableDebugLogs: true);
            using var media = new Media(libVLC, path);
            using var mp = new MediaPlayer(media);
            mp.Play();

        }
    }
}
