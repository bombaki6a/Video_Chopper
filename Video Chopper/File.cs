using System;
using Windows.Storage;
using Windows.Media.MediaProperties;

namespace Video_Chopper
{
    public class File
    {
        public StorageFile Intpu { get; set; }
        public StorageFile Output { get; set; }

        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }

        public VideoEncodingQuality Quality { get; set; }
    }
}
