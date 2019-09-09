using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class XmlSettings : ELXmlConfig<XmlSettings>
{
	public struct RateItSettings
	{
		public string iosAppId;

		public string androidAppId;

		public int coins;

		public int firstTrigger;

		public int nextTrigger;
	}

	public struct ShareSettings
	{
		public int coins;

		public int coinsNumber;

		public int firstTrigger;

		public int nextTrigger;
	}

	public struct UrlSettings
	{
		public string privacy;

		public string about;
	}

	public struct CoinsConfig
	{
		public int initial;

		public int pack;

		public int perfect;

		public int ladybug;
	}

	public struct CoinsInLevelsConfig
	{
		public int packs;

		public int bTierEachLevel;

		public int minCoinsA;

		public int maxCoinsA;

		public int minCoinsB;

		public int maxCoinsB;
	}

	public struct ExtraWordsConfig
	{
		public int words;

		public int coins;
	}

	public struct HintConfig
	{
		public LevelHint type;

		public int coins;

		public int initial;

		public int level;
	}

	public class IapConfig
	{
		public ProductType type;

		public int coins;

		public int goodStart;

		public int startFinish;

		public int expose;

		public int tip;

		public int set;

		public bool noads;

		public int icon;

		public string iap;
	}

	public struct InterstitialAdConfig
	{
		public int pack;

		public int level;
	}

	public struct RewardedVideoAdConfig
	{
		public float timeSpan;

		public int coins;
	}

	public struct NotificationsConfig
	{
		public int pack;

		public int level;
	}

	public struct DailyPuzzleConfig
	{
		public float timeSpan;
	}

	public struct TutorialConfig
	{
		public TutorialType type;

		public int level;

		public float delay;

		public Vector2 hand;

		public Vector2 message;

		public bool isPerfect;

		public bool isDictionary;

		public bool isShop;

		public bool isExtraWord;
	}

	public RateItSettings rateItConfig;

	public ShareSettings shareConfig;

	public UrlSettings urlConfig;

	public CoinsConfig coinsConfig;

	public List<CoinsInLevelsConfig> coinsInLevelsConfig = new List<CoinsInLevelsConfig>();

	public List<ExtraWordsConfig> extraWordsConfigs = new List<ExtraWordsConfig>();

	public List<HintConfig> hintsConfig = new List<HintConfig>();

	public List<IapConfig> iapConfig = new List<IapConfig>();

	public List<InterstitialAdConfig> interstitialAdsConfig = new List<InterstitialAdConfig>();

	public RewardedVideoAdConfig rewardedVideoAdConfig;

	public NotificationsConfig notificationsConfig;

	public DailyPuzzleConfig dailyPuzzleConfig;

	public List<TutorialConfig> tutorialsConfig = new List<TutorialConfig>();

	public override void LoadSettingsCompleted()
	{
		rateItConfig.iosAppId = StringXPathed("/application/rate_it/@ios_app_id");
		rateItConfig.androidAppId = StringXPathed("/application/rate_it/@android_app_id");
		rateItConfig.coins = IntXPathed("/application/rate_it/@coins");
		rateItConfig.firstTrigger = IntXPathed("/application/rate_it/@first_trigger");
		rateItConfig.nextTrigger = IntXPathed("/application/rate_it/@next_trigger");
		shareConfig.coins = IntXPathed("/application/share/@coins");
		shareConfig.coinsNumber = IntXPathed("/application/share/@coins_num");
		shareConfig.firstTrigger = IntXPathed("/application/share/@first_trigger");
		shareConfig.nextTrigger = IntXPathed("/application/share/@next_trigger");
		urlConfig.privacy = StringXPathed("/application/url[@type='privacy']/@value");
		urlConfig.about = StringXPathed("/application/url[@type='about']/@value");
		coinsConfig.initial = IntXPathed("/application/coins/@initial");
		coinsConfig.pack = IntXPathed("/application/coins/@pack");
		coinsConfig.perfect = IntXPathed("/application/coins/@perfect");
		coinsConfig.ladybug = IntXPathed("/application/coins/@ladybug");
		int num = ElementCount("/application/coins_in_levels");
		for (int i = 0; i < num; i++)
		{
			CoinsInLevelsConfig item = default(CoinsInLevelsConfig);
			item.packs = IntXPathed($"/application/coins_in_levels[{i + 1}]/@packs");
			item.bTierEachLevel = IntXPathed($"/application/coins_in_levels[{i + 1}]/@b_tier_each_level");
			item.minCoinsA = IntXPathed($"/application/coins_in_levels[{i + 1}]/@min_coins_a");
			item.maxCoinsA = IntXPathed($"/application/coins_in_levels[{i + 1}]/@max_coins_a");
			item.minCoinsB = IntXPathed($"/application/coins_in_levels[{i + 1}]/@min_coins_b");
			item.maxCoinsB = IntXPathed($"/application/coins_in_levels[{i + 1}]/@max_coins_b");
			coinsInLevelsConfig.Add(item);
		}
		num = ElementCount("/application/extra_words");
		for (int j = 0; j < num; j++)
		{
			ExtraWordsConfig item2 = default(ExtraWordsConfig);
			item2.words = IntXPathed($"/application/extra_words[{j + 1}]/@words");
			item2.coins = IntXPathed($"/application/extra_words[{j + 1}]/@coins");
			extraWordsConfigs.Add(item2);
		}
		num = ElementCount("/application/hint");
		for (int k = 0; k < num; k++)
		{
			HintConfig item3 = default(HintConfig);
			string a = StringXPathed($"/application/hint[{k + 1}]/@type");
			item3.type = (string.Equals(a, "good_start") ? LevelHint.GoodStart : (string.Equals(a, "start_and_finish") ? LevelHint.StartAndFinish : (string.Equals(a, "expose") ? LevelHint.Expose : (string.Equals(a, "tip") ? LevelHint.Tip : LevelHint.None))));
			item3.coins = IntXPathed($"/application/hint[{k + 1}]/@coins");
			item3.initial = IntXPathed($"/application/hint[{k + 1}]/@initial");
			item3.level = IntXPathed($"/application/hint[{k + 1}]/@level");
			hintsConfig.Add(item3);
		}
		ELSingleton<HintManager>.Instance.Setup();
		num = ElementCount("/application/iap");
		for (int l = 0; l < num; l++)
		{
			IapConfig iapConfig = new IapConfig();
			string a2 = StringXPathed($"/application/iap[{l + 1}]/@type");
			iapConfig.type = ((!string.Equals(a2, "consumable")) ? ProductType.NonConsumable : ProductType.Consumable);
			iapConfig.iap = StringXPathed($"/application/iap[{l + 1}]/@iap");
			iapConfig.set = IntXPathed($"/application/iap[{l + 1}]/@set");
			iapConfig.coins = IntXPathed($"/application/iap[{l + 1}]/@coins");
			iapConfig.goodStart = IntXPathed($"/application/iap[{l + 1}]/@good_start");
			iapConfig.startFinish = IntXPathed($"/application/iap[{l + 1}]/@start_finish");
			iapConfig.expose = IntXPathed($"/application/iap[{l + 1}]/@expose");
			iapConfig.tip = IntXPathed($"/application/iap[{l + 1}]/@tip");
			iapConfig.noads = BoolXPathed($"/application/iap[{l + 1}]/@noads");
			iapConfig.icon = ((ElementXPathed($"/application/iap[{l + 1}]/@icon") != null) ? IntXPathed($"/application/iap[{l + 1}]/@icon") : 0);
			this.iapConfig.Add(iapConfig);
		}
		ELSingleton<IapManager>.Instance.InitializePurchasing();
		num = ElementCount("/application/interstitial_ad");
		for (int m = 0; m < num; m++)
		{
			InterstitialAdConfig item4 = default(InterstitialAdConfig);
			item4.pack = IntXPathed($"/application/interstitial_ad[{m + 1}]/@pack");
			item4.level = IntXPathed($"/application/interstitial_ad[{m + 1}]/@level");
			interstitialAdsConfig.Add(item4);
		}
		rewardedVideoAdConfig.timeSpan = FloatXPathed("/application/rewarded_video_ad/@time_span");
		rewardedVideoAdConfig.coins = IntXPathed("/application/rewarded_video_ad/@coins");
		notificationsConfig.pack = IntXPathed("/application/notifications/@pack");
		notificationsConfig.level = IntXPathed("/application/notifications/@level");
		dailyPuzzleConfig.timeSpan = FloatXPathed("/application/daily_puzzle/@time_span");
		num = ElementCount("/application/tutorial");
		for (int n = 0; n < num; n++)
		{
			TutorialConfig item5 = default(TutorialConfig);
			string a3 = StringXPathed($"/application/tutorial[{n + 1}]/@type");
			item5.type = (string.Equals(a3, "good_start") ? TutorialType.GoodStart : (string.Equals(a3, "start_and_finish") ? TutorialType.StartAndFinish : (string.Equals(a3, "expose") ? TutorialType.Expose : (string.Equals(a3, "tip") ? TutorialType.Tip : (string.Equals(a3, "perfect") ? TutorialType.Perfect : (string.Equals(a3, "dictionary") ? TutorialType.Dictionary : (string.Equals(a3, "shop") ? TutorialType.Shop : (string.Equals(a3, "extra_word") ? TutorialType.ExtraWord : TutorialType.None))))))));
			item5.level = IntXPathed($"/application/tutorial[{n + 1}]/@level");
			item5.delay = FloatXPathed($"/application/tutorial[{n + 1}]/@delay");
			item5.hand = new Vector2(FloatXPathed($"/application/tutorial[{n + 1}]/@hand_x"), FloatXPathed($"/application/tutorial[{n + 1}]/@hand_y"));
			item5.message = new Vector2(FloatXPathed($"/application/tutorial[{n + 1}]/@message_x"), FloatXPathed($"/application/tutorial[{n + 1}]/@message_y"));
			item5.isPerfect = BoolXPathed($"/application/tutorial[{n + 1}]/@perfect");
			item5.isDictionary = BoolXPathed($"/application/tutorial[{n + 1}]/@dictionary");
			item5.isShop = BoolXPathed($"/application/tutorial[{n + 1}]/@shop");
			item5.isExtraWord = BoolXPathed($"/application/tutorial[{n + 1}]/@extra_word");
			tutorialsConfig.Add(item5);
		}
		ELSingleton<TutorialManager>.Instance.Setup();
		base.LoadSettingsCompleted();
	}
}
