using System;
namespace MultiShop.Core.Entities.Base;
public abstract class BaseEntity
{
	public int Id { get; set; }
	public DateTime CreatedTime { get; set; }
	public DateTime? UpdateTime { get; set; }
	public bool IsDeleted { get; set; }
}

