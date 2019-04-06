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
		
		private void OnURLChanged()
		{
			EventHandler handler = URLChanged;
			handler?.Invoke(this, null);
		}

		private string currentURL = "JVD";
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
    }
}
