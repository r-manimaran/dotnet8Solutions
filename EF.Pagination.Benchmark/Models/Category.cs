﻿using System.ComponentModel.DataAnnotations;

namespace EF.Pagination.Benchmark;

public class Category
{
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
