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
	public partial class MobileDLPage : DLPage
	{
		public MobileDLPage ()
		{
			InitializeComponent ();

			BindingContext = AppContainer.Container.Resolve<DLViewModel>();

			BackBtn.Clicked += BackBtn_Clicked;
			webby.Navigated += base.Webby_Navigated;
			base.mainWebView = webby;
		}

		private void BackBtn_Clicked(object sender, EventArgs e)
		{
			Navigation.PopModalAsync();
		}
	}
}