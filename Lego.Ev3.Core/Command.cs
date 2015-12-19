using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;

#if WINRT
using Windows.Foundation.Metadata;
using Windows.Foundation;
using Windows.Storage.Streams;
#endif

namespace Lego.Ev3.Core
{
	/// <summary>
	/// Command or chain of commands to be written to the EV3 brick
	/// </summary>
	public sealed class Command
	{
		private BinaryWriter _writer;
		private MemoryStream _stream;
		private readonly Brick _brick;

		internal CommandType CommandType { get; set; }

		internal Response Response { get; set; }

		internal Command(Brick brick) : this(CommandType.DirectNoReply)
		{
			_brick = brick;
		}

		internal Command(CommandType commandType) : this(commandType, 0, 0)
		{
		}

		internal Command(CommandType commandType, ushort globalSize, int localSize)
		{
			Initialize(commandType, globalSize, localSize);
		}

		/// <summary>
		/// Start a new command of a specific type
		/// </summary>
		/// <param name="commandType">The type of the command to start</param>
		public void Initialize(CommandType commandType)
		{
			Initialize(commandType, 0, 0);
		}

		/// <summary>
		/// Start a new command of a speicifc type with a global and/or local buffer on the EV3 brick
		/// </summary>
		/// <param name="commandType">The type of the command to start</param>
		/// <param name="globalSize">The size of the global buffer in bytes (maximum of 1024 bytes)</param>
		/// <param name="localSize">The size of the local buffer in bytes (maximum of 64 bytes)</param>
		public void Initialize(CommandType commandType, ushort globalSize, int localSize)
		{
			if(globalSize > 1024)
				throw new ArgumentException("Global buffer must be less than 1024 bytes", "globalSize");
			if(localSize > 64)
				throw new ArgumentException("Local buffer must be less than 64 bytes", "localSize");

			_stream = new MemoryStream();
			_writer = new BinaryWriter(_stream);
			Response = ResponseManager.CreateResponse();

			CommandType = commandType;

			// 2 bytes (this gets filled in later when the user calls ToBytes())
			_writer.Write((ushort)0xffff);

			// 2 bytes
			_writer.Write(Response.Sequence);

			// 1 byte
			_writer.Write((byte)commandType);

			if(commandType == CommandType.DirectReply || commandType == CommandType.DirectNoReply)
			{
				// 2 bytes (llllllgg gggggggg)
				_writer.Write((byte)globalSize); // lower bits of globalSize
				_writer.Write((byte)((localSize << 2) | (globalSize >> 8) & 0x03)); // upper bits of globalSize + localSize
			}
		}

		internal void AddOpcode(Opcode opcode)
		{
			// 1 or 2 bytes (opcode + subcmd, if applicable)
			// I combined opcode + sub into ushort where applicable, so we need to pull them back apart here
			if(opcode > Opcode.Tst)
				_writer.Write((byte)((ushort)opcode >> 8));
			_writer.Write((byte)opcode);
		}

		internal void AddOpcode(SystemOpcode opcode)
		{
			_writer.Write((byte)opcode);
		}

		internal void AddGlobalIndex(byte index)
		{
			// 0xe1 = global index, long format, 1 byte
			_writer.Write((byte)(0xe1));
			_writer.Write(index);
		}

		internal void AddParameter(byte parameter)
		{
			// 0x81 = long format, 1 byte
			_writer.Write((byte)ArgumentSize.Byte);
			_writer.Write(parameter);
		}

		internal void AddParameter(short parameter)
		{
			// 0x82 = long format, 2 bytes
			_writer.Write((byte)ArgumentSize.Short);
			_writer.Write(parameter);
		}

		internal void AddParameter(ushort parameter)
		{
			// 0x82 = long format, 2 bytes
			_writer.Write((byte)ArgumentSize.Short);
			_writer.Write(parameter);
		}

		internal void AddParameter(uint parameter)
		{
			// 0x83 = long format, 4 bytes
			_writer.Write((byte)ArgumentSize.Int);
			_writer.Write(parameter);
		}

		internal void AddParameter(string s)
		{
			// 0x84 = long format, null terminated string
			_writer.Write((byte)ArgumentSize.String);
			byte[] bytes = Encoding.UTF8.GetBytes(s);
			_writer.Write(bytes);
			_writer.Write((byte)0x00);
		}

