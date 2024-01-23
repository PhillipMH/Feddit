using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Feddit_Domain.Models
{
    public class Users
    {
        public Guid UserId { get; set; }
        [MaxLength(75)]
        public string Name { get; set; }
        [MaxLength(200)]
        public string Email { get; set; }
        [MaxLength(100)]
        public string Password { get; set; }
        public bool IsDeleted { get; set; }
        public bool Admin { get; set; }
        public Users() { }
        public Users(Guid userid, string mail, string name, string pass, bool isdeleted, bool admin)
        {
            UserId = userid;
            Email = mail;
            Name = name;
            Password = pass;
            IsDeleted = isdeleted;
            Admin = admin;
        }

    }
}
