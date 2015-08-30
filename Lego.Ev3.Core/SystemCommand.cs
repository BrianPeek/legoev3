using System;
using System.IO;
using System.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;

#if WINRT
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.Storage.Streams;
#elif WINDOWS_PHONE
using System.IO.IsolatedStorage;
#endif

namespace Lego.Ev3.Core
{
	/// <summary>
	/// Direct commands for the EV3 brick
	/// </summary>
	public sealed class SystemCommand
	{
		private readonly Brick _brick;

		internal SystemCommand(Brick brick)
		{
			_brick = brick;
		}

		/// <summary>
		/// Write a file to the EV3 brick
		/// </summary>
		/// <param name="data">Data to write.</param>
		/// <param name="devicePath">Destination path on the brick.</param>
		/// <returns></returns>
		///	<remarks>devicePath is relative from "lms2012/sys" on the EV3 brick.  Destination folders are automatically created if provided in the path.  The path must start with "apps", "prjs", or "tools".</remarks>
		public 
#if WINRT
		IAsyncAction
#else
		Task
#endif
		WriteFileAsync([ReadOnlyArray]byte[] data, string devicePath)
		{
			return WriteFileAsyncInternal(data, devicePath)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}

#if !ANDROID
		/// <summary>
		/// Copy a local file to the EV3 brick
		/// </summary>
		/// <param name="localPath">Source path on the computer.</param>
		/// <param name="devicePath">Destination path on the brick.</param>
		/// <returns></returns>
		///	<remarks>devicePath is relative from "lms2012/sys" on the EV3 brick.  Destination folders are automatically created if provided in the path.  The path must start with "apps", "prjs", or "tools".</remarks>
		public 
#if WINRT
		IAsyncAction
#else
		Task
#endif
		CopyFileAsync(string localPath, string devicePath)
		{
			return CopyFileAsyncInternal(localPath, devicePath)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}
#endif

		/// <summary>
		/// Create a directory on the EV3 brick
		/// </summary>
		/// <param name="devicePath">Destination path on the brick.</param>
		/// <returns></returns>
		///	<remarks>devicePath is relative from "lms2012/sys" on the EV3 brick.  Destination folders are automatically created if provided in the path.  The path must start with "apps", "prjs", or "tools".</remarks>
		public 
#if WINRT
		IAsyncAction
#else
		Task
#endif
		CreateDirectoryAsync(string devicePath)
		{
			return CreateDirectoryAsyncInternal(devicePath)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}

		/// <summary>
		/// Delete file from the EV3 brick
		/// </summary>
		/// <param name="devicePath">Destination path on the brick.</param>
		/// <returns></returns>
		/// <remarks>devicePath is relative from "lms2012/sys" on the EV3 brick.  The path must start with "apps", "prjs", or "tools".</remarks>
		public 
#if WINRT
		IAsyncAction
#else
		Task
#endif
		DeleteFileAsync(string devicePath)
		{
			return DeleteFileAsyncInternal(devicePath)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}

		internal async Task DeleteFileAsyncInternal(string devicePath)
		{
			Response r = ResponseManager.CreateResponse();
			Command c = new Command(CommandType.SystemReply);
			c.DeleteFile(devicePath);
			await _brick.SendCommandAsyncInternal(c);
			if(r.SystemReplyStatus != SystemReplyStatus.Success)
				throw new Exception("Error deleting file: " + r.SystemReplyStatus);
		}

		internal async Task CreateDirectoryAsyncInternal(string devicePath)
		{
			Response r = ResponseManager.CreateResponse();
			Command c = new Command(CommandType.SystemReply);
			c.CreateDirectory(devicePath);
			await _brick.SendCommandAsyncInternal(c);
			if(r.SystemReplyStatus != SystemReplyStatus.Success)
				throw new Exception("Error creating directory: " + r.SystemReplyStatus);
		}

		internal async Task CopyFileAsyncInternal(string localPath, string devicePath)
		{
			byte[] data = await GetFileContents(localPath);
			await WriteFileAsyncInternal(data, devicePath);
		}

		internal async Task WriteFileAsyncInternal(byte[] data, string devicePath)
		{
			const int chunkSize = 960;

			Command commandBegin = new Command(CommandType.SystemReply);
			commandBegin.AddOpcode(SystemOpcode.BeginDownload);
			commandBegin.AddRawParameter((uint)data.Length);
			commandBegin.AddRawParameter(devicePath);

			await _brick.SendCommandAsyncInternal(commandBegin);
			if(commandBegin.Response.SystemReplyStatus != SystemReplyStatus.Success)
				throw new Exception("Could not begin file save: " + commandBegin.Response.SystemReplyStatus);

			byte handle = commandBegin.Response.Data[0];
			int sizeSent = 0;

			while(sizeSent < data.Length)
			{
				Command commandContinue = new Command(CommandType.SystemReply);
				commandContinue.AddOpcode(SystemOpcode.ContinueDownload);
				commandContinue.AddRawParameter(handle);
				int sizeToSend = Math.Min(chunkSize, data.Length - sizeSent);
				commandContinue.AddRawParameter(data, sizeSent, sizeToSend);
				sizeSent += sizeToSend;

				await _brick.SendCommandAsyncInternal(commandContinue);
				if(commandContinue.Response.SystemReplyStatus != SystemReplyStatus.Success &&
					(commandContinue.Response.SystemReplyStatus != SystemReplyStatus.EndOfFile && sizeSent == data.Length))
					throw new Exception("Error saving file: " + commandContinue.Response.SystemReplyStatus);
			}

			//Command commandClose = new Command(CommandType.SystemReply);
			//commandClose.AddOpcode(SystemOpcode.CloseFileHandle);
			//commandClose.AddRawParameter(handle);
			//await _brick.SendCommandAsyncInternal(commandClose);
			//if(commandClose.Response.SystemReplyStatus != SystemReplyStatus.Success)
			//	throw new Exception("Could not close handle: " + commandClose.Response.SystemReplyStatus);
		}

#if WINRT
		private async Task<byte[]> GetFileContents(string localPath)
		{
			StorageFile sf = await StorageFile.GetFileFromPathAsync(localPath);
			IBuffer buffer = await FileIO.ReadBufferAsync(sf);
			return buffer.ToArray();
		}
#elif WINDOWS_PHONE
		private async Task<byte[]> GetFileContents(string localPath)
		{
			IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication();
			IsolatedStorageFileStream stream = isf.OpenFile(localPath, FileMode.Open);

			byte[] data = new byte[stream.Length];

			await stream.ReadAsync(data, 0, data.Length);

			return data;
		}
#elif ANDROID
		private Task<byte[]> GetFileContents(string localPath)
		{
			throw new NotImplementedException("On Android, please use WriteFileAsync instead of CopyFileAsync");
		}
#else
		private Task<byte[]> GetFileContents(string localPath)
		{
			return Task.FromResult(File.ReadAllBytes(localPath));
		}
#endif
	}
}
