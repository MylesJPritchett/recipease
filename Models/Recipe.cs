using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recipease.Models;

public class Recipe
{
    private static readonly string[] value = { string.Empty };

    public int Id { get; set; }
    [StringLength(60, MinimumLength = 3)]
    [Required]
    public string Title { get; set; } = string.Empty;

    [Display(Name = "Date Added")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [DataType(DataType.Date)]
    public DateTime DateAdded { get; set; }

    [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
    [StringLength(30)]
    public string? Cuisine { get; set; } = string.Empty;


    [Range(1, 100)]
    [DataType(DataType.Currency)]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }


   
    public int Rating { get; set; }
    
    
}