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
        [Required]
        [RegularExpression("^09(1[0-9]|3[1-9]|2[1-9])-?[0-9]{3}-?[0-9]{4}", ErrorMessage = "شماره همراه معتبر نیست")]
        public string? ManufacturePhone { get; set; }
        [EmailAddress]
        public string? ManufactureEmail { get; set; }
    }
}
