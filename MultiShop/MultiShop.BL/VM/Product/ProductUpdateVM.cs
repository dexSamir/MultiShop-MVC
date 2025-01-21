using System;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MultiShop.BL.VM.Product;
public class ProductUpdateVM
{
    [Required(ErrorMessage = "Name is required")]
    [MaxLength(64, ErrorMessage = "Name must be less than 64 charachters")]
    public string Name { get; set; }
    public IFormFile? Image { get; set; }
    public string ExistingImageUrl { get; set; }
    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }
    [Range(0.01, double.MaxValue)]
    public decimal? DiscountinuedPrice { get; set; }
    [Range(0, 5.0)]
    public double Rating { get; set; }
    public int RatingCount { get; set; }
    public int? CategoryId { get; set; }
}

