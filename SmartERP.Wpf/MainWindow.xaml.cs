using System.Windows;

namespace SmartERP
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Inject the application-wide DI container into BlazorWebView.
            // App.Services is built in App.xaml.cs before MainWindow is created.
            blazorWebView.Services = App.Services;
        }
    }
}
