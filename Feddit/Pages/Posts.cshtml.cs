using Feddit_Domain.Connections;
using Feddit_Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Runtime.Versioning;

namespace Feddit.Pages
{
    public class PostsModel : PageModel
    {
        private readonly Connection _connection;
        public PostsModel(Connection connection)
        {
            _connection = connection;
        }
        [BindProperty]
        public Guid SubfedditID { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SubFedditName { get; set; }
        [BindProperty]
        public string PicturePath { get; set; }
        [BindProperty]
        public string Title { get; set; }
        [BindProperty]
        public string Content { get; set; }
        [BindProperty]
        public List<SubFedditPosts> SubFedditPosts { get; set; }
        public async Task OnGet()
        {
            SubFeddits subfedditemp = await _connection.GetSubFedditByName(SubFedditName);
            SubfedditID = subfedditemp.SubFedditId;
            SubFedditPosts = await _connection.GetAllSubfedditPosts(SubfedditID);
        }
        public async Task OnPost()
        {
            string temp = HttpContext.Session.GetString("Mail");
            Users user = await _connection.GetUserByMail(temp);
            SubFeddits subfeddit = await _connection.GetSubFedditByName(SubFedditName);
            SubfedditID = subfeddit.SubFedditId;
            await _connection.AddPostToSubFeddit(user.UserId, SubfedditID, Title, Content);
        }
    }
}
