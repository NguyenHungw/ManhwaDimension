namespace ManhwaDimension.ULT.Email
{
    public class EmailMessage
    {
        public List<string> ToEmails { get; set; }
        public List<string> CcEmails { get; set; }
        public List<string> BccEmails { get; set; }
        public string Subject { get; set; }
        public string TemplateName { get; set; }
        public string TemplatePath { get; set; }
        public object Model { get; set; }
    }
}
