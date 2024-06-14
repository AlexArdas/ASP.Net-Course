namespace CST.BusinessLogic.Configuration.Sections
{
    public class EmailConfiguration
    {
        public string Pop3Host { get; set; }

        public int Pop3Port { get; set; }

        public string Pop3User { get; set; }
        
        public string Pop3Password { get; set; }

        public string SmtpHost { get; set; }

        public int SmtpPort { get; set; }

        public string SmtpUser { get; set; }

        public string SmtpPassword { get; set; }
    }
}
