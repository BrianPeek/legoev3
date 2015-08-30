using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Lego.Ev3.Core;
using Microsoft.Win32.SafeHandles;

namespace Lego.Ev3.Desktop
{
	/// <summary>
	/// Communicate with EV3 brick over USB HID.
	/// </summary>
	public class UsbCommunication : ICommunication
	{
		/// <summary>
		/// Event fired when a complete report is received from the EV3 brick.
		/// </summary>
		public event EventHandler<ReportReceivedEventArgs> ReportReceived;

		// our brick VID and PID
		private const UInt16 VID = 0x0694;
		private const UInt16 PID = 0x0005;

		// full-size report
		private byte[] _inputReport;
		private byte[] _outputReport;

		// read/write handle to the device
		private SafeFileHandle _handle;

		// a pretty .NET stream to read/write from/to
		private FileStream _stream;

		// token to dump out of reading loop
		private CancellationTokenSource _tokenSource;

		/// <summary>
		/// Connect to the EV3 brick.
		/// </summary>
		/// <returns></returns>
		public Task ConnectAsync()
		{
			FindEv3();

			_tokenSource = new CancellationTokenSource();

			Task t = Task.Factory.StartNew(async () =>
			{
				while(!_tokenSource.IsCancellationRequested)
				{
					// if the stream is valid and ready
					if(_stream != null && _stream.CanRead)
					{
						await _stream.ReadAsync(_inputReport, 0, _inputReport.Length, _tokenSource.Token);

						short size = (short)(_inputReport[1] | _inputReport[2] << 8);
						if(size == 0)
							return;

						byte[] report = new byte[size];
						Array.Copy(_inputReport, 3, report, 0, size);
						if (ReportReceived != null)
							ReportReceived(this, new ReportReceivedEventArgs { Report = report });
					}
				}
			}, _tokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current);

			return Task.FromResult(true);
		}

		/// <summary>
		/// Disconnect from the EV3 brick.
		/// </summary>
		public void Disconnect()
		{
			// close up the stream and handle
			if(_stream != null)
				_stream.Close();

			if(_handle != null)
				_handle.Close();
		}

		/// <summary>
		/// Write data to the EV3 brick.
		/// </summary>
		/// <param name="data">Byte array to send to the EV3 brick.</param>
		/// <returns></returns>
		public async Task WriteAsync(byte[] data)
		{
			if(_stream != null)
			{
				data.CopyTo(_outputReport, 1);
				await _stream.WriteAsync(_outputReport, 0, _outputReport.Length);
				_stream.Flush();
			}

			// invalid stream, bail out, but don't tank who is awaiting us
			await Task.FromResult(false);
		}

		internal void FindEv3()
		{
			int index = 0;
			bool found = false;
			Guid guid;

			// get the GUID of the HID class
			HidImports.HidD_GetHidGuid(out guid);

			// get a handle to all devices that are part of the HID class
			IntPtr hDevInfo = HidImports.SetupDiGetClassDevs(ref guid, null, IntPtr.Zero, HidImports.DIGCF_DEVICEINTERFACE | HidImports.DIGCF_PRESENT);

			// create a new interface data struct and initialize its size
			HidImports.SP_DEVICE_INTERFACE_DATA diData = new HidImports.SP_DEVICE_INTERFACE_DATA();
			diData.cbSize = Marshal.SizeOf(diData);

			// get a device interface to a single device (enumerate all devices)
			while(HidImports.SetupDiEnumDeviceInterfaces(hDevInfo, IntPtr.Zero, ref guid, index, ref diData))
			{
				UInt32 size;

				// get the buffer size for this device detail instance (returned in the size parameter)
				HidImports.SetupDiGetDeviceInterfaceDetail(hDevInfo, ref diData, IntPtr.Zero, 0, out size, IntPtr.Zero);

				// create a detail struct and set its size
				HidImports.SP_DEVICE_INTERFACE_DETAIL_DATA diDetail = new HidImports.SP_DEVICE_INTERFACE_DETAIL_DATA();

				// yeah, yeah...well, see, on Win x86, cbSize must be 5 for some reason.  On x64, apparently 8 is what it wants.
				// someday I should figure this out.  Thanks to Paul Miller on this...
				diDetail.cbSize = (uint)(IntPtr.Size == 8 ? 8 : 5);

				// actually get the detail struct
				if(HidImports.SetupDiGetDeviceInterfaceDetail(hDevInfo, ref diData, ref diDetail, size, out size, IntPtr.Zero))
				{
					Debug.WriteLine("{0}: {1} - {2}", index, diDetail.DevicePath, Marshal.GetLastWin32Error());

					// open a read/write handle to our device using the DevicePath returned
					_handle = HidImports.CreateFile(diDetail.DevicePath, FileAccess.ReadWrite, FileShare.ReadWrite, IntPtr.Zero, FileMode.Open, HidImports.EFileAttributes.Overlapped, IntPtr.Zero);

					// create an attributes struct and initialize the size
					HidImports.HIDD_ATTRIBUTES attrib = new HidImports.HIDD_ATTRIBUTES();
					attrib.Size = Marshal.SizeOf(attrib);

					// get the attributes of the current device
					if(HidImports.HidD_GetAttributes(_handle.DangerousGetHandle(), ref attrib))
					{
						// if the vendor and product IDs match up
						if(attrib.VendorID == VID && attrib.ProductID == PID)
						{
							// it's a Ev3
							Debug.WriteLine("Found one!");
							found = true;

							IntPtr preparsedData;
							if(!HidImports.HidD_GetPreparsedData(_handle.DangerousGetHandle(), out preparsedData))
								throw new Exception("Could not get preparsed data for HID device");

							HidImports.HIDP_CAPS caps;
							if(HidImports.HidP_GetCaps(preparsedData, out caps) != HidImports.HIDP_STATUS_SUCCESS)
								throw new Exception("Could not get CAPS for HID device");

							HidImports.HidD_FreePreparsedData(ref preparsedData);

							_inputReport = new byte[caps.InputReportByteLength];
							_outputReport = new byte[caps.OutputReportByteLength];

							// create a nice .NET FileStream wrapping the handle above
							_stream = new FileStream(_handle, FileAccess.ReadWrite, _inputReport.Length, true);

							break;
						}

						_handle.Close();
					}
				}
				else
				{
					// failed to get the detail struct
					throw new Exception("SetupDiGetDeviceInterfaceDetail failed on index " + index);
				}

				// move to the next device
				index++;
			}

			// clean up our list
			HidImports.SetupDiDestroyDeviceInfoList(hDevInfo);

			// if we didn't find a EV3, throw an exception
			if(!found)
				throw new Exception("No LEGO EV3s found in HID device list.");
		}
	}
}
