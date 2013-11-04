using System;
using System.Windows;
using System.Windows.Controls;

namespace SampleApp.Controls
{
	/// <summary>
	/// Interaction logic for ConnectionControl.xaml
	/// </summary>
	public partial class ConnectionControl : UserControl
	{
		public event RoutedEventHandler ConnectToBrick;

		public ConnectionControl()
		{
			InitializeComponent();
		}

		private void RoundButton_Click(object sender, RoutedEventArgs e)
		{
			if (ConnectToBrick != null)
				ConnectToBrick.Invoke(sender, e);
		}

		public ConnectionType GetConnectionType()
		{
			var conTypeString = ConnectionTypeStyle.SelectedItem.ToString();
			ConnectionType conType;

			Enum.TryParse(conTypeString, true, out conType);

			return conType;
		}

		public string GetIpAddress()
		{
			return IpAddress.Text;
		}

		public string GetComportNumber()
		{
			return "COM" + int.Parse(ComportNumber.Text);
		}
	}
}
