using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1
{
    // You exclude the 'Extension' suffix when using in Xaml markup
    [ContentProperty ("Text")]
	public class TranslateExtension : IMarkupExtension
	{
        readonly CultureInfo ci = null;
		const string ResourceId = "App1.Resx.AppResources";

		public TranslateExtension() {
            if (Xamarin.Forms.Device.RuntimePlatform == "Android" || Xamarin.Forms.Device.RuntimePlatform == "Ios")
            {
                //ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
                ci = DependencyService.Get<ILocalize>().GetCultureInfo();
            }
		}

		public string Text { get; set; }

		public object ProvideValue (IServiceProvider serviceProvider)
		{
			if (Text == null)
				return "";

			ResourceManager temp = new ResourceManager(ResourceId, typeof(TranslateExtension).GetTypeInfo().Assembly);

			var translation = temp.GetString (Text, ci);
            if (translation == null)
            {
#if DEBUG
                throw new ArgumentException(
                    String.Format("Key '{0}' was not found in resources '{1}' for culture '{2}'.", Text, ResourceId, ci.Name),
                    "Text");
#else
				translation = Text; // HACK: returns the key, which GETS DISPLAYED TO THE USER
#endif
            }
            return translation;
		}
	}
}
