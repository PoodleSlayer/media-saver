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
	public partial class GalleryPage : ContentPage
	{
		public GalleryPage()
		{
			InitializeComponent ();
			BindingContext = AppContainer.Container.Resolve<GalleryViewModel>();

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

		private void BackBtn_Clicked(object sender, EventArgs e)
		{
			Navigation.PopModalAsync();
		}

		private GalleryViewModel ViewModel
		{
			get => BindingContext as GalleryViewModel;
		}
	}
}