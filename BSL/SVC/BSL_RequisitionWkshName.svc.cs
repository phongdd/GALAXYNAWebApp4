using DTO.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace BSL.SVC
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "BSL_RequisitionWkshName" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select BSL_RequisitionWkshName.svc or BSL_RequisitionWkshName.svc.cs at the Solution Explorer and start debugging.
    public class BSL_RequisitionWkshName : IBSL_RequisitionWkshName
    {
        BLL.Classes.BLL_RequisitionWkshName bll = new BLL.Classes.BLL_RequisitionWkshName();
        public IList<data_RequisitionWkshName> GetAll(string company, string username)
        {
            return bll.GetAll(company, username);
        }

        public IList<data_RequisitionWkshName> GetDataByUser(string username)
        {
            return bll.GetDataByUser(username);
        }
    }
}
