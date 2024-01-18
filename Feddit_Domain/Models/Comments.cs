using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feddit_Domain.Models
{
    public class Comments
    {
        public Guid CommentId { get; set; }
        [MaxLength(500)]
        public string CommentContent { get; set; }
        public DateTime CurrentTime { get; set; }
        public SubFedditPosts PostId { get; set; }
        public Users UserId { get; set; }
    }
}
