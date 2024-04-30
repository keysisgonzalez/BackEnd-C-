using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.Computers
{
    public class Computer    
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public string Storage { get; set; }
        public string PrimaryImageUrl { get; set;}
        public int Year { get; set; }
        public bool IsUsed { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

    }
}
