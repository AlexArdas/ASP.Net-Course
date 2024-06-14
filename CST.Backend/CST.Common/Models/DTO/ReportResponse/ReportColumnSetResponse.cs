﻿using CST.Common.Attributes;

namespace CST.Common.Models.DTO.ReportResponse
{
    public class ReportColumnSetResponse
    {
        [IsNotReportable]
        public Guid Id { get; set; }
        public bool IncludeName { get; set; }

        public bool IncludeNotificationChannel { get; set; }

        public bool IncludeAuthor { get; set; }

        public bool IncludeLocation { get; set; }

        public bool IncludeEmployees { get; set; }

        public bool IncludeReadTime { get; set; }

        public bool IncludeReopens { get; set; }

        public bool IncludeOpenRate { get; set; }

        public bool IncludeRating { get; set; }

        public bool IncludeClicks { get; set; }

        public bool IncludeComments { get; set; }

        public bool IncludeSendDate { get; set; }
    }
}
