using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Product.Domain
{
    public class ProductModel
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        public string? Name { get; set; }
        public string ProductDate { get; set; } = DateTime.Now.ToShortDateString();
        public bool? IsAvailable { get; set; }
        public string IssuedAdminToken { get; set; }
        public int ManufactureId { get; set; }
        [ForeignKey("ManufactureId")]
        public Manufacture? Manufacture { get; set; }
    }
}
