using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.WebClientModels.Requests
{
    public class RegistrationUser
    {
        public string userName { get; set; }
        public string userLastName { get; set; }
        public string userLogin { get; set; }
        public string userPassword { get; set; }
        public string userConfirmPassword { get; set; }

        public RegistrationUser(
            string userName,
            string userLastName,
            string userLogin,
            string userPassword,
            string userConfirmPassword)
        {
            this.userName = userName;
            this.userLastName = userLastName;
            this.userLogin = userLogin;
            this.userPassword = userPassword;
            this.userConfirmPassword = userConfirmPassword;
        }
    }
}
