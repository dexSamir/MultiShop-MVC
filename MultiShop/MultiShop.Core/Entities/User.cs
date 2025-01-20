using System;
using Microsoft.AspNetCore.Identity;

namespace MultiShop.Core.Entities;
public class User : IdentityUser
{
	public string Fullname { get; set; }
}

