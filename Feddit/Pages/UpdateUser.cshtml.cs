using Feddit_Domain.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Feddit_Domain.Models;
using Feddit.Pages.Shared;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace Feddit.Pages
{
    public class UpdateUserModel : PageModel
    {
        private readonly Connection _connection;
        public UpdateUserModel(Connection connection)
        {
            _connection = connection;
        }
        [BindProperty]
        public string Mail { get; set; }
        [BindProperty]
        public string Password { get; set; }
        [BindProperty]
        public string AccountName { get; set; }
        public Guid Userid { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsDeleted { get; set; }
        public bool LoginSuccess { get; set; }
        public async void OnGet(string mail)
        {
            mail = HttpContext.Session.GetString("Mail");
            Users founduser = await _connection.GetUserByMail(mail);
            if (founduser == null)
            {
                RedirectToPage("/index");
            }
            if (founduser != null) 
            {
            Mail = founduser.Email; 
            Password = founduser.Password;
            AccountName = founduser.Name;
            }
        }
        public async Task<IActionResult> OnPostupdateForm(string mail)
        {
            mail = HttpContext.Session.GetString("Mail");
            Users user = await _connection.GetUserByMail(mail);
                user.Email = Mail;
                user.Password = Password;
                user.Name = AccountName;
                user.Admin = IsAdmin;
                user.IsDeleted = IsDeleted;
                
            Users updateduser = new Users();
            updateduser = await _connection.UpdateUser(user);
            if (updateduser != null)
            {
                
            }
            return Page();
        }
        public async Task OnPost(Guid id, string mail)
        {
            mail = HttpContext.Session.GetString("Mail");
            Users user = await _connection.GetUserByMail(mail);
            id = user.UserId;
            await _connection.DeleteUserById(id);
            RedirectToPage("/Login");
            HttpContext.Session.Clear();
        }
    }
}
