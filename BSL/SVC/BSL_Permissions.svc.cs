using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using DTO.Classes;

namespace BSL.SVC
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "BSL_Permissions" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select BSL_Permissions.svc or BSL_Permissions.svc.cs at the Solution Explorer and start debugging.
    public class BSL_Permissions : IBSL_Permissions
    {
        BLL.Classes.BLL_Permissions bll = new BLL.Classes.BLL_Permissions();
        
        public string CopyPermission(string userFrom, string userTo, string CompanyFrom, string CompanyTo)
        {
            //return bll.CopyPermission(userFrom, userTo, CompanyFrom, CompanyTo);
            return "";
        }

        public IList<data_Permissions> GetPermissionByUser(string username)
        {
            return bll.GetPermissionByUser(username);
        }
    }
}
