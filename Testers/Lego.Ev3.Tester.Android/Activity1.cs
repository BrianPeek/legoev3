using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using Lego.Ev3.Android;
using Lego.Ev3.Core;
using Environment = System.Environment;

namespace Lego.Ev3.Tester.Android
{
	[Activity(Label = "Lego EV3 Tester", MainLauncher = true, Icon = "@drawable/icon")]
	public class Activity1 : Activity
	{
		private Brick _brick;
		private TextView _output;
		private TextView _ports;
		private StringBuilder _portText = new StringBuilder();

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			_output = FindViewById<TextView>(Resource.Id.output);
			_ports = FindViewById<TextView>(Resource.Id.ports);

			FindViewById<Button>(Resource.Id.connect).Click += connect_Click;
			FindViewById<Button>(Resource.Id.disconnect).Click += disconnect_Click;
			FindViewById<Button>(Resource.Id.playTone).Click += playTone_Click;
			FindViewById<Button>(Resource.Id.getFwVersion).Click += getFwVersion_Click;
			FindViewById<Button>(Resource.Id.turnMotorAtPower).Click += turnMotorAtPower_Click;
			FindViewById<Button>(Resource.Id.turnMotorAtSpeed).Click += turnMotorAtSpeed_Click;
			FindViewById<Button>(Resource.Id.stepMotorAtPower).Click += stepMotorAtPower_Click;
			FindViewById<Button>(Resource.Id.stepMotorAtSpeed).Click += stepMotorAtSpeed_Click;
			FindViewById<Button>(Resource.Id.timeMotorAtPower).Click += timeMotorAtPower_Click;
			FindViewById<Button>(Resource.Id.timeMotorAtSpeed).Click += timeMotorAtSpeed_Click;

			FindViewById<Button>(Resource.Id.stopMotor).Click += stopMotor_Click;
			FindViewById<Button>(Resource.Id.setLed).Click += setLed_Click;
			FindViewById<Button>(Resource.Id.playSound).Click += playSound_Click;
			FindViewById<Button>(Resource.Id.draw).Click += draw_Click;
			FindViewById<Button>(Resource.Id.batchNoReply).Click += batchNoReply_Click;
			FindViewById<Button>(Resource.Id.batchReply).Click += batchReply_Click;


			_brick = new Brick(new BluetoothCommunication());
			//_brick = new Brick(new NetworkCommunication("192.168.2.237"));
			_brick.BrickChanged += brick_BrickChanged;
		}

		async void connect_Click(object sender, EventArgs e)
		{
			try
			{
				await _brick.ConnectAsync();
				_output.Text = "Connected";
			}
			catch(Exception ex)
			{
				AlertDialog.Builder x = new AlertDialog.Builder(this);
				x.SetMessage(ex.ToString());
				x.Create().Show();
			}
		}

		private void disconnect_Click(object sender, EventArgs e)
		{
			_brick.Disconnect();
			_output.Text = "Disconnected";
		}

		private async void playTone_Click(object sender, EventArgs e)
		{
			await _brick.DirectCommand.PlayToneAsync(2, 1000, 400);
		}

		private async void getFwVersion_Click(object sender, EventArgs e)
		{
			_output.Text = await _brick.DirectCommand.GetFirmwareVersionAsync();
		}

		private async void turnMotorAtPower_Click(object sender, EventArgs e)
		{
			await _brick.DirectCommand.TurnMotorAtPowerAsync(OutputPort.All, 50);
		}

		private async void turnMotorAtSpeed_Click(object sender, EventArgs e)
		{
			await _brick.DirectCommand.TurnMotorAtSpeedAsync(OutputPort.All, 50);
		}

		private async void stepMotorAtSpeed_Click(object sender, EventArgs e)
		{
			await _brick.DirectCommand.StepMotorAtSpeedAsync(OutputPort.All, 50, 0, 360, 0, false);
		}

		private async void stepMotorAtPower_Click(object sender, EventArgs e)
		{
			await _brick.DirectCommand.StepMotorAtPowerAsync(OutputPort.All, 50, 0, 360, 0, false);
		}

		private async void timeMotorAtSpeed_Click(object sender, EventArgs e)
		{
			await _brick.DirectCommand.TurnMotorAtSpeedForTimeAsync(OutputPort.A, 50, 0, 2000, 0, false);
		}

		private async void timeMotorAtPower_Click(object sender, EventArgs e)
		{
			await _brick.DirectCommand.TurnMotorAtPowerForTimeAsync(OutputPort.A, 50, 0, 2000, 0, false);
		}

		private async void stopMotor_Click(object sender, EventArgs e)
		{
			await _brick.DirectCommand.StopMotorAsync(OutputPort.All, false);
		}

		private async void setLed_Click(object sender, EventArgs e)
		{
			await _brick.DirectCommand.SetLedPatternAsync(LedPattern.OrangeFlash);
		}

		private async void playSound_Click(object sender, EventArgs e)
		{
			await UploadFiles();
			await _brick.DirectCommand.PlaySoundAsync(50, "../prjs/Tester/Overpower");
		}

		private async void batchReply_Click(object sender, EventArgs e)
		{
			_brick.BatchCommand.Initialize(CommandType.DirectReply, 0x10, 0);
			_brick.BatchCommand.PlayTone(2, 1000, 400);
			_brick.BatchCommand.DrawCircle(Color.Foreground, 20, 20, 20, true);
			_brick.BatchCommand.UpdateUI();
			_brick.BatchCommand.ReadySI(InputPort.One, 0, 0);
			byte[] b = await _brick.BatchCommand.SendCommandAsync();
			_output.Text = BitConverter.ToString(b);
		}

		private async void batchNoReply_Click(object sender, EventArgs e)
		{
			_brick.BatchCommand.PlayTone(2, 1000, 400);
			_brick.BatchCommand.DrawCircle(Color.Foreground, 20, 20, 20, true);
			_brick.BatchCommand.UpdateUI();
			_brick.BatchCommand.TurnMotorAtPowerForTime(OutputPort.A, 20, 2000, false);
			_brick.BatchCommand.OutputReady(OutputPort.A);
			_brick.BatchCommand.TurnMotorAtPowerForTime(OutputPort.A, 50, 1000, false);
			_brick.BatchCommand.OutputReady(OutputPort.A);
			await _brick.BatchCommand.SendCommandAsync();
		}

		private async void draw_Click(object sender, EventArgs e)
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

		private void brick_BrickChanged(object sender, BrickChangedEventArgs e)
		{
			_portText.Clear();

			foreach(Port port in e.Ports.Values)
			{
				_portText.AppendFormat("{0}: {1}, {2}, {3}", port.Name, port.Type, port.Mode, port.SIValue);
				_portText.AppendLine();
			}

			_ports.Text = _portText.ToString();
		}

		private async Task UploadFiles()
		{
			await _brick.SystemCommand.WriteFileAsync(await ReadFileAsync("Overpower.rsf"), "../prjs/Tester/Overpower.rsf");
			await _brick.SystemCommand.WriteFileAsync(await ReadFileAsync("LEGO.rgf"), "../prjs/Tester/LEGO.rgf");
		}

		private async Task<byte[]> ReadFileAsync(string file)
		{
			using(Stream s = Assets.Open(file))
			{
				using(MemoryStream ms = new MemoryStream())
				{
					await s.CopyToAsync(ms);
					return ms.ToArray();
				}
			}
		}
	}
}

