using NLog;
using NLog.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ACE_Mall.Common
{
    public class NLogHelper
    {
        /// <summary>
        /// 项目日志封装
        /// </summary>
        public class Logs
        {
            private static Logger logger = LogManager.GetCurrentClassLogger(); //初始化日志类

            /// <summary>
            /// 静态构造函数
            /// </summary>
            static Logs()
            {
                string url = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "NLog.config";
                //初始化配置日志
                LogManager.Configuration = new XmlLoggingConfiguration(url);
            }

            /// <summary>
            /// 平台操作日志
            /// </summary>
            /// <param name="msg">信息</param>
            /// <param name="userId">用户id</param>
            /// <param name="account">用户账户</param>
            /// <param name="name">用户名称</param>
            /// <param name="logType">操作类型</param>
            public static void LogWriter(String msg,int userId,string account,string name,string logType)
            {
                LogEventInfo let = new LogEventInfo();
                try
                {  
                    let.Properties["UserID"] = userId;
                    let.Properties["UserName"] = name;
                    let.Properties["Account"] = account;
                    let.Properties["OpType"] = logType;
                    let.Properties["OpContent"] = account + ":" + msg;
                    let.Level = LogLevel.Info;
                    logger.Log(let);
                }
                catch
                {
                    //日志代码错误,直接记录日志
                    Fatal(msg);
                    Warn(msg);
                }
            }
            /// <summary>
            /// 调试日志
            /// </summary>
            /// <param name="msg">日志内容</param>
            public static void Debug(String msg)
            {
                logger.Debug(msg);
            }

            /// <summary>
            /// 信息日志
            /// </summary>
            /// <param name="msg">日志内容</param>
            /// <remarks>
            ///     适用大部分场景
            ///     1.记录日志文件
            /// </remarks>
            public static void Info(String msg)
            {
                logger.Info(msg);
            }

            /// <summary>
            /// 错误日志
            /// </summary>
            /// <param name="msg">日志内容</param>
            /// <remarks>
            ///     适用异常,错误日志记录
            ///     1.记录日志文件
            /// </remarks>
            public static void Error(String msg)
            {
                logger.Error(msg);
            }

            /// <summary>
            /// 严重致命错误日志
            /// </summary>
            /// <param name="msg">日志内容</param>
            /// <remarks>
            ///     1.记录日志文件
            ///     2.控制台输出
            /// </remarks>
            public static void Fatal(String msg)
            {
                logger.Fatal(msg);
            }

            /// <summary>
            /// 警告日志
            /// </summary>
            /// <param name="msg">日志内容</param>
            /// <remarks>
            ///     1.记录日志文件
            ///     2.发送日志邮件
            /// </remarks>
            public static void Warn(String msg)
            {
                try
                {
                    logger.Warn(msg);
                }
                catch { }
            }
        }
    }
}
