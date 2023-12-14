using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Feddit_Domain.Models
{
    public class SubFedditPosts
    {
        public Guid PostId { get; set; }
        [MaxLength(100)]
        public string PostTitle { get; set; }
        [MaxLength(100)]
        public string PostContent { get; set; }
        public string PostPic { get; set; }
        public DateTime CurrentTime { get; set; }


    }
}
