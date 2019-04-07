using System;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using PhotoHelper.Droid.IoC;

namespace PhotoHelper.Droid
{
    [Activity(Label = "PhotoHelper", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App(new SetupDroid()));

			// additional setup, if needed
			Task.Run(() => GetFilePermissionsAsync());
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