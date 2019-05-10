using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using PhotoHelper.IoC;
using UIKit;

namespace PhotoHelper.iOS.IoC
{
	public class ToastService : IToastService
	{
		//NSTimer alertDelay;
		//UIAlertController alert;

		public void MakeToast(string message)
		{
			// just want a quick popup like Android Toasts
			UIAlertController alert = UIAlertController.Create(null, message, UIAlertControllerStyle.Alert);

			NSTimer alertDelay = NSTimer.CreateScheduledTimer(3.0, (obj) =>
			{
				// passing a new alert and alertDelay each time doesn't seem ideal,
				// maybe think of a better way to do this. probably one alert with a 
				// cancellation token or something similar
				DismissMessage(alert, obj);
			});

			UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(alert, true, null);
		}

		private void DismissMessage(UIAlertController alert, NSTimer alertDelay)
		{
			if (alert != null)
			{
				alert.DismissViewController(true, null);
			}
			if (alertDelay != null)
			{
				alertDelay.Dispose();
			}
		}
	}
}