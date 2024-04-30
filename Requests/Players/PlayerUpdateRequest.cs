using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Players
{
    public class PlayerUpdateRequest : PlayerAddRequest
    {
        public int Id { get; set; }
    }
}
