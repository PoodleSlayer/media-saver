using System;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using PhotoHelper.Droid.IoC;
using static Android.App.ActivityManager;

namespace PhotoHelper.Droid
{
    [Activity(Label = "PhotoHelper", Icon = "@mipmap/icon", Theme = "@style/SplashTheme.Splash", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
			// once created, swap back to the main style.
			// this is a quick and dirty way to handle the splash screen rather
			// than switching to a whole new activity.
			base.Window.RequestFeature(Android.Views.WindowFeatures.ActionBar);
			base.SetTheme(Resource.Style.MainTheme);

			// in versions later than Lollipop let's style the TaskDescription a bit
			// THIS SEEMS REALLY GROSS but supposedly it's actually how you do it?
			// seems sketchy to look up the app icon like this when the default
			// taskdescription already has it just fine...
			if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
			{
				Bitmap appIcon = BitmapFactory.DecodeResource(Resources, Resource.Mipmap.icon);
				TaskDescription description = new TaskDescription(ApplicationInfo.LoadLabel(PackageManager), appIcon, Color.ParseColor("#0400FF"));
				base.SetTaskDescription(description);
			}

			TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

#if DEBUG
			global::Android.Webkit.WebView.SetWebContentsDebuggingEnabled(true);
#else
			global::Android.Webkit.WebView.SetWebContentsDebuggingEnabled(false);
#endif

			base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App(new SetupDroid()));

			// additional setup, if needed
			Task.Run(() => GetFilePermissionsAsync());
        }

		protected override void OnResume()
		{
			// in case we need some special handling when the app resumes from background...
			base.OnResume();
		}

		/// <summary>
		/// This method will prompt the user for permission to read/write to external storage.
		/// </summary>
		/// <returns></returns>
		async Task GetFilePermissionsAsync()
		{
			const string permission = Manifest.Permission.WriteExternalStorage;
			string[] permissionList =
			{
				Manifest.Permission.ReadExternalStorage,
				Manifest.Permission.WriteExternalStorage
			};
			if (ContextCompat.CheckSelfPermission(this, permission) == (int)Permission.Granted)
			{
				// do nothing? app should be able to read/write external storage
				;
			}

			if (ActivityCompat.ShouldShowRequestPermissionRationale(this, permission))
			{
				// make a layout view to tell the user what this permission is for
				;
			}

			ActivityCompat.RequestPermissions(this, permissionList, 10);
		}
    }
}