		// Raw methods below don't get format specifier added prior to the data itself...these are used in system commands (only?)
		internal void AddRawParameter(byte parameter)
		{
			_writer.Write(parameter);
		}

		internal void AddRawParameter(ushort parameter)
		{
			_writer.Write(parameter);
		}

		internal void AddRawParameter(uint parameter)
		{
			_writer.Write(parameter);
		}

		internal void AddRawParameter(string s)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(s);
			_writer.Write(bytes);
			_writer.Write((byte)0x00);
		}

		internal void AddRawParameter(byte[] data, int index, int count)
		{
			_writer.Write(data, index, count);
		}
		
		internal byte[] ToBytes()
		{
			byte[] buff = _stream.ToArray();

			// size of data, not including the 2 size bytes
			ushort size = (ushort)(buff.Length - 2);

			// little-endian
			buff[0] = (byte)size;
			buff[1] = (byte)(size >> 8);

			return buff;
		}

		/// <summary>
		/// Start the motor(s) based on previous commands
		/// </summary>
		/// <param name="ports">Port or ports to apply the command to.</param>
		public void StartMotor(OutputPort ports)
		{
			AddOpcode(Opcode.OutputStart);
			AddParameter(0x00);			// layer
			AddParameter((byte)ports);	// ports
		}

		/// <summary>
		/// Turns the specified motor at the specified power
		/// </summary>
		/// <param name="ports">Port or ports to apply the command to.</param>
		/// <param name="power">The amount of power to apply to the specified motors (-100% to 100%).</param>
		public void TurnMotorAtPower(OutputPort ports, int power)
		{
			if(power < -100 || power > 100)
				throw new ArgumentException("Power must be between -100 and 100 inclusive.", "power");

			AddOpcode(Opcode.OutputPower);
			AddParameter(0x00);			// layer
			AddParameter((byte)ports);	// ports
			AddParameter((byte)power);	// power
		}

		/// <summary>
		/// Turn the specified motor at the specified speed.
		/// </summary>
		/// <param name="ports">Port or ports to apply the command to.</param>
		/// <param name="speed">The speed to apply to the specified motors (-100% to 100%).</param>
		public void TurnMotorAtSpeed(OutputPort ports, int speed)
		{
			if(speed < -100 || speed > 100)
				throw new ArgumentException("Speed must be between -100 and 100 inclusive.", "speed");

			AddOpcode(Opcode.OutputSpeed);
			AddParameter(0x00);			// layer
			AddParameter((byte)ports);	// ports
			AddParameter((byte)speed);		// speed
		}

		/// <summary>
		/// Step the motor connected to the specified port or ports at the specified power for the specified number of steps.
		/// </summary>
		/// <param name="ports">A specific port or Ports.All.</param>
		/// <param name="power">The power at which to turn the motor (-100% to 100%).</param>
		/// <param name="steps">The number of steps to turn the motor.</param>
		/// <param name="brake">Apply brake to motor at end of routine.</param>
		public void StepMotorAtPower(OutputPort ports, int power, uint steps, bool brake)
		{
			StepMotorAtPower(ports, power, 0, steps, 10, brake);
		}

		/// <summary>
		/// Step the motor connected to the specified port or ports at the specified power for the specified number of steps.
		/// </summary>
		/// <param name="ports">A specific port or Ports.All.</param>
		/// <param name="power">The power at which to turn the motor (-100% to 100%).</param>
		/// <param name="rampUpSteps"></param>
		/// <param name="constantSteps"></param>
		/// <param name="rampDownSteps"></param>
		/// <param name="brake">Apply brake to motor at end of routine.</param>
		public void StepMotorAtPower(OutputPort ports, int power, uint rampUpSteps, uint constantSteps, uint rampDownSteps, bool brake)
		{
			if(power < -100 || power > 100)
				throw new ArgumentException("Power must be between -100 and 100 inclusive.", "power");

			AddOpcode(Opcode.OutputStepPower);
			AddParameter(0x00);			// layer
			AddParameter((byte)ports);	// ports
			AddParameter((byte)power);			// power
			AddParameter(rampUpSteps);	// step1
			AddParameter(constantSteps);	// step2
			AddParameter(rampDownSteps);	// step3
			AddParameter((byte)(brake ? 0x01 : 0x00));		// brake (0 = coast, 1 = brake)
		}

