using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Review: BaseModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(500)]
        public string ReviewText { get; set; }
        public int Rating { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        [ForeignKey("ProductId")]        
        public Product Product { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

    }
}
