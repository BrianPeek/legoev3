using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lego.Ev3.Core;

namespace Lego.Ev3
#if ANDROID
	.Android
#else
	.Desktop
#endif
{
	/// <summary>
	/// Communicate with EV3 brick over TCP
	/// </summary>
	public class NetworkCommunication : ICommunication
	{
		/// <summary>
		/// Event fired when a complete report is received from the EV3 brick.
		/// </summary>
		public event EventHandler<ReportReceivedEventArgs> ReportReceived;

		private const string UnlockCommand = "GET /target?sn=\r\nProtocol:EV3\r\n\r\n";

		private TcpClient _client;
		private readonly string _address;
		private NetworkStream _stream;
		private CancellationTokenSource _tokenSource;
		private readonly byte[] _sizeBuffer = new byte[2];

		/// <summary>
		/// Create a new NetworkCommunication object
		/// </summary>
		/// <param name="address">The IP address of the EV3 brick</param>
		public NetworkCommunication(string address)
		{
			_address = address;
		}

		/// <summary>
		/// Connect to the EV3 brick.
		/// </summary>
		/// <returns></returns>
		public async Task ConnectAsync()
		{
			_client = new TcpClient();
			await _client.ConnectAsync(_address, 5555);
			_stream = _client.GetStream();

			// unlock the brick (doesn't actually need serial number?)
			byte[] buff = Encoding.UTF8.GetBytes(UnlockCommand);
			await _stream.WriteAsync(buff, 0, buff.Length);

			// read the "Accept:EV340\r\n\r\n" response
			int read = await _stream.ReadAsync(buff, 0, buff.Length);
			string response = Encoding.UTF8.GetString(buff, 0, read);
			if(string.IsNullOrEmpty(response))
				throw new Exception("LEGO EV3 brick did not respond to the unlock command.");

			_tokenSource = new CancellationTokenSource();

			Task t = Task.Factory.StartNew(async () =>
			{
				while(!_tokenSource.IsCancellationRequested)
				{
					// if the stream is valid and ready
					if(_stream != null && _stream.CanRead)
					{
						await _stream.ReadAsync(_sizeBuffer, 0, _sizeBuffer.Length);

						short size = (short)(_sizeBuffer[0] | _sizeBuffer[1] << 8);
						if(size == 0)
							return;

						byte[] report = new byte[size];
						await _stream.ReadAsync(report, 0, report.Length);
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
			_client.Close();
		}

		/// <summary>
		/// Write data to the EV3 brick.
		/// </summary>
		/// <param name="data">Byte array to write to the EV3 brick.</param>
		/// <returns></returns>
		public Task WriteAsync(byte[] data)
		{
			return _stream.WriteAsync(data, 0, data.Length);
		}
	}
}