		/// <summary>
		/// Step the motor connected to the specified port or ports at the specified speed for the specified number of steps.
		/// </summary>
		/// <param name="ports">A specific port or Ports.All.</param>
		/// <param name="speed">The speed at which to turn the motor (-100% to 100%).</param>
		/// <param name="steps"></param>
		/// <param name="brake">Apply brake to motor at end of routine.</param>
		public void StepMotorAtSpeed(OutputPort ports, int speed, uint steps, bool brake)
		{
			StepMotorAtSpeed(ports, speed, 0, steps, 0, brake);
		}

		/// <summary>
		/// Step the motor connected to the specified port or ports at the specified speed for the specified number of steps.
		/// </summary>
		/// <param name="ports">A specific port or Ports.All.</param>
		/// <param name="speed">The speed at which to turn the motor (-100% to 100%).</param>
		/// <param name="rampUpSteps"></param>
		/// <param name="constantSteps"></param>
		/// <param name="rampDownSteps"></param>
		/// <param name="brake">Apply brake to motor at end of routine.</param>
		public void StepMotorAtSpeed(OutputPort ports, int speed, uint rampUpSteps, uint constantSteps, uint rampDownSteps, bool brake)
		{
			if(speed < -100 || speed > 100)
				throw new ArgumentException("Speed must be between -100 and 100 inclusive.", "speed");

			AddOpcode(Opcode.OutputStepSpeed);
			AddParameter(0x00);			// layer
			AddParameter((byte)ports);	// ports
			AddParameter((byte)speed);			// speed
			AddParameter(rampUpSteps);	// step1
			AddParameter(constantSteps);	// step2
			AddParameter(rampDownSteps);	// step3
			AddParameter((byte)(brake ? 0x01 : 0x00));		// brake (0 = coast, 1 = brake)
		}

		/// <summary>
		/// Turn the motor connected to the specified port or ports at the specified power for the specified times.
		/// </summary>
		/// <param name="ports">A specific port or Ports.All.</param>
		/// <param name="power">The power at which to turn the motor (-100% to 100%).</param>
		/// <param name="milliseconds">Number of milliseconds to run at constant power.</param>
		/// <param name="brake">Apply brake to motor at end of routine.</param>
		/// <returns></returns>
		public void TurnMotorAtPowerForTime(OutputPort ports, int power, uint milliseconds, bool brake)
		{
			TurnMotorAtPowerForTime(ports, power, 0, milliseconds, 0, brake);
		}

		/// <summary>
		/// Turn the motor connected to the specified port or ports at the specified power for the specified times.
		/// </summary>
		/// <param name="ports">A specific port or Ports.All.</param>
		/// <param name="power">The power at which to turn the motor (-100% to 100%).</param>
		/// <param name="msRampUp">Number of milliseconds to get up to power.</param>
		/// <param name="msConstant">Number of milliseconds to run at constant power.</param>
		/// <param name="msRampDown">Number of milliseconds to power down to a stop.</param>
		/// <param name="brake">Apply brake to motor at end of routine.</param>
		/// <returns></returns>
		public void TurnMotorAtPowerForTime(OutputPort ports, int power, uint msRampUp, uint msConstant, uint msRampDown, bool brake)
		{
			if(power < -100 || power > 100)
				throw new ArgumentException("Power must be between -100 and 100 inclusive.", "power");

			AddOpcode(Opcode.OutputTimePower);
			AddParameter(0x00);			// layer
			AddParameter((byte)ports);	// ports
			AddParameter((byte)power);	// power
			AddParameter(msRampUp);		// step1
			AddParameter(msConstant);	// step2
			AddParameter(msRampDown);	// step3
			AddParameter((byte)(brake ? 0x01 : 0x00));		// brake (0 = coast, 1 = brake)
		}

		/// <summary>
		/// Turn the motor connected to the specified port or ports at the specified speed for the specified times.
		/// </summary>
		/// <param name="ports">A specific port or Ports.All.</param>
		/// <param name="speed">The power at which to turn the motor (-100% to 100%).</param>
		/// <param name="milliseconds">Number of milliseconds to run at constant speed.</param>
		/// <param name="brake">Apply brake to motor at end of routine.</param>
		/// <returns></returns>
		public void TurnMotorAtSpeedForTime(OutputPort ports, int speed, uint milliseconds, bool brake)
		{
			TurnMotorAtSpeedForTime(ports, speed, 0, milliseconds, 0, brake);
		}

