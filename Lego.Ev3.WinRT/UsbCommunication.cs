using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.HumanInterfaceDevice;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Storage;
using Lego.Ev3.Core;

namespace Lego.Ev3.WinRT
{
	/// <summary>
	/// Communicate with EV3 brick over USB HID
	/// </summary>
	public sealed class UsbCommunication : ICommunication
	{
		/// <summary>
		/// Event fired when a complete report is received from the EV3 brick.
		/// </summary>
		public event EventHandler<ReportReceivedEventArgs> ReportReceived;

		private const UInt16 VID = 0x0694;
		private const UInt16 PID = 0x0005;
		private const UInt16 UsagePage = 0xff00;
		private const UInt16 UsageId = 0x0001;

		private HidDevice _hidDevice;

		/// <summary>
		/// Connect to the EV3 brick.
		/// </summary>
		/// <returns></returns>
		public IAsyncAction ConnectAsync()
		{
			return ConnectAsyncInternal().AsAsyncAction();
		}

		private async Task ConnectAsyncInternal()
		{
			string selector = HidDevice.GetDeviceSelector(UsagePage, UsageId, VID, PID);
			DeviceInformationCollection devices = await DeviceInformation.FindAllAsync(selector);
			DeviceInformation brick = devices.FirstOrDefault();
			if(brick == null)
				throw new Exception("No LEGO EV3 bricks found.");

			_hidDevice = await HidDevice.FromIdAsync(brick.Id, FileAccessMode.ReadWrite);
			if(_hidDevice == null)
				throw new Exception("Unable to connect to LEGO EV3 brick...is the manifest set properly?");

			_hidDevice.InputReportReceived += HidDeviceInputReportReceived;
		}

		/// <summary>
		/// Disconnect from the EV3 brick.
		/// </summary>
		public void Disconnect()
		{
			_hidDevice.InputReportReceived -= HidDeviceInputReportReceived;

			if(_hidDevice != null)
			{
				_hidDevice.Dispose();
				_hidDevice = null;
			}
		}

		/// <summary>
		/// Write data to the EV3 brick.
		/// </summary>
		/// <param name="data">Byte array to write to the EV3 brick.</param>
		/// <returns></returns>
		public IAsyncAction WriteAsync([ReadOnlyArray]byte[] data)
		{
			return WriteAsyncInternal(data).AsAsyncAction();
		}

		private async Task WriteAsyncInternal(byte[] data)
		{
			if(_hidDevice == null)
				return;

			HidOutputReport report = _hidDevice.CreateOutputReport();
			data.CopyTo(0, report.Data, 1, data.Length); 
			await _hidDevice.SendOutputReportAsync(report);
		}

		private void HidDeviceInputReportReceived(HidDevice sender, HidInputReportReceivedEventArgs args)
		{
			byte[] data = args.Report.Data.ToArray();

			short size = (short)(data[1] | data[2] << 8);
			if(size == 0)
				return;

			byte[] report = new byte[size];
			Array.Copy(data, 3, report, 0, size);
			if (ReportReceived != null)
				ReportReceived(this, new ReportReceivedEventArgs { Report = report });
		}
	}
}
