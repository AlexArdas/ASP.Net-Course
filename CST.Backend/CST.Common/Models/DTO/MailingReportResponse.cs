using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CST.Common.Models.DTO
{
    public class MailingReportResponse
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public string ChannelName { get; set; }
        public string AuthorName { get; set; }
        public string LocationNames { get; set; }
        public List<Guid> MailingLocations { get; set; }
        public int Employees { get; set; }
        public double ReadTime { get; set; }
        public int Reopens { get; set; }
        public int OpenRate { get; set; }
        public decimal? Rating { get; set; }
        public int Clicks { get; set; }
        public int Comments { get; set; }
        public DateTime? SendOn { get; set; }
    }
}
