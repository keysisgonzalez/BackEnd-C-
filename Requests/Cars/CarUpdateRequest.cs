using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Cars
{
    public class CarUpdateRequest : CarAddRequest
    {
        public int Id { get; set; }
    }
}
