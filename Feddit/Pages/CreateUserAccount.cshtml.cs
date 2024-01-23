using Feddit_Domain.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Feddit.Pages
{
    public class CreateUserAccountModel : PageModel
    {
        private readonly Connection _connection;
        public CreateUserAccountModel(Connection connection)
        {
            _connection = connection;
        }
        [BindProperty]
        [MaxLength(75)]
        public string Mail { get; set; }
        [BindProperty]
        [MaxLength(25)]
        public string AccountUsername { get; set; }
        [BindProperty]
        [MaxLength(100)]
        public string Password { get; set; }
        [BindProperty]
        [MaxLength(100)]
        public string ConfirmPassword { get; set; }

        public void OnGet()
        {
        }
        public async Task OnPost()
        {
            if(Password == ConfirmPassword)
            {
                await _connection.CreateUser(Mail, Password, AccountUsername);
            }
            else
            {
                Page();
            }
        }
    }
}
