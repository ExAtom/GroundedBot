﻿using System;
using System.Collections.Generic;
using GroundedBot.Json;
using Discord;
using Discord.WebSocket;
using Discord.Rest;

namespace GroundedBot.Commands
{
    class Ping
    {
        public static List<ulong> RequiredRoles =
            new List<ulong>(BaseConfig.GetConfig().Roles.PtanB);

        public static string[] Aliases =
        {
            "ping",
            "latency",
            "pong"
        };

        public static async void DoCommand(bool isResponse)
        {
            await Program.Log("command");

            var message = Recieved.Message;
            if (message.Content.Split().Length > 1)
                return;

            if (isResponse)
            {
                var latency = DateTime.Now - Recieved.PingTime;
                var oldMsg = await ((IMessageChannel)Program._client.GetChannel(Recieved.PongMessage.Channel.Id)).GetMessageAsync(Recieved.PongMessage.Id);
                await ((IUserMessage)oldMsg).ModifyAsync(m => m.Content = $"Pong! `{latency.TotalMilliseconds:f0}ms`");
            }
            else
            {
                var pongMessage = await message.Channel.SendMessageAsync($"Pinging...");
                Recieved.PongMessage = pongMessage;
                Recieved.PingTime = DateTime.Now;
            }
        }
    }
}
