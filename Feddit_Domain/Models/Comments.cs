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
        [MaxLength(75)]
        public string CommentTitle { get; set; }
        [MaxLength(500)]
        public string CommentContent { get; set; }
        public DateTime CurrentTime { get; set; }
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
    }
}
