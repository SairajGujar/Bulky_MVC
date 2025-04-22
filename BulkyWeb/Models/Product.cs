using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BulkyWeb.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        [Range(0, 1000), Display(Name = "List Price")]
        public double ListPrice { get; set; }

        [Required]
        [Range(0,1000), Display(Name ="Price for 0-50")]
        public double Price { get; set; }

        [Required]
        [Range(0, 1000), Display(Name = "Price for 50+")]
        public double Price50 { get; set; }

        [Required]
        [Range(0, 1000), Display(Name = "Price for 100+")]
        public double Price100 { get; set; }

        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }
        [ValidateNever]
        public string ImageURL { get; set; }


    }
}
