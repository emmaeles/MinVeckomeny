namespace MinVeckomeny.Models
{

	public class ExtraPrisModel
	{
		public Result[] results { get; set; }
        public string ErrorMessage { get; set; }
    }

	public class Result
	{
		public Potentialpromotion[] potentialPromotions { get; set; }
		public float priceValue { get; set; }
		public string price { get; set; }
		public Image image { get; set; }
		public float ranking { get; set; }
		public string depositPrice { get; set; }
		public float? averageWeight { get; set; }
		public string comparePrice { get; set; }
		public string comparePriceUnit { get; set; }
		public bool isDrugProduct { get; set; }
		public float solrSearchScore { get; set; }
		public object energyDeclaration { get; set; }
		public bool newsSplashProduct { get; set; }
		public bool notAllowedB2b { get; set; }
		public bool notAllowedAnonymous { get; set; }
		public string productLine2 { get; set; }
		public string pickupProductLine2 { get; set; }
		public string priceUnit { get; set; }
		public string priceNoUnit { get; set; }
		public float savingsAmount { get; set; }
		public string googleAnalyticsCategory { get; set; }
		public bool gdprTrackingIncompliant { get; set; }
		public bool showGoodPriceIcon { get; set; }
		public bool nicotineMedicalProduct { get; set; }
		public bool tobaccoFreeNicotineProduct { get; set; }
		public bool nonEkoProduct { get; set; }
		public bool tobaccoProduct { get; set; }
		public string[] labels { get; set; }
		public string manufacturer { get; set; }
		public float incrementValue { get; set; }
		public Productbaskettype productBasketType { get; set; }
		public Thumbnail thumbnail { get; set; }
		public bool outOfStock { get; set; }
		public bool online { get; set; }
		public string displayVolume { get; set; }
		public bool addToCartDisabled { get; set; }
		public string code { get; set; }
		public string name { get; set; }
	}

	public class Image
	{
		public string imageType { get; set; }
		public string format { get; set; }
		public string url { get; set; }
		public object altText { get; set; }
		public object galleryIndex { get; set; }
		public object width { get; set; }
	}

	public class Productbaskettype
	{
		public string code { get; set; }
		public string type { get; set; }
	}

	public class Thumbnail
	{
		public string imageType { get; set; }
		public string format { get; set; }
		public string url { get; set; }
		public object altText { get; set; }
		public object galleryIndex { get; set; }
		public object width { get; set; }
	}

	public class Potentialpromotion
	{
		public float? threshold { get; set; }
		public bool applied { get; set; }
		public Lowesthistoricalprice lowestHistoricalPrice { get; set; }
		public string campaignType { get; set; }
		public Promotiontheme promotionTheme { get; set; }
		public string textLabelGenerated { get; set; }
		public object textLabelManual { get; set; }
		public Price price { get; set; }
		public string[] productCodes { get; set; }
		public string promotionType { get; set; }
		public string conditionLabel { get; set; }
		public string rewardLabel { get; set; }
		public object textLabel { get; set; }
		public string redeemLimitLabel { get; set; }
		public object cartLabel { get; set; }
		public object validUntil { get; set; }
		public object timesUsed { get; set; }
		public int? qualifyingCount { get; set; }
		public string splashTitleText { get; set; }
		public bool realMixAndMatch { get; set; }
		public string mainProductCode { get; set; }
		public string comparePrice { get; set; }
		public string conditionLabelFormatted { get; set; }
		public object promotionRedeemLimit { get; set; }
		public object promotionPercentage { get; set; }
		public string cartLabelFormatted { get; set; }
		public string code { get; set; }
		public int priority { get; set; }
	}

	public class Lowesthistoricalprice
	{
		public string currencyIso { get; set; }
		public float value { get; set; }
		public string priceType { get; set; }
		public string formattedValue { get; set; }
		public object minQuantity { get; set; }
		public object maxQuantity { get; set; }
	}

	public class Promotiontheme
	{
		public string code { get; set; }
		public object visible { get; set; }
	}

	public class Price
	{
		public string currencyIso { get; set; }
		public float value { get; set; }
		public string priceType { get; set; }
		public string formattedValue { get; set; }
		public object minQuantity { get; set; }
		public object maxQuantity { get; set; }
	}


}
