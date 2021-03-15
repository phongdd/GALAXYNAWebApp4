using Ext.Net;
using GLXNAVWebApp.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GLXNAVWebApp.Handler
{
    public class LocationListHandler : IHttpHandler
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
            string CustomerNo = string.Empty;
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
            if (!string.IsNullOrEmpty(context.Request["customer_no"]))
            {
                CustomerNo = context.Request["customer_no"];
            }
            if (!string.IsNullOrEmpty(context.Request["CurCompany"]))
            {
                Company = context.Request["CurCompany"];
            }

            LocationList.LocationList_Service s = new LocationList.LocationList_Service();
            s.Url = common.ReBuildUrl(s.Url, Company);
            s.Credentials = common.CheckCredentials();
            List<LocationList.LocationList_Filter> DataFilter = new List<LocationList.LocationList_Filter>();

            LocationList.LocationList[] Results = s.ReadMultiple(DataFilter.ToArray(), bookmarkKey, fetchSize);
            List<LocationList.LocationList> ListResult = new List<LocationList.LocationList>();
            while (Results.Length > 0)
            {
                bookmarkKey = Results.Last().Key;
                ListResult.AddRange(Results);
                Results = s.ReadMultiple(DataFilter.ToArray(), bookmarkKey, fetchSize);
            }
            Paging<LocationList.LocationList> list = PlantsPaging(start, limit, sort, dir, query, ListResult);
            context.Response.Write(string.Format("{{total:{1},'data':{0}}}", JSON.Serialize(list.Data), list.TotalRecords));
        }

        public static Paging<LocationList.LocationList> PlantsPaging(int start, int limit, string sort, string dir, string filter, List<LocationList.LocationList> listdata)
        {
            if (!string.IsNullOrEmpty(filter) && filter != "*")
            {
                listdata.RemoveAll(d => !d.Code.ToLower().StartsWith(filter.ToLower()));
            }

            if (!string.IsNullOrEmpty(sort))
            {
                listdata.Sort(delegate (LocationList.LocationList x, LocationList.LocationList y)
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

            List<LocationList.LocationList> rangePlants = (start < 0 || limit < 0) ? listdata : listdata.GetRange(start, limit);

            return new Paging<LocationList.LocationList>(rangePlants, listdata.Count);
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