using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace Recipease.Models;

public class Recipe
{
    private static readonly string[] value = { string.Empty };

    public int Id { get; set; }
    [Required]
    public string Title { get; set; } = string.Empty;

    public string Image {get; set; } = "https://static.wixstatic.com/media/bf242e_6133b4ae6a104cc2b50d70179f35efea~mv2.jpg/v1/fill/w_500,h_376,al_c,lg_1,q_80,enc_auto/food-placeholder.jpg";

    [Display(Name = "Date Added")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [DataType(DataType.Date)]
    public DateTime DateAdded { get; set; }

    [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
    
    public string? Cuisine { get; set; } = string.Empty;


    [DataType(DataType.Currency)]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }

    public int Rating { get; set; }

    public string Summary { get; set; } = string.Empty;
    
    
}