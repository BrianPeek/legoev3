using System.Threading;

namespace Lego.Ev3.Core
{
	internal class Response
	{
		public ReplyType ReplyType { get; set; }
		public ushort Sequence { get; set; }
		public ManualResetEvent Event { get; set; }
		public byte[] Data { get; set; }
		public SystemOpcode SystemCommand { get; set; }
		public SystemReplyStatus SystemReplyStatus { get; set; }

		internal Response(ushort sequence)
		{
			Sequence = sequence;
			Event = new ManualResetEvent(false);
		}
	}
}