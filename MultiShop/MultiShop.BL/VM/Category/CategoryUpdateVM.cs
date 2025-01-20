using System;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MultiShop.BL.VM.Category;
public class CategoryUpdateVM
{

    [Required(ErrorMessage = "Name is Required"), MaxLength(64, ErrorMessage = "Name must be less than 64 characters")]
    public string Name { get; set; }
    public string? ExistingImageUrl { get; set; }
    public IFormFile? Image { get; set; }
}

