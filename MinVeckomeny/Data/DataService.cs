using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using MinVeckomeny.Models;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using static System.Net.WebRequestMethods;

namespace MinVeckomeny.Data
{
	public class DataService
	{
		private readonly IHttpClientFactory clientFactory;
		private readonly ApplicationContext context;

		public DataService(IHttpClientFactory clientFactory, ApplicationContext context)
		{
			this.clientFactory = clientFactory;
			this.context = context;
		}


		public List<IndexModel> GetAll()
		{

			var result = context
				.Recipes
				.Select(r => new IndexModel
				{
					Id = r.Id,
					Name = r.Name,
					Description = r.Description,
					Image = r.Image,
				})
				.OrderBy(x => Guid.NewGuid())
				.ToList();

			return result;
		}

		public void Add(AddRecipeModel model)
		{

			context.Recipes.Add(new Recipe
			{
				Name = model.Name,
				Description = model.Description,
				Instructions = model.Instructions,
				NoOfPortions = model.NoOfPortions,
				Image = model.FileName,

			});
			context.SaveChanges();

			foreach (var item in model.Hashtags)
			{
				var hashtagId = context.Hashtags.Where(o => o.Name == item).First().Id;
				var recipeId = context.Recipes.Where(o => o.Name == model.Name).First().Id;

				context.Hashtags2Recipes.Add(new Hashtags2Recipes
				{
					HashtagId = hashtagId,
					RecipeId = recipeId
				});
			}
			context.SaveChanges();
		}

		public void AddIngredients2Recipes(List<string> amountsAndIds, string recipeName, bool isEdit)
		{
			int recipeId = context.Recipes.Where(o => o.Name == recipeName).FirstOrDefault().Id;


			foreach (var item in amountsAndIds)
			{
				var data = item.Split(' ');

				if (isEdit)
				{
					var entry = context.Ingredients2Recipes
						.Where(o => o.IngredientId == Convert.ToInt16(data[1]))
						.Where(o => o.RecipeId == recipeId).FirstOrDefault();

					if (entry != null) { context.Ingredients2Recipes.Remove(entry); }
				}

				context.Ingredients2Recipes.Add(new Ingredients2Recipes
				{
					RecipeId = recipeId,
					IngredientAmount = decimal.Parse(data[0]),
					IngredientId = Convert.ToInt16(data[1])
				});
			}

			context.SaveChanges();
		}

		public void RemoveFromConnectionTable(List<int> ingredientIds, int recipeId)
		{
			foreach (var item in ingredientIds)
			{
				var currentItem = context.Ingredients2Recipes
					.Where(o => o.RecipeId == recipeId)
					.Where(o => o.IngredientId == item)
					.Single();

				context.Ingredients2Recipes.Remove(currentItem);
			}
			context.SaveChanges();
		}


		public void Edit(AddRecipeModel newRecipe, int id)
		{
			var oldRecipe = context.Recipes.Find(id);
			oldRecipe.Name = newRecipe.Name;
			oldRecipe.Description = newRecipe.Description;
			oldRecipe.Instructions = newRecipe.Instructions;
			oldRecipe.NoOfPortions = newRecipe.NoOfPortions;
			oldRecipe.Image = newRecipe.FileName;
			context.SaveChanges();


			var oldHashtagIds = context.Hashtags2Recipes.Where(o => o.RecipeId == id).Select(o => o.HashtagId).ToList();

			foreach (var item in newRecipe.Hashtags)
			{
				bool newHashtag = true;
				var hashtagId = context.Hashtags.Where(o => o.Name == item).First().Id;
				var alreadyExistingConnection = context.Hashtags2Recipes.Where(o => o.RecipeId == id).Where(o => o.HashtagId == hashtagId).FirstOrDefault();
				if (alreadyExistingConnection != null)
				{
					newHashtag = false;
				}

				if (newHashtag)
				{
					context.Hashtags2Recipes.Add(new Hashtags2Recipes
					{
						RecipeId = id,
						HashtagId = hashtagId
					});
				}
			}

			bool[] whatOldHashtagToThrow = new bool[oldHashtagIds.Count];
			for (int i = 0; i < oldHashtagIds.Count; i++)
			{
				var oldHashtagName = context.Hashtags.Where(o => o.Id == oldHashtagIds[i]).First().Name;

				foreach (var newHashtagName in newRecipe.Hashtags)
				{
					if (oldHashtagName == newHashtagName)
					{
						whatOldHashtagToThrow[i] = true; break;
					}
				}
			}

			for (int i = 0; i < whatOldHashtagToThrow.Length; i++)
			{
				if (!whatOldHashtagToThrow[i])
				{
					var itemToRemove = context.Hashtags2Recipes.Where(o => o.RecipeId == id).Where(o => o.HashtagId == oldHashtagIds[i]).First();
					context.Hashtags2Recipes.Remove(itemToRemove);
				}
			}


			context.SaveChanges();

		}

		public Recipe GetRecipeByID(int id)
		{
			return context.Recipes.Find(id);
		}

