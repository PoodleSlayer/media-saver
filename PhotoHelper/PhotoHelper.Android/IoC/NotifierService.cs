using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PhotoHelper.IoC;

namespace PhotoHelper.Droid.IoC
{
	public class NotifierService : INotifierService
	{
		public void Notify(string msg)
		{
			// user notification
		}
	}
}