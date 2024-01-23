using Feddit_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feddit_Domain.Connections
{
    public interface IConnection
    {
        Task CreateUser(string mail, string password, string name);
        Task<Users> GetUserByMail(string mail);
        Task<Users> UpdateUser(Users user);
        Task DeleteUserById(Guid id);
    }
}
