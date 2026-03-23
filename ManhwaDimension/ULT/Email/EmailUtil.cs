using ManhwaDimension.Models.Response;
using System.Net;
using System.Net.Mail;

namespace ManhwaDimension.ULT.Email
{
    public class EmailUtil
    {
        //public static string EMAIL_CREDENTIAL_NAME = "lacvietauctionvn@gmail.com";
        //public static string EMAIL_CREDENTIAL_PASSWORD = "Novatic25";
        public static string EMAIL_CREDENTIAL_NAME = AppSettingConfig.Instance.Get<string>("EmailConfig:Username");
        public static string EMAIL_CREDENTIAL_PASSWORD = AppSettingConfig.Instance.Get<string>("EmailConfig:Password");
        public static string EMAIL_CREDENTIAL_NAME_OFFICE365 = AppSettingConfig.Instance.Get<string>("EmailConfig:Office365Username");
        public static string EMAIL_CREDENTIAL_PASSWORD_OFFICE365 = AppSettingConfig.Instance.Get<string>("EmailConfig:Office365Password");
        public static string EMAIL_SENDER_NAME = AppSettingConfig.Instance.Get<string>("EmailConfig:NameSender");
        //public static string currentUrl = SystemConstant.DEFAULT_URL;
        public static string currentUrl = "";
        private static string currentUrlLogo = "";
        //private static string headerEmail = @"

        //";
        public static ManhwaDimensionResponse SendEmail(string recipients, string subject, string body)
        {
            //var novaticResponse = SendEmailOffice365(recipients, subject, body);
            var novaticResponse = SendEmailOffice365(recipients, subject, body);
            return novaticResponse;
        }
        public static ManhwaDimensionResponse SendEmailOffice365(string recipients, string subject, string body)
        {
            var novaticResponse = ManhwaDimensionResponse.SUCCESS();

            string emailUsername = EMAIL_CREDENTIAL_NAME_OFFICE365;
            string emailPassword = EMAIL_CREDENTIAL_PASSWORD_OFFICE365;
            string senderName = EMAIL_SENDER_NAME;
            //string recipients = "hung.nguyen@novatic.vn,nguyenhungbk92@gmail.com";
            //string subject = "Hello!";
            //string body = "<h1>Hello World</h1>";

            try
            {
                var toAddresses = recipients.Split(',');
                foreach (var to in toAddresses)
                {
                    int tryAgain = 20;
                    bool failed = false;
                    do
                    {
                        Thread.Sleep(3000);
                        new Thread(() =>
                        {
                            SmtpClient client = new SmtpClient();
                            client.Host = "smtp.office365.com";
                            client.Port = 587;
                            client.UseDefaultCredentials = false; // This require to be before setting Credentials property
                            client.DeliveryMethod = SmtpDeliveryMethod.Network;
                            client.Credentials = new NetworkCredential(emailUsername, emailPassword); // you must give a full email address for authentication 
                            client.TargetName = "STARTTLS/smtp.office365.com"; // Set to avoid MustIssueStartTlsFirst exception
                            client.EnableSsl = true;// Set to avoid secure connection exception
                            client.Timeout = 1000000000;                //Timeout = 1000000000
                            MailMessage message = new MailMessage();
                            message.From = new MailAddress(emailUsername, senderName); // sender must be a full email address
                            message.Subject = subject;
                            message.IsBodyHtml = true;
                            message.Body = body;
                            message.BodyEncoding = System.Text.Encoding.UTF8;
                            message.SubjectEncoding = System.Text.Encoding.UTF8;
                            message.To.Add(to.Trim());
                            try
                            {
                                client.Send(message);
                            }
                            catch (Exception ex)
                            {
                                //throw;
                                failed = true;
                                tryAgain--;
                                var exception = ex.Message.ToString();
                            }
                        }).Start();
                    } while (failed && tryAgain != 0);
                }
            }
            catch (Exception e)
            {
                //novaticResponse = ManhwaDimensionResponse.BAD_REQUEST(e);
            }


            return novaticResponse;
        }
        //public static ManhwaDimensionResponse SendEmailGmail(string recipients, string subject, string body)
        //{
        //    var novaticResponse = ManhwaDimensionResponse.SUCCESS();
        //    string emailUsername = EMAIL_CREDENTIAL_NAME;
        //    string emailPassword = EMAIL_CREDENTIAL_PASSWORD;
        //    string senderName = EMAIL_SENDER_NAME;
        //    //string emailUsername = "novaticvn@outlook.com";
        //    //string emailPassword = "Novatic@25";
        //    //string recipients = "hung.nguyen@novatic.vn,nguyenhungbk92@gmail.com";
        //    try
        //    {
        //        var toAddresses = recipients.Split(',');
        //        foreach (var to in toAddresses)
        //        {
        //            int tryAgain = 20;
        //            bool failed = false;
        //            do
        //            {
        //                Thread.Sleep(3000);
        //                new Thread(() =>
        //                {
        //                    SmtpClient client = new SmtpClient();
        //                    client.Host = "smtp.gmail.com";
        //                    client.Port = 587;
        //                    client.UseDefaultCredentials = false; // This require to be before setting Credentials property
        //                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
        //                    client.Credentials = new NetworkCredential(emailUsername, emailPassword); // you must give a full email address for authentication 
        //                    //client.TargetName = "STARTTLS/smtp.office365.com"; // Set to avoid MustIssueStartTlsFirst exception
        //                    client.EnableSsl = true;// Set to avoid secure connection exception
        //                    client.Timeout = 1000000000;                //Timeout = 1000000000
        //                    MailMessage message = new MailMessage();
        //                    message.From = new MailAddress(emailUsername, senderName); // sender must be a full email address
        //                    message.Subject = subject;
        //                    message.IsBodyHtml = true;
        //                    message.Body = body;
        //                    message.BodyEncoding = System.Text.Encoding.UTF8;
        //                    message.SubjectEncoding = System.Text.Encoding.UTF8;
        //                    message.To.Add(to.Trim());
        //                    try
        //                    {
        //                        client.Send(message);
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        //throw;
        //                        failed = true;
        //                        tryAgain--;
        //                        var exception = ex.Message.ToString();
        //                    }
        //                }).Start();
        //            } while (failed && tryAgain != 0);

        //            //            foreach (var to in toAddresses)
        //            //{
        //            //	new Thread(() =>
        //            //	{

        //            //		// send  email 
        //            //		var client = new SmtpClient("smtp.gmail.com", 587)
        //            //		{
        //            //			Credentials = new NetworkCredential(EmailUtil.EMAIL_CREDENTIAL_NAME, EmailUtil.EMAIL_CREDENTIAL_PASSWORD),
        //            //			EnableSsl = true
        //            //		};

        //            //		MailMessage msg = new MailMessage(EmailUtil.EMAIL_CREDENTIAL_NAME, to, subject, body);
        //            //		msg.IsBodyHtml = true;
        //            //		client.Send(msg);
        //            //	}).Start();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        // novaticResponse = ManhwaDimensionResponse.BAD_REQUEST(e);
        //        throw;
        //    }
        //    return novaticResponse;
        //}
    }
}
