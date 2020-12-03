using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zoom_sdk_demo
{
    public static class JoinMeetingRequest
    {
        public static string MeetingId { get; set;  }

        public static string Password { get; set;  }

        public static string CaptionUrl { get; set; }

        public static bool IsBootup { get; set; }
    }
}
