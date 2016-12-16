using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
