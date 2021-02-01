using GalaSoft.MvvmLight.Command;
using System;

namespace PhotoHelper.ViewModels
{
	/// <summary>
	/// This is the ViewModel for the main page where the WebView lives.
	/// Not sure what all could be useful here.
	/// </summary>
    public class MainViewModel : PHViewModel
    {
		public event EventHandler URLChanged;
		public RelayCommand ToggleCommand { get; set; }

		public MainViewModel()
		{
			SetupCommands();
			IsInstagram = true;
			ModeLabel = "Instagram";
		}

		private void SetupCommands()
		{
			ToggleCommand = new RelayCommand(() =>
			{
				ToggleMode();
			});
		}

		private void ToggleMode()
		{
			// flip them! should always be opposite
			IsInstagram = !IsInstagram;
			IsTikTok = !IsTikTok;
			if (IsInstagram)
			{
				ModeLabel = "Instagram";
			}
			else
			{
				ModeLabel = "TikTok";
			}
		}
		
		private void OnURLChanged()
		{
			EventHandler handler = URLChanged;
			handler?.Invoke(this, null);
		}

		private string currentURL;
		public string CurrentURL
		{
			get
			{
				return currentURL;
			}
			set
			{
				if (currentURL != value)
				{
					currentURL = value;
					RaisePropertyChanged("CurrentURL");
					OnURLChanged();
				}
			}
		}

		private int buttonFontSize = 16;
		public int ButtonFontSize
		{
			get => buttonFontSize;
			set
			{
				if (buttonFontSize != value)
				{
					buttonFontSize = value;
					RaisePropertyChanged("ButtonFontSize");
				}
			}
		}

		private bool masterWebViewVisible = true;
		public bool MasterWebViewVisible
		{
			get => masterWebViewVisible;
			set
			{
				if (masterWebViewVisible != value)
				{
					masterWebViewVisible = value;
					RaisePropertyChanged("MasterWebViewVisible");
				}
			}
		}

		private bool detailWebViewVisible = false;
		public bool DetailWebViewVisible
		{
			get => detailWebViewVisible;
			set
			{
				if (detailWebViewVisible != value)
				{
					detailWebViewVisible = value;
					RaisePropertyChanged("DetailWebViewVisible");
				}
			}
		}

		public bool IsInstagram { get; set; }

		public bool IsTikTok { get; set; }

		private string modeLabel = "";
		public string ModeLabel
		{
			get => modeLabel;
			set
			{
				if (modeLabel != value)
				{
					modeLabel = value;
					RaisePropertyChanged("ModeLabel");
				}
			}
		}

	}
}
