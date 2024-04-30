using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Computers
{
    public class ComputerAddRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Model { get; set; }

        [Required]
        public string Storage { get; set; }

        [Required]
        public string PrimaryImageUrl { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public bool IsUsed { get; set; }
    }
}
