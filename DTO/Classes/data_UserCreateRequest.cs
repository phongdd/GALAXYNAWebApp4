using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Classes
{
    public class data_UserCreateRequest
    {
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Company { get; set; }
        public string Location { get; set; }
        public string Batch { get; set; }
        public string Role { get; set; }
        public string Companies { get; set; }
    }
}
