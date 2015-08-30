using System;
using System.Text;
using System.Threading.Tasks;

#if WINRT
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.Storage.Streams;
using System.Runtime.InteropServices.WindowsRuntime;
#endif

namespace Lego.Ev3.Core
{
	/// <summary>
	/// Direct commands for the EV3 brick
	/// </summary>
	public sealed class DirectCommand
	{
		private readonly Brick _brick;

		internal DirectCommand(Brick brick)
		{
			_brick = brick;
		}

		/// <summary>
		/// Turn the motor connected to the specified port or ports at the specified power.
		/// </summary>
		/// <param name="ports">A specific port or Ports.All.</param>
		/// <param name="power">The power at which to turn the motor (-100 to 100).</param>
		/// <returns></returns>
		public 
#if WINRT
		IAsyncAction
#else
		Task
#endif
		TurnMotorAtPowerAsync(OutputPort ports, int power)
		{
			return TurnMotorAtPowerAsyncInternal(ports, power)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}

		/// <summary>
		/// Turn the specified motor at the specified speed.
		/// </summary>
		/// <param name="ports">Port or ports to apply the command to.</param>
		/// <param name="speed">The speed to apply to the specified motors (-100 to 100).</param>
		public 
#if WINRT
		IAsyncAction
#else
		Task
#endif
		TurnMotorAtSpeedAsync(OutputPort ports, int speed)
		{
			return TurnMotorAtSpeedAsyncInternal(ports, speed)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}

		/// <summary>
		/// Step the motor connected to the specified port or ports at the specified power for the specified number of steps.
		/// </summary>
		/// <param name="ports">A specific port or Ports.All.</param>
		/// <param name="power">The power at which to turn the motor (-100 to 100).</param>
		/// <param name="steps"></param>
		/// <param name="brake">Apply brake to motor at end of routine.</param>
		public 
#if WINRT
		IAsyncAction
#else
		Task
#endif
		StepMotorAtPowerAsync(OutputPort ports, int power, uint steps, bool brake)
		{
			return StepMotorAtPowerAsyncInternal(ports, power, 0, steps, 0, brake)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}

		/// <summary>
		/// Step the motor connected to the specified port or ports at the specified power for the specified number of steps.
		/// </summary>
		/// <param name="ports">A specific port or Ports.All.</param>
		/// <param name="power">The power at which to turn the motor (-100 to 100).</param>
		/// <param name="rampUpSteps"></param>
		/// <param name="constantSteps"></param>
		/// <param name="rampDownSteps"></param>
		/// <param name="brake">Apply brake to motor at end of routine.</param>
		public 
#if WINRT
		IAsyncAction
#else
		Task
#endif
		StepMotorAtPowerAsync(OutputPort ports, int power, uint rampUpSteps, uint constantSteps, uint rampDownSteps, bool brake)
		{
			return StepMotorAtPowerAsyncInternal(ports, power, rampUpSteps, constantSteps, rampDownSteps, brake)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}

		/// <summary>
		/// Step the motor connected to the specified port or ports at the specified speed for the specified number of steps.
		/// </summary>
		/// <param name="ports">A specific port or Ports.All.</param>
		/// <param name="speed">The speed at which to turn the motor (-100 to 100).</param>
		/// <param name="steps"></param>
		/// <param name="brake">Apply brake to motor at end of routine.</param>
		public 
#if WINRT
		IAsyncAction
#else
		Task
#endif
		StepMotorAtSpeedAsync(OutputPort ports, int speed, uint steps, bool brake)
		{
			return StepMotorAtSpeedAsyncInternal(ports, speed, 0, steps, 0, brake)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}


		/// <summary>
		/// Step the motor connected to the specified port or ports at the specified speed for the specified number of steps.
		/// </summary>
		/// <param name="ports">A specific port or Ports.All.</param>
		/// <param name="speed">The speed at which to turn the motor (-100 to 100).</param>
		/// <param name="rampUpSteps"></param>
		/// <param name="constantSteps"></param>
		/// <param name="rampDownSteps"></param>
		/// <param name="brake">Apply brake to motor at end of routine.</param>
		public 
#if WINRT
		IAsyncAction
#else
		Task
#endif
		StepMotorAtSpeedAsync(OutputPort ports, int speed, uint rampUpSteps, uint constantSteps, uint rampDownSteps, bool brake)
		{
			return StepMotorAtSpeedAsyncInternal(ports, speed, rampUpSteps, constantSteps, rampDownSteps, brake)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}

