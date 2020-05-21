using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoHelper.ViewModels
{
    public class DLViewModel : PHViewModel
    {
		public event EventHandler URLChanged;

		public DLViewModel()
		{
			// default constructor if needed
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
	}
}
