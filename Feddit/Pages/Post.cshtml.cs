using Feddit_Domain.Connections;
using Feddit_Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Runtime.CompilerServices;

namespace Feddit.Pages
{
    public class PostModel : PageModel
    {
        private readonly Connection _connection;

        public PostModel(Connection connection)
        {
            _connection = connection;
        }

        [BindProperty(SupportsGet = true)]
        public Guid PostId { get; set; }

        [BindProperty]
        public string Title { get; set; }

        [BindProperty]
        public string PicturePath { get; set; }

        [BindProperty]
        public string Content { get; set; }

        [BindProperty]
        public DateTime DateCreated { get; set; }
        [BindProperty]
        public List<Comments> Comments { get; set; }
        [BindProperty]
        public string Mail  { get; set; }
        public Guid UserId { get; set; }

        public async Task OnGet()
        {
            SubFedditPosts post = await _connection.GetPostById(PostId);

            if (post != null)
            {
                Title = post.PostTitle;
                PicturePath = post.PostPic;
                Content = post.PostContent;
                DateCreated = post.DateCreated;
            }
            Comments = await _connection.GetAllCommentsAsync(PostId);
        }
        public async Task OnPost()
        {
            Mail = HttpContext.Session.GetString("Mail");
            Users founduser = await _connection.GetUserByMail(Mail);
            UserId = founduser.UserId;
            Comments comment = await _connection.CommentOnPost(PostId, UserId, Title, Content, DateTime.Now);
        }
    }

}
