﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Computers
{
    public class ComputerUpdateRequest : ComputerAddRequest
    {
        public int Id { get; set; }
    }
}
