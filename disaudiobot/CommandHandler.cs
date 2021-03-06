﻿using disaudiobot.Modules;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace disaudiobot
{
    class CommandHandler
    {
        DiscordSocketClient _client;
        CommandService _services;
        Config _cfg;

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
            if (msg == null) return;
            var context = new SocketCommandContext(_client, msg);
            int argPos = 0;
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
