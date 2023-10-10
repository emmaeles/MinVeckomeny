namespace MinVeckomeny.Models
{
	public class DetailsModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Instructions { get; set; }
		public string Image { get; set; }
		public decimal NoOfPortions { get; set; }
        public List<string> Hashtags { get; set; }
    }
}
