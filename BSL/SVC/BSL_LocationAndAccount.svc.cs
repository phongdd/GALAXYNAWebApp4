using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using DTO.Classes;
using BLL.Classes;

namespace BSL.SVC
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "BSL_LocationAndAccount" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select BSL_LocationAndAccount.svc or BSL_LocationAndAccount.svc.cs at the Solution Explorer and start debugging.
    public class BSL_LocationAndAccount : IBSL_LocationAndAccount
    {
        BLL_LocationAndAccount bll = new BLL_LocationAndAccount();
        public IList<data_LocationAndAccount> GetLocationByUser(string username)
        {
            return bll.GetLocationByUser(username);
        }
    }
}
