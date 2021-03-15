using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace DTO.Classes
{
    [DataContract]
    public class data_RequisitionWkshName
    {
        [DataMember]
        public string UID;
        [DataMember]
        public string Worksheet_Template_Name;
        [DataMember]
        public string Name;
        [DataMember]
        public string Description;
        [DataMember]
        public string Template_Type;
        [DataMember]
        public bool? Recurring;
        [DataMember]
        public string Company;
        [DataMember]
        public string Account;

        public data_RequisitionWkshName()
        {

        }

        public data_RequisitionWkshName(DAL.LinqToSql.Requisition_Wksh__Name data)
        {
            this.UID = data.UID.ToString();
            this.Worksheet_Template_Name = data.Worksheet_Template_Name;
            this.Name = data.Name;
            this.Description = data.Description;
            this.Template_Type = data.Template_Type;
            this.Recurring = data.Recurring;
            this.Company = data.Company;
            this.Account = data.Account;
        }
    }
}
