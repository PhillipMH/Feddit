using Feddit_Domain.Connections;
using Feddit_Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Feddit.Pages
{
    public class ProfileModel : PageModel
    {
        private readonly Connection _connection;
        public ProfileModel(Connection connection)
        {
            _connection = connection;
        }
        [BindProperty]
        public string Mail { get; set; }
        [BindProperty]
        public string Name { get; set; }
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
                Name = founduser.Name;
            }
        }
    }
}