		/// <summary>
		/// Turn the motor connected to the specified port or ports at the specified speed for the specified times.
		/// </summary>
		/// <param name="ports">A specific port or Ports.All.</param>
		/// <param name="speed">The power at which to turn the motor (-100% to 100%).</param>
		/// <param name="msRampUp">Number of milliseconds to get up to speed.</param>
		/// <param name="msConstant">Number of milliseconds to run at constant speed.</param>
		/// <param name="msRampDown">Number of milliseconds to slow down to a stop.</param>
		/// <param name="brake">Apply brake to motor at end of routine.</param>
		/// <returns></returns>
		public void TurnMotorAtSpeedForTime(OutputPort ports, int speed, uint msRampUp, uint msConstant, uint msRampDown, bool brake)
		{
			if(speed < -100 || speed > 100)
				throw new ArgumentException("Speed must be between -100 and 100 inclusive.", "speed");

			AddOpcode(Opcode.OutputTimeSpeed);
			AddParameter(0x00);			// layer
			AddParameter((byte)ports);	// ports
			AddParameter((byte)speed);			// power
			AddParameter(msRampUp);		// step1
			AddParameter(msConstant);		// step2
			AddParameter(msRampDown);		// step3
			AddParameter((byte)(brake ? 0x01 : 0x00));		// brake (0 = coast, 1 = brake)
		}

		/// <summary>
		/// Append the Set Polarity command to an existing Command object
		/// </summary>
		/// <param name="ports">Port or ports to change polarity</param>
		/// <param name="polarity">The new polarity (direction) value</param>
		public void SetMotorPolarity(OutputPort ports, Polarity polarity)
		{
			AddOpcode(Opcode.OutputPolarity);
			AddParameter(0x00);
			AddParameter((byte)ports);
			AddParameter((byte)polarity);
		}

		/// <summary>
		/// Synchronize stepping of motors.
		/// </summary>
		/// <param name="ports">The port or ports to which the stop command will be sent.</param>
		/// <param name="speed">Speed to turn the motor(s). (-100 to 100)</param>
		/// <param name="turnRatio">The turn ratio to apply. (-200 to 200)</param>
		/// <param name="step">The number of steps to turn the motor(s).</param>
		/// <param name="brake">Brake or coast at the end.</param>
		/// <returns></returns>
		public void StepMotorSync(OutputPort ports, int speed, short turnRatio, uint step, bool brake)
		{
			if(speed < -100 || speed > 100)
				throw new ArgumentException("Speed must be between -100 and 100", "speed");

			if(turnRatio < -200 || turnRatio > 200)
				throw new ArgumentException("Turn ratio must be between -200 and 200", "turnRatio");

			AddOpcode(Opcode.OutputStepSync);
			AddParameter(0x00);
			AddParameter((byte)ports);
			AddParameter((byte)speed);
			AddParameter(turnRatio);
			AddParameter(step);
			AddParameter((byte)(brake ? 0x01 : 0x00));		// brake (0 = coast, 1 = brake)
		}

		/// <summary>
		/// Synchronize timing of motors.
		/// </summary>
		/// <param name="ports">The port or ports to which the stop command will be sent.</param>
		/// <param name="speed">Speed to turn the motor(s). (-100 to 100)</param>
		/// <param name="turnRatio">The turn ratio to apply. (-200 to 200)</param>
		/// <param name="time">The time to turn the motor(s).</param>
		/// <param name="brake">Brake or coast at the end.</param>
		/// <returns></returns>
		public void TimeMotorSync(OutputPort ports, int speed, short turnRatio, uint time, bool brake)
		{
			if(speed < -100 || speed > 100)
				throw new ArgumentException("Speed must be between -100 and 100", "speed");

			if(turnRatio < -200 || turnRatio > 200)
				throw new ArgumentException("Turn ratio must be between -200 and 200", "turnRatio");

			AddOpcode(Opcode.OutputTimeSync);
			AddParameter(0x00);
			AddParameter((byte)ports);
			AddParameter((byte)speed);
			AddParameter(turnRatio);
			AddParameter(time);
			AddParameter((byte)(brake ? 0x01 : 0x00));		// brake (0 = coast, 1 = brake)
		}

