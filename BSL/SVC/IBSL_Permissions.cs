using DTO.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace BSL.SVC
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IBSL_Permissions" in both code and config file together.
    [ServiceContract]
    public interface IBSL_Permissions
    {
        [OperationContract]
        IList<data_Permissions> GetPermissionByUser(string username);

        [OperationContract]
        string CopyPermission(string userFrom, string userTo, string CompanyFrom, string CompanyTo);
    }
}
