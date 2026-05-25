using Microsoft.AspNetCore.Components.WebView.Maui;

namespace SmartERP;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        blazorWebView.BlazorWebViewInitialized += OnBlazorWebViewInitialized;
    }

    private void OnBlazorWebViewInitialized(object? sender, BlazorWebViewInitializedEventArgs e)
    {
#if WINDOWS
        var coreWebView2 = (e.WebView as Microsoft.UI.Xaml.Controls.WebView2)?.CoreWebView2;
        if (coreWebView2 != null)
        {
            // بدون این تنظیم، WebView2 دسترسی Blazor به storage را مسدود می‌کند
            // و هیچ رویداد @onclick ای به .NET نمی‌رسد
            coreWebView2.Profile.PreferredTrackingPreventionLevel =
                Microsoft.Web.WebView2.Core.CoreWebView2TrackingPreventionLevel.None;
            coreWebView2.Settings.IsLocalContentCanAccessRemoteContent = true;
        }
#endif
    }
}