		/// <summary>
		/// Append the Stop Motor command to an existing Command object
		/// </summary>
		/// <param name="ports">Port or ports to stop</param>
		/// <param name="brake">Apply the brake at the end of the command</param>
		public void StopMotor(OutputPort ports, bool brake)
		{
			AddOpcode(Opcode.OutputStop);
			AddParameter(0x00);			// layer
			AddParameter((byte)ports);	// ports
			AddParameter((byte)(brake ? 0x01 : 0x00));		// brake (0 = coast, 1 = brake)
		}

		/// <summary>
		/// Append the Clear All Devices command to an existing Command object
		/// </summary>
		public void ClearAllDevices()
		{
			AddOpcode(Opcode.InputDevice_ClearAll);
			AddParameter(0x00);			// layer
		}

		/// <summary>
		/// Append the Clear Changes command to an existing Command object
		/// </summary>
		public void ClearChanges(InputPort port)
		{
			AddOpcode(Opcode.InputDevice_ClearChanges);
			AddParameter(0x00);			// layer
			AddParameter((byte)port);			// port
		}

		/// <summary>
		/// Append the Play Tone command to an existing Command object
		/// </summary>
		/// <param name="volume">Volme to play the tone (0-100)</param>
		/// <param name="frequency">Frequency of the tone in hertz</param>
		/// <param name="duration">Duration of the tone in milliseconds</param>
		public void PlayTone(int volume, ushort frequency, ushort duration)
		{
			if(volume < 0 || volume > 100)
				throw new ArgumentException("Volume must be between 0 and 100", "volume");

			AddOpcode(Opcode.Sound_Tone);
			AddParameter((byte)volume);		// volume
			AddParameter(frequency);	// frequency
			AddParameter(duration);	// duration (ms)
		}

		/// <summary>
		/// Append the Play Sound command to an existing Command object
		/// </summary>
		/// <param name="volume">Volume to play the sound</param>
		/// <param name="filename">Filename on the Brick of the sound to play</param>
		public void PlaySound(int volume, string filename)
		{
			if(volume < 0 || volume > 100)
				throw new ArgumentException("Volume must be between 0 and 100", "volume");

			AddOpcode(Opcode.Sound_Play);
			AddParameter((byte)volume);
			AddParameter(filename);
		}

		/// <summary>
		/// Append the Get Firmware Version command to an existing Command object
		/// </summary>
		/// <param name="maxLength">Maximum length of string to be returned</param>
		/// <param name="index">Index at which the data should be returned inside of the global buffer</param>
		public void GetFirwmareVersion(uint maxLength, uint index)
		{
			if(maxLength > 0xff)
				throw new ArgumentException("String length cannot be greater than 255 bytes", "maxLength");
			if(index > 1024)
				throw new ArgumentException("Index cannot be greater than 1024", "index");

			AddOpcode(Opcode.UIRead_GetFirmware);
			AddParameter((byte)maxLength);		// global buffer size
			AddGlobalIndex((byte)index);		// index where buffer begins
		}

		/// <summary>
		/// Add the Is Brick Pressed command to an existing Command object
		/// </summary>
		/// <param name="button">Button to check</param>
		/// <param name="index">Index at which the data should be returned inside of the global buffer</param>
		public void IsBrickButtonPressed(BrickButton button, int index)
		{
			if(index > 1024)
				throw new ArgumentException("Index cannot be greater than 1024", "index");

			AddOpcode(Opcode.UIButton_Pressed);
			AddParameter((byte)button);
			AddGlobalIndex((byte)index);
		}

		/// <summary>
		/// Append the Set LED Pattern command to an existing Command object
		/// </summary>
		/// <param name="ledPattern">The LED pattern to display</param>
		public void SetLedPattern(LedPattern ledPattern)
		{
			AddOpcode(Opcode.UIWrite_LED);
			AddParameter((byte)ledPattern);
		}

		/// <summary>
		/// Append the Clean UI command to an existing Command object
		/// </summary>
		public void CleanUI()
		{
			AddOpcode(Opcode.UIDraw_Clean);
		}

