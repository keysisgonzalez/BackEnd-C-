using Sabio.Models.Domain;
using Sabio.Models.Domain.ComputerImages;
using Sabio.Models.Domain.Monitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Sabio.Models.Domain.Computers
{
    public class ComputerV3
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public string Storage { get; set; }
        public string PrimaryImageUrl { get; set; }
        public int Year { get; set; }
        public ComputerImage Image { get; set; }
        public List<Monitor> Monitors { get; set; }
        public bool IsUsed { get; set; }
        public int UserId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
       
      

    }
}
