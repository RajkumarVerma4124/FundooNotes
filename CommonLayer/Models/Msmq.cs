﻿using Experimental.System.Messaging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace CommonLayer.Models
{
    /// <summary>
    /// Created The Class For Microsoft Messaging Queueing
    /// </summary>
    public class Msmq
    {
        //Created The Object Reference For MessageQueue
        MessageQueue messageQueue = new MessageQueue();
        private string recieverEmailAddr;

        //Method To Send Token Using MessageQueue And Delegate
        public void SendMessage(string token,string emailId)
        {
            recieverEmailAddr = emailId;
            messageQueue.Path = @".\Private$\Token";
            try
            {
                if (!MessageQueue.Exists(messageQueue.Path))
                {
                    MessageQueue.Create(messageQueue.Path);
                }
                messageQueue.Formatter = new XmlMessageFormatter(new Type[] {typeof(string)});
                messageQueue.ReceiveCompleted += MessageQueue_ReceiveCompleted;
                messageQueue.Send(token);
                messageQueue.BeginReceive();
                messageQueue.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Delegate To Send Token As Message To The Sender EmailId Using Smtp And MailMessage
        private void MessageQueue_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            try
            {
                var msg = messageQueue.EndReceive(e.AsyncResult);
                string token = msg.Body.ToString();
                MailMessage mailMessage = new MailMessage();
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    EnableSsl = true,
                    Credentials = new NetworkCredential("iamrajvermapro@gmail.com", "QwertyKey@123"),
                };
                mailMessage.From = new MailAddress("iamrajvermapro@gmail.com");
                mailMessage.To.Add(new MailAddress(recieverEmailAddr));
                mailMessage.Body = token;
                mailMessage.Subject = "Fundoo Notes Password Reset Link";
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
