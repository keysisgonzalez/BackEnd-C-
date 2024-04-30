using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Players
{
    public class PlayerAddRequest
    {
        public string Title { get; set; }
        public int StatusId { get; set; }
        public string PrimaryImageUrl { get; set; }        
        public int UserId { get; set; }
    }
}
