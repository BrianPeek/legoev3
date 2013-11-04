using System;
using System.Collections.Generic;

namespace Lego.Ev3.Core
{
	/// <summary>
	/// Arguments for PortsChanged event
	/// </summary>
	public sealed class BrickChangedEventArgs
#if !WINRT
		: EventArgs
#endif
	{
		/// <summary>
		/// A map of all ports on the EV3 brick
		/// </summary>
		public IDictionary<InputPort,Port> Ports { get; set; }

		/// <summary>
		/// Buttons on the face of the LEGO EV3 brick
		/// </summary>
		public BrickButtons Buttons { get; set; }
	}
}