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
            // Baixa o áudio de um vídeo do YouTube usando yt-dlp
            public static Task BaixarAsync(
                string url,
                string pastaDestino,
                Action<int> onProgress,
                Action<string> onStatus)
            {
                // Verifica se a pasta de destino existe, se não, cria
                return Task.Run(() =>
                {

                    var psi = new ProcessStartInfo // Configura o processo para executar o yt-dlp com os argumentos necessários
                    {
                        FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "yt-dlp.exe"), // Caminho para o executável do yt-dlp
                        Arguments = $"-x --audio-format mp3 --audio-quality 5 --no-playlist " + $"-o \"{pastaDestino}\\%(title)s.%(ext)s\" " + $"\"{url}\"", // Argumentos para extrair o áudio, definir a qualidade, evitar playlists e especificar o formato de saída
                        UseShellExecute = false, // Necessário para redirecionar a saída
                        RedirectStandardOutput = true, // Redireciona a saída padrão para capturar o progresso
                        RedirectStandardError = true, // Redireciona a saída de erro para capturar mensagens de erro
                        CreateNoWindow = true // Evita que uma janela de console seja exibida durante a execução
                    };

                    using (var process = new Process())
                    { // Inicia o processo e captura a saída para monitorar o progresso e status
                        process.StartInfo = psi; // Configura o processo com as informações definidas anteriormente

                        process.OutputDataReceived += (s, e) =>
                        { // Evento para capturar a saída do processo, onde e.Data contém a linha de saída atual
                            if (e.Data == null) return; // Verifica se a linha de saída é nula, o que indica o fim da saída

                            onStatus?.Invoke(e.Data); // Invoca o callback de status para atualizar a interface do usuário ou logar a mensagem atual

                            var match = Regex.Match(e.Data, @"(\d{1,3}\.\d)%"); // Expressão regular para extrair o progresso em porcentagem da linha de saída, procurando por um número seguido de um ponto e mais números, seguido de um símbolo de porcentagem
                            if (match.Success) // Verifica se a expressão regular encontrou um match, indicando que a linha de saída contém informações de progresso
                            {
                                int progress = (int)double.Parse(match.Groups[1].Value.Replace(".", ",")); // Converte a string capturada para um número inteiro representando o progresso, substituindo o ponto por vírgula para garantir a conversão correta em culturas que usam vírgula como separador decimal
                                onProgress?.Invoke(progress); // Invoca o callback de progresso para atualizar a interface do usuário ou logar o progresso atual
                            }
                        };

                        process.Start(); // Inicia o processo do yt-dlp com as configurações definidas
                        process.BeginOutputReadLine(); // Começa a leitura assíncrona da saída do processo para capturar o progresso e status em tempo real
                        process.WaitForExit(); // Aguarda o processo terminar antes de continuar, garantindo que o download seja concluído antes de prosseguir com outras operações
                    }
                });
            }
        }
    }
}
