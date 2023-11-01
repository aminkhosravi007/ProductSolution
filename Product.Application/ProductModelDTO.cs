﻿using Product.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Application
{
    public class ProductModelDTO
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string? Name { get; set; }
        public DateTime ProductDate { get; set; }
        public bool? IsAvailable { get; set; }
        public int ManufactureId { get; set; }
        public Manufacture? Manufacture { get; set; }
    }
}
