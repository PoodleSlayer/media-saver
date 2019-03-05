using Autofac;
using PhotoHelper.IoC;
using PhotoHelper.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PhotoHelper.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SettingsPage : ContentPage
	{
		public SettingsPage ()
		{
			InitializeComponent ();
			BackBtn.Clicked += BackBtn_Clicked;
		}

		private void BackBtn_Clicked(object sender, EventArgs e)
		{
			Navigation.PopModalAsync();
		}
	}
}