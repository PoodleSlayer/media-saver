using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using PhotoHelper.IoC;

namespace PhotoHelper.Droid.IoC
{
	public class WebService : IWebService
	{
		public string UserAgent { get; set; }
		public void ClearCache()
		{
			var cookieManager = CookieManager.Instance;

			cookieManager.RemoveAllCookie();
			cookieManager.RemoveSessionCookie();
		}

		public string GetCookies()
		{
			// sometimes this crashes but i have yet to reliably reproduce it...
			var cookieManager = CookieManager.Instance;

			string cookies = cookieManager.GetCookie("https://www.tiktok.com");

			return cookies;
		}
	}
}