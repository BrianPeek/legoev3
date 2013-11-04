using System;
using System.Windows;
using System.Windows.Controls;

using Lego.Ev3.Core;

namespace SampleApp.Controls
{
	/// <summary>
	/// Interaction logic for SensorSettingsControl.xaml
	/// </summary>
	public partial class SensorSettingsControl : UserControl
	{
		public event EventHandler<SensorSettingsEventArgs> SaveSettings;
		private Type _sensorType;

		public SensorSettingsControl()
		{
			InitializeComponent();
		}

		private void RoundButton_Click(object sender, RoutedEventArgs e)
		{
			if (SaveSettings != null && _sensorType != null)
			{
				SaveSettings.Invoke(sender, new SensorSettingsEventArgs
				{
					SensorMode = Convert.ToByte(Enum.ToObject(_sensorType, SensorStyle.SelectedItem))
				});
			}

			Visibility = Visibility.Collapsed;
		}

		public void Show(InputPort port, DeviceType deviceType, byte sensorMode)
		{
			_sensorType = GetTypeFromSensorType(deviceType);

			BindData(_sensorType);

			if (_sensorType != null)
				SensorStyle.SelectedItem = Enum.ToObject(_sensorType, sensorMode);

			PortName.Text = port.ToString();
			ObjectName.Text = deviceType.ToString();

			Visibility = Visibility.Visible;
		}

		private void BindData(Type t)
		{
			if (t == null)
			{
				SensorStyle.Visibility = Visibility.Collapsed;
			}
			else
			{
				SensorStyle.Visibility = Visibility.Visible;
				SensorStyle.ItemsSource = Enum.GetValues(t);
			}
		}

		private static Type GetTypeFromSensorType(DeviceType sensorType)
		{
			Type t = null;

			switch (sensorType)
			{
				case DeviceType.Color:
					t = typeof(ColorMode);
					break;
				case DeviceType.Gyroscope:
					t = typeof(GyroscopeMode);
					break;
				case DeviceType.Infrared:
					t = typeof(InfraredMode);
					break;
				case DeviceType.NxtColor:
					t = typeof(NxtColorMode);
					break;
				case DeviceType.NxtLight:
					t = typeof(NxtLightMode);
					break;
				case DeviceType.NxtSound:
					t = typeof(NxtSoundMode);
					break;
				case DeviceType.NxtTemperature:
					t = typeof(NxtTemperatureMode);
					break;
				case DeviceType.Touch:
				case DeviceType.NxtTouch:
					t = typeof(TouchMode);
					break;
				case DeviceType.NxtUltrasonic:
					t = typeof(NxtUltrasonicMode);
					break;
				case DeviceType.Ultrasonic:
					t = typeof(UltrasonicMode);
					break;
			}
			return t;
		}
	}
}
