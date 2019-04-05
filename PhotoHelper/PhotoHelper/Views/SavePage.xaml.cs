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
	public partial class SavePage : ContentPage
	{
		public SavePage ()
		{
			InitializeComponent ();
			BindingContext = AppContainer.Container.Resolve<SaveViewModel>();

			BackBtn.Clicked += BackBtn_Clicked;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			ViewModel.DidAppear();
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			ViewModel.DidDisappear();
		}

		// move all of these to a command in the base view model since basically all pages will have a back button
		private void BackBtn_Clicked(object sender, EventArgs e)
		{
			Navigation.PopModalAsync();
		}

		private SaveViewModel ViewModel
		{
			get => BindingContext as SaveViewModel;
		}
	}
}