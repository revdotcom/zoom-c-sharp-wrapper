#include "stdafx.h"
#include "meeting_closedcaption_ctrl_dotnet_wrap.h"
#include "zoom_sdk_dotnet_wrap_util.h"
#include "wrap/sdk_wrap.h"

namespace ZOOM_SDK_DOTNET_WRAP {
	CClosedCaptionCtrlDotNetWrap::CClosedCaptionCtrlDotNetWrap()
	{

	}
	CClosedCaptionCtrlDotNetWrap::~CClosedCaptionCtrlDotNetWrap()
	{

	}

	SDKError CClosedCaptionCtrlDotNetWrap::SendClosedCaption(String^ ccMsg)
	{
		pin_ptr<const wchar_t> convertedValue = PtrToStringChars(ccMsg);
		const wchar_t* constValue = convertedValue;

		return (SDKError)ZOOM_SDK_NAMESPACE::CSDKWrap::GetInst().GetMeetingServiceWrap()
			.GetMeetingClosedCaptionController().SendClosedCaption(constValue);
	}

	bool CClosedCaptionCtrlDotNetWrap::IsMeetingSupportCC() {
		return ZOOM_SDK_NAMESPACE::CSDKWrap::GetInst().GetMeetingServiceWrap()
			.GetMeetingClosedCaptionController().IsMeetingSupportCC();
	}

	bool CClosedCaptionCtrlDotNetWrap::CanSendClosedCaption(long userId) {
		return ZOOM_SDK_NAMESPACE::CSDKWrap::GetInst().GetMeetingServiceWrap()
			.GetMeetingClosedCaptionController().CanSendClosedCaption();
	}
}