		/// <summary>
		/// Append the Draw Line command to an existing Command object
		/// </summary>
		/// <param name="color">Color of the line</param>
		/// <param name="x0">X start</param>
		/// <param name="y0">Y start</param>
		/// <param name="x1">X end</param>
		/// <param name="y1">Y end</param>
		public void DrawLine(Color color, ushort x0, ushort y0, ushort x1, ushort y1)
		{
			AddOpcode(Opcode.UIDraw_Line);
			AddParameter((byte)color);
			AddParameter(x0);
			AddParameter(y0);
			AddParameter(x1);
			AddParameter(y1);
		}

		/// <summary>
		/// Append the Draw Pixel command to an existing Command object
		/// </summary>
		/// <param name="color">Color of the pixel</param>
		/// <param name="x">X position</param>
		/// <param name="y">Y position</param>
		public void DrawPixel(Color color, ushort x, ushort y)
		{
			AddOpcode(Opcode.UIDraw_Pixel);
			AddParameter((byte)color);
			AddParameter(x);
			AddParameter(y);
		}

		/// <summary>
		/// Append the Draw Rectangle command to an existing Command object
		/// </summary>
		/// <param name="color">Color of the rectangle</param>
		/// <param name="x">X position</param>
		/// <param name="y">Y position</param>
		/// <param name="width">Width of rectangle</param>
		/// <param name="height">Height of the rectangle</param>
		/// <param name="filled">Draw a filled or empty rectangle</param>
		public void DrawRectangle(Color color, ushort x, ushort y, ushort width, ushort height, bool filled)
		{
			AddOpcode(filled ? Opcode.UIDraw_FillRect : Opcode.UIDraw_Rect);
			AddParameter((byte)color);
			AddParameter(x);
			AddParameter(y);
			AddParameter(width);
			AddParameter(height);
		}

		/// <summary>
		/// Append the Draw Inverse Rectangle command to an existing Command object
		/// </summary>
		/// <param name="x">X position</param>
		/// <param name="y">Y position</param>
		/// <param name="width">Width of rectangle</param>
		/// <param name="height">Height of rectangle</param>
		public void DrawInverseRectangle(ushort x, ushort y, ushort width, ushort height)
		{
			AddOpcode(Opcode.UIDraw_InverseRect);
			AddParameter(x);
			AddParameter(y);
			AddParameter(width);
			AddParameter(height);
		}

		/// <summary>
		/// Append the Draw Circle command to an existing Command object
		/// </summary>
		/// <param name="color">Color of the circle</param>
		/// <param name="x">X position</param>
		/// <param name="y">Y position</param>
		/// <param name="radius">Radius of circle</param>
		/// <param name="filled">Draw a filled or empty circle</param>
		public void DrawCircle(Color color, ushort x, ushort y, ushort radius, bool filled)
		{
			AddOpcode(filled ? Opcode.UIDraw_FillCircle : Opcode.UIDraw_Circle);
			AddParameter((byte)color);
			AddParameter(x);
			AddParameter(y);
			AddParameter(radius);
		}

		/// <summary>
		/// Append the Draw Text command to an existing Command object
		/// </summary>
		/// <param name="color">Color of the text</param>
		/// <param name="x">X position</param>
		/// <param name="y">Y position</param>
		/// <param name="text">Text to draw</param>
		public void DrawText(Color color, ushort x, ushort y, string text)
		{
			AddOpcode(Opcode.UIDraw_Text);
			AddParameter((byte)color);
			AddParameter(x);
			AddParameter(y);
			AddParameter(text);
		}

		/// <summary>
		/// Append the Draw Fill Window command to an existing Command object
		/// </summary>
		/// <param name="color">The color to fill</param>
		/// <param name="y0">Y start</param>
		/// <param name="y1">Y end</param>
		public void DrawFillWindow(Color color, ushort y0, ushort y1)
		{
			AddOpcode(Opcode.UIDraw_FillWindow);
			AddParameter((byte)color);
			AddParameter(y0);
			AddParameter(y1);
		}

