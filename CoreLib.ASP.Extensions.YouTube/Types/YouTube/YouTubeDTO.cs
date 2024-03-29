﻿#region

using System;

#endregion

namespace CoreLib.ASP.Extensions.YouTube.Types.YouTube
{
    public class YouTubeDTO
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime PublishDate { get; set; }

        public Thumbnails Thumbnails { get; set; }
    }
}