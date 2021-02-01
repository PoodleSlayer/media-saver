using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using PhotoHelper.IoC;
using UIKit;

namespace PhotoHelper.iOS.IoC
{
	public class WebService : IWebService
	{
		public string UserAgent { get; set; }
		public void ClearCache()
		{
			NSHttpCookieStorage CookieStorage = NSHttpCookieStorage.SharedStorage;
			foreach (var cookie in CookieStorage.Cookies)
			{
				CookieStorage.DeleteCookie(cookie);
			}
		}

		public string GetCookies()
		{
			return "";
		}
	}
}