		/// <summary>
		/// Turn the motor connected to the specified port or ports at the specified power for the specified times.
		/// </summary>
		/// <param name="ports">A specific port or Ports.All.</param>
		/// <param name="power">The power at which to turn the motor (-100 to 100).</param>
		/// <param name="milliseconds">Number of milliseconds to run at constant power.</param>
		/// <param name="brake">Apply brake to motor at end of routine.</param>
		/// <returns></returns>
		public 
#if WINRT
		IAsyncAction
#else
		Task
#endif
		TurnMotorAtPowerForTimeAsync(OutputPort ports, int power, uint milliseconds, bool brake)
		{
			return TurnMotorAtPowerForTimeAsyncInternal(ports, power, 0, milliseconds, 0, brake)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}

		/// <summary>
		/// Turn the motor connected to the specified port or ports at the specified power for the specified times.
		/// </summary>
		/// <param name="ports">A specific port or Ports.All.</param>
		/// <param name="power">The power at which to turn the motor (-100 to 100).</param>
		/// <param name="msRampUp">Number of milliseconds to get up to power.</param>
		/// <param name="msConstant">Number of milliseconds to run at constant power.</param>
		/// <param name="msRampDown">Number of milliseconds to power down to a stop.</param>
		/// <param name="brake">Apply brake to motor at end of routine.</param>
		/// <returns></returns>
		public 
#if WINRT
		IAsyncAction
#else
		Task
#endif
		TurnMotorAtPowerForTimeAsync(OutputPort ports, int power, uint msRampUp, uint msConstant, uint msRampDown, bool brake)
		{
			return TurnMotorAtPowerForTimeAsyncInternal(ports, power, msRampUp, msConstant, msRampDown, brake)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}

		/// <summary>
		/// Turn the motor connected to the specified port or ports at the specified speed for the specified times.
		/// </summary>
		/// <param name="ports">A specific port or Ports.All.</param>
		/// <param name="speed">The power at which to turn the motor (-100 to 100).</param>
		/// <param name="milliseconds">Number of milliseconds to run at constant speed.</param>
		/// <param name="brake">Apply brake to motor at end of routine.</param>
		/// <returns></returns>
		public 
#if WINRT
		IAsyncAction
#else
		Task
#endif
		TurnMotorAtSpeedForTimeAsync(OutputPort ports, int speed, uint milliseconds, bool brake)
		{
			return TurnMotorAtSpeedForTimeAsyncInternal(ports, speed, 0, milliseconds, 0, brake)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}
		/// <summary>
		/// Turn the motor connected to the specified port or ports at the specified speed for the specified times.
		/// </summary>
		/// <param name="ports">A specific port or Ports.All.</param>
		/// <param name="speed">The power at which to turn the motor (-100 to 100).</param>
		/// <param name="msRampUp">Number of milliseconds to get up to speed.</param>
		/// <param name="msConstant">Number of milliseconds to run at constant speed.</param>
		/// <param name="msRampDown">Number of milliseconds to slow down to a stop.</param>
		/// <param name="brake">Apply brake to motor at end of routine.</param>
		/// <returns></returns>
		public 
#if WINRT
		IAsyncAction
#else
		Task
#endif
		TurnMotorAtSpeedForTimeAsync(OutputPort ports, int speed, uint msRampUp, uint msConstant, uint msRampDown, bool brake)
		{
			return TurnMotorAtSpeedForTimeAsyncInternal(ports, speed, msRampDown, msConstant, msRampDown, brake)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}

		/// <summary>
		/// Set the polarity (direction) of a motor.
		/// </summary>
		/// <param name="ports">Port or ports to change polarity</param>
		/// <param name="polarity">The new polarity (direction) value</param>
		/// <returns></returns>
		public 
#if WINRT
		IAsyncAction
#else
		Task
#endif
		SetMotorPolarityAsync(OutputPort ports, Polarity polarity)
		{
			return SetMotorPolarityAsyncInternal(ports, polarity)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}

		/// <summary>
		/// Start motors on the specified ports.
		/// </summary>
		/// <param name="ports">The port or ports to which the stop command will be sent.</param>
		/// <returns></returns>
		public
#if WINRT
		IAsyncAction
#else
		Task
#endif
		StartMotorAsync(OutputPort ports)
		{
			return StartMotorAsyncInternal(ports)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}

