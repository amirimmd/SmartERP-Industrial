namespace SmartERP;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        blazorWebView.BlazorWebViewInitialized += (_, e) =>
        {
#if WINDOWS
            var coreWebView2 = (e.WebView as Microsoft.UI.Xaml.Controls.WebView2)?.CoreWebView2;
            if (coreWebView2 != null)
            {
                coreWebView2.Profile.PreferredTrackingPreventionLevel =
                    Microsoft.Web.WebView2.Core.CoreWebView2TrackingPreventionLevel.None;
            }
#endif
        };
    }
}
