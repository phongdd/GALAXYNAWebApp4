using DTO.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Classes
{
    public class BLL_LocationAndAccount
    {
        DAL.LinqToSql.LocationAndAccountDataContext db = new DAL.LinqToSql.LocationAndAccountDataContext();
        public IList<data_LocationAndAccount> GetLocationByUser(string username)
        {
            var list_items = from p in db.LocationAndAccounts
                             where p.Account == username
                             orderby p.Location
                             select new data_LocationAndAccount(p);
            return list_items.ToList();
        }

        public bool CheckExistsLocationAndAccount(string username, string location)
        {
            //True => Allow Insert
            try
            {
                if (db.LocationAndAccounts.Any(o => o.Account == username && o.Location == location))
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
