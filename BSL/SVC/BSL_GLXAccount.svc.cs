using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using DTO.Classes;

namespace BSL.SVC
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "BSL_GLXAccount" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select BSL_GLXAccount.svc or BSL_GLXAccount.svc.cs at the Solution Explorer and start debugging.
    public class BSL_GLXAccount : IBSL_GLXAccount
    {
        BLL.Classes.BLL_GLXAccount bll = new BLL.Classes.BLL_GLXAccount();
        public bool CheckExistsItem(string username, string company)
        {
            return bll.CheckExistsItem(username, company);
        }

        public string createUser(data_UserCreateRequest request)
        {
            return bll.createUser(request);
        }

        public string DelUser(string uname)
        {
            return bll.DelUser(uname);
        }

        public IList<data_GLXAccount> GetAll(string username)
        {
            return bll.GetAll(username);
        }

        public IList<data_Account> GetAllAccount(string username)
        {
            return bll.GetAllAccount(username);
        }
    }
}
