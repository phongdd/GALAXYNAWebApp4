using DTO.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace BSL.SVC
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IBSL_POWaitingForApprove" in both code and config file together.
    [ServiceContract]
    public interface IBSL_POWaitingForApprove
    {
        [OperationContract]
        IList<data_POWaitingForApprove> GetAll(string batch, DateTime fromdate, DateTime todate, string username);

        [OperationContract]
        bool CheckPermission(string company, string username, string page, string action);

        [OperationContract]
        string UpdateToApprove(string uid, string username, string newStatus);

        [OperationContract]
        string Reopen(string uid, string username);

        [OperationContract]
        string UpdateConverted(string uid, string convertType, string username);

        [OperationContract]
        string InsertComment(string uid, string commentText, string username);

        [OperationContract]
        string TobeDeleted(string uid, string username, string newStatus);

        [OperationContract]
        data_POWaitingForApprove GetRecIdFromKey(string uid);

        [OperationContract]
        string UpdateData(string uid, data_POWaitingForApprove data, string username);
    }
}
