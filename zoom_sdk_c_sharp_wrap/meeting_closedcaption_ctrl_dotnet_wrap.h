#pragma once
using namespace System;
using namespace System::Windows;
using namespace System::Drawing;
#include "zoom_sdk_dotnet_wrap_def.h"
namespace ZOOM_SDK_DOTNET_WRAP {
	public interface class IClosedCaptionCtrlDotNetWrap
	{
	public:
		bool IsMeetingSupportCC();
		bool CanSendClosedCaption(long userId);
		SDKError SendClosedCaption(String^ ccMsg);
	};

	private ref class CClosedCaptionCtrlDotNetWrap sealed : public IClosedCaptionCtrlDotNetWrap
	{
		// TODO: Add your methods for this class here.
	public:
		static property CClosedCaptionCtrlDotNetWrap^ Instance
		{
			CClosedCaptionCtrlDotNetWrap^ get() { return m_Instance; }
		}
		virtual bool IsMeetingSupportCC();
		virtual bool CanSendClosedCaption(long userId);
		virtual SDKError SendClosedCaption(String^ ccMsg);

	private:
		CClosedCaptionCtrlDotNetWrap();
		virtual ~CClosedCaptionCtrlDotNetWrap();

		static CClosedCaptionCtrlDotNetWrap^ m_Instance = gcnew CClosedCaptionCtrlDotNetWrap;
	};
}