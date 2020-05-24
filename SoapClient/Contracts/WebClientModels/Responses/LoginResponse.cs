using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.WebClientModels.Responses
{
    public class LoginResponse
    {
        public int id { get; set; }
        public string userName { get; set; }
        public string userLastName { get; set; }
        public string userLogin { get; set; }
        public string userPassword { get; set; }
        public string userConfirmPassword { get; set; }
        public bool isAdmin { get; set; }

        public LoginResponse(
            int id,
            string userName,
            string userLastName,
            string userLogin,
            string userPassword,
            string userConfirmPassword,
            bool isAdmin)
        {
            this.id = id;
            this.userName = userName;
            this.userLastName = userLastName;
            this.userLogin = userLogin;
            this.userPassword = userPassword;
            this.userConfirmPassword = userConfirmPassword;
            this.isAdmin = isAdmin;
        }
    }
}
