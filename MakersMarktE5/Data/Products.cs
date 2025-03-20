using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

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

        public List<Category> Categories { get; set; } = [];
		public List<ProductCategory> ProductCategories { get; set; } = [];
		public List<MaterialProduct> MaterialProducts { get; } = [];
		public List<Material> Materials { get; } = [];
        public string ProductionTime { get; set; }
        public string Complexity { get; set; }
        public string Sustainability { get; set; }

        [ForeignKey("UniqueProperty")]
        public int? PropertyId { get; set; }
        public UniqueProperty? UniqueProperty { get; set; }

        public ICollection<Sale> Sales { get; set; }
        public ICollection<Review> Reviews { get; set; }

		public int? CreatorId { get; set; }
		public User? Creator { get; set; }

		[DefaultValue(false)]
		public bool IsVerified { get; set; } = false;
	}
}
