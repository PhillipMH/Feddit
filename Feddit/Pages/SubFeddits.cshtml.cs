using Feddit_Domain.Connections;
using Feddit_Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Feddit.Pages
{
    public class SubFedditsModel : PageModel
    {
        private readonly Connection _connection;
        public SubFedditsModel(Connection connection)
        {
            _connection = connection;
        }
        [BindProperty]
        public string SubFedditName { get; set; }
        [BindProperty]
        public DateTime SubFedditCreatedAt { get; set; }
        [BindProperty]
        public List<SubFeddits> SubFeddits { get; set; }
        public async Task OnGet()
        {
            SubFeddits = await _connection.GetAllSubFeddits();
        }
    }
}
