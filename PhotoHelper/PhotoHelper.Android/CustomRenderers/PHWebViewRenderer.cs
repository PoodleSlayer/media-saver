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
using Java.Interop;
using PhotoHelper.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Xamarin.Forms.WebView), typeof(PhotoHelper.Droid.CustomRenderers.PHWebViewRenderer))]
namespace PhotoHelper.Droid.CustomRenderers
{
	public class PHWebViewRenderer : WebViewRenderer
	{
		new IWebViewController ElementController => Element;
		Context _context;
		const string JavascriptFunction = "function invokeCSharpAction(data){jsBridge.invokeAction(data);}";

		public PHWebViewRenderer(Context context) : base(context)
		{
			_context = context;
			SetLayerType(LayerType.Hardware, null);
		}

		//protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
		//{
		//	base.OnElementChanged(e);

		//	// hopefully this tells our PHWebView not to keep track of cookies, which can cause issues
		//	Control.Settings.SetAppCacheEnabled(false);
		//	Control.Settings.CacheMode = Android.Webkit.CacheModes.NoCache;
		//}


		// this was some goofy attempt at seeing why Android WebView fires Navigated event multiple times
		// based on this cool guy here:
		// https://forums.xamarin.com/discussion/138821/navigating-on-android-webview-doesnt-always-fire

		protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null)
			{
				Control.RemoveJavascriptInterface("jsBridge");
				((PHWebView)Element).Cleanup();
			}

			if (e.NewElement != null)
			{
				Control.SetWebViewClient(new WebClient(this, $"javascript: {JavascriptFunction}"));
				Control.AddJavascriptInterface(new JSBridge(this), "jsBridge");
				Control.Settings.SetAppCacheEnabled(false);
				Control.Settings.CacheMode = CacheModes.NoCache;
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				((PHWebView)Element).Cleanup();
			}

			base.Dispose(disposing);
		}

		class WebClient : FormsWebViewClient
		{
			string _javascript;

			public WebClient(PHWebViewRenderer renderer, string javascript) : base(renderer)
			{
				_javascript = javascript;
			}

			public override void OnPageFinished(Android.Webkit.WebView view, string url)
			{
				base.OnPageFinished(view, url);
				view.EvaluateJavascript(_javascript, null);
			}
		}

		class JSBridge : Java.Lang.Object
		{
			readonly WeakReference<PHWebViewRenderer> phWebViewRenderer;

			public JSBridge(PHWebViewRenderer webViewRenderer)
			{
				phWebViewRenderer = new WeakReference<PHWebViewRenderer>(webViewRenderer);
			}

			[JavascriptInterface]
			[Export("invokeAction")]
			public void InvokeAction(string data)
			{
				PHWebViewRenderer webViewRenderer;

				if (phWebViewRenderer != null && phWebViewRenderer.TryGetTarget(out webViewRenderer))
				{
					((PHWebView)webViewRenderer.Element).InvokeAction(data);
				}
			}
		}

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