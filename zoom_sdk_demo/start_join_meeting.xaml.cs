using System;
using System.ComponentModel; // CancelEventArgs
using System.Windows;
using ZOOM_SDK_DOTNET_WRAP;

namespace zoom_sdk_demo
{
    /// <summary>
    /// Interaction logic for start_join_meeting.xaml
    /// </summary>
    public partial class start_join_meeting : Window
    {
        public start_join_meeting()
        {
            InitializeComponent();
        }

        //ZOOM_SDK_DOTNET_WRAP.onMeetingStatusChanged
        public void onMeetingStatusChanged(MeetingStatus status, int iResult)
        {
            switch(status)
            {
                case ZOOM_SDK_DOTNET_WRAP.MeetingStatus.MEETING_STATUS_ENDED:
                case ZOOM_SDK_DOTNET_WRAP.MeetingStatus.MEETING_STATUS_FAILED:
                    {
                        System.Windows.Application.Current.Shutdown();
                    }
                    break;
                default://todo
                    break;
            }
        }

        public void onUserJoin(Array lstUserID)
        {
            if (null == (Object)lstUserID)
                return;

            for (int i = lstUserID.GetLowerBound(0); i <= lstUserID.GetUpperBound(0); i++)
            {
                UInt32 userid = (UInt32)lstUserID.GetValue(i);
                ZOOM_SDK_DOTNET_WRAP.IUserInfoDotNetWrap user = ZOOM_SDK_DOTNET_WRAP.CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().
                    GetMeetingParticipantsController().GetUserByUserID(userid);
                if (null != (Object)user)
                {
                    string name = user.GetUserNameW();
                    Console.Write(name);
                    if (name == "Rev.ai Transcriber")
                    {
                        CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().GetMeetingAudioController()
                            .MuteAudio(userid, false);
                    }
                }
            }
        }
        public void onUserLeft(Array lstUserID)
        {
            //todo
        }
        public void onHostChangeNotification(UInt32 userId)
        {
            //todo
        }
        public void onLowOrRaiseHandStatusChanged(bool bLow, UInt32 userid)
        {
            //todo
        }
        public void onUserNameChanged(UInt32 userId, string userName)
        {
            //todo
        }
        private void RegisterCallBack()
        {
            ZOOM_SDK_DOTNET_WRAP.CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().Add_CB_onMeetingStatusChanged(onMeetingStatusChanged);
            ZOOM_SDK_DOTNET_WRAP.CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().
                GetMeetingParticipantsController().Add_CB_onHostChangeNotification(onHostChangeNotification);
            ZOOM_SDK_DOTNET_WRAP.CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().
                GetMeetingParticipantsController().Add_CB_onLowOrRaiseHandStatusChanged(onLowOrRaiseHandStatusChanged);
            ZOOM_SDK_DOTNET_WRAP.CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().
                GetMeetingParticipantsController().Add_CB_onUserJoin(onUserJoin);
            ZOOM_SDK_DOTNET_WRAP.CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().
                GetMeetingParticipantsController().Add_CB_onUserLeft(onUserLeft);
            ZOOM_SDK_DOTNET_WRAP.CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().
                GetMeetingParticipantsController().Add_CB_onUserNameChanged(onUserNameChanged);
        }

        private void button_join_api_Click(object sender, RoutedEventArgs e)
        {
            RegisterCallBack();
            ZOOM_SDK_DOTNET_WRAP.JoinParam4WithoutLogin join_api_param = new ZOOM_SDK_DOTNET_WRAP.JoinParam4WithoutLogin {
                meetingNumber = UInt64.Parse(JoinMeetingRequest.MeetingId),
                psw = String.IsNullOrWhiteSpace(JoinMeetingRequest.Password) ? null : JoinMeetingRequest.Password,
                userName = "Rev.ai Transcriber"
            };
            var captionUrl = JoinMeetingRequest.CaptionUrl;
            ZOOM_SDK_DOTNET_WRAP.JoinParam param = new ZOOM_SDK_DOTNET_WRAP.JoinParam {
                userType = ZOOM_SDK_DOTNET_WRAP.SDKUserType.SDK_UT_WITHOUT_LOGIN
            };
            param.withoutloginJoin = join_api_param;

            ZOOM_SDK_DOTNET_WRAP.SDKError err = ZOOM_SDK_DOTNET_WRAP.CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().Join(param);
            if (ZOOM_SDK_DOTNET_WRAP.SDKError.SDKERR_SUCCESS == err)
            {
                Hide();
                var audioDelegate = new AudioDelegate(captionUrl);
                var res = CZoomSDKeDotNetWrap.Instance.GetRawAudioHelper().subscribe(audioDelegate);
            }
            else//error handle
            {
            }
        }

        void Wnd_Closing(object sender, CancelEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
