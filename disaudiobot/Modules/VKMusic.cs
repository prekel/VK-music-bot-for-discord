﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VkNet;
using VkNet.AudioBypassService;
using VkNet.AudioBypassService.Extensions;
using VkNet.Utils;
using VkNet.Model.Attachments;
using System.IO;
using Discord.Commands;
using System.Runtime.Serialization.Formatters.Binary;
using Discord;

namespace disaudiobot.Modules
{
    class VKMusic
    {
        public static async Task Join(string login, string password)
        {
            var services = new ServiceCollection();
            services.AddAudioBypass();
            var api = new VkApi(services);
            await api.AuthorizeAsync(new VkNet.Model.ApiAuthParams
            {
                Login = login,
                Password = password
            });
            Program._vkapi = api;
            Console.WriteLine(new LogMessage(LogSeverity.Verbose,"VK.net","Joined"));
        }


        public static async Task GetSongs(VkApi api, int ownerid, ulong guildid)
        {
            VkCollection<Audio> audios = null;
            try
            {
                audios = await api.Audio.GetAsync(new VkNet.Model.RequestParams.AudioGetParams { OwnerId = ownerid });
            }
            catch (Exception)
            {
                Console.WriteLine(new LogMessage(LogSeverity.Error,"Vk.net","Cant get audio(Token confirmation)"));
                return;
            }

            Audio[] audio = new Audio[audios.Count];
            for (int i = 0; i < audio.Length; ++i)
            {
                audio[i] = audios[i];
            }

            BinaryFormatter formatter = new BinaryFormatter();

            string uri = $@"{Directory.GetCurrentDirectory()}\servers\{guildid}\{ownerid}.dat";

            using (FileStream fs = new FileStream(uri, FileMode.OpenOrCreate, FileAccess.Write))
            {
                formatter.Serialize(fs, audio);
            }

            await Task.CompletedTask;
        }

        public static async Task DownloadSongs(Audio Song, string name)
        {
            if (Song.Url == null || name == null)
            {
                if (Song.Url == null)
                    throw new ArgumentException("Song url wasn't found");
                if (name == null)
                    throw new ArgumentException("Name is equal to null!");
            }


            using (WebClient client = new WebClient())
            {
                client.DownloadFile(Song.Url, name);
                Console.WriteLine(new LogMessage(LogSeverity.Verbose, "BOT", "Sound downloaded"));
                await Task.CompletedTask;
            }
        }

    }
}
