using System;

namespace Lego.Ev3.Core
{
	enum ArgumentSize
	{
		Byte = 0x81,	// 1 byte
		Short = 0x82,	// 2 bytes
		Int = 0x83,		// 4 bytes
		String = 0x84	// null-terminated string
	}

	enum ReplyType
	{
		DirectReply = 0x02,
		SystemReply = 0x03,
		DirectReplyError = 0x04,
		SystemReplyError = 0x05
	}

	enum Opcode
	{
		UIRead_GetFirmware = 0x810a,

		UIWrite_LED = 0x821b,

		UIButton_Pressed = 0x8309,

		UIDraw_Update = 0x8400,
		UIDraw_Clean = 0x8401,
		UIDraw_Pixel = 0x8402,
		UIDraw_Line = 0x8403,
		UIDraw_Circle = 0x8404,
		UIDraw_Text = 0x8405,
		UIDraw_FillRect = 0x8409,
		UIDraw_Rect = 0x840a,
		UIDraw_InverseRect = 0x8410,
		UIDraw_SelectFont = 0x8411,
		UIDraw_Topline = 0x8412,
		UIDraw_FillWindow = 0x8413,
		UIDraw_DotLine = 0x8415,
		UIDraw_FillCircle = 0x8418,
		UIDraw_BmpFile = 0x841c,

		Sound_Break = 0x9400,
		Sound_Tone = 0x9401,
		Sound_Play = 0x9402,
		Sound_Repeat = 0x9403,
		Sound_Service = 0x9404,

		InputDevice_GetTypeMode = 0x9905,
		InputDevice_GetDeviceName = 0x9915,
		InputDevice_GetModeName = 0x9916,
		InputDevice_ReadyPct = 0x991b,
		InputDevice_ReadyRaw = 0x991c,
		InputDevice_ReadySI = 0x991d,
		InputDevice_ClearAll = 0x990a,
		InputDevice_ClearChanges = 0x991a,

		InputRead = 0x9a,
		InputReadExt = 0x9e,
		InputReadSI = 0x9d,

		OutputStop = 0xa3,
		OutputPower = 0xa4,
		OutputSpeed = 0xa5,
		OutputStart = 0xa6,
		OutputPolarity = 0xa7,
		OutputReady = 0xaa,
		OutputStepPower = 0xac,
		OutputTimePower = 0xad,
		OutputStepSpeed = 0xae,
		OutputTimeSpeed = 0xaf,
		OutputStepSync = 0xb0,
		OutputTimeSync = 0xb1,

		Tst = 0xff,
	}

	enum SystemOpcode
	{
		BeginDownload = 0x92,
		ContinueDownload = 0x93,
		CloseFileHandle = 0x98,
		CreateDirectory = 0x9b,
		DeleteFile = 0x9c
	}

	enum SystemReplyStatus
	{
		Success,
		UnknownHandle,
		HandleNotReady,
		CorruptFile,
		NoHandlesAvailable,
		NoPermission,
		IllegalPath,
		FileExists,
		EndOfFile,
		SizeError,
		UnknownError,
		IllegalFilename,
		IllegalConnection
	}

	/// <summary>
	/// The type of command being sent to the brick
	/// </summary>
	public enum CommandType
	{
		/// <summary>
		/// Direct command with a reply expected
		/// </summary>
		DirectReply = 0x00,
		/// <summary>
		/// Direct command with no reply
		/// </summary>
		DirectNoReply = 0x80,

		/// <summary>
		///  System command with a reply expected
		/// </summary>
		SystemReply = 0x01,
		/// <summary>
		/// System command with no reply
		/// </summary>
		SystemNoReply = 0x81
	}

	/// <summary>
	/// Format for sensor data.
	/// </summary>
	public enum Format
	{
		/// <summary>
		/// Percentage
		/// </summary>
		Percent = 0x10,
		/// <summary>
		/// Raw
		/// </summary>
		Raw = 0x11,
		/// <summary>
		/// International System of Units
		/// </summary>
		SI = 0x12
	}

