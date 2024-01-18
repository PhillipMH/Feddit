using Feddit_Domain.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Feddit.Pages
{
    public class CreateSubFedditModel : PageModel
    {
        private readonly Connection _connection;
        public CreateSubFedditModel(Connection connection)
        {
            _connection = connection;
        }
        [BindProperty]
        public string name { get; set; }
        [BindProperty]
        public DateTime creationdate { get; set; } = DateTime.Now;
        public void OnGet()
        {
        }
        public async Task OnPost()
        {
            await _connection.CreateSubfeddit(name, creationdate);

        }
    }
}
