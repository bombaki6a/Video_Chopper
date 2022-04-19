using System;
using Windows.Storage;
using Windows.Storage.Pickers;
using System.Collections.Generic;
using Windows.Media.MediaProperties;
using Windows.Storage.FileProperties;

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
        private VideoProperties videoProperties;

        private IDictionary<string, object> frameRateRetrieve;
        private readonly List<string> encodingRetrieve = new List<string>() { "System.Video.FrameRate" };

        private readonly List<double> bitrateError = new List<double>() { 1.3, 2.3 };
        private readonly List<VideoEncodingQuality> qualities = new List<VideoEncodingQuality>()
        {
            VideoEncodingQuality.Auto,
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
                
                if (TimeSpan.Compare(fileData.End, fileData.Start) != 1)
                {
                    fileData.End = TimeSpan.Parse("00:00:00");
                }

                videoProperties = await fileData.Intpu.Properties.GetVideoPropertiesAsync();
                fileData.Quality = (VideoEncodingQuality)QualityBox.SelectedItem;

                frameRateRetrieve = await fileData.Intpu.Properties.RetrievePropertiesAsync(encodingRetrieve);

                uint frame = (uint)frameRateRetrieve[encodingRetrieve[0]];
                if (((decimal)frame) / 1000 % 1 == 0)
                {
                    fileData.FrameRateDominator = 1;
                    fileData.FrameRateNumerator = frame / 1000;
                }
                else
                {
                    fileData.FrameRateDominator = 1001;
                    fileData.FrameRateNumerator = (uint)Math.Round((double)frame / 1000) * 1000;
                }

                if (fileData.Quality == VideoEncodingQuality.HD1080p)
                {
                    fileData.VideoBitrate = (uint)(videoProperties.Bitrate * bitrateError[1]);
                }
                else
                {
                    fileData.VideoBitrate = (uint)(videoProperties.Bitrate * bitrateError[0]);
                }

                SlideNavigationTransitionInfo animation = new SlideNavigationTransitionInfo
                { Effect = SlideNavigationTransitionEffect.FromBottom };
                Frame.Navigate(typeof(ProgressPage), fileData, animation);
            }
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
