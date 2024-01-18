using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feddit_Domain.Models
{
    public class SubFeddits
    {
        public Guid SubFedditId { get; set; }
        [MaxLength(75)]
        public string SubFedditName { get; set; }
        public DateTime TimeCreated { get; set; }
    }   
}
