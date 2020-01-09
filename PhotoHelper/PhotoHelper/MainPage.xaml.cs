using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

using PhotoHelper.Views;
using PhotoHelper.IoC;
using Autofac;
using PhotoHelper.ViewModels;
using PhotoHelper.Utility;
using PhotoHelper.Models;
using System.Diagnostics;

namespace PhotoHelper
{
	public partial class MainPage : ContentPage
	{
		private HttpClient client;
		private GalleryPage galleryPage;
		private SettingsPage settingsPage;
		private SavePage savePage;
		private IFileService fileHelper;
		private string currentURL;
		private double screenHeight;
		private double screenWidth;

		public MainPage()
		{
			InitializeComponent();

			BindingContext = AppContainer.Container.Resolve<MainViewModel>();
			fileHelper = AppContainer.Container.Resolve<IFileService>();

			DownloadBtn.Clicked += DownloadBtn_Clicked;
			BackBtn.Clicked += BackBtn_Clicked;
			SettingsBtn.Clicked += SettingsBtn_Clicked;
			GalleryBtn.Clicked += GalleryBtn_Clicked;
			SaveBtn.Clicked += SaveBtn_Clicked;
			URLBtn.Clicked += URLBtn_Clicked;
			HideBtn.Clicked += HideBtn_Clicked;

			webby.Navigated += Webby_Navigated;
			ViewModel.URLChanged += ViewModel_URLChanged;
			SizeChanged += MainPage_SizeChanged;

			// load the Settings and any other stuff we may need
			Settings.LoadSettings();
			if (!String.IsNullOrEmpty(Settings.DefaultPageURL))
			{
				// kinda goofy but this intercepts the webview's initial URL
				// otherwise we'd have to deal with navigating twice when the
				// ViewModel's URL changes which looks weird
				webby.Source = Settings.DefaultPageURL;
			}

			// initialize all pages since there's only a few
			client = new HttpClient();
			galleryPage = new GalleryPage();
			settingsPage = new SettingsPage();
			savePage = new SavePage();

			// platform specific layout, if needed
			if (Device.RuntimePlatform == Device.Android)
			{
				AlbumIndexStackLayout.Margin = new Thickness(0, -15, 0, 0);
			}
		}

		private async void HideBtn_Clicked(object sender, EventArgs e)
		{
			await RemovePopup();
		}

		private async Task<bool> RemovePopup(bool waitToTry = false)
		{
			// attempt to remove the Login popup...

			if (waitToTry)
			{
				await Task.Delay(1000);
			}

			string dialogHTML = await webby.EvaluateJavaScriptAsync("function fixit(){var a = document.querySelector(\"div[role = 'dialog']\").parentNode.style.visibility = \"hidden\"; var b = document.getElementsByTagName(\"body\")[0].style.removeProperty(\"overflow\");}");
			string runDialogHTML = await webby.EvaluateJavaScriptAsync("fixit()");
			return true;
		}

		private void MainPage_SizeChanged(object sender, EventArgs e)
		{
			// on orientation change? and window resize in UWP?
			screenHeight = this.Height;
			screenWidth = this.Width;

			if (screenHeight > screenWidth)
			{
				// orientation is Portrait
				if (screenHeight < 600)
				{
					ViewModel.ButtonFontSize = 12;
				}
				else
				{
					ViewModel.ButtonFontSize = 16;
				}
			}
			else
			{
				// orientation is Landscape
				if (screenWidth < 500)
				{
					ViewModel.ButtonFontSize = 10;
				}
				else
				{
					ViewModel.ButtonFontSize = 14;
				}
			}
		}

		private void ViewModel_URLChanged(object sender, EventArgs e)
		{
			// when app is first loaded the WebView will navigate once and the currentURL will be null,
			// so just ignore that.
			if (ViewModel.CurrentURL != currentURL && !String.IsNullOrEmpty(currentURL))
			{
				webby.Source = ViewModel.CurrentURL;
			}
		}

		private void URLBtn_Clicked(object sender, EventArgs e)
		{
			LoadNewURL(URLEntry.Text);

			// strip the URL down to just the username so it looks nicer if a user 
			// pastes a URL into the app
			string tempURL = URLEntry.Text;

			if (String.IsNullOrEmpty(tempURL))
			{
				return;
			}

			if (tempURL.Contains(@"instagram.com"))
			{
				var URLParts = tempURL.Split(new string[] { ".com/" }, StringSplitOptions.None);
				tempURL = URLParts[1]; // should be everything after the .com/
				URLParts = tempURL.Split('/');
				URLEntry.Text = URLParts[0];
			}
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			screenHeight = this.Height;
			screenWidth = this.Width;
		}

		protected override bool OnBackButtonPressed()
		{
			//WebViewGoBack();
			//AlbumIndex.Text = string.Empty;
			GoBack();

			return true;
		}

