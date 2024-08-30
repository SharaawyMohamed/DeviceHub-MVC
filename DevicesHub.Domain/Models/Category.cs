﻿

using System.ComponentModel.DataAnnotations;

namespace DevicesHub.Domain.Models
{
	public class Category
	{
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		public string? Description { get; set; }
		public DateTime CreatedDate { get; set; } = DateTime.Now;
	}
}
