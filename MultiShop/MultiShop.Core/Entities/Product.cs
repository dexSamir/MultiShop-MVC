using System;
using MultiShop.Core.Entities.Base;

namespace MultiShop.Core.Entities;
public class Product : BaseEntity
{
	public string Name { get; set; }
    public string? ImageUrl { get; set; }
    public decimal Price { get; set; }
    public decimal? DiscountinuedPrice { get; set; }
    public double Rating { get; set; }
    public int RatingCount { get; set; }
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }
}

