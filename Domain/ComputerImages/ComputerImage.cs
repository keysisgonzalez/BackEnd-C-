﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.ComputerImages
{
    public class ComputerImage
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int TypeId { get; set; }

    }
}
