using System.Web.Mvc;
using Service.IService;
using System.Data;
using System.Data.SqlClient;
using Common;
using System.Linq;
using WebPage.Controllers;

namespace WebPage.Areas.BnsManage.Controllers
{
    public class CustomerController : BaseController
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
        /// <summary>
        /// 客户管理主页
        /// </summary>
        /// <returns></returns>
        [UserAuthorize(ModuleAlias = "Customer", OperaAction = "View")]
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

        /// <summary>
        /// 客户详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [UserAuthorize(ModuleAlias = "Customer", OperaAction = "Detail")]
        public ActionResult Detail(int? id)
        {
            //初始化客户
            //var entity = new Domain.SYS_BUSSINESSCUSTOMER() { ChargePersionSex = 1 };

            //if (id != null && id > 0)
            //{
            //    //客户实体
            //    entity = BussinessCustomerManage.Get(p => p.ID == id);
            //    //公司介绍
            //    ViewData["CompanyInstroduce"] = ContentManage.Get(p => p.FK_RELATIONID == entity.FK_RELATIONID && p.FK_TABLE == "SYS_BUSSINESSCUSTOMER") ?? new Domain.COM_CONTENT();
            //}

            ////客户类型
            //ViewBag.KHLX = this.CodeManage.LoadAll(p => p.CODETYPE == "LXRLX").OrderBy(p => p.SHOWORDER).ToList();

            return View();
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
        
        #endregion
    }
}