using DTO.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Classes
{
    public class BLL_GLXAccount
    {

        DAL.LinqToSql.GLXAccountDataContext db = new DAL.LinqToSql.GLXAccountDataContext();
        DAL.LinqToSql.AccountDataContext db_Account = new DAL.LinqToSql.AccountDataContext();
        DAL.LinqToSql.LocationAndAccountDataContext db_Location = new DAL.LinqToSql.LocationAndAccountDataContext();
        DAL.LinqToSql.RequisitionWkshNameDataContext db_Req = new DAL.LinqToSql.RequisitionWkshNameDataContext();
        DAL.LinqToSql.PermissionsDataContext db_Permission = new DAL.LinqToSql.PermissionsDataContext();

        BLL_LocationAndAccount bllocationAndAccount = new BLL_LocationAndAccount();
        BLL_RequisitionWkshName bllRequisitionWkshName = new BLL_RequisitionWkshName();
        BLL_Permissions bllPermissions = new BLL_Permissions();

        public bool CheckExistsItem(string username, string company)
        {
            //True => Allow Insert
            try
            {
                if (db.GLXAccounts.Any(o => o.Account == username && o.Company == company))
                    return true;
            }
            catch (Exception e)
            {
                string mes = e.Message;
                return false;
            }
            return false;
        }

        public IList<data_GLXAccount> GetAll(string username)
        {
            var list_items = from p in db.GLXAccounts
                             where p.Account == username
                             orderby p.Company descending, p.Account descending
                             select new data_GLXAccount(p);
            return list_items.ToList();
        }

        //--------------------------------------//--------------------------------------//
        //Account
        DAL.LinqToSql.AccountDataContext AccDataContext = new DAL.LinqToSql.AccountDataContext();
        public IList<data_Account> GetAllAccount(string username)
        {
            var list_items = from p in AccDataContext.Accounts
                             orderby p.Account1
                             select new data_Account(p);
            return list_items.ToList();
        }

        public bool CheckExistsAccount(string username)
        {
            try
            {
                if (AccDataContext.Accounts.Any(o => o.Account1 == username))
                    return true;
            }
            catch (Exception e)
            {
                string mes = e.Message;
                return false;
            }
            return false;
        }

        public string createUser(data_UserCreateRequest request)
        {
            //if (CheckExistsItem(request.Username, request.Company))
            //    return "The account and company is already existed.";

            //if (bllocationAndAccount.CheckExistsLocationAndAccount(request.Username, request.Location))
            //    return "The account and location is already existed.";

            //if (bllRequisitionWkshName.CheckExistsUserWkshName(request.Username, request.Batch))
            //    return "The account and worksheet name is already existed.";

            //Account table
            var account = new DAL.LinqToSql.Account();
            account.UID = Guid.NewGuid();
            account.Account1 = request.Username;
            account.FullName = request.FullName;

            //GLXAccount
            var gLXAccount = new DAL.LinqToSql.GLXAccount();
            gLXAccount.UID = Guid.NewGuid();
            gLXAccount.Account = request.Username;
            gLXAccount.Company = request.Company;

            //LocationAndAccount
            var locationAndAccount = new DAL.LinqToSql.LocationAndAccount();
            locationAndAccount.UID = Guid.NewGuid();
            locationAndAccount.Account = request.Username;
            locationAndAccount.Location = request.Location;

            //Requisition Wksh. Name
            var RequisitionWkshName = new DAL.LinqToSql.Requisition_Wksh__Name();
            RequisitionWkshName.UID = Guid.NewGuid();
            RequisitionWkshName.Account = request.Username;
            RequisitionWkshName.Company = request.Company;
            RequisitionWkshName.Name = request.Batch;
            RequisitionWkshName.Worksheet_Template_Name = "REQ.";

            try
            {
                if (!CheckExistsAccount(request.Username))
                    db_Account.Accounts.InsertOnSubmit(account);

                if (!CheckExistsItem(request.Username, request.Company))
                    db.GLXAccounts.InsertOnSubmit(gLXAccount);

                if (!bllocationAndAccount.CheckExistsLocationAndAccount(request.Username, request.Location))
                    db_Location.LocationAndAccounts.InsertOnSubmit(locationAndAccount);

                if (!bllRequisitionWkshName.CheckExistsUserWkshName(request.Company, request.Username, request.Batch))
                    db_Req.Requisition_Wksh__Names.InsertOnSubmit(RequisitionWkshName);

                db_Account.SubmitChanges();
                db.SubmitChanges();
                db_Location.SubmitChanges();
                db_Req.SubmitChanges();

                GrantPermission(request.Username, request.Companies, request.Role);
            }
            catch (Exception e)
            {
                return e.Message;
            }

            return "";
        }

        private void GrantPermission(string Username, string Companies, string Role)
        {
            List<string> arrCompanies = Companies.Split(',').ToList();
            arrCompanies.RemoveAt(arrCompanies.Count - 1);

            foreach (string company in arrCompanies)
            {
                if (bllPermissions.CheckExistPermission(Username, company))
                    bllPermissions.CopyPermission("phongdd", Username, "GALAXY_BMT_COPY", company, Role);
            }
        }


        public string DelUser(string uname)
        {
            var account = db_Account.Accounts.FirstOrDefault(x => x.Account1 == uname);
            var glxAccount = db.GLXAccounts.FirstOrDefault(x => x.Account == uname);
            var userLocation = db_Location.LocationAndAccounts.FirstOrDefault(x => x.Account == uname);
            var reqUsername = db_Req.Requisition_Wksh__Names.FirstOrDefault(x => x.Account == uname);
            var perUsername = from permissions in db_Permission.Permissions where permissions.Username == uname select permissions;
            try
            {
                db_Account.Accounts.DeleteOnSubmit(account);
                db.GLXAccounts.DeleteOnSubmit(glxAccount);
                db_Location.LocationAndAccounts.DeleteOnSubmit(userLocation);
                db_Req.Requisition_Wksh__Names.DeleteOnSubmit(reqUsername);
                foreach (var per in perUsername)
                {
                    db_Permission.Permissions.DeleteOnSubmit(per);
                }

                db_Account.SubmitChanges();
                db.SubmitChanges();
                db_Location.SubmitChanges();
                db_Req.SubmitChanges();
                db_Permission.SubmitChanges();
            }
            catch (Exception ex)
            {
                return ex.Message;
                throw;
            }

            return "";
        }
    }
}
