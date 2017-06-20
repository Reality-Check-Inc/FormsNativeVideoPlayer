using System;
using Xamarin.Forms;

namespace FormsNativeVideoPlayer
{
    public class StreamingVideoView : View
    {
		public static readonly BindableProperty VideoUrlProperty = BindableProperty.Create(
		  propertyName: "VideoUrl",
		  returnType: typeof(string),
		  declaringType: typeof(StreamingVideoView),
		  defaultValue: null);

		public string VideoUrl
		{
			get { return (string)GetValue(VideoUrlProperty); }
			set { SetValue(VideoUrlProperty, value); }
		}
		
        public StreamingVideoView()
        {
        }
    }
}
