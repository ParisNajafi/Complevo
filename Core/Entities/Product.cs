using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Product
    {
        [Key]
        public long Id { get; set; }
        [MaxLength(200)]
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public int Quantity { get; set; }
    }
}
