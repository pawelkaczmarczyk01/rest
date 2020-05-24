using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.WebClientModels.Requests
{
    public class LoginRequest
    {
        public LoginRequest(string login, string password)
        {
            this.login = login;
            this.password = password;
        }

        public string login { get; set; }
        public string password { get; set; }
    }
}
