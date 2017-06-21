using System;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using FormsNativeVideoPlayer;
using FormsNativeVideoPlayer.Droid;

using Android.Views;
using Android.Widget;

[assembly: ExportRenderer(typeof(StreamingVideoView), typeof(VideoPlayer_CustomRenderer))]
namespace FormsNativeVideoPlayer.Droid
{
	public class VideoPlayer_CustomRenderer : ViewRenderer
	{
        VideoView _videoView;
        Android.Widget.MediaController _mediaController;

		string _movieUrl;

		protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.View> e)
        {
            base.OnElementChanged(e);

            if (Control == null) {
                _videoView = new VideoView(Context);
				_mediaController = new MediaController(Context);
				_videoView.SetMediaController(_mediaController);
                SetNativeControl(_videoView);
			}

            if (e.OldElement != null) {
            }

            if (e.NewElement != null)
            {
                _movieUrl = ((StreamingVideoView)e.NewElement).VideoUrl;
                Console.WriteLine("FormsNativeVideoPlayer.OnElementChanged {0}", _movieUrl);

                var uri = Android.Net.Uri.Parse(_movieUrl);
                _videoView.SetVideoURI(uri);
                _videoView.RequestFocus();

                //videoView.SetOnPreparedListener(new SetOn)
                // show a frame...
                _videoView.SeekTo(100);
            }
		}      

		protected override void OnLayout (bool changed, int l, int t, int r, int b)
		{
			base.OnLayout (changed, l, t, r, b);
			int width = r - l;
			int height = b - t;
            Console.WriteLine("FormsNativeVideoPlayer.OnLayout {0} x {1}", width, height);
			_videoView.Layout(0, 0, width, height);
			_videoView.Holder.SetFixedSize(width, height);
		}

		protected override void OnConfigurationChanged (Android.Content.Res.Configuration newConfig)
		{
			base.OnConfigurationChanged (newConfig);
			Console.WriteLine("FormsNativeVideoPlayer.OnConfigurationChanged");

			// Hide the Status Bar when in full screen. 
			if (newConfig.Orientation == Android.Content.Res.Orientation.Landscape) {
				StatusBarHelper.DecorView.SystemUiVisibility = StatusBarVisibility.Hidden;
				//If you have an ActionBar, uncomment the line below
				//StatusBarHelper.AppActionBar.Hide ();
			} else {
				StatusBarHelper.DecorView.SystemUiVisibility = StatusBarVisibility.Visible;
				//If you have an ActionBar, uncomment the line below
				//StatusBarHelper.AppActionBar.Show ();
			}
		}
	}
}