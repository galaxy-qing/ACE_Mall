﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ACE_Mall.Common
{
    public class GetModelHelper
    {
        public static Pr GetModel<Ch, Pr>(Ch model, Pr efmodel)
        {
            Type type = typeof(Ch);
            Type type2 = typeof(Pr);
            foreach (PropertyInfo pro in type.GetProperties())
            {
                //&& pro.GetValue(model).ToString() != "0"&& pro.GetValue(model).ToString() != ""
                if (pro.GetValue(model) != null)
                {
                    PropertyInfo pp = type2.GetProperty(pro.Name);
                    pp.SetValue(efmodel, pro.GetValue(model));
                }
            }
            return efmodel;
        }
    }
}
