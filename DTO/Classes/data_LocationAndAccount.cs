using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Classes
{
    [DataContract]
    public class data_LocationAndAccount
    {
        [DataMember]
        public string UID;
        [DataMember]
        public string Account;
        [DataMember]
        public string Location;
       
        public data_LocationAndAccount(DAL.LinqToSql.LocationAndAccount data)
        {
            this.UID = data.UID.ToString();
            this.Account = data.Account;
            this.Location = data.Location;
        }
    }
}
