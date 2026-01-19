using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace baixarMusicasYoutube
{
    internal class youTubeDownload
    {


        public class YouTubeDownloader
        {
            public static Task BaixarAsync(
                string url,
                string pastaDestino,
                Action<int> onProgress,
                Action<string> onStatus)
            {
                return Task.Run(() =>
                {
                    
                    var psi = new ProcessStartInfo
                    {
                        FileName = Path.Combine(
                            AppDomain.CurrentDomain.BaseDirectory,
                            "yt-dlp.exe"
                        ),
                        Arguments = $"-x --audio-format mp3 --audio-quality 0 --no-playlist " + $"-o \"{pastaDestino}\\%(title)s.%(ext)s\" " + $"\"{url}\"",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    };

                    using (var process = new Process())
                    {
                        process.StartInfo = psi;

                        process.OutputDataReceived += (s, e) =>
                        {
                            if (e.Data == null) return;

                            onStatus?.Invoke(e.Data);

                            var match = Regex.Match(e.Data, @"(\d{1,3}\.\d)%");
                            if (match.Success)
                            {
                                int progress = (int)double.Parse(
                                    match.Groups[1].Value.Replace(".", ",")
                                );
                                onProgress?.Invoke(progress);
                            }
                        };

                        process.Start();
                        process.BeginOutputReadLine();
                        process.WaitForExit();
                    }
                });
            }
        }
    }
}
