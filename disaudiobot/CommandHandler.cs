using System;
using System.Reflection;
using System.Threading.Tasks;

using disaudiobot.Modules;

using Discord.Commands;
using Discord.WebSocket;

namespace disaudiobot
{
    internal class CommandHandler
    {
        private Config _cfg;
        private DiscordSocketClient _client;
        private CommandService _services;

        public async Task InitializeAsync(DiscordSocketClient client, Config cfg)
        {
            _client = client;
            _cfg = cfg;
            _services = new CommandService();
            await _services.AddModulesAsync(Assembly.GetEntryAssembly(), null);
            _client.MessageReceived += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketMessage s)
        {
            var msg = s as SocketUserMessage;
            if (msg == null)
            {
                return;
            }

            var context = new SocketCommandContext(_client, msg);
            var argPos = 0;
            if (msg.HasCharPrefix(_cfg.Prefix, ref argPos) || msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                var result = await _services.ExecuteAsync(context, argPos, null);
                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                {
                    Console.WriteLine(result.ErrorReason);
                }
            }
        }
    }
}
