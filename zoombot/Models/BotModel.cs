using System;
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

        [Display(Name = "Caption Url")]
        [Required(ErrorMessage = "Please enter a caption url")]
        public string CaptionUrl { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Please specify a name for the owner")]
        public string Name { get; set; }

        public string Id => $"{Name}-{MeetingId}";
    }
}
