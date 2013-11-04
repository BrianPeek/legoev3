using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SampleApp.Controls
{
	public sealed partial class ProgressOverlayControl : UserControl
	{
		public ProgressOverlayControl()
		{
			this.InitializeComponent();
			DataContext = this;
		}

		private string _text = "Working";

		public string Text
		{
			get { return _text; }
			set { _text = value; }
		}

		public void Show(string text = null)
		{
			if (text != null)
				Text = text;

			ProgBar.IsIndeterminate = true;

			Visibility = Visibility.Visible;
		}

		public void Hide()
		{
			ProgBar.IsIndeterminate = false;

			Visibility = Visibility.Collapsed;
		}
	}
}
