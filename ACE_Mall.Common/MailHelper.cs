using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;

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
        //public static void SendQQEmail(string receiveEmail,string receiveTheme,string receiveContent)
        //{
        //    System.Web.Mail.MailMessage mail = new System.Web.Mail.MailMessage();
        //    try        //    {
        //        mail.To = receiveEmail;
        //        mail.From = "1945697586@qq.com";
        //        mail.Subject = receiveTheme;
        //        mail.BodyFormat = System.Web.Mail.MailFormat.Html;
        //        mail.Body = receiveContent;
        //        mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1"); //身份验证
        //        mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", mail.From); //邮箱登录账号，这里跟前面的发送账号一样就行
        //        mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", "********"); //这个密码要注意：如果是一般账号，要用授权码；企业账号用登录密码
        //        mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", 465);//端口
        //        mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", "true");//SSL加密
        //        System.Web.Mail.SmtpMail.SmtpServer = "smtp.qq.com";    //企业账号用smtp.exmail.qq.com
        //        System.Web.Mail.SmtpMail.Send(mail);

        //        //邮件发送成功
        //    }
        //    catch (Exception ex)
        //    {
        //        //失败，错误信息：ex.Message;
        //    }
        //}
        #endregion
    }
}
