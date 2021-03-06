﻿using System;
using System.Linq;
using System.Globalization;
using System.Threading.Tasks;
using GroundedBot.Json;

namespace GroundedBot.Events
{
    class PtanCheck
    {
        public async static Task DoEvent()
        {
            var members = Members.PullData();
            for (int i = 0; i < members.Count; i++)
            {
                DateTime date = DateTime.Now;
                try { date = DateTime.ParseExact(members[i].PPlusDate, "dd/MM/yyyy", CultureInfo.InvariantCulture); }
                catch (Exception) { }
                if (date < DateTime.Now && members[i].PPlusDate != "")
                {
                    members[i].PPlusDate = "";
                    var user = Program._client.GetGuild(830806387956514826).GetUser(members[i].ID);
                    foreach (var role in user.Roles)
                    {
                        if (!BaseConfig.GetConfig().Roles.Mod.Contains(role.Id))
                        {
                            if (BaseConfig.GetConfig().Roles.PtanB.Contains(role.Id))
                                await user.RemoveRoleAsync(role);
                            if (BaseConfig.GetConfig().Roles.PtanS.Contains(role.Id))
                                await user.RemoveRoleAsync(role);
                            if (BaseConfig.GetConfig().Roles.PtanP.Contains(role.Id))
                                await user.RemoveRoleAsync(role);
                        }
                    }
                    await Program.Log($"{user.Username}#{user.Discriminator}'s Ptan+ subscription has expired");
                    Members.PushData(members);
                }
            }
        }
    }
}
