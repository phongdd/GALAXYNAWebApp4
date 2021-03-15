using DTO.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Classes
{
    public class BLL_RequisitionWkshName
    {
        DAL.LinqToSql.RequisitionWkshNameDataContext db = new DAL.LinqToSql.RequisitionWkshNameDataContext();
        public IList<data_RequisitionWkshName> GetAll(string company, string username)
        {
            var list_items = from p in db.Requisition_Wksh__Names
                             where p.Worksheet_Template_Name == "REQ." && p.Company == company && p.Account == username
                             orderby p.Name
                             select new data_RequisitionWkshName(p);
            return list_items.ToList();
        }

        public IList<data_RequisitionWkshName> GetDataByUser(string username)
        {
            var list_items = from p in db.Requisition_Wksh__Names
                             where p.Account == username
                             orderby p.Name
                             select new data_RequisitionWkshName(p);
            return list_items.ToList();
        }

        public bool CheckExistsUserWkshName(string company, string username, string ReqWkshName)
        {
            //True => Allow Insert
            try
            {
                if (db.Requisition_Wksh__Names.Any(o => o.Account == username &&o.Company == company && o.Name == ReqWkshName))
                    return true;
            }
            catch (Exception e)
            {
                string mes = e.Message;
                return false;
            }
            return false;
        }

    }
}
