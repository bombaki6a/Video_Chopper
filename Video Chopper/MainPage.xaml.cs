using System;
using Windows.Storage;
using Windows.Storage.Pickers;

using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Video_Chopper
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
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

        // Open File And Switch Page
        private async void ChooseFile_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker picker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.Desktop,
            };
            picker.FileTypeFilter.Add(".mp4");

            StorageFile input = await picker.PickSingleFileAsync();
            if (input != null)
            {
                SlideNavigationTransitionInfo animation = new SlideNavigationTransitionInfo
                { Effect = SlideNavigationTransitionEffect.FromRight };
                Frame.Navigate(typeof(IntervalPage), new File { Intpu = input }, animation);
            }
        }

        // Fuction For Open InfoPage
        private void ButtonInfo_Click(object sender, RoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 1);
            DrillInNavigationTransitionInfo animation = new DrillInNavigationTransitionInfo();
            Frame.Navigate(typeof(InfoPage), null, animation);
        }
    }
}
