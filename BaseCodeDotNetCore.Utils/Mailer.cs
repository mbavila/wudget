// <copyright file="Mailer.cs" company="Alliance Software Inc">
// Copyright (c) Alliance Software Inc. All rights reserved.
// </copyright>

namespace BaseCodeDotNetCore.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Net;
    using System.Net.Mail;
    using BaseCodeDotNetCore.Utils.Models;

    public static class Mailer
    {
        public static bool Send(string subject, string body, List<string> toList, List<string> ccList = null, List<string> bccList = null)
        {
            bool isSent = true;
            SMTPSettings settings = new SMTPSettings();

            try
            {
                using MailMessage mail = new MailMessage();
                using SmtpClient smtpClient = new SmtpClient(settings.Host, Convert.ToInt32(settings.Port, CultureInfo.CurrentCulture));
                smtpClient.EnableSsl = Convert.ToBoolean(settings.EnableSSL, CultureInfo.CurrentCulture);

                var alternateView = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
                mail.From = new MailAddress(settings.EmailAddress, settings.Name);

                if (toList?.Count > 0)
                {
                    toList.ForEach(x => mail.To.Add(x));
                }

                if (ccList?.Count > 0)
                {
                    ccList.ForEach(x => mail.CC.Add(x));
                }

                if (bccList?.Count > 0)
                {
                    bccList.ForEach(x => mail.Bcc.Add(x));
                }

                mail.Subject = subject;
                mail.IsBodyHtml = true;
                mail.AlternateViews.Add(alternateView);

                smtpClient.Credentials = new NetworkCredential(settings.EmailAddress, settings.Password);

                smtpClient.Send(mail);
            }
            catch (Exception)
            {
                isSent = false;
            }

            return isSent;
        }
    }
}
