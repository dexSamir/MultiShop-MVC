using System;
using MultiShop.Core.Entities.Base;

namespace MultiShop.Core.Entities;
public class Category : BaseEntity
{
	public string Name { get; set; }
	public string ImageUrl { get; set; }
	public ICollection<Product> Products { get; set; } = new HashSet<Product>(); 
}