		/// <summary>
		/// Synchronize stepping of motors.
		/// </summary>
		/// <param name="ports">The port or ports to which the stop command will be sent.</param>
		/// <param name="speed">Speed to turn the motor(s).</param>
		/// <param name="turnRatio">The turn ratio to apply.</param>
		/// <param name="step">The number of steps to turn the motor(s).</param>
		/// <param name="brake">Brake or coast at the end.</param>
		/// <returns></returns>
		public
#if WINRT
		IAsyncAction
#else
		Task
#endif
		StepMotorSyncAsync(OutputPort ports, int speed, short turnRatio, uint step, bool brake)
		{
			return StepMotorSyncAsyncInternal(ports, speed, turnRatio, step, brake)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}

		/// <summary>
		/// Synchronize timing of motors.
		/// </summary>
		/// <param name="ports">The port or ports to which the stop command will be sent.</param>
		/// <param name="speed">Speed to turn the motor(s).</param>
		/// <param name="turnRatio">The turn ratio to apply.</param>
		/// <param name="time">The time to turn the motor(s).</param>
		/// <param name="brake">Brake or coast at the end.</param>
		/// <returns></returns>
		public
#if WINRT
		IAsyncAction
#else
		Task
#endif
		TimeMotorSyncAsync(OutputPort ports, int speed, short turnRatio, uint time, bool brake)
		{
			return TimeMotorSyncAsyncInternal(ports, speed, turnRatio, time, brake)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}


		/// <summary>
		/// Stops motors on the specified ports.
		/// </summary>
		/// <param name="ports">The port or ports to which the stop command will be sent.</param>
		/// <param name="brake">Apply brake to motor at end of routine.</param>
		/// <returns></returns>
		public
#if WINRT
		IAsyncAction
#else
		Task
#endif
		StopMotorAsync(OutputPort ports, bool brake)
		{
			return StopMotorAsyncInternal(ports, brake)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}

		/// <summary>
		/// Resets all ports and devices to defaults.
		/// </summary>
		/// <returns></returns>
		public 
#if WINRT
		IAsyncAction
#else
		Task
#endif
		ClearAllDevicesAsync()
		{
			return ClearAllDevicesAsyncInternal()
#if WINRT
			.AsAsyncAction()
#endif
			;
		}

		/// <summary>
		/// Clears changes on specified port
		/// </summary>
		///	<param name="port">The port to clear</param>
		/// <returns></returns>
		public 
#if WINRT
		IAsyncAction
#else
		Task
#endif
		ClearChangesAsync(InputPort port)
		{
			return ClearChangesAsyncInternal(port)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}

		/// <summary>
		/// Plays a tone of the specified frequency for the specified time.
		/// </summary>
		/// <param name="volume">Volume of tone (0-100).</param>
		/// <param name="frequency">Frequency of tone, in hertz.</param>
		/// <param name="duration">Duration to play tone, in milliseconds.</param>
		/// <returns></returns>
		public 
#if WINRT
		IAsyncAction
#else
		Task
#endif
		PlayToneAsync(int volume, ushort frequency, ushort duration)
		{
			return PlayToneAsyncInternal(volume, frequency, duration)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}

		/// <summary>
		/// Play a sound file stored on the EV3 brick
		/// </summary>
		/// <param name="volume">Volume of the sound (0-100)</param>
		/// <param name="filename">Filename of sound stored on brick, without the .RSF extension</param>
		/// <returns></returns>
		public 
#if WINRT
		IAsyncAction
#else
		Task
#endif
		PlaySoundAsync(int volume, string filename)
		{
			return PlaySoundAsyncInternal(volume, filename)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}

		/// <summary>
		/// Return the current version number of the firmware running on the EV3 brick.
		/// </summary>
		/// <returns>Current firmware version.</returns>
		public 
#if WINRT
		IAsyncOperation<string>
#else
Task<string>
#endif
		GetFirmwareVersionAsync()
		{
			return GetFirmwareVersionAsyncInternal()
#if WINRT
			.AsAsyncOperation()
#endif
			;
		}

		/// <summary>
		/// Returns whether the specified BrickButton is pressed
		/// </summary>
		/// <param name="button">Button on the face of the EV3 brick</param>
		/// <returns>Whether or not the button is pressed</returns>
		public 
#if WINRT
		IAsyncOperation<bool>
#else
Task<bool>
#endif
		IsBrickButtonPressedAsync(BrickButton button)
		{
			return IsBrickButtonPressedAsyncInternal(button)
#if WINRT
			.AsAsyncOperation()
#endif
			;
		}

