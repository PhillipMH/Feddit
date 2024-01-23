using Feddit_Domain.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Feddit_Domain.Models;
using Feddit.Pages.Shared.SessionHelper;

namespace Feddit.Pages
{
    public class LoginModel : PageModel
    {
        private readonly Connection _connection;
        public LoginModel(Connection connection)
        {
            _connection = connection;
        }
        string successmessage = "";
        string errormessage = "";
        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string Password { get; set; }
        [BindProperty]
        public string AccountName { get; set; }
        public Guid userid { get; set; }
        public bool IsAdmin { get; } = true;
        public bool LoginSuccess { get; set; }

        public async Task<IActionResult> OnPostLogin(string email, string password, bool isadmin, bool LoginSuccess)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                LoginSuccess = false;
                errormessage = "Mail or Password incorrect";
                return null;
            }
            Users founduser = await _connection.LoginUsers(email, password);
            if (founduser is null)
            {
                LoginSuccess = false;
                errormessage = "Mail or Password incorrect";
            }
            if (founduser.Email == Email && founduser.Password == Password && founduser.Admin is false )
            {
                LoginSuccess = true;
                HttpContext.Session.SetSessionString(founduser.Email, "Mail");
                return RedirectToPage("/index");
            }
            if (founduser.Email == Email && founduser.Password == Password && founduser.Admin is true)
            {
                LoginSuccess = true;
                HttpContext.Session.SetSessionString(founduser.Email, "Mail");
                HttpContext.Session.SetSessionString(founduser.Admin.ToString(), "Admin");
                return RedirectToPage("/index");
            }
            return Page();
        }
    }
}
