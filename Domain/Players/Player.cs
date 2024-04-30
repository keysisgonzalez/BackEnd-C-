using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.Players
{
    public class Player
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int StatusId { get; set; }
        public string PrimaryImageUrl { get; set; }        
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public int UserId { get; set; }
    }
}
