using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using DTO.Classes;

namespace BSL.SVC
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "BSL_POWaitingForApprove" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select BSL_POWaitingForApprove.svc or BSL_POWaitingForApprove.svc.cs at the Solution Explorer and start debugging.
    public class BSL_POWaitingForApprove : IBSL_POWaitingForApprove
    {
        BLL.Classes.BLL_POWaitingForApprove bll = new BLL.Classes.BLL_POWaitingForApprove();

        public bool CheckPermission(string company, string username, string page, string action)
        {
            return bll.CheckPermission(company, username, page, action);
        }

        public IList<data_POWaitingForApprove> GetAll(string batch, DateTime fromdate, DateTime todate, string username)
        {
            return bll.GetAll(batch, fromdate, todate, username);
        }

        public data_POWaitingForApprove GetRecIdFromKey(string uid)
        {
            return bll.GetRecIdFromKey(uid);
        }

        public string InsertComment(string uid, string commentText, string username)
        {
            return bll.InsertComment(uid, commentText, username);
        }

        public string Reopen(string uid, string username)
        {
            return bll.Reopen(uid, username);
        }

        public string TobeDeleted(string uid, string username, string newStatus)
        {
            return bll.TobeDeleted(uid, username, newStatus);
        }

        public string UpdateConverted(string uid, string convertType, string username)
        {
            return bll.UpdateConverted(uid, convertType, username);
        }

        public string UpdateData(string uid, data_POWaitingForApprove data, string username)
        {
            return bll.UpdateData(uid, data, username);
        }

        public string UpdateToApprove(string uid, string username, string newStatus)
        {
            return bll.UpdateToApprove(uid, username, newStatus);
        }
    }
}