		private void Webby_Navigated(object sender, WebNavigatedEventArgs e)
		{
			// THIS IS GETTING CALLED MULTIPLE TIMES ON ANDROID
			// might be a bug with this version of XForms WebView. Try upgrading
			// or just making a custom renderer

			// don't care if they navigate to a specific image, so don't store these
			if (e.Url.Contains(@"instagram.com/p/"))
			{
				RemovePopup(true);
				return;
			}

			currentURL = e.Url;
			ViewModel.CurrentURL = e.Url;
		}

		private void SaveBtn_Clicked(object sender, EventArgs e)
		{
			Navigation.PushModalAsync(savePage);
		}

		private async void GalleryBtn_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushModalAsync(galleryPage);
		}

		private void SettingsBtn_Clicked(object sender, EventArgs e)
		{
			Navigation.PushModalAsync(settingsPage);
		}

		private void BackBtn_Clicked(object sender, EventArgs e)
		{
			//WebViewGoBack();
			//RemovePopup(true);
			//AlbumIndex.Text = string.Empty;
			GoBack();
		}

		private void GoBack()
		{
			WebViewGoBack();
			RemovePopup(true);
			AlbumIndex.Text = string.Empty;
		}

		private void WebViewGoBack()
		{
			webby.GoBack();
		}

		private async void DownloadBtn_Clicked(object sender, EventArgs e)
		{
			//*****************************************************************
			// just a graveyard of stupid stuff i tried for future Chris to learn from
			//
			//string testSource = await webby.EvaluateJavaScriptAsync("document.body.getElementsByTagName('img')[1]");
			//
			//string pageSource = await webby.EvaluateJavaScriptAsync("document.head.innerHTML");
			//string pageSource = await webby.EvaluateJavaScriptAsync("document.body.innerHTML");
			//string jsHorseCrap = "function GetThingPlease(){const metas = document.getElementsByTagName('meta'); for (let i = 0; i < metas.length; i++) { if (metas[i].getAttribute('property') === 'og:image') { return metas[i].getAttribute('content'); } } return '';}";
			//string jsHorseCrap = "function GetThingPlease(){const imgs = document.getElementsByTagName('img'); for (let i = 0; i < imgs.length; i++) { if (i == 1) { return imgs[i].getAttribute('src'); } } return '';}";
			//
			// uhhhhh
			//string jsHorseCrap = "function GetThingPlease(){const imgs = document.getElementsByTagName('img'); imgArray = ''; for (let i = 0; i < imgs.length; i++) { imgArray = imgArray + imgs[i].getAttribute('src') + ', '; } return imgArray;}";
			//
			//string pleaseWork = await webby.EvaluateJavaScriptAsync(jsHorseCrap);
			//string pleaseWork2 = await webby.EvaluateJavaScriptAsync("GetThingPlease()");
			//*****************************************************************

			// DEBUGGING ----------------------------------
			// Instagram added some weird login dialog that shows up after viewing a few pictures
			// while not logged in :c
			//
			//string pageSource = await webby.EvaluateJavaScriptAsync("document.body.innerHTML");
			//string dialogHTML = await webby.EvaluateJavaScriptAsync("document.querySelector(\"div[role = 'dialog']\").parentNode.outerHTML = \"\"");
			//string dialogHTML = await webby.EvaluateJavaScriptAsync("document.querySelector(\"div[role = 'dialog']\").parentNode.remove()");
			//string dialogHTML = await webby.EvaluateJavaScriptAsync("function fixit(){document.querySelector(\"div[role = 'dialog']\").parentNode.remove();}");
			//string runDialogHTML = await webby.EvaluateJavaScriptAsync("fixit()");
			//return;
			// DEBUGGING ----------------------------------

			// d'oh, don't attempt to save if user hasn't specified a location
			if (String.IsNullOrEmpty(Settings.SaveLocation))
			{
				await DisplayAlert("Specify Location", "Please specify a location to save files in the Settings page", "Oops OK Thanks!");
				return;
			}

			string pageUrl = await webby.EvaluateJavaScriptAsync("document.location.href");
			string urlToDownload = await GetImgUrl(pageUrl);
			
			await DownloadURL(urlToDownload);
		}

