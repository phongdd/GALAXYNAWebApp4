using DAL.LinqToSql;
using DTO.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Classes
{
    public class BLL_Permissions
    {
        PermissionsDataContext db = new DAL.LinqToSql.PermissionsDataContext();
        public IList<data_Permissions> GetPermissionByUser(string username)
        {
            var list_items = from p in db.Permissions
                             where p.Username == username
                             orderby p.Action
                             select new data_Permissions(p);
            return list_items.ToList();
        }

        public bool CheckExistPermission(string Username, string Company)
        {
            var pers = db.Permissions.FirstOrDefault(x => x.Username == Username && x.Company == Company);
            if (pers != null)
                return false; 
            return true;        //=> Chua ton tai => cho phep insert
        }

        public string CopyPermission(string userFrom, string userTo, string CompanyFrom, string CompanyTo, string Role)
        {
            try
            {
                using (var conn = new SqlConnection(db.Connection.ConnectionString))
                using (var command = new SqlCommand("CopyPermission", conn)
                {
                    CommandType = CommandType.StoredProcedure,
                    Parameters = {
                        new SqlParameter("userFrom", userFrom),
                        new SqlParameter("CompanyFrom", CompanyFrom),
                        new SqlParameter("userTo", userTo),
                        new SqlParameter("CompanyTo", CompanyTo),
                        new SqlParameter("Role", Role),
                    }
                })
                {
                    conn.Open();
                    command.ExecuteNonQuery();
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        
    }
}
