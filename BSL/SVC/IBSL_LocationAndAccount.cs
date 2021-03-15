using DTO.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace BSL.SVC
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IBSL_LocationAndAccount" in both code and config file together.
    [ServiceContract]
    public interface IBSL_LocationAndAccount
    {

        [OperationContract]
        IList<data_LocationAndAccount> GetLocationByUser(string username);
    }
}