		/// <summary>
		/// 
		/// <para>
		/// This method will async parse through the json response to try to find the media URLs
		/// to download. Should probably refactor it to handle the downloads as well, or at least
		/// call the download methods from here.
		/// </para>
		/// 
		/// <para>
		/// Current limitation is with gallery images. With the json response there isn't a good
		/// way to tell WHICH image in the gallery the user is viewing. Might just make this save
		/// ALL images of the gallery. Other option would be to have user specify a number for
		/// which photo in the gallery to grab, if it exists. Seems clunky though.
		/// </para>
		/// 
		/// </summary>
		/// <param name="imgPage"></param>
		/// <returns></returns>
		private async Task<string> GetImgUrl(string imgPage)
		{
			var uri = new Uri(imgPage + "?__a=1");
			HttpResponseMessage response = null;
			try
			{
				//var response = await client.GetAsync(uri);
				response = await client.GetAsync(uri);
			}
			catch (Exception e)
			{
				Debug.WriteLine(e.Message);
			}
			if (response.IsSuccessStatusCode)
			{
				var content = await response.Content.ReadAsStringAsync();

				JObject jsonContent = (JObject)JsonConvert.DeserializeObject(content);
				
				if ((bool)jsonContent["graphql"]["shortcode_media"]["is_video"] == true)
				{
					// it's a video!
					string videoURL = jsonContent["graphql"]["shortcode_media"]["video_url"].ToString();
					return videoURL;
				}
				else if (jsonContent["graphql"]["shortcode_media"]["edge_sidecar_to_children"] != null)
				{
					// it's an album!
					int albumIndex = 0;
					if (!String.IsNullOrEmpty(AlbumIndex.Text))
					{
						var isNumeric = int.TryParse(AlbumIndex.Text, out int n);
						if (isNumeric)
						{
							albumIndex = n > 0 ? n - 1 : 0; // for normal users we'll say this is 1-indexed
						}
						// else it just stays 0
					}

					JArray albumArray = (JArray)jsonContent["graphql"]["shortcode_media"]["edge_sidecar_to_children"]["edges"];
					if (albumIndex > albumArray.Count - 1 && albumArray.Count != 0)
					{
						return "";
					}

					JArray imgSources = (JArray)albumArray[albumIndex]["node"]["display_resources"];
					JObject bestImg = (JObject)imgSources.OrderByDescending(x => x["config_width"]).FirstOrDefault();

					return bestImg["src"].ToString();
				}
				else
				{
					// just a normal image
					JArray imgSources = (JArray)jsonContent["graphql"]["shortcode_media"]["display_resources"];
					JObject bestImg = (JObject)imgSources.OrderByDescending(x => x["config_width"]).FirstOrDefault();

					// for image galleries, need to look for property "edge_sidecar_to_children" in "shortcode_media". not
					// sure how to determine WHICH image in the gallery is being viewed though...

					return bestImg["src"].ToString();
				}
			}
			return "";
		}

		private async Task<bool> DownloadURL(string downloadURL)
		{
			if (String.IsNullOrEmpty(downloadURL) || String.IsNullOrEmpty(currentURL))
			{
				return false;
			}


			var pageCollection = App.Database.GetCollection<PageModel>(PageModel.CollectionName);
			PageModel selectedPage = pageCollection.FindOne(x => x.PageURL == currentURL);

			string filenameToUse = "";
			if (selectedPage != null && !String.IsNullOrEmpty(selectedPage.PageFileName))
			{
				filenameToUse = selectedPage.PageFileName;
			}
			else
			{
				//filenameToUse = "test_img";
				var urlIndex = currentURL.IndexOf(@".com/");
				var urlSubString = currentURL.Substring(urlIndex + 5);  // the ".com/" above is 5 chars
				urlSubString = urlSubString.Replace(@"/", "");

				if (!String.IsNullOrEmpty(urlSubString))
				{
					filenameToUse = urlSubString;
				}
				else
				{
					filenameToUse = "downloaded_img";
				}
			}
			// async download media from the URL. should be an image or a video.
			// save in the file path specified by the user in Settings.

			// wrap this in a try/catch
			await fileHelper.DownloadFile(downloadURL, filenameToUse);

			if (Settings.DownloadFeedback == Settings.DownloadToast)
			{
				// TOAST
				// not sure if this causes problems running NOT on the UI thread...
				AppContainer.Container.Resolve<IToastService>().MakeToast("Downloaded!");
			}
			else if (Settings.DownloadFeedback == Settings.DownloadNotif)
			{
				// NOTIFICATION
				AppContainer.Container.Resolve<INotifierService>().Notify(filenameToUse);
			}
			else
			{
				// DO NOTHING
			}

			// if download successful, toast or notify the user

			return true;
		}

		private void LoadNewURL(string urlName)
		{
			if (String.IsNullOrEmpty(urlName))
			{
				return;
			}

			if (urlName.StartsWith("http"))
			{
				// user entered the URL directly, just try to load it
				webby.Source = urlName;
			}
			else if (urlName.StartsWith("@"))
			{
				webby.Source = @"https://www.instagram.com/" + urlName.Replace("@", "") + @"/";
			}
			else
			{
				webby.Source = @"https://www.instagram.com/" + urlName + @"/";
			}
		}

		/// <summary>
		/// Helper readonly property for interacting with the BindingContext
		/// </summary>
		private MainViewModel ViewModel
		{
			get => BindingContext as MainViewModel;
		}
	}
}