		/// <summary>
		/// Append the Draw Image command to an existing Command object
		/// </summary>
		/// <param name="color">The color of the image to draw</param>
		/// <param name="x">X position</param>
		/// <param name="y">Y position</param>
		/// <param name="devicePath">Filename on the brick of the image to draw</param>
		public void DrawImage(Color color, ushort x, ushort y, string devicePath)
		{
			AddOpcode(Opcode.UIDraw_BmpFile);
			AddParameter((byte)color);
			AddParameter(x);
			AddParameter(y);
			AddParameter(devicePath);
		}

		/// <summary>
		/// Append the Select Font command to an existing Command object
		/// </summary>
		/// <param name="fontType">The font to select</param>
		public void SelectFont(FontType fontType)
		{
			AddOpcode(Opcode.UIDraw_SelectFont);
			AddParameter((byte)fontType);
		}

		/// <summary>
		/// Append the Enable Top Line command to an existing Command object
		/// </summary>
		/// <param name="enabled">Enable/disable the top status bar line</param>
		public void EnableTopLine(bool enabled)
		{
			AddOpcode(Opcode.UIDraw_Topline);
			AddParameter((byte)(enabled ? 1 : 0));
		}

		/// <summary>
		/// Append the Draw Dotted Line command to an existing Command object
		/// </summary>
		/// <param name="color">The color of the line</param>
		/// <param name="x0">X start</param>
		/// <param name="y0">Y start</param>
		/// <param name="x1">X end</param>
		/// <param name="y1">Y end</param>
		/// <param name="onPixels">Number of pixels the line is on</param>
		/// <param name="offPixels">Number of pixels the line is off</param>
		public void DrawDottedLine(Color color, ushort x0, ushort y0, ushort x1, ushort y1, ushort onPixels, ushort offPixels)
		{
			AddOpcode(Opcode.UIDraw_DotLine);
			AddParameter((byte)color);
			AddParameter(x0);
			AddParameter(y0);
			AddParameter(x1);
			AddParameter(y1);
			AddParameter(onPixels);
			AddParameter(offPixels);
		}

		/// <summary>
		/// Append the Update UI command to an existing Command object
		/// </summary>
		public void UpdateUI()
		{
			AddOpcode(Opcode.UIDraw_Update);
		}

		/// <summary>
		/// Append the Delete File command to an existing Command object
		/// </summary>
		/// <param name="devicePath">Filename on the brick to delete</param>
		public void DeleteFile(string devicePath)
		{
			AddOpcode(SystemOpcode.DeleteFile);
			AddRawParameter(devicePath);
		}

		/// <summary>
		/// Append the Create Directory command to an existing Command object
		/// </summary>
		/// <param name="devicePath">Directory name on the brick to create</param>
		public void CreateDirectory(string devicePath)
		{
			AddOpcode(SystemOpcode.CreateDirectory);
			AddRawParameter(devicePath);
		}

		/// <summary>
		/// Append the Get Type/Mode command to an existing Command object
		/// </summary>
		/// <param name="port">The port to query</param>
		/// <param name="typeIndex">The index to hold the Type value in the global buffer</param>
		/// <param name="modeIndex">The index to hold the Mode value in the global buffer</param>
		public void GetTypeMode(InputPort port, uint typeIndex, uint modeIndex)
		{
			if(typeIndex > 1024)
				throw new ArgumentException("Index for Type cannot be greater than 1024", "typeIndex");
			if(modeIndex > 1024)
				throw new ArgumentException("Index for Mode cannot be greater than 1024", "modeIndex");
		
			AddOpcode(Opcode.InputDevice_GetTypeMode);
			AddParameter(0x00);			// layer
			AddParameter((byte)port);	// port
			AddGlobalIndex((byte)typeIndex);	// index for type
			AddGlobalIndex((byte)modeIndex);	// index for mode
		}

		/// <summary>
		/// Append the Ready SI command to an existing Command object
		/// </summary>
		/// <param name="port">The port to query</param>
		/// <param name="mode">The mode to read the data as</param>
		/// <param name="index">The index to hold the return value in the global buffer</param>
		public void ReadySI(InputPort port, int mode, int index)
		{
			if(index > 1024)
				throw new ArgumentException("Index cannot be greater than 1024", "index");

			AddOpcode(Opcode.InputDevice_ReadySI);
			AddParameter(0x00);				// layer
			AddParameter((byte)port);		// port
			AddParameter(0x00);				// type
			AddParameter((byte)mode);				// mode
			AddParameter(0x01);				// # values
			AddGlobalIndex((byte)index);			// index for return data
		}

