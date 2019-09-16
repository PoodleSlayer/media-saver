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
		public void ClearCache()
		{
			var cookieManager = CookieManager.Instance;

			cookieManager.RemoveAllCookie();
			cookieManager.RemoveSessionCookie();
		}
	}
}