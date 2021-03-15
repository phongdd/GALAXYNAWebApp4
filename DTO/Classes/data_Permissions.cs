using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Classes
{
    [DataContract]
    public class data_Permissions
    {
        [DataMember]
        string UID;
        [DataMember]
        string Company;
        [DataMember]
        string Username;
        [DataMember]
        string Page;
        [DataMember]
        string Action;
        [DataMember]
        bool? Allow;

        public data_Permissions(DAL.LinqToSql.Permission data)
        {
            this.UID = data.UID.ToString();
            this.Company = data.Company;
            this.Username = data.Username;
            this.Page = data.Page;
            this.Action = data.Action;
            this.Allow = data.Allow;
        }
    }
}
