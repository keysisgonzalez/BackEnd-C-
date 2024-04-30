﻿using Sabio.Models.Domain.Images;
using Sabio.Models.Domain.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.Customers
{
    public class CustomerV3
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Bio { get; set; }
        public string Summary { get; set; }
        public string Headline { get; set; }
        public string Slug { get; set; }
        public int StatusId { get; set; }
        public int PrimaryImageId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public int UserId { get; set; }
        public ImageV3 PrimaryImage { get; set; }
        public List<SkillV3> Skills { get; set; }
    }
}
