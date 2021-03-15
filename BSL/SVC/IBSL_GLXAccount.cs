using DTO.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace BSL.SVC
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IBSL_GLXAccount" in both code and config file together.
    [ServiceContract]
    public interface IBSL_GLXAccount
    {
        [OperationContract]
        bool CheckExistsItem(string username, string company);

        [OperationContract]
        IList<data_GLXAccount> GetAll(string username);

        [OperationContract]
        IList<data_Account> GetAllAccount(string username);

        [OperationContract]
        string createUser(data_UserCreateRequest request);

        [OperationContract]
        string DelUser(string uname);
    }
}
