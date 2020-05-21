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
	public partial class DLPage : ContentPage
	{
		private string currentURL;
		protected WebView mainWebView;

		public DLPage()
		{
			InitializeComponent();
		}

		protected void Webby_Navigated(object sender, WebNavigatedEventArgs e)
		{
			// THIS IS GETTING CALLED MULTIPLE TIMES ON ANDROID
			// might be a bug with this version of XForms WebView. Try upgrading
			// or just making a custom renderer

			// don't care if they navigate to a specific image, so don't store these
			if (e.Url.Contains(@"instagram.com/p/"))
			{
				_ = RemovePopup(true);
				return;
			}

			currentURL = e.Url;
			ViewModel.CurrentURL = e.Url;
		}

		private async Task<bool> RemovePopup(bool waitToTry = false)
		{
			// attempt to remove the Login popup...

			if (waitToTry)
			{
				await Task.Delay(1000);
			}

			string dialogHTML = await mainWebView.EvaluateJavaScriptAsync("function fixit(){var a = document.querySelector(\"div[role = 'dialog']\").parentNode.style.visibility = \"hidden\"; var b = document.getElementsByTagName(\"body\")[0].style.removeProperty(\"overflow\");}");
			string runDialogHTML = await mainWebView.EvaluateJavaScriptAsync("fixit()");
			return true;
		}

		protected DLViewModel ViewModel
		{
			get => BindingContext as DLViewModel;
		}
	}
}