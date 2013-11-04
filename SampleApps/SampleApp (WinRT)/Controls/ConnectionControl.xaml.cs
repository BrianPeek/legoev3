using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace SampleApp.Controls
{
	public sealed partial class ConnectionControl : UserControl
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
			if (ConnectionTypeStyle.SelectedItem != null)
			{
				var conTypeString = ((ComboBoxItem)ConnectionTypeStyle.SelectedItem).Content.ToString();
				ConnectionType conType;

				Enum.TryParse(conTypeString, true, out conType);

				return conType;
			}

			return ConnectionType.Unknown;
		}

		public string GetIpAddress()
		{
			return IpAddress.Text;
		}
	}
}
