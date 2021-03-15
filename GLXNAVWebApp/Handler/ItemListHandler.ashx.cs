using Ext.Net;
using GLXNAVWebApp.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GLXNAVWebApp.Handler
{
    public class ItemListHandler : IHttpHandler
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

            if (!string.IsNullOrEmpty(context.Request["CurCompany"]))
            {
                Company = context.Request["CurCompany"];
            }

            ItemList.ItemList_Service s = new ItemList.ItemList_Service();
            s.Url = common.ReBuildUrl(s.Url, Company);
            s.Credentials = common.CheckCredentials();
            List<ItemList.ItemList_Filter> DataFilter = new List<ItemList.ItemList_Filter>();

            #region Filter
            //Replenishment_System = Purchase
            ItemList.ItemList_Filter ReplenishmentSystemFilter = new ItemList.ItemList_Filter();
            ReplenishmentSystemFilter.Field = ItemList.ItemList_Fields.Replenishment_System;
            ReplenishmentSystemFilter.Criteria = "Purchase";
            DataFilter.Add(ReplenishmentSystemFilter);

            ItemList.ItemList_Filter Inventory_Posting_GroupFilter = new ItemList.ItemList_Filter();
            Inventory_Posting_GroupFilter.Field = ItemList.ItemList_Fields.Inventory_Posting_Group;
            Inventory_Posting_GroupFilter.Criteria = "RAW MT|RESALE|IT-TECH";
            DataFilter.Add(Inventory_Posting_GroupFilter);

            ItemList.ItemList_Filter BlockedFilter = new ItemList.ItemList_Filter();
            BlockedFilter.Field = ItemList.ItemList_Fields.Blocked;
            BlockedFilter.Criteria = "false";
            DataFilter.Add(BlockedFilter);

            //No
            ItemList.ItemList_Filter NoFilter = new ItemList.ItemList_Filter();
            NoFilter.Field = ItemList.ItemList_Fields.No;
            NoFilter.Criteria = "*" + query + "*";
            DataFilter.Add(NoFilter);
            
            #endregion 

            ItemList.ItemList[] Results = s.ReadMultiple(DataFilter.ToArray(), bookmarkKey, fetchSize);
            List<ItemList.ItemList> ListResult = new List<ItemList.ItemList>();
            while (Results.Length > 0)
            {
                bookmarkKey = Results.Last().Key;
                ListResult.AddRange(Results);
                Results = s.ReadMultiple(DataFilter.ToArray(), bookmarkKey, fetchSize);
            }
            Paging<ItemList.ItemList> list = PlantsPaging(start, limit, sort, dir, query, ListResult);
            context.Response.Write(string.Format("{{total:{1},'data':{0}}}", JSON.Serialize(list.Data), list.TotalRecords));
        }

        public static Paging<ItemList.ItemList> PlantsPaging(int start, int limit, string sort, string dir, string filter, List<ItemList.ItemList> listdata)
        {
            if (!string.IsNullOrEmpty(filter) && filter != "*")
            {
                listdata.RemoveAll(d => !d.No.ToLower().StartsWith(filter.ToLower()));
            }

            if (!string.IsNullOrEmpty(sort))
            {
                listdata.Sort(delegate (ItemList.ItemList x, ItemList.ItemList y)
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

            List<ItemList.ItemList> rangePlants = (start < 0 || limit < 0) ? listdata : listdata.GetRange(start, limit);

            return new Paging<ItemList.ItemList>(rangePlants, listdata.Count);
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