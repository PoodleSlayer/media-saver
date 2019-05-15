using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Notifications;
using PhotoHelper.IoC;
using Windows.UI.Notifications;

namespace PhotoHelper.UWP.IoC
{
	public class NotifierService : INotifierService
	{
		private ToastNotifier toaster = ToastNotificationManager.CreateToastNotifier();

		public void Notify(string msg)
		{
			// notify the user

			// build the ToastContent
			ToastContent toasty = new ToastContent()
			{
				Launch = "kittycats",
				Visual = new ToastVisual()
				{
					BindingGeneric = new ToastBindingGeneric()
					{
						Children =
						{
							new AdaptiveText()
							{
								Text = "Media downloaded!",
								HintMaxLines = 1
							},
							new AdaptiveText()
							{
								Text = "Finished downloading " + msg,
							},
						}
					}
				},
				Duration = ToastDuration.Short,
				Actions = null,
				Audio = new ToastAudio()
				{
					Silent = true,
				},
			};

			// make the actual Toast
			var toast = new ToastNotification(toasty.GetXml());
			toast.ExpirationTime = DateTime.Now.AddSeconds(15);

			// ding toast is ready i'm so funny
			toaster.Show(toast);
		}
	}
}
