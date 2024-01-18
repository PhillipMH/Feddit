using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Feddit_Domain.Connections;
using Feddit_Domain.Models;
using System.Runtime.CompilerServices;

namespace Feddit.Pages
{
    public class TestMethodsModel : PageModel
    {
        private readonly Connection _connection;
        public TestMethodsModel(Connection connection)
        {
            _connection = connection;
        }
        [BindProperty]
        public string name { get; set; }
        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string password { get; set; }
        [BindProperty]
        public Users UserId { get; set; }
        public async void OnPost()
        {
            List<string> users = new List<string>();
            try
            {
                Users Users = await _connection.GetUserByMail(Email);
            }
            catch (Exception)
            {

                throw;
            }
            
            //List<string> email = new List<string>();
            //try
            //{
            //    //email = _connection.GetUserByMail(Email);
            //   await _connection.CreateUser(Email, password, name);
            //}
            //catch (Exception)
            //{

            //    throw;
            //}


        }
    }
}
