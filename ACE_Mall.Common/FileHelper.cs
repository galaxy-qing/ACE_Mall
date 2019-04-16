using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ACE_Mall.Common
{
    public class FileHelper
    {
        /// <summary>
        /// base64存图片
        /// </summary>
        /// <param name="strImg"></param>
        /// <returns></returns>
        public static  string SaveImage(string strImg)
        {
            try
            {
                if (string.IsNullOrEmpty(strImg))
                {
                    return "err";
                }
                if (strImg.Length < 100 && strImg.Length > 10)
                {
                    return "err";
                }
                string fpath = "/images/users/";
                //物理路径
                string Pfpath = System.Web.HttpContext.Current.Server.MapPath(fpath);//图片存储文件夹路径
                string picturename = Utils.GetOrderNumber();//文件名
                if (!Directory.Exists(Pfpath))//查看存储路径的文件是否存在
                {
                    Directory.CreateDirectory(Pfpath);   //创建文件夹，并上传文件
                }
                byte[] arr = Convert.FromBase64String(strImg);
                MemoryStream ms = new MemoryStream(arr);
                Bitmap bmp = new Bitmap(ms);
                bmp.Save(Pfpath + picturename + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);   //保存为.jpg格式
                ms.Close();
                return "http://192.168.0.143:60391"+fpath + picturename + ".jpg";

            }
            catch (Exception err)
            {
                return "err";
            }

        }
    }
}
