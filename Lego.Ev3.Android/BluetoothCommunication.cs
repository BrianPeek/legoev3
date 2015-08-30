using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Android.Bluetooth;
using Java.Util;
using Lego.Ev3.Core;

namespace Lego.Ev3.Android
{
	public class BluetoothCommunication : ICommunication
	{
		public event EventHandler<ReportReceivedEventArgs> ReportReceived;

		private readonly string _deviceName = "EV3";
		private BluetoothSocket _socket;
		private CancellationTokenSource _tokenSource;
		private readonly byte[] _sizeBuffer = new byte[2];

		public BluetoothCommunication()
		{
		}

		public BluetoothCommunication(string deviceName)
		{
			_deviceName = deviceName;
		}

		/// <summary>
		/// Connect to the EV3 brick.
		/// </summary>
		public async Task ConnectAsync()
		{
			BluetoothAdapter adapter = BluetoothAdapter.DefaultAdapter;
			if(adapter == null)
				throw new Exception("No Bluetooth adapter found");

			if(!adapter.IsEnabled)
				throw new Exception("Bluetooth adapter is not enabled");

			BluetoothDevice device = (from bd in adapter.BondedDevices where bd.Name == _deviceName select bd).FirstOrDefault();
			if(device == null)
				throw new Exception("LEGO EV3 brick named '" + _deviceName + "' not found.");

			_socket = device.CreateRfcommSocketToServiceRecord(UUID.FromString("00001101-0000-1000-8000-00805f9b34fb"));
			await _socket.ConnectAsync();

			_tokenSource = new CancellationTokenSource();

			Task t = Task.Factory.StartNew(async () =>
			{
				while(!_tokenSource.IsCancellationRequested)
				{
					Stream stream = _socket.InputStream;

					// if the stream is valid and ready
					if(stream != null && stream.CanRead)
					{
						await stream.ReadAsync(_sizeBuffer, 0, _sizeBuffer.Length);

						short size = (short)(_sizeBuffer[0] | _sizeBuffer[1] << 8);
						if(size == 0)
							return;

						byte[] report = new byte[size];
						await stream.ReadAsync(report, 0, report.Length);
						if (ReportReceived != null)
							ReportReceived(this, new ReportReceivedEventArgs { Report = report });
					}
				}
			}, _tokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current);
		}

		/// <summary>
		/// Disconnect from the EV3 brick.
		/// </summary>
		public void Disconnect()
		{
			if(_socket != null)
			{
				_socket.Dispose();
				_socket = null;
			}
		}

		/// <summary>
		/// Write a report to the EV3 brick.
		/// </summary>
		/// <param name="data"></param>
		public async Task WriteAsync(byte[] data)
		{
			if(_socket != null)
				await _socket.OutputStream.WriteAsync(data, 0, data.Length);
		}
	}
}