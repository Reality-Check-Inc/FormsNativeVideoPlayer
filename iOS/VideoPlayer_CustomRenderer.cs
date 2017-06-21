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
using MediaPlayer;

[assembly: ExportRenderer(typeof(StreamingVideoView), typeof(VideoPlayer_CustomRenderer))]

namespace FormsNativeVideoPlayer.iOS
{
    public class VideoPlayer_CustomRenderer : ViewRenderer
	{
        MPMoviePlayerController _mpc;

        AVPlayer _avp;
        AVPlayerLayer _avpl;

		string _movieUrl;
		AVAsset _asset;
		AVPlayerItem _playerItem;

		protected override void OnElementChanged (ElementChangedEventArgs<View> e)
		{
			base.OnElementChanged (e);

            if (_mpc == null) {
                _mpc = new MPMoviePlayerController();
             }
            /*
            if (_avp == null) {
                _avp = new AVPlayer();
				_avpl = AVPlayerLayer.FromPlayer(_avp);
			}
			 */

            if (e.OldElement != null) {
				Console.WriteLine("FormsNativeVideoPlayer.OnElementChanged old element adjusted");
			}

			if (e.NewElement != null) {
				_movieUrl = ((StreamingVideoView)e.NewElement).VideoUrl;
                Console.WriteLine("FormsNativeVideoPlayer.OnElementChanged {0}", _movieUrl);
				var url = new NSUrl(_movieUrl);
				_asset = AVAsset.FromUrl(url);
                if (_asset != null)
                {
                    Console.WriteLine("asset duration:D {0}", _asset.Duration);
                }

				if (_mpc != null) {
                    _mpc.ContentUrl = url;
                    _mpc.Play();
                }
                if (_avp != null) {
					_playerItem = new AVPlayerItem(_asset);
					_avp.ReplaceCurrentItemWithPlayerItem(_playerItem);
                }
			}
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

			if (DeviceHelper.iOSDevice.Orientation == UIDeviceOrientation.Portrait) {
                if (_mpc != null) {
                    _mpc.View.Frame = NativeView.Frame;
                    _mpc.ControlStyle = MPMovieControlStyle.Embedded;
					NativeView.Add(_mpc.View);
				}
                if (_avpl != null)
                {
                    _avpl.Frame = NativeView.Frame;
                    NativeView.Layer.AddSublayer(_avpl);
                }
			} else if (DeviceHelper.iOSDevice.Orientation == UIDeviceOrientation.LandscapeLeft || DeviceHelper.iOSDevice.Orientation == UIDeviceOrientation.LandscapeRight) {
                if (_mpc != null) {
					_mpc.View.Frame = UIKit.UIScreen.MainScreen.Bounds;
                    _mpc.ControlStyle = MPMovieControlStyle.Fullscreen;
					NativeView.Add(_mpc.View);
				}
                if (_avpl != null)
                {
                    _avpl.Frame = UIKit.UIScreen.MainScreen.Bounds;
                    NativeView.Layer.AddSublayer(_avpl);
                }
			}
		}
	}
}