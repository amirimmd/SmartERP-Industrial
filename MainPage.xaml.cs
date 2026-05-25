namespace SmartERP;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        blazorWebView.BlazorWebViewInitialized += (_, e) =>
        {
#if WINDOWS
            var webView2 = e.WebView;
            var coreWebView2 = webView2?.CoreWebView2;
            if (coreWebView2 != null)
            {
                coreWebView2.Profile.PreferredTrackingPreventionLevel =
                    Microsoft.Web.WebView2.Core.CoreWebView2TrackingPreventionLevel.None;

                // مشکل ریشه‌ای: در MAUI Blazor Hybrid روی Windows، اولین کلیک روی WebView2
                // صرف focus شدن خود WebView2 می‌شود و event کلیک به Blazor نمی‌رسد.
                // راه‌حل: بعد از اولین بارگذاری موفق صفحه، focus را برنامه‌ای به WebView2 می‌دهیم.
                bool firstLoad = true;
                coreWebView2.NavigationCompleted += (_, args) =>
                {
                    if (firstLoad && args.IsSuccess)
                    {
                        firstLoad = false;
                        webView2?.DispatcherQueue.TryEnqueue(() =>
                            webView2.Focus(Microsoft.UI.Xaml.FocusState.Programmatic));
                    }
                };
            }
#endif
        };
    }
}
