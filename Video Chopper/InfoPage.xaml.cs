using System;
using Windows.System;
using Windows.ApplicationModel;

using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Controls;

namespace Video_Chopper
{
    public sealed partial class InfoPage : Page
    {
        private readonly string desctiption = "This is an application " +
                                              "with a simple UI for trimming videos " +
                                              "and processing them in 1080p or 720p " +
                                              "resolution.";

        private readonly string appName = Package.Current.DisplayName;
        private readonly string publisher = Package.Current.PublisherDisplayName;

        private readonly PackageVersion version = Package.Current.Id.Version;

        private readonly string githubLink = @"https://github.com/bombaki6a/Video_Chopper";
        private readonly string linkedInLink = @"https://www.linkedin.com/in/steliyan-dobrev-12a698224/";

        public InfoPage()
        {
            InitializeComponent();

            // Set Information
            Name.Text = appName;
            Publisher.Text = publisher;
            Description.Text = desctiption;
            Version.Text = $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }

        // Function For Change Cursor To Hand
        private void MouseEntered(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Hand, 1);
        }

        // Function For Change Cursor To Arrow
        private void MouseExited(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 1);
        }

        // Back Button Event
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        // Open LinkedIn Link
        private async void LinkenIn_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri(linkedInLink));
        }

        // Open GitHub Link
        private async void GitHub_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri(githubLink));
        }
    }
}