	/// <summary>
	/// Polarity/direction to turn the motor
	/// </summary>
	public enum Polarity
	{
		/// <summary>
		/// Turn backward
		/// </summary>
		Backward = -1,
		/// <summary>
		/// Turn in the opposite direction
		/// </summary>
		Opposite = 0,
		/// <summary>
		/// Turn forward
		/// </summary>
		Forward = 1,
	}

	/// <summary>
	/// Ports which can receive input data
	/// </summary>
	public enum InputPort
	{
		/// <summary>
		/// Port 1
		/// </summary>
		One		= 0x00,
		/// <summary>
		/// Port 2
		/// </summary>
		Two		= 0x01,
		/// <summary>
		/// Port 3
		/// </summary>
		Three	= 0x02,
		/// <summary>
		/// Port 4
		/// </summary>
		Four	= 0x03,

		/// <summary>
		/// Port A
		/// </summary>
		A		= 0x10,
		/// <summary>
		/// Port B
		/// </summary>
		B		= 0x11,
		/// <summary>
		/// Port C
		/// </summary>
		C		= 0x12,
		/// <summary>
		/// Port D
		/// </summary>
		D		= 0x13,
	}

	/// <summary>
	/// Ports which can send output
	/// </summary>
	[Flags]
	public enum OutputPort
	{
		/// <summary>
		/// Port A
		/// </summary>
		A	= 0x01,
		/// <summary>
		/// Port B
		/// </summary>
		B	= 0x02,
		/// <summary>
		/// Port C
		/// </summary>
		C	= 0x04,
		/// <summary>
		/// Port D
		/// </summary>
		D	= 0x08,
		/// <summary>
		/// Ports A,B,C and D simultaneously
		/// </summary>
		All	= 0x0f
	}

	/// <summary>
	/// List of devices which can be recognized as input or output devices
	/// </summary>
	public enum DeviceType
	{
		// NXT devices

		/// <summary>
		/// NXT Touch sensor
		/// </summary>
		NxtTouch = 1,
		/// <summary>
		/// NXT Light sensor
		/// </summary>
		NxtLight = 2,
		/// <summary>
		/// NXT Sound sensor
		/// </summary>
		NxtSound = 3,
		/// <summary>
		/// NXT Color sensor
		/// </summary>
		NxtColor = 4,
		/// <summary>
		/// NXT Ultrasonic sensor
		/// </summary>
		NxtUltrasonic = 5,
		/// <summary>
		///  NXT Temperature sensor
		/// </summary>
		NxtTemperature = 6,

		// 2 motors
		/// <summary>
		/// Large motor
		/// </summary>
		LMotor = 7,
		/// <summary>
		/// Medium motor
		/// </summary>
		MMotor = 8,

		// Ev3 devices
		/// <summary>
		/// EV3 Touch sensor
		/// </summary>
		Touch = 16,
		/// <summary>
		/// EV3 Color sensor
		/// </summary>
		Color = 29,
		/// <summary>
		/// EV3 Ultrasonic sensor
		/// </summary>
		Ultrasonic = 30,
		/// <summary>
		/// EV3 Gyroscope sensor
		/// </summary>
		Gyroscope = 32,
		/// <summary>
		/// EV3 IR sensor
		/// </summary>
		Infrared = 33,

		// other
		/// <summary>
		/// Sensor is initializing
		/// </summary>
		Initializing = 0x7d,
		/// <summary>
		/// Port is empty
		/// </summary>
		Empty = 0x7e,
		/// <summary>
		/// Sensor is plugged into a motor port, or vice-versa
		/// </summary>
		WrongPort = 0x7f,
		/// <summary>
		/// Unknown sensor/status
		/// </summary>
		Unknown = 0xff
	}