		public DetailsModel? GetDetailsModelByID(int id)
		{

			return context
				.Recipes
				.Where(r => r.Id == id)
				.Select(r => new DetailsModel
				{
					Id = r.Id,
					Name = r.Name,
					Description = r.Description,
					Image = r.Image,
					Instructions = r.Instructions,
					NoOfPortions = r.NoOfPortions,
					Hashtags = GetHashtagsByRecipe(id)
				})
				.SingleOrDefault();

		}
		public AddRecipeModel? GetAddRecipeModelByID(int id)
		{
			return context
				.Recipes
				.Where(r => r.Id == id)
				.Select(r => new AddRecipeModel
				{
					Id = r.Id,
					Name = r.Name,
					Description = r.Description,
					Instructions = r.Instructions,
					NoOfPortions = r.NoOfPortions,
					FileName = r.Image
				})
				.SingleOrDefault();
		}


		internal void DeleteRecipe(int id)
		{

			context.Recipes.Remove(GetRecipeByID(id));

			foreach (var item in GetIngredients2RecipesByID(id))
			{
				context.Ingredients2Recipes.Remove(item);
			}
			foreach (var item2 in GetHashtags2RecipesByID(id))
			{
				context.Hashtags2Recipes.Remove(item2);
			}

			context.SaveChanges();
		}

		internal List<Ingredients2Recipes> GetIngredients2RecipesByID(int id)
		{
			return context.Ingredients2Recipes.Where(o => o.RecipeId == id).ToList();
		}
		internal List<Hashtags2Recipes> GetHashtags2RecipesByID(int id)
		{
			return context.Hashtags2Recipes.Where(o => o.RecipeId == id).ToList();
		}



		public async Task<ExtraPrisModel> GetExtraPrices()
		{
			ExtraPrisModel model = new ExtraPrisModel();

			var client = clientFactory.CreateClient();
			string baseAddress = "https://www.hemkop.se/search/campaigns/mix?q=4349&type=LOYALTY&size=70&onlineSize=0&offlineSize=0&avoid";
			string cache = Random.Shared.Next().ToString();

			try
			{
				model = await client.GetFromJsonAsync<ExtraPrisModel>(baseAddress + "Cache=" + cache);
				model.ErrorMessage = null;
			}
			catch (Exception ex)
			{
				model.ErrorMessage = ex.Message;
			}

			model.results = model.results.Where(o => o.potentialPromotions[0].textLabelGenerated != "Alltid bra pris!").ToArray();

			return model;
		}

		public List<IngredientModel> GetAllIngredientModels()
		{
			return context
				.Ingredients
				.OrderBy(o => o.Kategori)
				.Select(r => new IngredientModel
				{
					Id = r.Id,
					Name = r.Name,
					Kategori = r.Kategori,
					Enhet = r.Enhet,
					Temp = GetTemp(r.Enhet)
				})
				.ToList();
		}

		public List<string> GetAllIngredientNames()
		{
			return context.Ingredients.Select(o => o.Name).ToList();
		}

		public List<string> GetAllHashtagNames()
		{
			return context.Hashtags.Select(o => o.Name).ToList();
		}

		public List<IngredientModel> GetIngredientModelsByRecipe(int id)
		{
			var ingredientIdsAndAmounts = context.Ingredients2Recipes
				.Where(o => o.RecipeId == id).Select(o => o.IngredientId + " " + o.IngredientAmount).ToArray();

			List<IngredientModel> result = new();

			foreach (var item in ingredientIdsAndAmounts)
			{
				var data = item.Split(' ');

				result.Add(context.Ingredients.Where(o => o.Id == Convert.ToInt16(data[0]))
					.Select(o => new IngredientModel
					{
						Id = o.Id,
						Name = o.Name,
						Kategori = o.Kategori,
						Enhet = o.Enhet,
						Temp = decimal.Parse(data[1]),
						HarHemma = o.HarHemma,
						Discounted = o.Discounted
					}).Single());;
			}

			result = result.OrderBy(o => o.Kategori).ToList();

			return result;
		}

		private static int GetTemp(string enhet)
		{
			var result = enhet == "g" ? 400 : 1; return result;
		}

		public void AddIngredient(IngredientModel model)
		{
			context.Ingredients.Add(new Ingredient
			{
				Name = model.Name,
				Kategori = model.Kategori,
				Popularitet = 0,
				Enhet = model.Enhet,
				HarHemma = model.HarHemma
			});
			context.SaveChanges();

		}

		public IndexModel GetIndexModel(int id)
		{
			return context.Recipes
				.Where(o => o.Id == id)
				.Select(o => new IndexModel
				{
					Id = o.Id,
					Name = o.Name,
					Description = o.Description,
					Image = o.Image,
					NoOfPortions = o.NoOfPortions
				}).FirstOrDefault();
		}


		public List<string> GetAllHashtags()
		{
			return context.Hashtags.Select(o => o.Name).ToList();
		}

