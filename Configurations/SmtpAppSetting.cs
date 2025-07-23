namespace MVCWebApp.Configurations
{
    public class SmtpAppSetting
    {
        public string Host { get; set; }
        public string EmailFrom { get; set; }
        public List<string> EmailTo { get; set; }
    }
}
