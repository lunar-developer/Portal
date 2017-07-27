using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using DotNetNuke.Services.Mail;
using Website.Library.Enum;

namespace Website.Library.Global
{
    public static class MailBase
    {
        private static readonly string EmailAddress = FunctionBase.GetConfiguration(ConfigEnum.EmailAddress);
        private static readonly string EmailEnvelope = FunctionBase.GetConfiguration(ConfigEnum.ImageEmailEnvelope);
        private static readonly string Logo = FunctionBase.GetConfiguration(ConfigEnum.ImageLogo);

        public static void SendEmail(string toAddress, string subject, string body,
            List<Attachment> listAttachments, bool isUseTemplate = true)
        {
            SendEmail(EmailAddress, toAddress, subject, body, listAttachments, isUseTemplate);
        }

        public static void SendEmail(string fromAddress, string toAddress, string subject, string body,
            List<Attachment> listAttachments, bool isUseTemplate = true)
        {
            if (isUseTemplate)
            {
                if (listAttachments == null)
                {
                    listAttachments = new List<Attachment>();
                }
                Attachment emailEnvelope = CreateAttachment(EmailEnvelope, "Envelope");
                Attachment logo = CreateAttachment(Logo, "Logo");
                listAttachments.Add(emailEnvelope);
                listAttachments.Add(logo);
                body = body.Replace("@Envelope", emailEnvelope.ContentId).Replace("@Logo", logo.ContentId);
            }
            Task.Run(() => Mail.SendEmail(fromAddress, fromAddress, toAddress, subject, body, listAttachments));
        }

        private static Attachment CreateAttachment(string imageBase64, string name)
        {
            byte[] byteArray = Convert.FromBase64String(imageBase64);
            MemoryStream stream = new MemoryStream(byteArray);
            Attachment attachment = new Attachment(stream, name, MediaTypeNames.Image.Jpeg)
            {
                ContentId = Guid.NewGuid().ToString(PatternEnum.GuidDigits)
            };
            return attachment;
        }
    }
}