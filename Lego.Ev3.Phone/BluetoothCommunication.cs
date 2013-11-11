using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Lego.Ev3.Core;
using Windows.Networking.Proximity;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace Lego.Ev3.Phone
{
	/// <summary>
	/// Communicate with the EV3 brick over Bluetooth.
	/// </summary>
	public class BluetoothCommunication : ICommunication
	{
		/// <summary>
		/// Event fired when a complete report is received from the EV3 brick.
		/// </summary>
		public event EventHandler<ReportReceivedEventArgs> ReportReceived;

		private StreamSocket _socket;
		private DataReader _reader;
		private CancellationTokenSource _tokenSource;

        private readonly string _deviceName;

        /// <summary>
        /// Create a new NetworkCommunication object
        /// </summary>
        /// <param name="device">Devicename of the EV3 brick</param>
        public BluetoothCommunication(string device)
        {
            _deviceName = device;
        }
        /// <summary>
        /// Create a new NetworkCommunication object
        /// </summary>
        public BluetoothCommunication()
        {
            _deviceName = "EV3";
        }

        /// <summary>
		/// Connect to the EV3 brick.
		/// </summary>
		/// <returns></returns>
		public async Task ConnectAsync()
		{
			_tokenSource = new CancellationTokenSource();

			PeerFinder.AlternateIdentities["Bluetooth:Paired"] = "";
			IReadOnlyList<PeerInformation> peers = await PeerFinder.FindAllPeersAsync();
            PeerInformation peer = (from p in peers where p.DisplayName == _deviceName select p).FirstOrDefault();
			if(peer == null)
				throw new Exception(_deviceName + " Brick not found");

			_socket = new StreamSocket();
			await _socket.ConnectAsync(peer.HostName, "1");

			_reader = new DataReader(_socket.InputStream);
			_reader.ByteOrder = ByteOrder.LittleEndian;

			ThreadPool.QueueUserWorkItem(PollInput);
		}

		private async void PollInput(object state)
		{
			while(_socket != null)
			{
				try
				{
					DataReaderLoadOperation drlo = _reader.LoadAsync(2);
					await drlo.AsTask(_tokenSource.Token);
					short size = _reader.ReadInt16();

					byte[] data = new byte[size];
					drlo = _reader.LoadAsync((uint)size);
					await drlo.AsTask(_tokenSource.Token);
					_reader.ReadBytes(data);

					if(ReportReceived != null)
						ReportReceived(this, new ReportReceivedEventArgs { Report = data });
				}
				catch (TaskCanceledException)
				{
					return;
				}
			}
		}

		/// <summary>
		/// Disconnect from the EV3 brick.
		/// </summary>
		public void Disconnect()
		{
			_tokenSource.Cancel();
			if(_reader != null)
			{
				_reader.DetachStream();
				_reader = null;
			}

			if(_socket != null)
			{
				_socket.Dispose();
				_socket = null;
			}
		}

		/// <summary>
		/// Write data to the EV3 brick.
		/// </summary>
		/// <param name="data">Byte array to write to the EV3 brick.</param>
		/// <returns></returns>
		public async Task WriteAsync(byte[] data)
		{
			if(_socket != null)
				await _socket.OutputStream.WriteAsync(data.AsBuffer());
		}
	}
}
