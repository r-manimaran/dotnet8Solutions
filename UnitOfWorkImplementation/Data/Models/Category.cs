﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Category: BaseModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]       
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}