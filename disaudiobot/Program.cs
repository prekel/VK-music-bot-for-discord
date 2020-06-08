using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

using disaudiobot.Modules;

using Discord;
using Discord.WebSocket;

using VkNet;

namespace disaudiobot
{
    internal class Program
    {
        public static VkApi _vkApi;
        public static StorageData _data = new StorageData();
        private DiscordSocketClient _client;
        private CommandHandler _handler;

        private static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;

            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose
            });

            var jsonSerializer = new DataContractJsonSerializer(typeof(Config));
            using (var fs = new FileStream("config.json", FileMode.OpenOrCreate))
            {
                Utils._cfg = (Config) jsonSerializer.ReadObject(fs);
            }

            Utils._cfg.Color = new Color(Utils._cfg.ColorValue);

            await _client.LoginAsync(TokenType.Bot, Utils._cfg.Token);
            await _client.StartAsync();

            _client.Log += Log;
            _handler = new CommandHandler();

            await VKMusic.AuthAsync(Utils._cfg.Login, Utils._cfg.Password);
            await _handler.InitializeAsync(_client, Utils._cfg);

            await Task.Delay(-1);
        }


        private async Task Log(LogMessage message)
        {
            Console.WriteLine(message.ToString());
            await Task.CompletedTask;
        }
    }
}
