using System.Web.Mvc;
using Service.IService;
using System.Data;
using System.Data.SqlClient;
using Common;
using System.Linq;

namespace WebPage.Areas.SaleManage.Controllers
{
    public class CustomerController : WebPage.Controllers.BaseController
    {
        #region 容器聲明
        ICustomerManage CustomerManage;
        IPlaceInfoManage PlaceInfoManage;
        public CustomerController
            (
                ICustomerManage _customer,
                IPlaceInfoManage _place
            )
        {
            this.CustomerManage = _customer;
            this.PlaceInfoManage = _place;
        }
        #endregion

        #region 基本視圖
        public ActionResult Index()
        {
            ViewData["procList"] = new SelectList(
                PlaceInfoManage.LoadListAll(n => n.s_ParentID == 0),
                "s_PlaceID",
                "s_PlaceName");
            ViewBag.stateList = EnumHelper.EnumToSelectList(typeof(Domain.Enums.CustomerState));
            string Province = Request.QueryString["province"];
            string CustomerState = Request.QueryString["customerState"];
            if (Request.IsAjaxRequest())
            {
                return PartialView("DataView", BindList(Province, CustomerState));
            }
            return View(BindList(Province, CustomerState));
        }

        public ActionResult Index2(/*string name ,string year,string month,string title*/)
        {
            var list = CustomerManage.LoadAll(null).Select(x => new 
            {
                a =x.s_CustomerID,
                b = x.t_MonthlyCustomer.s_CreditLine,
                c = x.s_CustomerName
            }).ToList();
            return View(new { i=1,list=list});
        }

        [HttpPost]
        public ActionResult Index2(string a)
        {
            return Content(a);
        }
        #endregion

        #region 輔助方法

        private PageInfo BindList(string Province, string CustomerState)
        {
            var query = CustomerManage.LoadAll(null);
            //客户所在省份
            if (!string.IsNullOrEmpty(Province))
            {
                int _proc = int.Parse(Province);
                query = query.Where(p => p.s_Province == _proc);
            }

            //客户类型
            if (!string.IsNullOrEmpty(CustomerState))
            {
                int _state = int.Parse(CustomerState);
                query = query.Where(p => p.s_CustomerState == _state);
            }

            //查询关键字
            if (!string.IsNullOrEmpty(keywords))
            {
                keywords = keywords.ToLower();
                query = query.Where(p => p.s_CustomerName.Contains(keywords) || p.s_CustomerID.Contains(keywords));
            }
            //排序
            query = query.OrderByDescending(p => p.s_AddTime);
            //分页
            var result = this.CustomerManage.Query(query, pageindex, pagesize);

            var data = result.List.Select(x => new
            {
                x.s_CustomerID,
                x.s_CustomerName,
                x.s_CustomerState,
                x.s_Telephone,
                s_Province = PlaceInfoManage.Get(n => n.s_PlaceID == x.s_Province)?.s_PlaceName,
                s_City = PlaceInfoManage.Get(n => n.s_PlaceID == x.s_City)?.s_PlaceName,
                s_County = PlaceInfoManage.Get(n => n.s_PlaceID == x.s_County)?.s_PlaceName,
                x.s_AddTime
            }).ToList();

            return new PageInfo(result.Index, result.PageSize, result.Count, JsonConverter.JsonClass(data));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString">目标连接字符</param>
        /// <param name="TableName">目标表</param>
        /// <param name="dt">源数据</param>
        public void SqlBulkCopyByDatatable(string connectionString, string TableName, DataTable dt)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlBulkCopy sqlbulkcopy = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.UseInternalTransaction))
                {
                    try
                    {
                        sqlbulkcopy.DestinationTableName = TableName;
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            sqlbulkcopy.ColumnMappings.Add(dt.Columns[i].ColumnName, dt.Columns[i].ColumnName);
                        }
                        sqlbulkcopy.WriteToServer(dt);
                    }
                    catch (System.Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }
        #endregion
    }
}