using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Domain
{
    public class Manufacture
    {
        [Key]
        public int Id { get; set; }
        [Phone]
        public string? ManufacturePhone { get; set; }
        [EmailAddress]
        public string? ManufactureEmail { get; set; }
    }
}
