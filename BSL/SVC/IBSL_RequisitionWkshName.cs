using DTO.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace BSL.SVC
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IBSL_RequisitionWkshName" in both code and config file together.
    [ServiceContract]
    public interface IBSL_RequisitionWkshName
    {
        [OperationContract]
        IList<data_RequisitionWkshName> GetAll(string company, string username);

        [OperationContract]
        IList<data_RequisitionWkshName> GetDataByUser(string username);
    }
}
