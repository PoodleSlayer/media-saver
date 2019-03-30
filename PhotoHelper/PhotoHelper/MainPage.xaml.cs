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

namespace PhotoHelper
{
	public partial class MainPage : ContentPage
	{
		private HttpClient client;
		private GalleryPage galleryPage;
		private SettingsPage settingsPage;
		private SavePage savePage;
		private IFileService fileHelper;

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
			webby.Navigated += Webby_Navigated;

			client = new HttpClient();
			galleryPage = new GalleryPage();
			settingsPage = new SettingsPage();
			savePage = new SavePage();
		}

		private void Webby_Navigated(object sender, WebNavigatedEventArgs e)
		{
			ViewModel.CurrentURL = e.Url;
		}

		private void SaveBtn_Clicked(object sender, EventArgs e)
		{
			Navigation.PushModalAsync(savePage);
		}

		private void GalleryBtn_Clicked(object sender, EventArgs e)
		{
			Navigation.PushModalAsync(galleryPage);
		}

		private void SettingsBtn_Clicked(object sender, EventArgs e)
		{
			Navigation.PushModalAsync(settingsPage);
		}

		private void BackBtn_Clicked(object sender, EventArgs e)
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

			string pageUrl = await webby.EvaluateJavaScriptAsync("document.location.href");
			string imgJson = await GetImgUrl(pageUrl);

			await DownloadURL(imgJson);
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
			var response = await client.GetAsync(uri);
			if (response.IsSuccessStatusCode)
			{
				var content = await response.Content.ReadAsStringAsync();

				JObject jsonContent = (JObject)JsonConvert.DeserializeObject(content);

				// check if video
				if ((bool)jsonContent["graphql"]["shortcode_media"]["is_video"] == true)
				{
					// it's a video!
					string videoURL = jsonContent["graphql"]["shortcode_media"]["video_url"].ToString();
					return videoURL;
				}

				// check if gallery
				if (jsonContent["graphql"]["shortcode_media"].Contains("edge_sidecar_to_children"))
				{
					; // it's a gallery!
				}

				JArray imgSources = (JArray)jsonContent["graphql"]["shortcode_media"]["display_resources"];
				JObject bestImg = (JObject)imgSources.OrderByDescending(x => x["config_width"]).FirstOrDefault();

				// for image galleries, need to look for property "edge_sidecar_to_children" in "shortcode_media". not
				// sure how to determine WHICH image in the gallery is being viewed though...

				return bestImg["src"].ToString();
			}
			return "";
		}

		private async Task<bool> DownloadURL(string downloadURL)
		{
			if (String.IsNullOrEmpty(downloadURL))
			{
				return false;
			}

			// async download media from the URL. should be an image or a video.
			// save in the file path specified by the user in Settings.
			await fileHelper.DownloadFile(downloadURL);

			return true;
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
