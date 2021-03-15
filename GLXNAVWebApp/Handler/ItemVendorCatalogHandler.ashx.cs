using Ext.Net;
using GLXNAVWebApp.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GLXNAVWebApp.Handler
{
    public class ItemVendorCatalogHandler : IHttpHandler
    {
        Common common = new Common();
        public void ProcessRequest(HttpContext context)
        {

            context.Response.ContentType = "text/plain";
            //var username = string.Empty;
            //if (!string.IsNullOrEmpty(context.Request["UserName"]))
            //{
            //    username = context.Request["UserName"];
            //}
            int start = 0;
            int limit = 10;
            string sort = string.Empty;
            string dir = string.Empty;
            string query = string.Empty;
            string Item_No = string.Empty;
            string Company = string.Empty;
            const int fetchSize = 10;
            string bookmarkKey = null;

            if (!string.IsNullOrEmpty(context.Request["start"]))
            {
                start = int.Parse(context.Request["start"]);
            }

            if (!string.IsNullOrEmpty(context.Request["limit"]))
            {
                limit = int.Parse(context.Request["limit"]);
            }

            if (!string.IsNullOrEmpty(context.Request["sort"]))
            {
                sort = context.Request["sort"];
            }

            if (!string.IsNullOrEmpty(context.Request["dir"]))
            {
                dir = context.Request["dir"];
            }

            if (!string.IsNullOrEmpty(context.Request["query"]))
            {
                query = context.Request["query"];
            }

            if (!string.IsNullOrEmpty(context.Request["ItemNo"]))
            {
                Item_No = context.Request["ItemNo"];
            }
            if (!string.IsNullOrEmpty(context.Request["CurCompany"]))
            {
                Company = context.Request["CurCompany"];
            }

            ItemVendorCatalog.ItemVendorCatalog_Service s = new ItemVendorCatalog.ItemVendorCatalog_Service();
            s.Url = common.ReBuildUrl(s.Url, Company);
            s.Credentials = common.CheckCredentials();
            List<ItemVendorCatalog.ItemVendorCatalog_Filter> DataFilter = new List<ItemVendorCatalog.ItemVendorCatalog_Filter>();

            #region Filter
            //Replenishment_System
            ItemVendorCatalog.ItemVendorCatalog_Filter Item_NoFilter = new ItemVendorCatalog.ItemVendorCatalog_Filter();
            Item_NoFilter.Field = ItemVendorCatalog.ItemVendorCatalog_Fields.Item_No;
            Item_NoFilter.Criteria = Item_No;
            DataFilter.Add(Item_NoFilter);
            #endregion 

            ItemVendorCatalog.ItemVendorCatalog[] Results = s.ReadMultiple(DataFilter.ToArray(), bookmarkKey, fetchSize);
            List<ItemVendorCatalog.ItemVendorCatalog> ListResult = new List<ItemVendorCatalog.ItemVendorCatalog>();
            while (Results.Length > 0)
            {
                bookmarkKey = Results.Last().Key;
                ListResult.AddRange(Results);
                Results = s.ReadMultiple(DataFilter.ToArray(), bookmarkKey, fetchSize);
            }
            Paging<ItemVendorCatalog.ItemVendorCatalog> list = PlantsPaging(start, limit, sort, dir, query, ListResult);
            context.Response.Write(string.Format("{{total:{1},'data':{0}}}", JSON.Serialize(list.Data), list.TotalRecords));
        }

        public static Paging<ItemVendorCatalog.ItemVendorCatalog> PlantsPaging(int start, int limit, string sort, string dir, string filter, List<ItemVendorCatalog.ItemVendorCatalog> listdata)
        {
            if (!string.IsNullOrEmpty(filter) && filter != "*")
            {
                listdata.RemoveAll(d => !d.Vendor_No.ToLower().StartsWith(filter.ToLower()));
            }

            if (!string.IsNullOrEmpty(sort))
            {
                listdata.Sort(delegate (ItemVendorCatalog.ItemVendorCatalog x, ItemVendorCatalog.ItemVendorCatalog y)
                {
                    object a;
                    object b;

                    int direction = dir == "DESC" ? -1 : 1;

                    a = x.GetType().GetProperty(sort).GetValue(x, null);
                    b = y.GetType().GetProperty(sort).GetValue(y, null);

                    return CaseInsensitiveComparer.Default.Compare(a, b) * direction;
                });
            }

            if ((start + limit) > listdata.Count)
            {
                limit = listdata.Count - start;
            }

            List<ItemVendorCatalog.ItemVendorCatalog> rangePlants = (start < 0 || limit < 0) ? listdata : listdata.GetRange(start, limit);

            return new Paging<ItemVendorCatalog.ItemVendorCatalog>(rangePlants, listdata.Count);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}