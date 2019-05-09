using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PhotoHelper.Controls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PHButton : Button
	{
		public PHButton ()
		{
			InitializeComponent ();

			if (Device.RuntimePlatform == Device.Android)
			{
				BackgroundColor = Color.FromHex("#2196F3");
				TextColor = Color.White;
			}
			else if (Device.RuntimePlatform == Device.UWP)
			{
				BackgroundColor = Color.FromHex("#2196F3");
				TextColor = Color.White;
			}
		}
	}
}