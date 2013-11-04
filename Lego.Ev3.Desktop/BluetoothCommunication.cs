using System;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using Lego.Ev3.Core;
using System.Threading.Tasks;

namespace Lego.Ev3.Desktop
{
	/// <summary>
	/// Communicate to EV3 brick over Bluetooth
	/// </summary>
	public class BluetoothCommunication : ICommunication
	{
		/// <summary>
		/// Event fired when a complete report is received from the EV3 brick.
		/// </summary>
		public event EventHandler<ReportReceivedEventArgs> ReportReceived;

		private SerialPort _serialPort;
		private BinaryReader _reader;

		/// <summary>
		/// Initialize a BluetoothCommunication object.
		/// </summary>
		/// <param name="port">The COM port on which to connect.</param>
		public BluetoothCommunication(string port)
		{
			_serialPort = new SerialPort(port, 115200);
		}

		/// <summary>
		/// Connect to the EV3 brick.
		/// </summary>
		/// <returns></returns>
		public Task ConnectAsync()
		{
			_serialPort.DataReceived += SerialPortDataReceived;
			_serialPort.Open();

			_reader = new BinaryReader(_serialPort.BaseStream);

			return Task.FromResult(true);
		}

		private void SerialPortDataReceived(object sender, SerialDataReceivedEventArgs e)
		{
			if(e.EventType == SerialData.Chars)
			{
				int size = _reader.ReadInt16();
				byte[] b = _reader.ReadBytes(size);

				if(ReportReceived != null)
					ReportReceived(this, new ReportReceivedEventArgs { Report = b });
			}
		}

		/// <summary>
		/// Disconnect from the EV3 brick.
		/// </summary>
		public void Disconnect()
		{
			if(_serialPort != null)
			{
				_serialPort.DataReceived -= SerialPortDataReceived;
				_serialPort.Close();
				_serialPort = null;
			}
		}

		/// <summary>
		/// Write data to the EV3 brick.
		/// </summary>
		/// <param name="data">Byte array of data to send to the EV3 brick.</param>
		/// <returns></returns>
		public Task WriteAsync(byte[] data)
		{
			return Task.Run(() =>
			{
				if(!_serialPort.IsOpen)
					return;

				lock(_serialPort)
					_serialPort.Write(data, 0, data.Length);
			});
		}
	}
}
