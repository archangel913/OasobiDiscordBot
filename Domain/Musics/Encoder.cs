using System.Diagnostics;

namespace Domain.Musics
{
    internal class Encoder
    {
        public static Process VideoToWave()
        {
#pragma warning disable CS8603 // Null 参照戻り値である可能性があります。
            return Process.Start(new ProcessStartInfo
            {
                FileName = @"ffmpeg",
                Arguments = $"-loglevel panic -i pipe:0 -vn -f s16le -ar 48000 -ac 2 pipe:1.wav",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardInput = true,
                CreateNoWindow = true
            });
#pragma warning restore CS8603 // Null 参照戻り値である可能性があります。
        }
    }
}
