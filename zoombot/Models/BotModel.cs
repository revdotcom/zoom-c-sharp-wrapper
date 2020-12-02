using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using zoombot.Services;

namespace zoombot.Models
{
    public class BotModel
    {
        public IList<string> ActiveBots => BotLauncher.GetActiveBots();

        [Display(Name = "Meeting Id")]
        [Required(ErrorMessage = "Please enter a meeting id")]
        public string MeetingId { get; set; }

        [Display(Name = "Meeting Password")]
        public string MeetingPassword { get; set; }

        [Display(Name = "User")]
        [Required(ErrorMessage = "Please enter a Windows user")]
        public string UserName { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please enter a password")]
        public string Password { get; set; }

        [Display(Name = "Caption Url")]
        [Required(ErrorMessage = "Please enter a caption url")]
        public string CaptionUrl { get; set; }
    }
}