		public List<string> GetHashtagsByRecipe(int id)
		{
			var hashtagIds = context.Hashtags2Recipes.Where(o => o.RecipeId == id).Select(o => o.HashtagId).ToList();
			List<string> hashtags = new();

			foreach (var item in hashtagIds)
			{
				hashtags.Add(context.Hashtags.Where(o => o.Id == item).First().Name);
			}

			return hashtags;
		}

		public Dictionary<int, List<string>> GetAllHashTagsByRecipes()
		{
			Dictionary<int, List<string>> result = new();

			var allRecipeIds = context.Recipes.Select(o => o.Id).ToList();

			foreach (var item in allRecipeIds)
			{
				var hashtagsInThisRecipe = GetHashtagsByRecipe(item);

				result.Add(item, hashtagsInThisRecipe);
			}
			return result;
		}



		public Dictionary<IndexModel, List<string>> GettAllIngredientsByRecipes()
		{
			Dictionary<IndexModel, List<string>> result = new();

			var allRecipeIntexModels = GetAll();

			foreach (var item in allRecipeIntexModels)
			{
				List<string> ingredientsInThisRecipe = new();
				var ingredientIdsInThisRecipe = context.Ingredients2Recipes.Where(o => o.RecipeId == item.Id).ToList();
				foreach (var ingredient in ingredientIdsInThisRecipe)
				{
					var ingredientNameToAdd = context.Ingredients.Where(o => o.Id == ingredient.IngredientId).First().Name;
					ingredientsInThisRecipe.Add(ingredientNameToAdd);
				}

				result.Add(item, ingredientsInThisRecipe);
			}

			return result;
		}

		public async Task GetWeekIngrediets()
		{
			var ingredients = context.Ingredients.ToList();

			foreach (var item in ingredients)
			{
				item.Discounted = false;
			}
			context.SaveChanges();

			////var allIngredients = GetAllIngredientNames();
			var veckansKatalog = await GetExtraPrices();


			//TESTKATALOG//
			#region
			//var veckansKatalog = new ExtraPrisModel
			//{ 
			//	results = new Result[]
			//	{
			//		new Result
			//		{
			//			name = "Kokosmjölk",
			//			googleAnalyticsCategory = "skafferi|asien|kokosmjolk"
			//		},
			//		new Result
			//		{
			//			name = "Fläsklägg",
			//			googleAnalyticsCategory = "kott-och-fagel|kott|flaskkott"
			//		},
			//		new Result
			//		{
			//			name = "Smögåslimpa",
			//			googleAnalyticsCategory = "brod-och-kakor|brod|matbrod"
			//		}
			//	}
			//};

			//var allIngredients = new List<string> { "Mjölk", "Ägg", "Smör"};
			//////////TESTKATALOG////////////////
# endregion

			foreach (var ingredient in ingredients)
			{
				var splitIngredient = ingredient.Name.Split(' ');
				var varuKategori = ingredient.Kategori.Split(".");

				foreach (var item in veckansKatalog.results)
				{
					var googleCategory = item.googleAnalyticsCategory.Split("|");

					if (item.name.ToLower().Contains(splitIngredient[0].ToLower())
						&& googleCategory[0] == varuKategori[varuKategori.Count() - 1])
					{
						ingredient.Discounted = true;
					}

					var ingredientNameReplaced = ingredient.Name.Replace('ö', 'o');
					ingredientNameReplaced = ingredientNameReplaced.Replace('å', 'a');
					ingredientNameReplaced = ingredientNameReplaced.Replace('ä', 'a');
					if (googleCategory[googleCategory.Length - 1].Contains(ingredientNameReplaced.ToLower()))
					{
						//Problem: många kategorier innehåller bokstavskombinationen "ris"
						if (ingredientNameReplaced.ToLower() == "ris")
						{
							ingredient.Discounted = false;
						}
						//Problem: kategorin för zuccini och aubergine heter "zuccini-och-aubergine", varför
						//både zucchini och aubergine markeras som rabbatterade om en av dem är det
						else if (ingredientNameReplaced.ToLower() == "zucchini" || ingredientNameReplaced.ToLower() == "aubergine")
						{

						}
						else
						{
							ingredient.Discounted = true;
							context.SaveChanges();
						}
					}
				}
			}


			var discounted = context.Ingredients.Where(o => o.Discounted == true).ToList();
		}


		public Dictionary<int, int> GetNoOfDiscountsAndRecipeId()
		{
			Dictionary<int, int> recipesAndNoOfDisc = new Dictionary<int, int>();

			var discountedIngredientIds = context.Ingredients
				.Where(o => o.Discounted == true).Select(o => o.Id).ToList();

			var allRecipeIds = context.Recipes.Select(o => o.Id).ToList();

			foreach (var recipeId in allRecipeIds)
			{
				int noOfDiscountedIngredients = 0;
				
				foreach (var ingredientId in discountedIngredientIds)
				{
					noOfDiscountedIngredients += context.Ingredients2Recipes.Where(o => o.RecipeId == recipeId)
						.Where(o => o.IngredientId == ingredientId).Count();
				}

				recipesAndNoOfDisc.Add(recipeId, noOfDiscountedIngredients);
			}

			return recipesAndNoOfDisc;
		}

	}

}