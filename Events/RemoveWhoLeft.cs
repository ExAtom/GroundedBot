﻿using System;
using Discord.WebSocket;
using GroundedBot.Json;

namespace GroundedBot.Events
{
    class RemoveWhoLeft
    {
        public static async void DoEvent(SocketGuildUser user)
        {
            try
            {
                var members = Members.PullData();
                members.RemoveAt(members.IndexOf(members.Find(x => x.ID == user.Id)));
                Members.PushData(members);
                await Program.Log("event", $"{user.Username}#{user.Discriminator} ({user.Id}) removed from the database.");
            }
            catch (Exception) { }
        }
    }
}
