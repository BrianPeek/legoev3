using System;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
#if WINRT
using Windows.Foundation;
#endif

namespace Lego.Ev3.Core
{
	/// <summary>
	/// Dummy object for testing.  Does not actually connect or communicate with EV3 brick.
	/// </summary>
	public sealed class DummyCommunication : ICommunication
	{
		/// <summary>
		/// Event fired when a complete report is received from the EV3 brick.  In this dummy implementation, the event is never fired.
		/// </summary>
		public event EventHandler<ReportReceivedEventArgs> ReportReceived
#if WINRT
		;
#else
		{ add {} remove {} }
#endif

	/// <summary>
	/// Test Connect method.
	/// </summary>
	/// <returns></returns>
	public
#if WINRT
	IAsyncAction
#else
	Task
#endif
		ConnectAsync()
		{
			return Task.Run(() => Debug.WriteLine("connected"))
#if WINRT
			.AsAsyncAction()
#endif
			;
		}

		/// <summary>
		/// Test Disconnect method.
		/// </summary>
		public void Disconnect()
		{
		}

		/// <summary>
		/// Test WriteAsync method.  (Writes formatted data to Debug stream).
		/// </summary>
		/// <param name="data">Byte array to be written.</param>
		/// <returns></returns>
		public
#if WINRT
		IAsyncAction
#else
		Task
#endif
		WriteAsync([ReadOnlyArray]byte[] data)
		{
			return Task.Run(() => 
			{
				string s = string.Empty;
				for(int i = 3; i < data.Length; i++)
					s += data[i].ToString("X2") + " ";
				Debug.WriteLine("Write: " + s);
			})
#if WINRT
			.AsAsyncAction()
#endif
			;
		}
	}
}
