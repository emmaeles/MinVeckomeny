using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MinVeckomeny.Models
{
	public class AddRecipeModel
	{
		public int Id { get; set; }

		[Required]
		[StringLength(40)]
		public string Name { get; set; }

		public string? Description { get; set; }

		public string? Instructions { get; set; }

		public string? FileName { get; set; }


		[Range(0, 100)]
		public decimal NoOfPortions { get; set; }

        public List<string> Hashtags { get; set; }
    }
}
