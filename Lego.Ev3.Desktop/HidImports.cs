//////////////////////////////////////////////////////////////////////////////////
//	HIDImports.cs
//	Managed Wiimote Library
//	Written by Brian Peek (http://www.brianpeek.com/)
//	for MSDN's Coding4Fun (http://msdn.microsoft.com/coding4fun/)
//	Visit http://blogs.msdn.com/coding4fun/archive/2007/03/14/1879033.aspx
//  and http://www.codeplex.com/WiimoteLib
//	for more information
//////////////////////////////////////////////////////////////////////////////////

using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Lego.Ev3.Desktop
{
	/// <summary>
	/// Win32 import information for use with the Wiimote library
	/// </summary>
	internal class HidImports
	{
		//
		// Flags controlling what is included in the device information set built
		// by SetupDiGetClassDevs
		//
		public const int DIGCF_DEFAULT          = 0x00000001; // only valid with DIGCF_DEVICEINTERFACE
		public const int DIGCF_PRESENT          = 0x00000002;
		public const int DIGCF_ALLCLASSES       = 0x00000004;
		public const int DIGCF_PROFILE          = 0x00000008;
		public const int DIGCF_DEVICEINTERFACE  = 0x00000010;

		[Flags]
		public enum EFileAttributes : uint
		{
		   Readonly         = 0x00000001,
		   Hidden           = 0x00000002,
		   System           = 0x00000004,
		   Directory        = 0x00000010,
		   Archive          = 0x00000020,
		   Device           = 0x00000040,
		   Normal           = 0x00000080,
		   Temporary        = 0x00000100,
		   SparseFile       = 0x00000200,
		   ReparsePoint2F    = 0x00000400,
		   Compressed       = 0x00000800,
		   Offline          = 0x00001000,
		   NotContentIndexed= 0x00002000,
		   Encrypted        = 0x00004000,
		   Write_Through    = 0x80000000,
		   Overlapped       = 0x40000000,
		   NoBuffering      = 0x20000000,
		   RandomAccess     = 0x10000000,
		   SequentialScan   = 0x08000000,
		   DeleteOnClose    = 0x04000000,
		   BackupSemantics  = 0x02000000,
		   PosixSemantics   = 0x01000000,
		   OpenReparsePoint2F= 0x00200000,
		   OpenNoRecall     = 0x00100000,
		   FirstPipeInstance= 0x00080000
		}

		// FACILITY_HID_ERROR_CODE == 0x11
		//#define HIDP_ERROR_CODES(SEV, CODE) ((NTSTATUS) (((SEV) << 28) | (FACILITY_HID_ERROR_CODE << 16) | (CODE)))
		//#define HIDP_STATUS_SUCCESS                  (HIDP_ERROR_CODES(0x0,0))
		//#define HIDP_STATUS_NULL                     (HIDP_ERROR_CODES(0x8,1))
		//#define HIDP_STATUS_INVALID_PREPARSED_DATA   (HIDP_ERROR_CODES(0xC,1))
		//#define HIDP_STATUS_INVALID_REPORT_TYPE      (HIDP_ERROR_CODES(0xC,2))
		//#define HIDP_STATUS_INVALID_REPORT_LENGTH    (HIDP_ERROR_CODES(0xC,3))
		//#define HIDP_STATUS_USAGE_NOT_FOUND          (HIDP_ERROR_CODES(0xC,4))
		//#define HIDP_STATUS_VALUE_OUT_OF_RANGE       (HIDP_ERROR_CODES(0xC,5))
		//#define HIDP_STATUS_BAD_LOG_PHY_VALUES       (HIDP_ERROR_CODES(0xC,6))
		//#define HIDP_STATUS_BUFFER_TOO_SMALL         (HIDP_ERROR_CODES(0xC,7))
		//#define HIDP_STATUS_INTERNAL_ERROR           (HIDP_ERROR_CODES(0xC,8))
		//#define HIDP_STATUS_I8042_TRANS_UNKNOWN      (HIDP_ERROR_CODES(0xC,9))
		//#define HIDP_STATUS_INCOMPATIBLE_REPORT_ID   (HIDP_ERROR_CODES(0xC,0xA))
		//#define HIDP_STATUS_NOT_VALUE_ARRAY          (HIDP_ERROR_CODES(0xC,0xB))
		//#define HIDP_STATUS_IS_VALUE_ARRAY           (HIDP_ERROR_CODES(0xC,0xC))
		//#define HIDP_STATUS_DATA_INDEX_NOT_FOUND     (HIDP_ERROR_CODES(0xC,0xD))
		//#define HIDP_STATUS_DATA_INDEX_OUT_OF_RANGE  (HIDP_ERROR_CODES(0xC,0xE))
		//#define HIDP_STATUS_BUTTON_NOT_PRESSED       (HIDP_ERROR_CODES(0xC,0xF))
		//#define HIDP_STATUS_REPORT_DOES_NOT_EXIST    (HIDP_ERROR_CODES(0xC,0x10))
		//#define HIDP_STATUS_NOT_IMPLEMENTED          (HIDP_ERROR_CODES(0xC,0x20))

		public const int HIDP_STATUS_SUCCESS = (0x00 << 28) | (0x11 << 16) | 0;

		[StructLayout(LayoutKind.Sequential)]
		public struct SP_DEVINFO_DATA
		{
			public uint cbSize;
			public Guid ClassGuid;
			public uint DevInst;
			public IntPtr Reserved;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct SP_DEVICE_INTERFACE_DATA
		{
			public int cbSize;
			public Guid InterfaceClassGuid;
			public int Flags;
			public IntPtr RESERVED;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct SP_DEVICE_INTERFACE_DETAIL_DATA
		{
			public UInt32 cbSize;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
			public string DevicePath;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct HIDD_ATTRIBUTES
		{
			public int Size;
			public short VendorID;
			public short ProductID;
			public short VersionNumber;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct HIDP_CAPS
		{
			public short Usage;
			public short UsagePage;
			public short InputReportByteLength;
			public short OutputReportByteLength;
			public short FeatureReportByteLength;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
			public short[] Reserved;
			public short NumberLinkCollectionNodes;
			public short NumberInputButtonCaps;
			public short NumberInputValueCaps;
			public short NumberInputDataIndices;
			public short NumberOutputButtonCaps;
			public short NumberOutputValueCaps;
			public short NumberOutputDataIndices;
			public short NumberFeatureButtonCaps;
			public short NumberFeatureValueCaps;
			public short NumberFeatureDataIndices;
		}

		[DllImport(@"hid.dll", CharSet=CharSet.Auto, SetLastError = true)]
		public static extern void HidD_GetHidGuid(out Guid gHid);

		[DllImport("hid.dll", SetLastError = true)]
		public static extern Boolean HidD_GetAttributes(IntPtr HidDeviceObject, ref HIDD_ATTRIBUTES Attributes);

		[DllImport("hid.dll", SetLastError = true)]
		public static extern bool HidD_GetPreparsedData(IntPtr hFile, out IntPtr lpData);

		[DllImport("hid.dll", SetLastError = true)]
		public static extern int HidP_GetCaps(IntPtr lpData, out HIDP_CAPS oCaps);

		[DllImport("hid.dll", SetLastError = true)]
		public static extern bool HidD_FreePreparsedData(ref IntPtr pData);

		[DllImport("hid.dll", SetLastError = true)]
		internal extern static bool HidD_SetOutputReport(
			IntPtr HidDeviceObject,
			byte[] lpReportBuffer,
			uint ReportBufferLength);

		[DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr SetupDiGetClassDevs(
			ref Guid ClassGuid,
			[MarshalAs(UnmanagedType.LPTStr)] string Enumerator,
			IntPtr hwndParent,
			UInt32 Flags
			);

		[DllImport(@"setupapi.dll", CharSet=CharSet.Auto, SetLastError = true)]
		public static extern Boolean SetupDiEnumDeviceInterfaces(
			IntPtr hDevInfo,
			//ref SP_DEVINFO_DATA devInfo,
			IntPtr devInvo,
			ref Guid interfaceClassGuid,
			Int32 memberIndex,
			ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData
		);

		[DllImport(@"setupapi.dll", SetLastError = true)]
		public static extern Boolean SetupDiGetDeviceInterfaceDetail(
			IntPtr hDevInfo,
			ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData,
			IntPtr deviceInterfaceDetailData,
			UInt32 deviceInterfaceDetailDataSize,
			out UInt32 requiredSize,
			IntPtr deviceInfoData
		);

		[DllImport(@"setupapi.dll", SetLastError = true)]
		public static extern Boolean SetupDiGetDeviceInterfaceDetail(
			IntPtr hDevInfo,
			ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData,
			ref SP_DEVICE_INTERFACE_DETAIL_DATA deviceInterfaceDetailData,
			UInt32 deviceInterfaceDetailDataSize,
			out UInt32 requiredSize,
			IntPtr deviceInfoData
		);

		[DllImport(@"setupapi.dll", CharSet=CharSet.Auto, SetLastError = true)]
		public static extern UInt16 SetupDiDestroyDeviceInfoList( IntPtr hDevInfo );

		[DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern SafeFileHandle CreateFile(
			string fileName,
			[MarshalAs(UnmanagedType.U4)] FileAccess fileAccess,
			[MarshalAs(UnmanagedType.U4)] FileShare fileShare,
			IntPtr securityAttributes,
			[MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
			[MarshalAs(UnmanagedType.U4)] EFileAttributes flags,
			IntPtr template);

			[DllImport("kernel32.dll", SetLastError=true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool CloseHandle(IntPtr hObject);
	}
}
