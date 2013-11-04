using System.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Lego.Ev3.Core;

namespace SampleApp.Controls
{
	public sealed partial class SensorDataControl : UserControl
	{
		public event RoutedEventHandler SettingClicked;

		public InputPort BrickInputPort { get; set; }
		private DeviceType _sensorType = DeviceType.Empty;

		public SensorDataControl()
		{
			InitializeComponent();
			DataContext = this;

			Loaded += SensorData_Loaded;
		}

		void SensorData_Loaded(object sender, RoutedEventArgs e)
		{
			UpdateUx();
		}

		public void Update(Brick brick)
		{
			_sensorType = brick.Ports[BrickInputPort].Type;

			SensorDataStackPanel.Visibility = IsInvalidSensorOrWrongPort() ? Visibility.Collapsed : Visibility.Visible;

			RawRun.Text = brick.Ports[BrickInputPort].RawValue.ToString(CultureInfo.InvariantCulture);
			SiRun.Text = brick.Ports[BrickInputPort].SIValue.ToString(CultureInfo.InvariantCulture);
			PercentageRun.Text = brick.Ports[BrickInputPort].PercentValue.ToString(CultureInfo.InvariantCulture);

			UpdateUx();
		}

		public void UpdateUx()
		{
			MotorType.Text = SensorTypeAsString();
			PortName.Text = BrickInputPort.ToString();
		}

		private void SensorSettingClick(object sender, RoutedEventArgs e)
		{
			if (SettingClicked != null)
				SettingClicked.Invoke(this, e);
		}

		private string SensorTypeAsString()
		{
			return IsInvalidSensor() ? "Unknown" : _sensorType.ToString();
		}

		private bool IsInvalidSensor()
		{
			return _sensorType == DeviceType.Empty ||
				   _sensorType == DeviceType.Initializing ||
				   _sensorType == DeviceType.LMotor ||
				   _sensorType == DeviceType.MMotor ||
				   _sensorType == DeviceType.Unknown;
		}

		private bool IsInvalidSensorOrWrongPort()
		{
			return IsInvalidSensor() || _sensorType == DeviceType.WrongPort;
		}
	}
}
