using System;
using System.Diagnostics;
using Foundation;
using PhotoHelper.iOS.IoC;
using Photos;
using UIKit;

namespace PhotoHelper.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App(new SetupiOS()));

			CheckAuths();  // this sigabrts for some reason :c

			return base.FinishedLaunching(app, options);
		}

		/// <summary>
		/// Use this method to check for any necessary user permissions as soon as the app
		/// is first run. Might need to make async? Not sure about performance here.
		/// </summary>
		private void CheckAuths()
		{
			PHPhotoLibrary.RequestAuthorization(status =>
			{
				switch (status)
				{
					case PHAuthorizationStatus.Authorized:
					// user is permitted!
						Debug.WriteLine("user has access to Photos");
						break;
					case PHAuthorizationStatus.Denied:
						// user is NOT permitted :c
						Debug.WriteLine("user DOES NOT have access to Photos");
						break;
					case PHAuthorizationStatus.Restricted:
						// uhhhh
						Debug.WriteLine("user has restricted access to Photos?");
						break;
					default:
						break;
				}
			});
		}
    }
}
