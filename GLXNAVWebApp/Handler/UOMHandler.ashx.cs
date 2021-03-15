using Ext.Net;
using GLXNAVWebApp.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GLXNAVWebApp.Handler
{
    /// <summary>
    /// Summary description for UOMHandler
    /// </summary>
    public class UOMHandler : IHttpHandler
    {
        Common Clsc = new Common();
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
            string Type = string.Empty;
            string CustomerNo = string.Empty;
            string Company = string.Empty;
            string itemNo = string.Empty;
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
            if (!string.IsNullOrEmpty(context.Request["customer_no"]))
            {
                CustomerNo = context.Request["customer_no"];
            }

            if (!string.IsNullOrEmpty(context.Request["type"]))
            {
                Type = context.Request["type"];
            }
            if (!string.IsNullOrEmpty(context.Request["CurCompany"]))
            {
                Company = context.Request["CurCompany"];
            }
            if (!string.IsNullOrEmpty(context.Request["itemNo"]))
            {
                itemNo = context.Request["itemNo"];
            }
            if (itemNo == string.Empty) return;
            #region GetData
            ItemUnitsofMeasure.ItemUnitsofMeasure_Service s = new ItemUnitsofMeasure.ItemUnitsofMeasure_Service();
            s.Url = Clsc.ReBuildUrl(s.Url, Company);
            s.Credentials = Clsc.CheckCredentials();
            //Filter Item No.
            List<ItemUnitsofMeasure.ItemUnitsofMeasure_Filter> DataFilter = new List<ItemUnitsofMeasure.ItemUnitsofMeasure_Filter>();
            ItemUnitsofMeasure.ItemUnitsofMeasure_Filter TypeFilter = new ItemUnitsofMeasure.ItemUnitsofMeasure_Filter();
            TypeFilter.Field = ItemUnitsofMeasure.ItemUnitsofMeasure_Fields.Item_No;
            TypeFilter.Criteria = itemNo;
            DataFilter.Add(TypeFilter);
            try
            {
                ItemUnitsofMeasure.ItemUnitsofMeasure[] Results = s.ReadMultiple(DataFilter.ToArray(), "", fetchSize);
                List<ItemUnitsofMeasure.ItemUnitsofMeasure> ListResult = new List<ItemUnitsofMeasure.ItemUnitsofMeasure>();
                while (Results.Length > 0)
                {
                    bookmarkKey = Results.Last().Key;
                    ListResult.AddRange(Results);
                    Results = s.ReadMultiple(DataFilter.ToArray(), bookmarkKey, fetchSize);
                }
                Paging<ItemUnitsofMeasure.ItemUnitsofMeasure> list = PlantsPaging(start, limit, sort, dir, query, ListResult);
                context.Response.Write(string.Format("{{total:{1},'data':{0}}}", JSON.Serialize(list.Data), list.TotalRecords));
            }
            catch
            {
                throw;
            }
            #endregion C2
        }

        public static Paging<ItemUnitsofMeasure.ItemUnitsofMeasure> PlantsPaging(int start, int limit, string sort, string dir, string filter, List<ItemUnitsofMeasure.ItemUnitsofMeasure> listdata)
        {
            if (!string.IsNullOrEmpty(filter) && filter != "*")
            {
                listdata.RemoveAll(d => !d.Code.ToLower().StartsWith(filter.ToLower()));
            }

            if (!string.IsNullOrEmpty(sort))
            {
                listdata.Sort(delegate (ItemUnitsofMeasure.ItemUnitsofMeasure x, ItemUnitsofMeasure.ItemUnitsofMeasure y)
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

            List<ItemUnitsofMeasure.ItemUnitsofMeasure> rangePlants = (start < 0 || limit < 0) ? listdata : listdata.GetRange(start, limit);

            return new Paging<ItemUnitsofMeasure.ItemUnitsofMeasure>(rangePlants, listdata.Count);
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