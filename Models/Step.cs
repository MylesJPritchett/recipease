namespace Recipease.Models
{
    public class Step
    {
        public int Number { get; set; }
        public string Directions { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public List<Equipment> Equipment { get; set; }
        public Length Length { get; set; }
    }
}
