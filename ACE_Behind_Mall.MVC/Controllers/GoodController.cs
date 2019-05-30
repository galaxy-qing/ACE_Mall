using ACE_Mall.BLL;
using ACE_Mall.Common;
using ACE_Mall.Model;
using NLog.Fluent;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ACE_Behind_Mall.MVC.Controllers
{
    public class GoodController : Controller
    {
        protected ModelResponse<dynamic> mr = new ModelResponse<dynamic>();
        private GoodBLL goodbll = new GoodBLL();
        // GET: Good
        public ActionResult GoodList(string goodName)
        {
            return View();
        }
        /// <summary>
        /// 商品详情页面
        /// </summary>
        /// <returns></returns>
        public ActionResult GoodDetail()
        {
            return View();
        }
        public string GetGoodDetail(int id)
        {
            var goodmodel = goodbll.GetList(x => x.ID == id).ToList();
            mr.data = goodmodel;
            return JsonHelper.Instance.Serialize(mr);
        }
        public string GetGoodList()
        {
            var goodmodel = goodbll.GetList(x => true).ToList().Select(x => new
            {
                x.ID,
                x.Name,
                x.CoverImage,
                x.IsDelete,
                x.PresentPrice,
                x.OriginalPrice
            });
            mr.data = goodmodel;
            return JsonHelper.Instance.Serialize(mr);
            //foreach (var item in goodmodel)
            //{
            //    int ID = item.ID;
            //    string goodName = item.Name;
            //    string coverImage = item.CoverImage;
            //    int isDelete = item.IsDelete; 
            //}
        }
        /// <summary>
        /// 下架商品
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        public string OffShelves(string[] idList)
        {
            try
            {
                foreach (var item in idList)
                {
                    var model = goodbll.GetList(x => x.ID == Convert.ToInt32(item)).FirstOrDefault();
                    model.IsDelete = 1;
                    Mall_Good m = goodbll.GetUpdateModel<Mall_Good>(model, "ID");
                    bool flag = goodbll.Update(m);
                    mr.message = "商品下架成功";
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }
            return JsonHelper.Instance.Serialize(mr);
        }
        /// <summary>
        /// 上架商品
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        public string OnShelves(string[] idList)
        {
            try
            {
                foreach (var item in idList)
                {
                    var model = goodbll.GetList(x => x.ID == Convert.ToInt32(item)).FirstOrDefault();
                    model.IsDelete = 0;
                    Mall_Good m = goodbll.GetUpdateModel<Mall_Good>(model, "ID");
                    bool flag = goodbll.Update(m);
                    mr.message = "商品上架成功";
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }
            return JsonHelper.Instance.Serialize(mr);
        }
        public bool GoodSearch(string goodName)
        {
            ViewBag.goodName = goodName;
            return true;
        }
        /// <summary>
        /// 上传封面图片
        /// </summary>
        /// <returns></returns>
        public string UploadImage1()
        {
            HttpFileCollectionBase files = Request.Files;
            HttpPostedFileBase file = Request.Files["file"];
            string extName = Path.GetExtension(file.FileName).ToLower();
            string path = Server.MapPath("/images/goods/");
            //生成新文件的名称，guid保证某一时刻内唯一的（保证了文件不会被覆盖）
            string fileNewName = Utils.GetRamIdcode();
            var  imagePath = "/images/goods/" + fileNewName + extName;
            try
            {
                file.SaveAs(path + fileNewName + extName);
                mr.message = imagePath;
                // NLogHelper.Logs.LogWriter("保存用户头像：【" + file.FileName + "】成功", _userData.User.Id, _userData.User.Account, _userData.User.Name, OpType.Edit);
            }
            catch (Exception ex)
            {
                mr.status = 1;
                NLogHelper.Logs.Error(ex.Message);
            }
            return JsonHelper.Instance.Serialize(mr);
        }
        public string UploadImage2()
        {
            HttpFileCollectionBase files = Request.Files;
            HttpPostedFileBase file = Request.Files["file"];
            string extName = Path.GetExtension(file.FileName).ToLower();
            string path = Server.MapPath("/images/goods/");
            //生成新文件的名称，guid保证某一时刻内唯一的（保证了文件不会被覆盖）
            string fileNewName = Utils.GetRamIdcode();
            var imagePath = "/images/goods/" + fileNewName + extName;
            try
            {
                file.SaveAs(path + fileNewName + extName);
                mr.message = imagePath;
            }
            // NLogHelper.Logs.LogWriter("保存用户头像：【" + file.FileName + "】成功", _userData.User.Id, _userData.User.Account, _userData.User.Name, OpType.Edit);
            catch (Exception ex)
            {
                NLogHelper.Logs.Error(ex.Message);
            }
            return JsonHelper.Instance.Serialize(mr);
        }
        public string UploadImage3()
        {
            HttpFileCollectionBase files = Request.Files;
            HttpPostedFileBase file = Request.Files["file"];
            string extName = Path.GetExtension(file.FileName).ToLower();
            string path = Server.MapPath("/images/goods/");
            //生成新文件的名称，guid保证某一时刻内唯一的（保证了文件不会被覆盖）
            string fileNewName = Utils.GetRamIdcode();
            var imagePath = "/images/goods/" + fileNewName + extName;
            try
            {
                file.SaveAs(path + fileNewName + extName);
                mr.message = imagePath;
            }
            // NLogHelper.Logs.LogWriter("保存用户头像：【" + file.FileName + "】成功", _userData.User.Id, _userData.User.Account, _userData.User.Name, OpType.Edit);
            catch (Exception ex)
            {
                NLogHelper.Logs.Error(ex.Message);
            }
            return JsonHelper.Instance.Serialize(mr);
        }
        /// <summary>
        /// 商品信息添加/修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string SubmitGoodInfo(Mall_Good model,string[] DetailImage,string[] InfoImage)
        {
            bool flag = false;
            string ip = "http://47.101.45.222";
            try
            {
                if (model.ID != 0) //修改
                {
                    var goodmodel = goodbll.GetList(x => x.ID == model.ID).FirstOrDefault();
                    model.DetailImage = "";
                    model.InfoImage = "";
                    model.CoverImage = model.CoverImage;
                    foreach (var item in DetailImage)
                    {
                        model.DetailImage += item + ",";
                        if (!item.Contains(ip))
                        {
                            model.DetailImage += ip + item + ",";
                        }   
                    }
                    model.DetailImage = model.DetailImage.TrimEnd(',');
                    foreach (var item in InfoImage)
                    {
                        model.InfoImage += item + ",";
                        if (!item.Contains(ip))
                        {
                            model.InfoImage += ip + item + ",";
                        }
                    }
                    model.InfoImage = model.InfoImage.TrimEnd(',');
                    Mall_Good m = goodbll.GetUpdateModel<Mall_Good>(model, "ID");
                    flag = goodbll.Update(m);
                }
                else  //添加
                {
                    model.CreateTime = DateTime.Now;
                    model.IsDelete = 0;
                    model.DetailImage = "";
                    model.InfoImage = "";
                    model.CoverImage = ip + model.CoverImage;
                    foreach (var item in DetailImage)
                    {
                        model.DetailImage += ip + item +",";
                    }
                    model.DetailImage = model.DetailImage.TrimEnd(',');
                    foreach (var item in InfoImage)
                    {
                        model.InfoImage += ip + item + ",";
                    }
                    model.InfoImage = model.InfoImage.TrimEnd(',');
                    goodbll.Add(model);
                }
            }
            catch (Exception ex)
            {
                NLogHelper.Logs.Error(ex.Message);
            }
            if(flag==true)
            {
                mr.message = "保存成功";
            }
            return JsonHelper.Instance.Serialize(mr);

        }
    }
}