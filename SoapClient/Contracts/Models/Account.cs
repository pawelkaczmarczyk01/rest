using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Models
{
    public class Account
    {
        public int AccountId { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public bool IsAdmin { get; set; }

        public Account(int id, string login, 
            string name,
            string lastName,
            bool isAdmin)
        {
            AccountId = id;
            Login = login;
            Name = name;
            LastName = lastName;
            IsAdmin = isAdmin;
        }
    }
}