	/// <summary>
	/// Buttons on the face of the EV3 brick
	/// </summary>
	public enum BrickButton
	{
		/// <summary>
		/// No button
		/// </summary>
		None,
		/// <summary>
		/// Up button
		/// </summary>
		Up,
		/// <summary>
		/// Enter button
		/// </summary>
		Enter,
		/// <summary>
		/// Down button
		/// </summary>
		Down,
		/// <summary>
		/// Right button
		/// </summary>
		Right,
		/// <summary>
		/// Left button
		/// </summary>
		Left,
		/// <summary>
		/// Back button
		/// </summary>
		Back,
		/// <summary>
		/// Any button
		/// </summary>
		Any
	}

	/// <summary>
	/// Pattern to light up the EV3 brick's LED
	/// </summary>
	public enum LedPattern
	{
		/// <summary>
		/// LED off
		/// </summary>
		Black,
		/// <summary>
		/// Solid green
		/// </summary>
		Green,
		/// <summary>
		/// Solid red
		/// </summary>
		Red,
		/// <summary>
		/// Solid orange
		/// </summary>
		Orange,
		/// <summary>
		/// Flashing green
		/// </summary>
		GreenFlash,
		/// <summary>
		/// Flashing red
		/// </summary>
		RedFlash,
		/// <summary>
		/// Flashing orange
		/// </summary>
		OrangeFlash,
		/// <summary>
		/// Pulsing green
		/// </summary>
		GreenPulse,
		/// <summary>
		/// Pulsing red
		/// </summary>
		RedPulse,
		/// <summary>
		/// Pulsing orange
		/// </summary>
		OrangePulse
	}

	/// <summary>
	/// UI colors
	/// </summary>
	public enum Color
	{
		/// <summary>
		/// Color of the background
		/// </summary>
		Background,
		/// <summary>
		/// Color of the foreground
		/// </summary>
		Foreground
	}

	/// <summary>
	/// Font types for drawing text to the screen
	/// </summary>
	public enum FontType
	{
		/// <summary>
		/// Small font
		/// </summary>
		Small,
		/// <summary>
		/// Medium font
		/// </summary>
		Medium,
		/// <summary>
		/// Large font
		/// </summary>
		Large
	}

	/// <summary>
	/// NXT and EV3 Touch Sensor mode
	/// </summary>
	public enum TouchMode
	{
		/// <summary>
		/// On when pressed, off when released
		/// </summary>
		Touch,
		/// <summary>
		/// Running counter of number of presses
		/// </summary>
		Bumps
	}

	/// <summary>
	/// NXT Light Sensor mode
	/// </summary>
	public enum NxtLightMode
	{
		/// <summary>
		/// Amount of reflected light
		/// </summary>
		Reflect,
		/// <summary>
		/// Amoutn of ambient light
		/// </summary>
		Ambient
	}

	/// <summary>
	/// NXT Sound Sensor mode
	/// </summary>
	public enum NxtSoundMode
	{
		/// <summary>
		/// Decibels
		/// </summary>
		Decibels,
		/// <summary>
		/// Adjusted Decibels
		/// </summary>
		AdjustedDecibels
	}

	/// <summary>
	/// NXT Color Sensor mode
	/// </summary>
	public enum NxtColorMode
	{
		/// <summary>
		/// Reflected color
		/// </summary>
		Reflective,
		/// <summary>
		/// Ambient color
		/// </summary>
		Ambient,
		/// <summary>
		/// Specific color
		/// </summary>
		Color,
		/// <summary>
		/// Amount of green
		/// </summary>
		Green,
		/// <summary>
		/// Amount of blue
		/// </summary>
		Blue,
		/// <summary>
		/// Raw sensor value
		/// </summary>
		Raw
	}

	/// <summary>
	/// NXT Ultrasonic Sensor mode
	/// </summary>
	public enum NxtUltrasonicMode
	{
		/// <summary>
		/// Values in centimeter units
		/// </summary>
		Centimeters,
		/// <summary>
		/// Values in inch units
		/// </summary>
		Inches
	}

