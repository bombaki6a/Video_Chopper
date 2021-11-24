using System;
using Windows.Storage;
using Windows.Storage.Pickers;
using System.Collections.Generic;
using Windows.Media.MediaProperties;


using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Animation;

namespace Video_Chopper
{
    public sealed partial class IntervalPage : Page
    {
        private File fileData;
        private readonly List<VideoEncodingQuality> qualities = new List<VideoEncodingQuality>()
        {
            VideoEncodingQuality.HD1080p,
            VideoEncodingQuality.HD720p
        };

        public IntervalPage()
        {
            InitializeComponent();
            QualityBox.ItemsSource = qualities;
        }

        // Get Args From Previous Page
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            fileData = e.Parameter as File;
            FileName.Text = fileData.Intpu.Name;
        }

        // Return To Previous Page Button Event
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        // Save As File Buttton Event
        private async void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            FileSavePicker picker = new FileSavePicker
            {
                SuggestedFileName = fileData.Intpu.Name,
                SuggestedStartLocation = PickerLocationId.Desktop
            };
            picker.FileTypeChoices.Add("Video File", new List<string> { ".mp4" });

            StorageFile output = await picker.PickSaveFileAsync();
            if (output != null)
            {
                fileData.Output = output;
                fileData.End = TimeSpan.Parse(EndTime.Text);
                fileData.Start = TimeSpan.Parse(StartTime.Text);
                fileData.Quality = (VideoEncodingQuality)QualityBox.SelectedItem;
            }

            SlideNavigationTransitionInfo animation = new SlideNavigationTransitionInfo
            { Effect = SlideNavigationTransitionEffect.FromBottom };
            Frame.Navigate(typeof(ProgressPage), fileData, animation);
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
    }
}
