using System;

using Xamarin.Forms;

namespace FormsNativeVideoPlayer
{
	public class LandingPage : ContentPage
	{
		StreamingVideoView videoPlayer;

		public LandingPage ()
		{
			videoPlayer = new StreamingVideoView {
				WidthRequest = App.ScreenWidth,
				HeightRequest = App.ScreenHeight/3,
                VideoUrl = "https://logminds.com/video/30011-2.mp4",
			};

			Content = new StackLayout {
				Children = {
					videoPlayer
				}
			};
		}

		protected override void LayoutChildren (double x, double y, double width, double height)
		{
			//need to change the size of the ContentView for Landscape Orientation
			//This enables fullscreen capabilities in the Custom Renderer
			if (width > height) {
				//Landscape Orientation
				videoPlayer.WidthRequest = App.ScreenWidth;
				videoPlayer.HeightRequest = App.ScreenHeight;
			} else if (width < height) {
				//Portrait Orientation
				videoPlayer.WidthRequest = App.ScreenWidth/2;
				videoPlayer.HeightRequest = App.ScreenHeight/2;
			}
				
			base.LayoutChildren (x, y, width, height);
		}
	}
}