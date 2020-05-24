using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Requests
{
    public class RegisterUserRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }

        public RegisterUserRequest(
            string login,
            string password,
            string name,
            string lastName)
        {
            Login = login;
            Password = password;
            Name = name;
            LastName = lastName;
        }
    }
}
