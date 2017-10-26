using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.HttpModel
{
    public class HttpTrackModel
    {
        public string @event { get; set; }
        public TrackModelProperties properties { get; set; }
    }

    public class TrackModelProperties
    {
        public string Id { get; set; }

        public string source { get; set; }

        public string openType { get; set; }
        public string loginTimespan { get; set; }
        public string startDatetime { get; set; }
    }
}