		/// <summary>
		/// Append the Ready Raw command to an existing Command object
		/// </summary>
		/// <param name="port">The port to query</param>
		/// <param name="mode">The mode to query the value as</param>
		/// <param name="index">The index in the global buffer to hold the return value</param>
		public void ReadyRaw(InputPort port, int mode, int index)
		{
			if(index > 1024)
				throw new ArgumentException("Index cannot be greater than 1024", "index");

			AddOpcode(Opcode.InputDevice_ReadyRaw);
			AddParameter(0x00);				// layer
			AddParameter((byte)port);		// port
			AddParameter(0x00);				// type
			AddParameter((byte)mode);				// mode
			AddParameter(0x01);				// # values
			AddGlobalIndex((byte)index);			// index for return data
		}

		/// <summary>
		/// Append the Ready Percent command to an existing Command object
		/// </summary>
		/// <param name="port">The port to query</param>
		/// <param name="mode">The mode to query the value as</param>
		/// <param name="index">The index in the global buffer to hold the return value</param>
		public void ReadyPercent(InputPort port, int mode, int index)
		{
			if(index > 1024)
				throw new ArgumentException("Index cannot be greater than 1024", "index");

			AddOpcode(Opcode.InputDevice_ReadyPct);
			AddParameter(0x00);				// layer
			AddParameter((byte)port);		// port
			AddParameter(0x00);				// type
			AddParameter((byte)mode);				// mode
			AddParameter(0x01);				// # values
			AddGlobalIndex((byte)index);			// index for return data
		}

		/// <summary>
		/// Append the Get Device Name command to an existing Command object
		/// </summary>
		/// <param name="port">The port to query</param>
		/// <param name="bufferSize">Size of the buffer to hold the returned data</param>
		/// <param name="index">Index to the position of the returned data in the global buffer</param>
		public void GetDeviceName(InputPort port, int bufferSize, int index)
		{
			if(index > 1024)
				throw new ArgumentException("Index cannot be greater than 1024", "index");

			AddOpcode(Opcode.InputDevice_GetDeviceName);
			AddParameter(0x00);
			AddParameter((byte)port);
			AddParameter((byte)bufferSize);
			AddGlobalIndex((byte)index);
		}

		/// <summary>
		/// Append the Get Mode Name command to an existing Command object
		/// </summary>
		/// <param name="port">The port to query</param>
		/// <param name="mode">The mode of the name to get</param>
		/// <param name="bufferSize">Size of the buffer to hold the returned data</param>
		/// <param name="index">Index to the position of the returned data in the global buffer</param>
		public void GetModeName(InputPort port, int mode, int bufferSize, int index)
		{
			if(index > 1024)
				throw new ArgumentException("Index cannot be greater than 1024", "index");

			AddOpcode(Opcode.InputDevice_GetModeName);
			AddParameter(0x00);
			AddParameter((byte)port);
			AddParameter((byte)mode);
			AddParameter((byte)bufferSize);
			AddGlobalIndex((byte)index);
		}

		/// <summary>
		/// Wait for the specificed output port(s) to be ready for the next command
		/// </summary>
		/// <param name="ports">Port(s) to wait for</param>
		/// <returns></returns>
		public void OutputReady(OutputPort ports)
		{
			AddOpcode(Opcode.OutputReady);
			AddParameter(0x00);			// layer
			AddParameter((byte)ports);	// ports
		}

		/// <summary>
		/// End and send a Command to the EV3 brick.
		/// </summary>
		/// <returns>A byte array containing the response from the brick, if any.</returns>
		public 
#if WINRT
		IAsyncOperation<IBuffer>
#else
		async Task<byte[]>
#endif
		SendCommandAsync()
		{
#if WINRT
			return AsyncInfo.Run(async _ => 
			{
				await _brick.SendCommandAsyncInternal(this);
				byte[] response = Response.Data;
				Initialize(CommandType.DirectNoReply);
				if(response == null)
					return null;
				return response.AsBuffer();
			});
#else
			await _brick.SendCommandAsyncInternal(this);
			byte[] response = Response.Data;
			Initialize(CommandType.DirectNoReply);
			return response;
#endif
		}
	}
}
