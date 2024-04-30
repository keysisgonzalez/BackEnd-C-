using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Cars
{
    public class CarAddRequest
    {   
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public bool IsUsed { get; set; }
        //public int ManufacturedId { get; set; }
       
    }
}
