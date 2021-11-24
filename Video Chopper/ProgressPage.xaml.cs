using System.Threading.Tasks;

using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Animation;


namespace Video_Chopper
{
    public sealed partial class ProgressPage : Page
    {
        private Trim trim;
        private File fileData;

        public ProgressPage()
        {
            InitializeComponent();
        }

        private void ButtonVisibility()
        {
            Stop.Visibility = Visibility.Collapsed;
            BackButton.Visibility = Visibility.Visible;
        }
        private void ProgressUpdate(double percent)
        {
            ProgressBar.Value = percent;
            Percentage.Text = $"{percent}%";

            if (percent == 100) ButtonVisibility();
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            trim.Stop();
            ButtonVisibility();
            ProgressBar.Value = 100;
            Percentage.Text = "Stopped";
            ProgressBar.Foreground = new SolidColorBrush(Colors.Salmon);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            SlideNavigationTransitionInfo animation = new SlideNavigationTransitionInfo
            { Effect = SlideNavigationTransitionEffect.FromLeft };
            _ = Frame.Navigate(typeof(MainPage), null, animation);
        }

        private void MouseEntered(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Hand, 1);
        }

        private void MouseExited(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 1);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            fileData = e.Parameter as File;

            trim = new Trim(ProgressUpdate);
            Task.Run(() => trim.Run(fileData));
        }
    }
}
