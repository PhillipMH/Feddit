using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Feddit_Domain.Models
{
    public class SubFedditPosts
    {
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
        public Guid SubFedditId { get; set; }
        [MaxLength(100)]
        public string PostTitle { get; set; }
        [MaxLength(100)]
        public string PostContent { get; set; }
        public string PostPic { get; set; }
        public DateTime DateCreated { get; set; }
        public SubFedditPosts() { }
        public SubFedditPosts(Guid postId, Guid userId, Guid subFedditId, string postPic, string postTitle, string postContent, DateTime dateCreated)
        {
            PostId = postId;
            UserId = userId;
            SubFedditId = subFedditId;
            PostTitle = postTitle;
            PostContent = postContent;
            PostPic = postPic;
            DateCreated = dateCreated;
        }
    }
}
