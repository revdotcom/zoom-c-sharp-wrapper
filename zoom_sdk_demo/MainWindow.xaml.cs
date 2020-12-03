using System.ComponentModel; // CancelEventArgs
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using ZOOM_SDK_DOTNET_WRAP;

namespace zoom_sdk_demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        start_join_meeting start_meeting_wnd = new start_join_meeting();
        public MainWindow()
        {
            InitializeComponent();
            //register callback
            ZOOM_SDK_DOTNET_WRAP.CZoomSDKeDotNetWrap.Instance.GetAuthServiceWrap().Add_CB_onAuthenticationReturn(onAuthenticationReturn);

            ZOOM_SDK_DOTNET_WRAP.AuthParam param = new ZOOM_SDK_DOTNET_WRAP.AuthParam {
                appKey = "MfgQItI2gIPgSkBrjbVHmH9hDL1Ct2CHrVDT",
                appSecret = "XeUOrpLUoKDsHr4WJWJj02pohMPBAODIgPML"
            };
            ZOOM_SDK_DOTNET_WRAP.CZoomSDKeDotNetWrap.Instance.GetAuthServiceWrap().SDKAuth(param);

            Hide();
        }

        //callback
        public void onAuthenticationReturn(AuthResult ret)
        {
            if (ZOOM_SDK_DOTNET_WRAP.AuthResult.AUTHRET_SUCCESS == ret)
            {
                if (JoinMeetingRequest.IsBootup)
                    return;
                start_meeting_wnd.Show();
                _ = Click(); // this callback must complete for authentication to be set, so we await yield the click call
            }
            else//error handle.todo
            {
                Show();
            }
        }

        private async Task Click()
        {
            await Task.Yield();
            start_meeting_wnd.button_join_api.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        }

        void Wnd_Closing(object sender, CancelEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
