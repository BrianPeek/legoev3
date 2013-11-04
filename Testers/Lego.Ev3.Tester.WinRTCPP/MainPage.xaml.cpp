//
// MainPage.xaml.cpp
// Implementation of the MainPage class.
//

#include "pch.h"
#include "MainPage.xaml.h"

using namespace Lego_Ev3_Tester_WinRTCPP;

using namespace Platform;
using namespace Windows::Foundation;
using namespace Windows::Foundation::Collections;
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Controls;
using namespace Windows::UI::Xaml::Controls::Primitives;
using namespace Windows::UI::Xaml::Data;
using namespace Windows::UI::Xaml::Input;
using namespace Windows::UI::Xaml::Media;
using namespace Windows::UI::Xaml::Navigation;
using namespace Lego::Ev3::Core;
using namespace Lego::Ev3::WinRT;
using namespace Concurrency;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

MainPage::MainPage()
{
	InitializeComponent();
}

void Lego_Ev3_Tester_WinRTCPP::MainPage::Page_Loaded(Platform::Object^ sender, Windows::UI::Xaml::RoutedEventArgs^ e)
{
	Brick^ brick = ref new Brick(ref new BluetoothCommunication());
	create_task(brick->ConnectAsync()).then([=]
	{
		return brick->DirectCommand->PlayToneAsync(2, 1000, 400);
	}, task_continuation_context::use_current());
}
