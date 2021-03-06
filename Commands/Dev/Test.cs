﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Discord;
using Discord.WebSocket;
using GroundedBot.Json;

namespace GroundedBot.Commands
{
    class Test
    {
        public static List<ulong> AllowedRoles =
            new List<ulong>(BaseConfig.GetConfig().Roles.Admin);

        public static string[] Aliases =
        {
            "test",
            "teszt"
        };
        public static string Description = "A simple command to test stuff.";
        public static string[] Usages = { "test [made parameters]" };
        public static string Permission = "Only Devs can use it.";
        public static string Trello = "https://trello.com/c/FTc2lM9h/7-test";

        public async static Task DoCommand()
        {
            await Program.Log();

            var message = Recieved.Message;
            var members = Members.PullData();

            try
            {
                switch (message.Content.Split()[1].ToLower())
                {
                    case "helpfloppy": // Give out the floppies based on the amount of help
                        for (int i = 0; i < members.Count; i++)
                        {
                            members[i].Floppy += members[i].Help;
                            members[i].LastHelp = members[i].Help;
                            members[i].Help = 0;
                        }
                        Members.PushData(members);
                        await message.Channel.SendMessageAsync("Done.");
                        return;

                    case "searchhelps": // List of all the members who helped this month
                        foreach (var i in members.Where(x => x.Help > 0))
                            await message.Channel.SendMessageAsync($"{Program._client.GetUser(i.ID).Mention} - {i.Help}");
                        await message.Channel.SendMessageAsync("Done.", allowedMentions: AllowedMentions.None);
                        return;

                    case "removeduplicates": // Remove duplicated objects from the database
                        var fixedMembers = new List<Members>();
                        foreach (var i in members)
                            if (fixedMembers.Find(x => x.ID == i.ID) == null)
                                fixedMembers.Add(i);
                        Members.PushData(fixedMembers);
                        await message.Channel.SendMessageAsync("Done.");
                        return;

                    case "makestaffsbroke": // Removes every Floppy from every staff member that didn't come from ranking up.
                        foreach (var i in members.Where(x => Program._client.GetGuild(642864087088234506).GetUser(x.ID).Roles.Count(y => y.Id == 642864137960947755) > 0 || Program._client.GetGuild(642864087088234506).GetUser(x.ID).Roles.Count(y => y.Id == 727070093816758352) > 0))
                            members[members.IndexOf(members.Find(x => x.ID == i.ID))].Floppy = members[members.IndexOf(members.Find(x => x.ID == i.ID))].Rank;
                        Members.PushData(members);
                        await message.Channel.SendMessageAsync("Done.");
                        return;

                    case "removewholeft": // Removes everyone from the database who left the server.
                        var lefts = new List<Members>();
                        foreach (var i in members)
                            try
                            {
                                if (Program._client.GetUser(i.ID) == null)
                                    lefts.Add(i);
                            }
                            catch (Exception) { lefts.Add(i); }
                        foreach (var i in lefts)
                            members.Remove(i);
                        Members.PushData(members);
                        await message.Channel.SendMessageAsync("Done.");
                        return;

                    case "floppystats": // Shows some economy statistics.
                        int count = 0;
                        int sum = 0;
                        foreach (var i in members)
                            if (i.Floppy > 0)
                            {
                                count++;
                                sum += i.Floppy;
                            }
                        await message.Channel.SendMessageAsync($"Economy balance: **{sum}**\nAverage per user (who already has Floppy): **{sum / count}**\nAverage per user: **{sum / ((SocketGuildChannel)message.Channel).Guild.MemberCount}**");
                        return;

                    default:
                        await message.Channel.SendMessageAsync($"ping||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||||​||{message.Author.Mention}");
                        await message.Channel.SendMessageAsync($"{message.Author.Mention}", allowedMentions: AllowedMentions.None);
                        return;
                }
            }
            catch (Exception e) { await message.Channel.SendMessageAsync($"```{e.Message}```"); }
        }
    }
}
