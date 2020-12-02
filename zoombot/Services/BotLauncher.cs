using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using zoombot.Models;

namespace zoombot.Services
{
    public static class BotLauncher
    {
        public static void LaunchBot(
            BotModel inputs
            )
        {
            var activeBot = new ActiveBot {
                Process = Process.Start(new ProcessStartInfo(fileName: BotExe, arguments: "") {

                }),
                UserName = inputs.UserName
            };
        }

        private const string BotExe = "";
        private static readonly IList<ActiveBot> _activeBots;
    }
}
