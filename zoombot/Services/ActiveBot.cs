using System.Diagnostics;

namespace zoombot.Services
{
    public class ActiveBot
    {
        public Process Process { get; set; }

        /// <summary>
        /// Name of the user that launched this bot
        /// </summary>
        public string UserName { get; set; }
    }
}
