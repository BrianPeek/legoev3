using System;

namespace SampleApp
{
	public class MotorSettingsEventArgs : EventArgs
	{
		public MotorMovementTypes MotorMovementType { get; set; }
		public int DegreeMovement { get; set; }
		public int TimeToMoveInSeconds { get; set; }
		public int PowerRatingMovement { get; set; }
	}
}