		/// <summary>
		/// Set EV3 brick LED pattern
		/// </summary>
		/// <param name="ledPattern">Pattern to display on LED</param>
		/// <returns></returns>
		public 
#if WINRT
		IAsyncAction
#else
		Task
#endif
		SetLedPatternAsync(LedPattern ledPattern)
		{
			return SetLedPatternAsyncInternal(ledPattern)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}

		/// <summary>
		/// Draw a line on the EV3 LCD screen
		/// </summary>
		/// <param name="color">Color of the line</param>
		/// <param name="x0">X start</param>
		/// <param name="y0">Y start</param>
		/// <param name="x1">X end</param>
		/// <param name="y1">Y end</param>
		/// <returns></returns>
		public 
#if WINRT
		IAsyncAction
#else
		Task
#endif
		DrawLineAsync(Color color, ushort x0, ushort y0, ushort x1, ushort y1)
		{
			return DrawLineAsyncInternal(color, x0, y0, x1, y1)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}

		/// <summary>
		/// Draw a single pixel
		/// </summary>
		/// <param name="color">Color of the pixel</param>
		/// <param name="x">X position</param>
		/// <param name="y">Y position</param>
		/// <returns></returns>
		public 
#if WINRT
		IAsyncAction
#else
		Task
#endif
		DrawPixelAsync(Color color, ushort x, ushort y)
		{
			return DrawPixelAsyncInternal(color, x, y)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}

		/// <summary>
		/// Draw a rectangle
		/// </summary>
		/// <param name="color">Color of the rectangle</param>
		/// <param name="x">X position</param>
		/// <param name="y">Y position</param>
		/// <param name="width">Width of rectangle</param>
		/// <param name="height">Height of rectangle</param>
		/// <param name="filled">Filled or empty</param>
		/// <returns></returns>
		public  
#if WINRT
		IAsyncAction
#else
		Task
#endif
		DrawRectangleAsync(Color color, ushort x, ushort y, ushort width, ushort height, bool filled)
		{
			return DrawRectangleAsyncInternal(color, x, y, width, height, filled)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}

		/// <summary>
		/// Draw a filled rectangle, inverting the pixels underneath it
		/// </summary>
		/// <param name="x">X position</param>
		/// <param name="y">Y position</param>
		/// <param name="width">Width of the rectangle</param>
		/// <param name="height">Height of the rectangle</param>
		/// <returns></returns>
		public  
#if WINRT
		IAsyncAction
#else
		Task
#endif
		DrawInverseRectangleAsync(ushort x, ushort y, ushort width, ushort height)
		{
			return DrawInverseRectangleAsyncInternal(x, y, width, height)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}

		/// <summary>
		/// Draw a circle
		/// </summary>
		/// <param name="color">Color of the circle</param>
		/// <param name="x">X position</param>
		/// <param name="y">Y position</param>
		/// <param name="radius">Radius of the circle</param>
		/// <param name="filled">Filled or empty</param>
		/// <returns></returns>
		public  
#if WINRT
		IAsyncAction
#else
		Task
#endif
		DrawCircleAsync(Color color, ushort x, ushort y, ushort radius, bool filled)
		{
			return DrawCircleAsyncInternal(color, x, y, radius, filled)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}

		/// <summary>
		/// Write a string to the screen
		/// </summary>
		/// <param name="color">Color of the text</param>
		/// <param name="x">X position</param>
		/// <param name="y">Y position</param>
		/// <param name="text">Text to draw</param>
		/// <returns></returns>
		public  
#if WINRT
		IAsyncAction
#else
		Task
#endif
		DrawTextAsync(Color color, ushort x, ushort y, string text)
		{
			return DrawTextAsyncInternal(color, x, y, text)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}

		/// <summary>
		/// Draw a dotted line
		/// </summary>
		/// <param name="color">Color of dotted line</param>
		/// <param name="x0">X start</param>
		/// <param name="y0">Y start</param>
		/// <param name="x1">X end</param>
		/// <param name="y1">Y end</param>
		/// <param name="onPixels">Number of pixels the line is drawn</param>
		/// <param name="offPixels">Number of pixels the line is empty</param>
		/// <returns></returns>
		public  
#if WINRT
		IAsyncAction
#else
		Task
#endif
		DrawDottedLineAsync(Color color, ushort x0, ushort y0, ushort x1, ushort y1, ushort onPixels, ushort offPixels)
		{
			return DrawDottedLineAsyncInternal(color, x0, y0, x1, y1, onPixels, offPixels)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}

