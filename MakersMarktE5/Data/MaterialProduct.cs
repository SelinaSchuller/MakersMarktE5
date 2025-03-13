using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakersMarktE5.Data
{
    public class MaterialProduct
    {
	
		[ForeignKey("Material")]
		public int MaterialId { get; set; }
		public Material Material { get; set; } = null!;

		[ForeignKey("Product")]
		public int ProductId { get; set; }
		public Product Product { get; set; } = null!;

	}
}
