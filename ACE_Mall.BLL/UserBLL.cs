﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE_Mall.Model;
namespace ACE_Mall.BLL
{
    public class UserBLL : BaseBLL<My_Data>
    {
        public List<My_Data> GetList(Func<My_Data, bool> exp)
        {

            var list = Dao.GetEntities(exp).ToList();
            return list;
        }
        /// <summary>
        /// 添加用户模型
        /// </summary>
        /// <param  ="model">数据模型</param>
        /// <returns></returns>
        public bool Add(My_Data model)
        {
            return Dao.Insert(model);
        }
        public new bool Update(My_Data model)
        {
            return Dao.Update(model);
        }
    }
}
