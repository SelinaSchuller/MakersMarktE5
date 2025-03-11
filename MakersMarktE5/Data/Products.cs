using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakersMarktE5.Data
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [ForeignKey("Type")]
        public int TypeId { get; set; }
        public Type Type { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public string MaterialUsage { get; set; }
        public string ProductionTime { get; set; }
        public string Complexity { get; set; }
        public string Sustainability { get; set; }

        [ForeignKey("UniqueProperty")]
        public int PropertyId { get; set; }
        public UniqueProperty UniqueProperty { get; set; }

        public ICollection<Sale> Sales { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}
