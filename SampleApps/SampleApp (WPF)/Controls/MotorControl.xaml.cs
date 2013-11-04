using System.Globalization;
using System.Windows;
using System.Windows.Controls;

using Lego.Ev3.Core;

namespace SampleApp.Controls
{
	/// <summary>
	/// Interaction logic for MotorControl.xaml
	/// </summary>
	public partial class MotorControl : UserControl
	{
		public event RoutedEventHandler PlayClicked;
		public event RoutedEventHandler SettingClicked;

		public InputPort BrickInputPort { get; set; }
		public OutputPort BrickOutputPort { get; set; }
		public MotorMovementTypes MotorMovementType { get; set; }
		public int DegreeMovement { get; set; }
		public int TimeToMoveInSeconds { get; set; }
		public int PowerRatingMovement { get; set; }

		private DeviceType _motorType = DeviceType.Empty;

		public MotorControl()
		{
			InitializeComponent();

			_motorType = DeviceType.Empty;

			MotorMovementType = MotorMovementTypes.Power;
			TimeToMoveInSeconds = 2;
			PowerRatingMovement = 50;

			DataContext = this;

			Loaded += MotorControl_Loaded;
		}

		void MotorControl_Loaded(object sender, RoutedEventArgs e)
		{
			UpdateUx();
		}

		public void Update(Brick brick)
		{
			_motorType = brick.Ports[BrickInputPort].Type;

			UpdateUx();
		}

		public void UpdateUx()
		{
			MotorType.Text = MotorTypeAsString();
			PortName.Text = BrickInputPort.ToString();

			TimeBasedTextBlock.Visibility = Visibility.Collapsed;
			DegreesTextBlock.Visibility = Visibility.Collapsed;

			PowerPercentageRun.Text = PowerRatingMovement.ToString(CultureInfo.InvariantCulture);

			if (MotorMovementType == MotorMovementTypes.Degrees)
			{
				if (DegreesTextBlock.Visibility != Visibility.Visible)
				{
					TimeBasedTextBlock.Visibility = Visibility.Collapsed;
					DegreesTextBlock.Visibility = Visibility.Visible;
				}

				DegreesRun.Text = DegreeMovement.ToString(CultureInfo.InvariantCulture);
			}
			else
			{
				if (TimeBasedTextBlock.Visibility != Visibility.Visible)
				{
					TimeBasedTextBlock.Visibility = Visibility.Visible;
					DegreesTextBlock.Visibility = Visibility.Collapsed;
				}

				SecondsRun.Text = TimeToMoveInSeconds.ToString(CultureInfo.InvariantCulture);
				SecondsDescRun.Text = (TimeToMoveInSeconds == 0) ? "continuous" : "seconds";
			}
		}

		private string MotorTypeAsString()
		{
			switch (_motorType)
			{
				case DeviceType.Empty:
					return "Empty";
				case DeviceType.MMotor:
					return "Medium";
				case DeviceType.LMotor:
					return "Large";
				default:
					return "Unknown";
			}
		}

		private void MotorSettingClick(object sender, RoutedEventArgs e)
		{
			if (SettingClicked != null)
				SettingClicked.Invoke(this, e);
		}

		private void PlayClick(object sender, RoutedEventArgs e)
		{
			if (PlayClicked != null)
				PlayClicked.Invoke(this, e);
		}
	}
}
