using System;
using System.Threading.Tasks;
using System.Windows;
using Lego.Ev3.Core;
using Lego.Ev3.Phone;
using Microsoft.Phone.Controls;

namespace Lego.Ev3.Tester.Phone
{
	public partial class MainPage : PhoneApplicationPage
	{
		private Brick _brick;

		// Constructor
		public MainPage()
		{
			InitializeComponent();
		}

		private async void Connect_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				_brick = new Brick(new BluetoothCommunication());
				//_brick = new Brick(new NetworkCommunication("192.168.2.237"));
				_brick.BrickChanged += brick_BrickChanged;
				await _brick.ConnectAsync();
				Output.Text = "Connected";
			}
			catch(Exception ex)
			{
				MessageBox.Show("Failed to connect: " + ex);
			}
		}

		private void Disconnect_Click(object sender, RoutedEventArgs e)
		{
			_brick.Disconnect();
			Output.Text = "Disconnected";
		}

		void brick_BrickChanged(object sender, BrickChangedEventArgs e)
		{
			InputPorts.DataContext = e.Ports;
		}

		private async void PlayTone_Click(object sender, RoutedEventArgs e)
		{
			await _brick.DirectCommand.PlayToneAsync(2, 1000, 400);
		}

		private async void TurnMotorPower_Click(object sender, RoutedEventArgs e)
		{
			await _brick.DirectCommand.TurnMotorAtPowerAsync(OutputPort.All, 50);
		}

		private async void TurnMotorSpeed_Click(object sender, RoutedEventArgs e)
		{
			await _brick.DirectCommand.TurnMotorAtSpeedAsync(OutputPort.All, 50);
		}

		private async void StepMotorPower_Click(object sender, RoutedEventArgs e)
		{
			await _brick.DirectCommand.StepMotorAtPowerAsync(OutputPort.All, 50, 0, 360, 0, false);
		}

		private async void StepMotorSpeed_Click(object sender, RoutedEventArgs e)
		{
			await _brick.DirectCommand.StepMotorAtSpeedAsync(OutputPort.All, 50, 0, 360, 0, false);
		}

		private async void TimeMotorPower_Click(object sender, RoutedEventArgs e)
		{
			await _brick.DirectCommand.TurnMotorAtPowerForTimeAsync(OutputPort.A, 50, 0, 2000, 0, false);
		}

		private async void TimeMotorSpeed_Click(object sender, RoutedEventArgs e)
		{
			await _brick.DirectCommand.TurnMotorAtSpeedForTimeAsync(OutputPort.A, 50, 0, 2000, 0, false);
		}

		private async void StopMotor_Click(object sender, RoutedEventArgs e)
		{
			await _brick.DirectCommand.StopMotorAsync(OutputPort.All, false);
		}

		private async void ClearAll_Click(object sender, RoutedEventArgs e)
		{
			await _brick.DirectCommand.ClearAllDevices();
		}

		private async void GetFirmwareVersion_Click(object sender, RoutedEventArgs e)
		{
			Output.Text = await _brick.DirectCommand.GetFirmwareVersionAsync();
		}

		private async void SetLed_Click(object sender, RoutedEventArgs e)
		{
			await _brick.DirectCommand.SetLedPatternAsync(LedPattern.OrangeFlash);
		}

		private async void PlaySound_Click(object sender, RoutedEventArgs e)
		{
			await UploadFiles();
			await _brick.DirectCommand.PlaySoundAsync(50, "../prjs/Tester/Overpower");
		}

		private async void Draw_Click(object sender, RoutedEventArgs e)
		{
			await UploadFiles();

			await _brick.DirectCommand.CleanUIAsync();
			await _brick.DirectCommand.EnableTopLineAsync(false);
			await _brick.DirectCommand.DrawLineAsync(Color.Foreground, 0, 0, 40, 40);
			await _brick.DirectCommand.DrawPixelAsync(Color.Foreground, 100, 100);
			await _brick.DirectCommand.DrawRectangleAsync(Color.Foreground, 50, 50, 20, 20, false);
			await _brick.DirectCommand.DrawRectangleAsync(Color.Foreground, 70, 70, 20, 20, true);
			await _brick.DirectCommand.DrawCircleAsync(Color.Foreground, 100, 50, 20, false);
			await _brick.DirectCommand.DrawCircleAsync(Color.Foreground, 100, 70, 20, true);
			await _brick.DirectCommand.SelectFontAsync(FontType.Small);
			await _brick.DirectCommand.DrawTextAsync(Color.Foreground, 20, 20, "Hello world!");
			await _brick.DirectCommand.SelectFontAsync(FontType.Medium);
			await _brick.DirectCommand.DrawTextAsync(Color.Foreground, 20, 30, "Hello world!");
			await _brick.DirectCommand.SelectFontAsync(FontType.Large);
			await _brick.DirectCommand.DrawTextAsync(Color.Foreground, 20, 40, "Hello world!");
			await _brick.DirectCommand.DrawInverseRectangleAsync(20, 20, 10, 10);
			await _brick.DirectCommand.DrawDottedLineAsync(Color.Foreground, 10, 0, 10, 100, 5, 5);
			await _brick.DirectCommand.DrawFillWindowAsync(Color.Foreground, 0, 24);
			await _brick.DirectCommand.DrawImageAsync(Color.Foreground, 0, 40, "../prjs/Tester/LEGO.rgf");
			await _brick.DirectCommand.UpdateUIAsync();
		}

		private async Task UploadFiles()
		{
			await _brick.SystemCommand.CopyFileAsync("Overpower.rsf", "../prjs/Tester/Overpower.rsf");
			await _brick.SystemCommand.CopyFileAsync("LEGO.rgf", "../prjs/Tester/LEGO.rgf");
		}

		private async void BatchNoReply_Click(object sender, RoutedEventArgs e)
		{
			_brick.BatchCommand.PlayTone(2, 1000, 400);
			_brick.BatchCommand.DrawCircle(Color.Foreground, 20, 20, 20, true);
			_brick.BatchCommand.UpdateUI();
			await _brick.BatchCommand.SendCommandAsync();
		}

		private async void BatchReply_Click(object sender, RoutedEventArgs e)
		{
			_brick.BatchCommand.Initialize(CommandType.DirectReply, 0x10, 0);
			_brick.BatchCommand.PlayTone(2, 1000, 400);
			_brick.BatchCommand.DrawCircle(Color.Foreground, 20, 20, 20, true);
			_brick.BatchCommand.UpdateUI();
			_brick.BatchCommand.ReadySI(InputPort.One, 0, 0);
			byte[] b = await _brick.BatchCommand.SendCommandAsync();
			Output.Text = BitConverter.ToString(b);
		}
	}
}