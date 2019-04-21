using System;
using System.Collections.Generic;
using System.Text;

using System.Collections;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace ACE_Mall.Common
{
    /// <summary>
    /// 用于发送邮件的类
    /// </summary>
    public class MailHelper
    {
        #region 发送邮件的方法
        /// <summary>
        /// 发送邮件的方法
        /// </summary>
        /// <param name="strto">接收地址(多个接收地址用逗号分隔)</param>
        /// <param name="strSubject">邮件主题</param>
        /// <param name="strBody">邮件内容</param>
        /// <param name="isHtmlFormat">邮件内容是否以html格式发送</param>
        public static void SendEmail(string strto, string strSubject, string strBody, bool isHtmlFormat)
        {
            SendEmail(strto, strSubject, strBody, isHtmlFormat, new ArrayList());
        }
        /// <summary>
        /// 发送邮件的方法
        /// </summary>
        /// <param name="strto">接收地址(多个接收地址用逗号分隔)</param>
        /// <param name="strSubject">邮件主题</param>
        /// <param name="strBody">邮件内容</param>
        /// <param name="isHtmlFormat">邮件内容是否以html格式发送</param>
        /// <param name="files">附件文件的集合</param>
        public static void SendEmail(string strto, string strSubject, string strBody, bool isHtmlFormat, ArrayList files)
        {
            SendEmail("mail.hd.bitauto.com", "monitor@hd.bitauto.com", "tt#23j", strto, strSubject, strBody, isHtmlFormat, files);
            //SendEmail("mail.qq.com", "920971988@qq.com", "wang111111", strto, strSubject, strBody, isHtmlFormat, files);
        }        
        /// <summary>
        /// 发送邮件的方法
        /// </summary>
        /// <param name="strSmtpServer">邮件服务器地址</param>
        /// <param name="strFrom">发送地址</param>
        /// <param name="strFromPass">发送密码</param>
        /// <param name="strto">接收地址(多个接收地址用逗号分隔)</param>
        /// <param name="strSubject">邮件主题</param>
        /// <param name="strBody">邮件内容</param>
        /// <param name="isHtmlFormat">邮件内容是否以html格式发送</param>
        public static void SendEmail(string strSmtpServer, string strFrom, string strFromPass, string strto, string strSubject, string strBody, bool isHtmlFormat)
        {
            SendEmail(strSmtpServer, strFrom, strFromPass, strto, strSubject, strBody, isHtmlFormat, new ArrayList());
        }        
        /// <summary>
        /// 发送邮件的方法
        /// </summary>
        /// <param name="strSmtpServer">邮件服务器地址</param>
        /// <param name="strFrom">发送地址</param>
        /// <param name="strFromPass">发送密码</param>
        /// <param name="strto">接收地址(多个接收地址用逗号分隔)</param>
        /// <param name="strSubject">邮件主题</param>
        /// <param name="strBody">邮件内容</param>
        /// <param name="isHtmlFormat">邮件内容是否以html格式发送</param>
        /// <param name="files">附件文件的集合</param>
        public static void SendEmail(string strSmtpServer, string strFrom, string strFromPass, string strto, string strSubject, string strBody, bool isHtmlFormat, ArrayList files)
        {
            SmtpClient client = new SmtpClient(strSmtpServer);
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(strFrom, strFromPass);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;

            string[] strtos = strto.Split(new char[] { ',' });
            for (int i = 0; i < strtos.Length; i++)
            {
                try
                {
                    if (strtos[i].Trim().Length > 0)
                    {
                        MailMessage message = new MailMessage(strFrom, strtos[i].Trim(), strSubject, strBody);
                        message.BodyEncoding = Encoding.Default;
                        message.IsBodyHtml = isHtmlFormat;

                        for (int j = 0; j < files.Count; j++)
                        {
                            if (File.Exists(files[j].ToString()))
                            {
                                message.Attachments.Add(new Attachment(files[j].ToString()));
                            }
                        }

                        client.Send(message);
                    }
                }
                catch
                {
                    continue;
                }
            }
        }
        #endregion
    }
}
