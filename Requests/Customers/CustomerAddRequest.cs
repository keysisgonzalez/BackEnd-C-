using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Customers
{
    public class CustomerAddRequest
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Bio { get; set; }

        [Required]
        public string Summary { get; set; }

        [Required]
        public string Headline { get; set; }

        [Required]
        public string Slug { get; set; }

        [Required]
        public int StatusId { get; set; }

        [Required]
        public int PrimaryImageId { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        public DateTime DateModified { get; set; }        
       
    }
}
