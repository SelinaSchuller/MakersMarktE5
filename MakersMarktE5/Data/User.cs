using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace MakersMarktE5.Data
{
    public class User
    {
        public static User LoggedInUser { get; set; }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }

        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public Role Role { get; set; }

        public ICollection<Sale> Sales { get; set; }
        public ICollection<Review> Reviews { get; set; }

		[DefaultValue(false)]
		public bool IsVerified { get; set; } = false;
	}
}


