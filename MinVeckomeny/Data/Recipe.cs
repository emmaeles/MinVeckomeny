namespace MinVeckomeny.Data
{
	public class Recipe
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string? Description { get; set; }
		public string? Instructions { get; set; }
		public string? Image { get; set; }
		public decimal NoOfPortions { get; set; }
		public int PricePerPortion { get; set; }
		public List<Ingredients2Recipes> IngredientsInThisRecipe { get; set; }

	}

	public class Ingredient
	{
		public int Id { get; set; }
		public string Name { get; set; }
        public string Kategori { get; set; }
		public string? Enhet { get; set; }
        public int Popularitet { get; set; }
        public bool HarHemma { get; set; }

        public List<Ingredients2Recipes> RecipesWithThisIngredient { get; set; }

	}

	public class Ingredients2Recipes
	{
		public int Id { get; set; }
		public int RecipeId { get; set; }
		public int IngredientId { get; set; }
		public decimal? IngredientAmount { get; set; }
		public Recipe Recipe { get; set; }
		public Ingredient Ingredient { get; set; }
	}


	public class Hashtag
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}

	public class Hashtags2Recipes
	{
		public int Id { get; set; }
		public int HashtagId { get; set; }
		public int RecipeId { get; set; }
		public Hashtag Hashtag { get; set; }
		public Recipe Recipe { get; set; }
	}
}