	/// <summary>
	/// NXT Temperature Sensor mode
	/// </summary>
	public enum NxtTemperatureMode
	{
		/// <summary>
		/// Values in Celsius units
		/// </summary>
		Celsius,
		/// <summary>
		/// Values in Fahrenheit units
		/// </summary>
		Fahrenheit,
	}

	/// <summary>
	/// Motor mode
	/// </summary>
	public enum MotorMode
	{
		/// <summary>
		/// Values in degrees
		/// </summary>
		Degrees,
		/// <summary>
		/// Values in rotations
		/// </summary>
		Rotations,
		/// <summary>
		/// Values in percentage
		/// </summary>
		Percent
	}

	/// <summary>
	/// EV3 Color Sensor mode
	/// </summary>
	public enum ColorMode
	{
		/// <summary>
		/// Reflected color
		/// </summary>
		Reflective,
		/// <summary>
		/// Ambient color
		/// </summary>
		Ambient,
		/// <summary>
		/// Specific color
		/// </summary>
		Color,
		/// <summary>
		/// Reflected color raw value
		/// </summary>
		ReflectiveRaw,
		/// <summary>
		/// Reflected color RGB value
		/// </summary>
		ReflectiveRgb,
		/// <summary>
		/// Calibration
		/// </summary>
		Calibration // TODO: ??
	}

	/// <summary>
	/// EV3 Ultrasonic Sensor mode
	/// </summary>
	public enum UltrasonicMode
	{
		/// <summary>
		/// Values in centimeter units
		/// </summary>
		Centimeters,
		/// <summary>
		/// Values in inch units
		/// </summary>
		Inches,
		/// <summary>
		/// Listen mode
		/// </summary>
		Listen,
		/// <summary>
		/// Unknown
		/// </summary>
		SiCentimeters,
		/// <summary>
		/// Unknown
		/// </summary>
		SiInches,
		/// <summary>
		/// Unknown
		/// </summary>
		DcCentimeters,	// TODO: DC?
		/// <summary>
		/// Unknown
		/// </summary>
		DcInches		// TODO: DC?
	}

	/// <summary>
	/// EV3 Gyroscope Sensor mode
	/// </summary>
	public enum GyroscopeMode
	{
		/// <summary>
		/// Angle
		/// </summary>
		Angle,
		/// <summary>
		/// Rate of movement
		/// </summary>
		Rate,
		/// <summary>
		/// Unknown
		/// </summary>
		Fas,		// TOOD: ??
		/// <summary>
		/// Unknown
		/// </summary>
		GandA,		// TODO: ??
		/// <summary>
		/// Calibrate
		/// </summary>
		Calibrate
	}

	/// <summary>
	/// EV3 Infrared Sensor mode
	/// </summary>
	public enum InfraredMode
	{
		/// <summary>
		/// Proximity
		/// </summary>
		Proximity,
		/// <summary>
		/// Seek
		/// </summary>
		Seek,
		/// <summary>
		/// EV3 remote control
		/// </summary>
		Remote,
		/// <summary>
		/// Unknown
		/// </summary>
		RemoteA,	// TODO: ??
		/// <summary>
		/// Unknown
		/// </summary>
		SAlt,		// TODO: ??
		/// <summary>
		///  Calibrate
		/// </summary>
		Calibrate
	}

	/// <summary>
	/// Values returned by the color sensor
	/// </summary>
	public enum ColorSensorColor
	{
		/// <summary>
		/// Transparent
		/// </summary>
		Transparent,
		/// <summary>
		/// Black
		/// </summary>
		Black,
		/// <summary>
		/// Blue
		/// </summary>
		Blue,
		/// <summary>
		/// Green
		/// </summary>
		Green,
		/// <summary>
		/// Yellow
		/// </summary>
		Yellow,
		/// <summary>
		/// Red
		/// </summary>
		Red,
		/// <summary>
		/// White
		/// </summary>
		White,
		/// <summary>
		/// Brown
		/// </summary>
		Brown
	}
}
