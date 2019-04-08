using Autofac;
using PhotoHelper.IoC;
using PhotoHelper.Utility;
using PhotoHelper.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PhotoHelper.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FileBrowserPage : ContentPage
	{
		private ObservableCollection<string> directoryList;
		private IFileService fileHelper;

		public FileBrowserPage ()
		{
			InitializeComponent ();

			fileHelper = AppContainer.Container.Resolve<IFileService>();
			directoryList = new ObservableCollection<string>();
			FileListView.ItemsSource = directoryList;

			BackBtn.Clicked += BackBtn_Clicked;
			OpenBtn.Clicked += OpenBtn_Clicked;
			SelectBtn.Clicked += SelectBtn_Clicked;
			FileListView.ItemSelected += FileListView_ItemSelected;
		}

		private void SelectBtn_Clicked(object sender, EventArgs e)
		{
			// return the selected folder name to the SettingsPage
			if (FileListView.SelectedItem == null)
			{
				return;
			}

			AppContainer.Container.Resolve<SettingsViewModel>().SaveLocation = (string)FileListView.SelectedItem;
			AppContainer.Container.Resolve<IFileService>().SaveLocation = (string)FileListView.SelectedItem;

			Navigation.PopModalAsync();
		}

		private void OpenBtn_Clicked(object sender, EventArgs e)
		{
			// query the selected folder for subdirectories
			if (FileListView.SelectedItem == null)
			{
				return;
			}
			UpdateDirectoryList((string)FileListView.SelectedItem);
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			UpdateDirectoryList();
		}

		private void FileListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			// ideally i should reuse the same ObservableCollection and just repopulate it
			//UpdateDirectoryList((string)e.SelectedItem);
			;
		}

		private void UpdateDirectoryList(string filepath = "")
		{
			directoryList.Clear();
			List<string> newFolders;
			if (String.IsNullOrEmpty(filepath))
			{
				newFolders = fileHelper.GetDirectories();
			}
			else
			{
				newFolders = fileHelper.GetDirectories(filepath);
			}
			foreach (string folder in newFolders)
			{
				directoryList.Add(folder);
			}
		}

		private void BackBtn_Clicked(object sender, EventArgs e)
		{
			Navigation.PopModalAsync();
		}
	}
}