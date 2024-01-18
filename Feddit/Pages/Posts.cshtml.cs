using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Feddit.Pages
{
    public class PostsModel : PageModel
    {
        public string SubFedditName { get; set; }
        public void OnGet()
        {
        }
    }
}
