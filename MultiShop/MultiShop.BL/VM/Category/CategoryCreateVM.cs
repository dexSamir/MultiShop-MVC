using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MultiShop.BL.VM.Category;
public class CategoryCreateVM
{
	[Required(ErrorMessage = "Name is Required"), MaxLength(64, ErrorMessage = "Name must be less than 64 characters")]
	public string Name { get; set; }
	[Required]
	public IFormFile? Image { get; set; }
}

