using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
#if WINRT
using Windows.Foundation.Metadata;
#endif

namespace Lego.Ev3.Core
{
	/// <summary>
	/// An input or output port on the EV3 brick
	/// </summary>
	public sealed class Port : INotifyPropertyChanged
	{
		internal int Index { get; set; }
		internal InputPort InputPort { get; set; }

		private string _name;
		private DeviceType _type;
		private byte _mode;
		private float _siValue;
		private int _rawValue;
		private byte _percentValue;
		private readonly SynchronizationContext _context;

		/// <summary>
		/// Constructor
		/// </summary>
		public Port()
		{
			_context = SynchronizationContext.Current;
		}

		/// <summary>
		/// Name of port.
		/// </summary>
		public string Name
		{
			get { return _name; }
			set { _name = value; OnPropertyChanged(); }
		}

		/// <summary>
		/// Device plugged into port.
		/// </summary>
		public DeviceType Type
		{
			get { return _type; }
			set { _type = value; OnPropertyChanged(); }
		}

		/// <summary>
		/// Device mode.  Some devices work in multiple modes.
		/// </summary>
		public byte Mode
		{
			get { return _mode; }
			private set { _mode = value; OnPropertyChanged(); }
		}

		/// <summary>
		/// Current International System of Units value associated with the Port.
		/// </summary>
		public float SIValue
		{
			get { return _siValue; }
			set { _siValue = value; OnPropertyChanged(); }
		}

		/// <summary>
		/// Raw value associated with the Port.
		/// </summary>
		public int RawValue
		{
			get { return _rawValue; }
			set { _rawValue = value; OnPropertyChanged(); }
		}

		/// <summary>
		/// Percentage value associated with the Port.
		/// </summary>
		public byte PercentValue
		{
			get { return _percentValue; }
			set { _percentValue = value; OnPropertyChanged(); }
		}

		/// <summary>
		/// Set the connected sensor's mode
		/// </summary>
		/// <param name="mode">The requested mode.</param>
#if WINRT
		[DefaultOverload]
#endif
		public void SetMode(byte mode)
		{
			Mode = mode;
		}

		/// <summary>
		/// Set the connected sensor's mode
		/// </summary>
		/// <param name="mode">The requested mode.</param>
		public void SetMode(TouchMode mode)
		{
			Mode = (byte)mode;
		}

		/// <summary>
		/// Set the connected sensor's mode
		/// </summary>
		/// <param name="mode">The requested mode.</param>
		public void SetMode(NxtLightMode mode)
		{
			Mode =	(byte)mode;
		}

		/// <summary>
		/// Set the connected sensor's mode
		/// </summary>
		/// <param name="mode">The requested mode.</param>
		public void SetMode(NxtSoundMode mode)
		{
			Mode = (byte)mode;
		}

		/// <summary>
		/// Set the connected sensor's mode
		/// </summary>
		/// <param name="mode">The requested mode.</param>
		public void SetMode(NxtUltrasonicMode mode)
		{
			Mode = (byte)mode;
		}

		/// <summary>
		/// Set the connected sensor's mode
		/// </summary>
		/// <param name="mode">The requested mode.</param>
		public void SetMode(NxtTemperatureMode mode)
		{
			Mode = (byte)mode;
		}

		/// <summary>
		/// Set the connected sensor's mode
		/// </summary>
		/// <param name="mode">The requested mode.</param>
		public void SetMode(MotorMode mode)
		{
			Mode = (byte)mode;
		}

		/// <summary>
		/// Set the connected sensor's mode
		/// </summary>
		/// <param name="mode">The requested mode.</param>
		public void SetMode(ColorMode mode)
		{
			Mode = (byte)mode;
		}

		/// <summary>
		/// Set the connected sensor's mode
		/// </summary>
		/// <param name="mode">The requested mode.</param>
		public void SetMode(UltrasonicMode mode)
		{
			Mode = (byte)mode;
		}

		/// <summary>
		/// Set the connected sensor's mode
		/// </summary>
		/// <param name="mode">The requested mode.</param>
		public void SetMode(GyroscopeMode mode)
		{
			Mode = (byte)mode;
		}

		/// <summary>
		/// Set the connected sensor's mode
		/// </summary>
		/// <param name="mode">The requested mode.</param>
		public void SetMode(InfraredMode mode)
		{
			Mode = (byte)mode;
		}

		/// <summary>
		/// INotifyProperty event
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if(handler != null)
			{
				if(_context == SynchronizationContext.Current)
					handler(this, new PropertyChangedEventArgs(propertyName));
				else
					_context.Post(delegate { handler(this, new PropertyChangedEventArgs(propertyName)); }, null);
			}
		}
	}
}