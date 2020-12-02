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
            if (!_activeBots.ContainsKey(inputs.UserName) || _activeBots[inputs.UserName].Process.HasExited)
            {
                var activeBot = new ActiveBot {
                    Process = Process.Start(new ProcessStartInfo(fileName: BotExe, arguments: $"{inputs.MeetingId} {inputs.CaptionUrl} {inputs.MeetingPassword ?? ""}") {
                        UserName = inputs.UserName,
                        Password = new NetworkCredential("", inputs.Password).SecurePassword,
                        Domain = Environment.UserDomainName
                    }),
                    UserName = inputs.UserName
                };
                if (_activeBots.ContainsKey(inputs.UserName))
                {
                    _activeBots.Remove(inputs.UserName);
                }
                _activeBots.Add(inputs.UserName, activeBot);
            }
        }

        public static void DeleteBot(
            string username
            )
        {
            if (_activeBots.ContainsKey(username) )
            {
                var bot = _activeBots[username];
                if (!bot.Process.HasExited)
                {
                    bot.Process.Kill();
                }

                _activeBots.Remove(username);
            }
        }

        public static IList<string> GetActiveBots() => _activeBots.Where(x => !x.Value.Process.HasExited).Select(x => x.Key).ToList();

        private const string BotExe = "C:\\work\\zoom-c-sharp-wrapper\\bin\\zoom_sdk_demo.exe";
        private static readonly IDictionary<string, ActiveBot> _activeBots =  new Dictionary<string, ActiveBot>();
    }
}
