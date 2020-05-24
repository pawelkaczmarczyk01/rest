using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.ViewModels.Admin
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public bool IsAdmin { get; set; } = false;

        public User(
            int id,
            string name,
            string lastName)
        {
            UserId = id;
            Name = name;
            LastName = lastName;
        }
    }
}
