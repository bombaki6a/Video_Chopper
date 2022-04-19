using System;
using System.Threading;
using System.Threading.Tasks;

using Windows.UI.Xaml;
using Windows.UI.Core;
using Windows.Media.Transcoding;
using Windows.Media.MediaProperties;

namespace Video_Chopper
{
    internal class Trim
    {
        private Progress<double> progress;
        private readonly Action<double> progressUpdate;

        private MediaTranscoder transcoder;
        private MediaEncodingProfile profile;
        private PrepareTranscodeResult result;

        private readonly CoreDispatcher dispatcher;
        private CancellationTokenSource cancellation;

        internal Trim(Action<double> progressUpdate)
        {
            this.progressUpdate = progressUpdate;
            dispatcher = Window.Current.Dispatcher;
        }

        internal async Task Run(File fileData)
        {
            transcoder = new MediaTranscoder
            {
                TrimStopTime = fileData.End,
                TrimStartTime = fileData.Start,
                VideoProcessingAlgorithm = MediaVideoProcessingAlgorithm.Default
            };

            profile = MediaEncodingProfile.CreateMp4(fileData.Quality);

            profile.Video.Bitrate = fileData.VideoBitrate;
            profile.Video.FrameRate.Numerator = fileData.FrameRateNumerator;
            profile.Video.FrameRate.Denominator = fileData.FrameRateDominator;

            cancellation = new CancellationTokenSource();

            result = await transcoder.PrepareFileTranscodeAsync(fileData.Intpu, fileData.Output, profile);

            if (result.CanTranscode)
            {
                progress = new Progress<double>(TranscodeProgress);
                await result.TranscodeAsync().AsTask(cancellation.Token, progress);
            }
        }

        private async void TranscodeProgress(double percentage)
        {
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                progressUpdate(Math.Floor(percentage));
            });
        }

        public void Stop()
        {
            try { cancellation.Cancel(); }
            catch (TaskCanceledException) { }
            finally { cancellation.Dispose(); }
        }
    }
}
