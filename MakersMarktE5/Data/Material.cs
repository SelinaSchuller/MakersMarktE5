using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakersMarktE5.Data
{
    public class Material
    {
		[Key]
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		public List<MaterialProduct> MaterialProducts { get; } = [];
		public List<Product> Products { get; } = [];
	}
}
