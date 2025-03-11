using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakersMarktE5.Data
{
    public class Type
    {
        [Key]
        public int TypeId { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