		/// <summary>
		/// Fills the width of the screen between the provided Y coordinates
		/// </summary>
		/// <param name="color">Color of the fill</param>
		/// <param name="y0">Y start</param>
		/// <param name="y1">Y end</param>
		/// <returns></returns>
		public  
#if WINRT
		IAsyncAction
#else
		Task
#endif
		DrawFillWindowAsync(Color color, ushort y0, ushort y1)
		{
			return DrawFillWindowAsyncInternal(color, y0, y1)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}

		/// <summary>
		/// Draw an image to the LCD screen
		/// </summary>
		/// <param name="color">Color of the image pixels</param>
		/// <param name="x">X position</param>
		/// <param name="y">Y position</param>
		/// <param name="devicePath">Path to the image on the EV3 brick</param>
		/// <returns></returns>
		public  
#if WINRT
		IAsyncAction
#else
		Task
#endif
		DrawImageAsync(Color color, ushort x, ushort y, string devicePath)
		{
			return DrawImageAsyncInternal(color, x, y, devicePath)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}

		/// <summary>
		/// Enable or disable the top status bar
		/// </summary>
		/// <param name="enabled">Enabled or disabled</param>
		/// <returns></returns>
		public  
#if WINRT
		IAsyncAction
#else
		Task
#endif
		EnableTopLineAsync(bool enabled)
		{
			return EnableTopLineAsyncInternal(enabled)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}

		/// <summary>
		/// Select the font for text drawing
		/// </summary>
		/// <param name="fontType">Type of font to use</param>
		/// <returns></returns>
		public  
#if WINRT
		IAsyncAction
#else
		Task
#endif
		SelectFontAsync(FontType fontType)
		{
			return SelectFontAsyncInternal(fontType)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}

		/// <summary>
		/// Clear the entire screen
		/// </summary>
		/// <returns></returns>
		public  
#if WINRT
		IAsyncAction
#else
		Task
#endif
		CleanUIAsync()
		{
			return CleanUIAsyncInternal()
#if WINRT
			.AsAsyncAction()
#endif
			;
		}

		/// <summary>
		/// Refresh the EV3 LCD screen
		/// </summary>
		/// <returns></returns>
		public  
#if WINRT
		IAsyncAction
#else
		Task
#endif
		UpdateUIAsync()
		{
			return UpdateUIAsyncInternal()
#if WINRT
			.AsAsyncAction()
#endif
			;
		}

		/// <summary>
		/// Get the type and mode of the device attached to the specified port
		/// </summary>
		/// <param name="port">The input port to query</param>
		/// <returns>2 bytes, index 0 being the type, index 1 being the mode</returns>
		public
#if WINRT
		IAsyncOperation<IBuffer>
#else
		Task<byte[]>
#endif
		GetTypeModeAsync(InputPort port)
		{
#if WINRT
			return AsyncInfo.Run(async _ => (await GetTypeModeAsyncInternal(port)).AsBuffer());
#else
			return GetTypeModeAsyncInternal(port);
#endif
		}

		/// <summary>
		/// Read the SI value from the specified port in the specified mode
		/// </summary>
		/// <param name="port">The port to query</param>
		/// <param name="mode">The mode used to read the data</param>
		/// <returns>The SI value</returns>
		public 
#if WINRT
		IAsyncOperation<float>
#else
		Task<float>
#endif
		ReadySIAsync(InputPort port, int mode)
		{
			return ReadySIAsyncInternal(port, mode)
#if WINRT
			.AsAsyncOperation()
#endif
			;
		}

		/// <summary>
		/// Read the raw value from the specified port in the specified mode
		/// </summary>
		/// <param name="port">The port to query</param>
		/// <param name="mode">The mode used to read the data</param>
		/// <returns>The Raw value</returns>
		public 
#if WINRT
		IAsyncOperation<int>
#else
		Task<int>
#endif
		ReadyRawAsync(InputPort port, int mode)
		{
			return ReadyRawAsyncInternal(port, mode)
#if WINRT
			.AsAsyncOperation()
#endif
			;
		}

		/// <summary>
		/// Read the percent value from the specified port in the specified mode
		/// </summary>
		/// <param name="port">The port to query</param>
		/// <param name="mode">The mode used to read the data</param>
		/// <returns>The percentage value</returns>
		public 
#if WINRT
		IAsyncOperation<int>
#else
		Task<int>
#endif
		ReadyPercentAsync(InputPort port, int mode)
		{
			return ReadyPercentAsyncInternal(port, mode)
#if WINRT
			.AsAsyncOperation()
#endif
			;
		}

