using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Common
{
    public class EnumHelper
    {
        public static IList<SelectListItem> EnumToSelectList(Type enumType)
        {
            IList<SelectListItem> list = new List<SelectListItem>();

            foreach (int item in Enum.GetValues(enumType))
            {
                list.Add(new SelectListItem {Value=item.ToString(),Text=Enum.GetName(enumType,item) });
            }
            return list;
        }
    }
}
