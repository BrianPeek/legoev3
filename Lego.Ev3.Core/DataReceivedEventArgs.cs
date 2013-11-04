using System;

namespace Lego.Ev3.Core
{
	/// <summary>
	/// Event arguments for the ReportReceived event.
	/// </summary>
	public sealed class ReportReceivedEventArgs
#if !WINRT
		: EventArgs
#endif
	{
		/// <summary>
		/// Byte array of the data received from the EV3 brick.
		/// </summary>
		public byte[] Report { get; set; }
	}
}