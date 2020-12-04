using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using zoombot.Models;

namespace zoombot.Services
{
    public static class BotLauncher
    {
        public static void LaunchBot(
            BotModel inputs
            )
        {
            if (!_activeBots.ContainsKey(inputs.Id) || _activeBots[inputs.Id].Process.HasExited)
            {
                var activeBot = new ActiveBot {
                    Process = Process.Start(new ProcessStartInfo(fileName: BotExe, arguments: $"{inputs.MeetingId} {inputs.CaptionUrl} {inputs.MeetingPassword ?? ""}")),
                    UserName = inputs.MeetingId
                };

                _activeBots[inputs.Id] = activeBot;
            }
        }

        public static void Bootup()
        {
            BootupBot = new ActiveBot {
                Process = Process.Start(new ProcessStartInfo(fileName: BotExe) {
                    UserName = "zoombot",
                    Password = new NetworkCredential("", "g7A>xPH-3/Q\"kv-!").SecurePassword,
                    Domain = Environment.UserDomainName,
                }),
                UserName = "zoombot"
            };
        }

        public static void DeleteBot(
            string id
            )
        {
            if (_activeBots.ContainsKey(id) )
            {
                var bot = _activeBots[id];
                if (!bot.Process.HasExited)
                {
                    bot.Process.Kill();
                }

                _activeBots.Remove(id);
            }
        }

        public static IList<string> GetActiveBots() => _activeBots.Where(x => !x.Value.Process.HasExited).Select(x => x.Key).ToList();

        public static ActiveBot BootupBot { get; set; }

        public static string BotExe { get; set; } = "zoom_sdk_demo.exe";

        private static readonly IDictionary<string, ActiveBot> _activeBots =  new Dictionary<string, ActiveBot>();
    }
}
