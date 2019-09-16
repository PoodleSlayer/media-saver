using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Xamarin.Forms.WebView), typeof(PhotoHelper.Droid.CustomRenderers.PHWebViewRenderer))]
namespace PhotoHelper.Droid.CustomRenderers
{
	public class PHWebViewRenderer : WebViewRenderer
	{
		new IWebViewController ElementController => Element;

		public PHWebViewRenderer(Context context) : base(context)
		{

		}

		protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
		{
			base.OnElementChanged(e);

			// hopefully this tells our PHWebView not to keep track of cookies, which can cause issues
			Control.Settings.SetAppCacheEnabled(false);
			Control.Settings.CacheMode = Android.Webkit.CacheModes.NoCache;
		}


		// this was some goofy attempt at seeing why Android WebView fires Navigated event multiple times
		// based on this cool guy here:
		// https://forums.xamarin.com/discussion/138821/navigating-on-android-webview-doesnt-always-fire

		//protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
		//{
		//	base.OnElementChanged(e);

		//	if (Control != null)
		//	{
		//		Control.SetWebViewClient(new WebClient(this));
		//	}
		//}

		//class WebClient : WebViewClient
		//{
		//	PHWebViewRenderer Renderer;

		//	public WebClient(PHWebViewRenderer renderer)
		//	{
		//		Renderer = renderer ?? throw new ArgumentNullException("renderer");
		//	}

		//	public override void OnPageFinished(Android.Webkit.WebView view, string url)
		//	{
		//		base.OnPageFinished(view, url);

		//		var source = new UrlWebViewSource { Url = url };
		//		var args = new WebNavigatedEventArgs(WebNavigationEvent.NewPage, source, url, WebNavigationResult.Success);
		//		Renderer.ElementController.SendNavigated(args);
		//	}

		//	public override void OnPageStarted(Android.Webkit.WebView view, string url, Bitmap favicon)
		//	{
		//		base.OnPageStarted(view, url, favicon);

		//		var args = new WebNavigatingEventArgs(WebNavigationEvent.NewPage, new UrlWebViewSource { Url = url }, url);
		//		Renderer.ElementController.SendNavigating(args);
		//	}
		//}
	}
}