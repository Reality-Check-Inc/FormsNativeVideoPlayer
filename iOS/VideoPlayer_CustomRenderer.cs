using System;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

using FormsNativeVideoPlayer;
using FormsNativeVideoPlayer.iOS;

using AVFoundation;
using Foundation;
using UIKit;
using CoreGraphics;
using AVKit;

[assembly: ExportRenderer(typeof(StreamingVideoView), typeof(VideoPlayer_CustomRenderer))]

namespace FormsNativeVideoPlayer.iOS
{
    public class VideoPlayer_CustomRenderer : ViewRenderer
	{
		string movieUrl;
		//globally declare variables
		AVAsset _asset;
		AVPlayerItem _playerItem;
		AVPlayer _player;

		AVPlayerLayer _playerLayer;
		UIButton playButton;

		protected override void OnElementChanged (ElementChangedEventArgs<View> e)
		{
			base.OnElementChanged (e);

            Console.WriteLine("FormsNativeVideoPlayer.OnElementChanged");
			if (e.NewElement != null) {
				movieUrl = ((StreamingVideoView)e.NewElement).VideoUrl;
			}

			//Get the video
			//bubble up to the AVPlayerLayer
			
            var url = new NSUrl (movieUrl);
			_asset = AVAsset.FromUrl (url);

			_playerItem = new AVPlayerItem (_asset);

			_player = new AVPlayer (_playerItem);

			_playerLayer = AVPlayerLayer.FromPlayer (_player);

			//Create the play button
			playButton = new UIButton ();
			playButton.SetTitle ("Play Video", UIControlState.Normal);
			playButton.BackgroundColor = UIColor.Gray;

			//Set the trigger on the play button to play the video
			playButton.TouchUpInside += (object sender, EventArgs arg) => {
				_player.Play();
			};
		}

        public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            Console.WriteLine("FormsNativeVideoPlayer.GetDesiredSize : {0} x {1}", widthConstraint, heightConstraint);
			return new SizeRequest(Size.Zero, Size.Zero);
        }

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
            Console.WriteLine("FormsNativeVideoPlayer.LayoutSubviews : {0} x {1} : {2} x {3}",
                              NativeView.Frame.Width, NativeView.Frame.Height,
                              UIKit.UIScreen.MainScreen.Bounds.Width, UIKit.UIScreen.MainScreen.Bounds.Height);

			//layout the elements depending on what screen orientation we are. 
			if (DeviceHelper.iOSDevice.Orientation == UIDeviceOrientation.Portrait) {
				playButton.Frame = new CGRect (0, NativeView.Frame.Bottom - 50, NativeView.Frame.Width, 50);
				_playerLayer.Frame = NativeView.Frame;
				NativeView.Layer.AddSublayer (_playerLayer);
				NativeView.Add (playButton);
			} else if (DeviceHelper.iOSDevice.Orientation == UIDeviceOrientation.LandscapeLeft || DeviceHelper.iOSDevice.Orientation == UIDeviceOrientation.LandscapeRight) {
				_playerLayer.Frame = UIKit.UIScreen.MainScreen.Bounds;
				NativeView.Layer.AddSublayer (_playerLayer);
				playButton.Frame = new CGRect (0, 0, 0, 0);
			}
		}
	}
}