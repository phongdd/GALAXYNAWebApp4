using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using DAL.LinqToSql;

namespace DTO.Classes
{
    [DataContract]
    public class data_GLXAccount
    {
        [DataMember]
        public string UID;
        [DataMember]
        public string Account;
        [DataMember]
        public string Company;
        public data_GLXAccount()
        {

        }
        public data_GLXAccount(DAL.LinqToSql.GLXAccount data)
        {
            this.UID = data.UID.ToString();
            this.Account = data.Account;
            this.Company = data.Company;
        }
    }

    [DataContract]
    public class data_GLXAccount_Simple
    {
        [DataMember]
        public string Account { get; set; }
        [DataMember]
        public string Fullname { get; set; }

        public data_GLXAccount_Simple(string acc, string name)
        {
            this.Account = acc;
            this.Fullname = name;
        }
    }

    [DataContract]
    public class data_Account
    {
        [DataMember]
        public string UID;
        [DataMember]
        public string Account;
        [DataMember]
        public string Fullname;

        public data_Account(Account data)
        {
            this.UID = data.UID.ToString();
            this.Account = data.Account1;
            this.Fullname = data.FullName;
        }
    }
}