		/// <summary>
		/// Get the name of the device attached to the specified port
		/// </summary>
		/// <param name="port">Port to query</param>
		/// <returns>The name of the device</returns>
		public 
#if WINRT
		IAsyncOperation<string>
#else
		Task<string>
#endif 
		GetDeviceNameAsync(InputPort port)
		{
			return GetDeviceNameAsyncInternal(port)
#if WINRT
			.AsAsyncOperation()
#endif
			;
		}

		/// <summary>
		/// Get the mode of the device attached to the specified port
		/// </summary>
		/// <param name="port">Port to query</param>
		/// <param name="mode">Mode of the name to get</param>
		/// <returns>The name of the mode</returns>
		public 
#if WINRT
		IAsyncOperation<string>
#else
		Task<string>
#endif 
		GetModeNameAsync(InputPort port, int mode)
		{
			return GetModeNameAsyncInternal(port, mode)
#if WINRT
			.AsAsyncOperation()
#endif
			;
		}

		/// <summary>
		/// Wait for the specificed output port(s) to be ready for the next command
		/// </summary>
		/// <param name="ports">Port(s) to wait for</param>
		/// <returns></returns>
		public 
#if WINRT
		IAsyncAction
#else
		Task
#endif 
		OutputReadyAsync(OutputPort ports)
		{
			return OutputReadyAsyncInternal(ports)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}

		internal async Task TurnMotorAtPowerAsyncInternal(OutputPort ports, int power)
		{
			Command c = new Command(CommandType.DirectNoReply);
			c.TurnMotorAtPower(ports, power);
			c.StartMotor(ports);
			await _brick.SendCommandAsyncInternal(c);
		}

		internal async Task TurnMotorAtSpeedAsyncInternal(OutputPort ports, int speed)
		{
			Command c = new Command(CommandType.DirectNoReply);
			c.TurnMotorAtSpeed(ports, speed);
			c.StartMotor(ports);
			await _brick.SendCommandAsyncInternal(c);
		}

		internal async Task StepMotorAtPowerAsyncInternal(OutputPort ports, int power, uint rampUpSteps, uint constantSteps, uint rampDownSteps, bool brake)
		{
			Command c = new Command(CommandType.DirectNoReply);
			c.StepMotorAtPower(ports, power, rampUpSteps, constantSteps, rampDownSteps, brake);
			await _brick.SendCommandAsyncInternal(c);
		}


		internal async Task StepMotorAtSpeedAsyncInternal(OutputPort ports, int speed, uint rampUpSteps, uint constantSteps, uint rampDownSteps, bool brake)
		{
			Command c = new Command(CommandType.DirectNoReply);
			c.StepMotorAtSpeed(ports, speed, rampUpSteps, constantSteps, rampDownSteps, brake);
			await _brick.SendCommandAsyncInternal(c);
		}

		internal async Task TurnMotorAtPowerForTimeAsyncInternal(OutputPort ports, int power, uint msRampUp, uint msConstant, uint msRampDown, bool brake)
		{
			Command c = new Command(CommandType.DirectNoReply);
			c.TurnMotorAtPowerForTime(ports, power, msRampUp, msConstant, msRampDown, brake);
			await _brick.SendCommandAsyncInternal(c);
		}

		internal async Task TurnMotorAtSpeedForTimeAsyncInternal(OutputPort ports, int speed, uint msRampUp, uint msConstant, uint msRampDown, bool brake)
		{
			Command c = new Command(CommandType.DirectNoReply);
			c.TurnMotorAtSpeedForTime(ports, speed, msRampUp, msConstant, msRampDown, brake);
			await _brick.SendCommandAsyncInternal(c);
		}

		internal async Task SetMotorPolarityAsyncInternal(OutputPort ports, Polarity polarity)
		{
			Command c = new Command(CommandType.DirectNoReply);
			c.SetMotorPolarity(ports, polarity);
			await _brick.SendCommandAsyncInternal(c);
		}

		internal async Task StopMotorAsyncInternal(OutputPort ports, bool brake)
		{
			Command c = new Command(CommandType.DirectNoReply);
			c.StopMotor(ports, brake);
			await _brick.SendCommandAsyncInternal(c);
		}

