using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakersMarktE5.Data
{
    public class Sale
    {
        public enum StatusType
		{
            Ordered = 1,
            InProgress = 2,
            Send = 3
        }
            
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Amount { get; set; }
        public string? Description { get; set; }
        public StatusType Status { get; set; }
    }
}
