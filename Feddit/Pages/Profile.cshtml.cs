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
        public string commenttitle { get; set; }
        [BindProperty]
        public string commentcontent { get; set; }
        [BindProperty]
        public string Mail { get; set; }
        [BindProperty]
        public string Name { get; set; }
        [BindProperty]
        public List<Comments> commentlist { get; set; }
        [BindProperty]
        public Guid Userid { get; set; }
        [BindProperty]
        public List<SubFedditPosts> postlist { get; set; }
        [BindProperty]
        public Guid PostId { get; set; }
        [BindProperty]
        public string posttitle { get; set; }
        [BindProperty]
        public string postcontent { get; set; }
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
                Users temp = await _connection.GetUserByMail(Mail);
                Userid = temp.UserId;
                commentlist = await _connection.GetAllCommentsFromSpecificUserAsync(Userid);
                postlist = await _connection.GetAllPostsFromSpecificUserAsync(Userid);
            }

        }
        public async Task OnPostUpdatePost(SubFedditPosts newpost)
        {
           await _connection.GetPostById(PostId);
            SubFedditPosts post = new SubFedditPosts();
            post.PostTitle = posttitle;
            post.PostContent = postcontent;
            newpost = await _connection.UpdatePost(post);
        }
    }
}
