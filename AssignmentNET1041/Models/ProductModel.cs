﻿using AssignmentNET1041.Repository.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssignmentNET1041.Models
{
	public class ProductModel
	{
		[Key]
		public long Id { get; set; }

		[Required, MinLength(4, ErrorMessage = "Yêu cầu nhập Tên Sản phẩm")]
		public string Name { get; set; }

		public string Slug { get; set; }

		[Required, MinLength(4, ErrorMessage = "Yêu cầu nhập Mô tả Sản phẩm")]
		public string Description { get; set; }

		[Required]
		public decimal Price { get; set; }

		public int BrandId { get; set; }
		public int Quantity { get; set; }
		public int Sold { get; set; }

		public int CategoryId { get; set; }

		public CategoryModel Category { get; set; }

		public BrandModel Brand { get; set; }
		public string Image { get; set; } 
		[NotMapped]
		[FileExtension]
		public IFormFile? ImageUpload { get; set; }
	}
}
