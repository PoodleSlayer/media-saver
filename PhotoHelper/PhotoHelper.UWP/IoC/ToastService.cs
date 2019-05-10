using PhotoHelper.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

namespace PhotoHelper.UWP.IoC
{
	public class ToastService : IToastService
	{
		public void MakeToast(string message)
		{
			// we want a small popup similar to how toasts work in Android
			// use flyout for now, look into Popup later
			var label = new TextBlock
			{
				Text = message,
				Foreground = new SolidColorBrush(Colors.White),
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center,
			};

			var style = new Style { TargetType = typeof(FlyoutPresenter) };
			style.Setters.Add(new Setter(Control.BackgroundProperty, new SolidColorBrush(Colors.Black)));
			style.Setters.Add(new Setter(FrameworkElement.MaxHeightProperty, 1));

			var flyout = new Flyout
			{
				Content = label,
				Placement = FlyoutPlacementMode.Bottom,
				FlyoutPresenterStyle = style,
			};

			flyout.ShowAt(Window.Current.Content as FrameworkElement);

			var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(3) };
			timer.Tick += (sender, e) =>
			{
				timer.Stop();
				flyout.Hide();
			};
			timer.Start();
		}
	}
}