		internal async Task StartMotorAsyncInternal(OutputPort ports)
		{
			Command c = new Command(CommandType.DirectNoReply);
			c.StartMotor(ports);
			await _brick.SendCommandAsyncInternal(c);
		}

		internal async Task StepMotorSyncAsyncInternal(OutputPort ports, int speed, short turnRatio, uint step, bool brake)
		{
			Command c = new Command(CommandType.DirectNoReply);
			c.StepMotorSync(ports, speed, turnRatio, step, brake);
			await _brick.SendCommandAsyncInternal(c);
		}

		internal async Task TimeMotorSyncAsyncInternal(OutputPort ports, int speed, short turnRatio, uint step, bool brake)
		{
			Command c = new Command(CommandType.DirectNoReply);
			c.TimeMotorSync(ports, speed, turnRatio, step, brake);
			await _brick.SendCommandAsyncInternal(c);
		}

		internal async Task ClearAllDevicesAsyncInternal()
		{
			Command c = new Command(CommandType.DirectNoReply);
			c.ClearAllDevices();
			await _brick.SendCommandAsyncInternal(c);
		}

		internal async Task ClearChangesAsyncInternal(InputPort port)
		{
			Command c = new Command(CommandType.DirectNoReply);
			c.ClearChanges(port);
			await _brick.SendCommandAsyncInternal(c);
		}

		internal async Task PlayToneAsyncInternal(int volume, ushort frequency, ushort duration)
		{
			Command c = new Command(CommandType.DirectNoReply);
			c.PlayTone(volume, frequency, duration);
			await _brick.SendCommandAsyncInternal(c);
		}

		internal async Task PlaySoundAsyncInternal(int volume, string filename)
		{
			Command c = new Command(CommandType.DirectNoReply);
			c.PlaySound(volume, filename);
			await _brick.SendCommandAsyncInternal(c);
		}

		internal async Task<string> GetFirmwareVersionAsyncInternal()
		{
			Command c = new Command(CommandType.DirectReply, 0x10, 0);
			c.GetFirwmareVersion(0x10, 0);
			await _brick.SendCommandAsyncInternal(c);
			if(c.Response.Data == null)
				return null;

			int index = Array.IndexOf(c.Response.Data, (byte)0);
			return Encoding.UTF8.GetString(c.Response.Data, 0, index);
		}

		internal async Task<bool> IsBrickButtonPressedAsyncInternal(BrickButton button)
		{
			Command c = new Command(CommandType.DirectReply, 1, 0);
			c.IsBrickButtonPressed(button, 0);
			await _brick.SendCommandAsyncInternal(c);
			return false;
		}

		internal async Task SetLedPatternAsyncInternal(LedPattern ledPattern)
		{
			Command c = new Command(CommandType.DirectNoReply);
			c.SetLedPattern(ledPattern);
			await _brick.SendCommandAsyncInternal(c);
		}

		internal async Task CleanUIAsyncInternal()
		{
			Command c = new Command(CommandType.DirectNoReply);
			c.CleanUI();
			await _brick.SendCommandAsyncInternal(c);
		}

		internal async Task DrawLineAsyncInternal(Color color, ushort x0, ushort y0, ushort x1, ushort y1)
		{
			Command c = new Command(CommandType.DirectNoReply);
			c.DrawLine(color, x0, y0, x1, y1);
			await _brick.SendCommandAsyncInternal(c);
		}

		internal async Task DrawPixelAsyncInternal(Color color, ushort x, ushort y)
		{
			Command c = new Command(CommandType.DirectNoReply);
			c.DrawPixel(color, x, y);
			await _brick.SendCommandAsyncInternal(c);
		}

		internal async Task DrawRectangleAsyncInternal(Color color, ushort x, ushort y, ushort width, ushort height, bool filled)
		{
			Command c = new Command(CommandType.DirectNoReply);
			c.DrawRectangle(color, x, y, width, height, filled);
			await _brick.SendCommandAsyncInternal(c);
		}

		internal async Task DrawInverseRectangleAsyncInternal(ushort x, ushort y, ushort width, ushort height)
		{
			Command c = new Command(CommandType.DirectNoReply);
			c.DrawInverseRectangle(x, y, width, height);
			await _brick.SendCommandAsyncInternal(c);
		}

		internal async Task DrawCircleAsyncInternal(Color color, ushort x, ushort y, ushort radius, bool filled)
		{
			Command c = new Command(CommandType.DirectNoReply);
			c.DrawCircle(color, x, y, radius, filled);
			await _brick.SendCommandAsyncInternal(c);
		}

