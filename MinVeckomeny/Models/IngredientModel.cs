namespace MinVeckomeny.Models
{
	public class IngredientModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Kategori { get; set; }
		public string? Enhet { get; set; }
		public bool HarHemma { get; set; }
        public decimal Temp { get; set; }
        public bool Discounted { get; set; }
    }
}
