using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Lego.Ev3.Core;
using Buffer = Windows.Storage.Streams.Buffer;
using ThreadPool = Windows.System.Threading.ThreadPool;

namespace Lego.Ev3
#if WINRT
.WinRT
#else
.Phone
#endif
{
	/// <summary>
	/// Communicate with EV3 brick over TCP
	/// </summary>
	public sealed class NetworkCommunication : ICommunication
	{
		/// <summary>
		/// Event fired when a complete report is received from the EV3 brick.
		/// </summary>
		public event EventHandler<ReportReceivedEventArgs> ReportReceived;

		private const string UnlockCommand = "GET /target?sn=\r\nProtocol:EV3\r\n\r\n";

		private CancellationTokenSource _tokenSource;

		private StreamSocket _socket;
		private readonly HostName _hostName;

		/// <summary>
		/// Create a new NetworkCommunication object
		/// </summary>
		/// <param name="address">The IP address of the EV3 brick</param>
		public NetworkCommunication(string address)
		{
			_hostName = new HostName(address);
		}

		/// <summary>
		/// Connect to the EV3 brick.
		/// </summary>
		/// <returns></returns>
#if WINRT
		public IAsyncAction ConnectAsync()
		{
			return ConnectAsyncInternal(_hostName).AsAsyncAction();
		}
#else
		public Task ConnectAsync()
		{
			return ConnectAsyncInternal(_hostName);
		}
#endif

		private async Task ConnectAsyncInternal(HostName hostName)
		{
			_tokenSource = new CancellationTokenSource();

			_socket = new StreamSocket();

			// connect to the brick on port 5555
			await _socket.ConnectAsync(hostName, "5555", SocketProtectionLevel.PlainSocket);

			// unlock the brick (doesn't actually need serial number?)
			await _socket.OutputStream.WriteAsync(Encoding.UTF8.GetBytes(UnlockCommand).AsBuffer());

			// read the "Accept:EV340\r\n\r\n" response
			IBuffer bufferResponse = new Buffer(128);
			await _socket.InputStream.ReadAsync(bufferResponse, bufferResponse.Capacity, InputStreamOptions.Partial);
			string response = Encoding.UTF8.GetString(bufferResponse.ToArray(), 0, (int) bufferResponse.Length);
			if(string.IsNullOrEmpty(response))
				throw new Exception("LEGO EV3 brick did not respond to the unlock command.");

			await ThreadPool.RunAsync(PollInput);
		}

		private async void PollInput(IAsyncAction operation)
		{
			while(!_tokenSource.IsCancellationRequested)
			{
				try
				{
					IBuffer sizeBuffer = new Buffer(2);
					await _socket.InputStream.ReadAsync(sizeBuffer, 2, InputStreamOptions.None);
					uint size = (uint) ((sizeBuffer.GetByte(0) | sizeBuffer.GetByte(1) << 8));

					if(size != 0)
					{
						IBuffer report = new Buffer(size);
						await _socket.InputStream.ReadAsync(report, size, InputStreamOptions.None);
						if(ReportReceived != null)
							ReportReceived(this, new ReportReceivedEventArgs { Report = report.ToArray() });
					}
				}
				catch(Exception)
				{
					// swallow exceptions...if we tank here, it's likely a disconnect and we can't do much anyway
				}
			}
		}

		/// <summary>
		/// Disconnect from the EV3 brick.
		/// </summary>
		public void Disconnect()
		{
			_tokenSource.Cancel();

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
#if WINRT
		public IAsyncAction WriteAsync([ReadOnlyArray]byte[] data)
		{
			return WriteAsyncInternal(data).AsAsyncAction();
		}
#else
		public Task WriteAsync([ReadOnlyArray]byte[] data)
		{
			return WriteAsyncInternal(data);
		}
#endif

		private async Task WriteAsyncInternal(byte[] data)
		{
			await _socket.OutputStream.WriteAsync(data.AsBuffer());
		}
	}
}
