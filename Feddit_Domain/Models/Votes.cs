using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feddit_Domain.Models
{
    public class Votes
    {
        public Guid VoteId { get; set; }
        public VoteType Vote { get; set; }
        public DateTime CurrentTime { get; set; }
        public SubFedditPosts PostId { get; set; }
        public Comments CommentId { get; set; }
        public Users UserId { get; set; }
    }
}