		internal async Task DrawTextAsyncInternal(Color color, ushort x, ushort y, string text)
		{
			Command c = new Command(CommandType.DirectNoReply);
			c.DrawText(color, x, y, text);
			await _brick.SendCommandAsyncInternal(c);
		}

		internal async Task DrawFillWindowAsyncInternal(Color color, ushort y0, ushort y1)
		{
			Command c = new Command(CommandType.DirectNoReply);
			c.DrawFillWindow(color, y0, y1);
			await _brick.SendCommandAsyncInternal(c);
		}

		internal async Task DrawImageAsyncInternal(Color color, ushort x, ushort y, string devicePath)
		{
			Command c = new Command(CommandType.DirectNoReply);
			c.DrawImage(color, x ,y, devicePath);
			await _brick.SendCommandAsyncInternal(c);
		}

		internal async Task SelectFontAsyncInternal(FontType fontType)
		{
			Command c = new Command(CommandType.DirectNoReply);
			c.SelectFont(fontType);
			await _brick.SendCommandAsyncInternal(c);
		}

		internal async Task EnableTopLineAsyncInternal(bool enabled)
		{
			Command c = new Command(CommandType.DirectNoReply);
			c.EnableTopLine(enabled);
			await _brick.SendCommandAsyncInternal(c);
		}

		internal async Task DrawDottedLineAsyncInternal(Color color, ushort x0, ushort y0, ushort x1, ushort y1, ushort onPixels, ushort offPixels)
		{
			Command c = new Command(CommandType.DirectNoReply);
			c.DrawDottedLine(color, x0, y0, x1, y1, onPixels, offPixels);
			await _brick.SendCommandAsyncInternal(c);
		}

		internal async Task UpdateUIAsyncInternal()
		{
			Command c = new Command(CommandType.DirectNoReply);
			c.UpdateUI();
			await _brick.SendCommandAsyncInternal(c);
		}

		internal async Task<byte[]> GetTypeModeAsyncInternal(InputPort port)
		{
			Command c = new Command(CommandType.DirectReply, 2, 0);
			c.GetTypeMode(port, 0, 1);
			await _brick.SendCommandAsyncInternal(c);
			return c.Response.Data;
		}

		internal async Task<float> ReadySIAsyncInternal(InputPort port, int mode)
		{
			Command c = new Command(CommandType.DirectReply, 4, 0);
			c.ReadySI(port, mode, 0);
			await _brick.SendCommandAsyncInternal(c);
			return BitConverter.ToSingle(c.Response.Data, 0);
		}

		internal async Task<int> ReadyRawAsyncInternal(InputPort port, int mode)
		{
			Command c = new Command(CommandType.DirectReply, 4, 0);
			c.ReadyRaw(port, mode, 0);
			await _brick.SendCommandAsyncInternal(c);
			return BitConverter.ToInt32(c.Response.Data, 0);
		}

		internal async Task<int> ReadyPercentAsyncInternal(InputPort port, int mode)
		{
			Command c = new Command(CommandType.DirectReply, 1, 0);
			c.ReadyRaw(port, mode, 0);
			await _brick.SendCommandAsyncInternal(c);
			return c.Response.Data[0];
		}

		internal async Task<string> GetDeviceNameAsyncInternal(InputPort port)
		{
			Command c = new Command(CommandType.DirectReply, 0x7f, 0);
			c.GetDeviceName(port, 0x7f, 0);
			await _brick.SendCommandAsyncInternal(c);
			int index = Array.IndexOf(c.Response.Data, (byte)0);
			return Encoding.UTF8.GetString(c.Response.Data, 0, index);
		}

		internal async Task<string> GetModeNameAsyncInternal(InputPort port, int mode)
		{
			Command c = new Command(CommandType.DirectReply, 0x7f, 0);
			c.GetModeName(port, mode, 0x7f, 0);
			await _brick.SendCommandAsyncInternal(c);
			int index = Array.IndexOf(c.Response.Data, (byte)0);
			return Encoding.UTF8.GetString(c.Response.Data, 0, index);
		}

		internal async Task OutputReadyAsyncInternal(OutputPort ports)
		{
			Command c = new Command(CommandType.DirectNoReply);
			c.OutputReady(ports);
			await _brick.SendCommandAsyncInternal(c);
		}
	}
}
