using System;
using UnityEngine;
using UnityEngine.Purchasing;

public class IapManager : ELSingleton<IapManager>, IStoreListener
{
    private static IStoreController m_StoreController;
    private static IExtensionProvider m_StoreExtensionProvider;

    public bool initialOfferConsummed;

	public Sprite imageGoodStart;

	public Sprite imageStartFinish;

	public Sprite imageExpose;

	public Sprite imageTip;

	private bool purchaseStarted;

	private bool restoreStarted;

	private long rewardedVideoAdDateLast;

	public long RewardedVideoAdDateLast
	{
		get
		{
			return rewardedVideoAdDateLast;
		}
		set
		{
			rewardedVideoAdDateLast = value;
		}
	}

	public void InitializePurchasing()
	{
        if (!IsInitialized())
        {
            ConfigurationBuilder configurationBuilder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            foreach (XmlSettings.IapConfig item in ELSingleton<XmlSettings>.Instance.iapConfig)
            {
                configurationBuilder.AddProduct(item.iap, item.type);
            }
            UnityPurchasing.Initialize(this, configurationBuilder);
        }
    }

	public bool IsInitialized()
	{
        if (m_StoreController != null)
        {
            return m_StoreExtensionProvider != null;
        }
        return false;
	}

	public double getPrice(string productId)
	{
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);
            if (product != null && product.availableToPurchase)
            {
                return decimal.ToDouble(product.metadata.localizedPrice);
            }
            ReportError("Not purchasing product, either is not found or is not available for purchase");
        }
        else
        {
            ReportError("Shop is not ready. Try again later.");
        }
        return 0.0;
	}

	public string getPriceString(string productId)
	{
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);
            if (product != null && product.availableToPurchase)
            {
                return product.metadata.localizedPriceString;
            }
            ReportError("Not purchasing product, either is not found or is not available for purchase");
        }
        else
        {
            ReportError("Shop is not ready. Try again later.");
        }
        return null;
	}

	public string getCurrency(string productId)
	{
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);
            if (product != null && product.availableToPurchase)
            {
                return product.metadata.isoCurrencyCode;
            }
            ReportError("Not purchasing product, either is not found or is not available for purchase");
        }
        else
        {
            ReportError("Shop is not ready. Try again later.");
        }
        return null;
	}

	public string getName(string productId)
	{
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);
            if (product != null && product.availableToPurchase)
            {
                if (Application.isEditor)
                {
                    string[] array = productId.Split('.');
                    return array[array.Length - 1];
                }
                return product.metadata.localizedTitle;
            }
            ReportError("Not purchasing product, either is not found or is not available for purchase");
        }
        else
        {
            ReportError("Shop is not ready. Try again later.");
        }
        return null;
	}

	public void BuyProductID(string productId)
	{
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);
            if (product != null && product.availableToPurchase)
            {
                UnityEngine.Debug.Log($"Purchasing product asychronously: '{product.definition.id}'");
                purchaseStarted = true;
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                ReportError("Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            ReportError("Shop is not ready. Try again later.");
        }
    }

	public void RestorePurchases()
	{
        if (!IsInitialized())
        {
            ReportError("Shop is not ready. Try again later.");
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
        {
            UnityEngine.Debug.Log("RestorePurchases started ...");
            IAppleExtensions extension = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            restoreStarted = true;
            extension.RestoreTransactions(delegate (bool result)
            {
                UnityEngine.Debug.Log("RestorePurchases continuing: " + result.ToString() + ". If no further messages, no purchases available to restore.");
            });
        }
        else
        {
            ReportError("Not supported on this platform. Current = " + Application.platform);
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        UnityEngine.Debug.Log("OnInitialized: PASS");
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        ReportError("Initialization Failure Reason:" + error);
    }

    public void CollectItems(XmlSettings.IapConfig iap)
	{
		ShopButton shopButton = null;
		CommonCoinPod commonCoinPod = null;
		MonoBehaviour monoBehaviour = null;
		Transform transform = null;
		if (ELSingleton<MenuWindow>.Instance.isActiveAndEnabled)
		{
			shopButton = ELSingleton<MenuWindow>.Instance.shopButton;
			commonCoinPod = ELSingleton<MenuWindow>.Instance.coinPod;
			monoBehaviour = ELSingleton<MenuWindow>.Instance;
			transform = ELSingleton<MenuWindow>.Instance.coinTarget.transform;
		}
		if (ELSingleton<GameWindow>.Instance.isActiveAndEnabled)
		{
			shopButton = ELSingleton<GameWindow>.Instance.shopButton;
			commonCoinPod = ELSingleton<GameWindow>.Instance.coinPod;
			monoBehaviour = ELSingleton<GameWindow>.Instance;
			transform = ELSingleton<GameWindow>.Instance.hintBar.transform;
		}
		if (ELSingleton<PackWindow>.Instance.isActiveAndEnabled)
		{
			shopButton = ELSingleton<PackWindow>.Instance.shopButton;
			commonCoinPod = ELSingleton<PackWindow>.Instance.coinPod;
			monoBehaviour = ELSingleton<PackWindow>.Instance;
			transform = ELSingleton<PackWindow>.Instance.coinTarget.transform;
		}
		if (shopButton != null && commonCoinPod != null)
		{
			if (iap.coins > 0)
			{
				shopButton.UpdateCoins(-iap.coins);
				commonCoinPod.ReleaseCoinsRadial(50, ELSingleton<MenuWindow>.Instance.transform.position, ELSingleton<MenuWindow>.Instance.coinTarget.transform.position, 1f, 2.2f, 2.2f, 0.5f, aIsFullUpdate: true, aIsWithParticles: false);
			}
			int num = Math.Sign(iap.expose) + Math.Sign(iap.goodStart) + Math.Sign(iap.startFinish) + Math.Sign(iap.tip);
			num = 30 / (num + 1);
			commonCoinPod.ReleaseCoinsRadialCustomImage(imageExpose, Math.Min(num, iap.expose), monoBehaviour.transform.position, transform.position, 1f, 0.9f, 0.9f, 0f);
			commonCoinPod.ReleaseCoinsRadialCustomImage(imageGoodStart, Math.Min(num, iap.goodStart), monoBehaviour.transform.position, transform.position, 1f, 0.9f, 0.9f, 0f);
			commonCoinPod.ReleaseCoinsRadialCustomImage(imageTip, Math.Min(num, iap.tip), monoBehaviour.transform.position, transform.position, 1f, 0.9f, 0.9f, 0f);
			commonCoinPod.ReleaseCoinsRadialCustomImage(imageStartFinish, Math.Min(num, iap.startFinish), monoBehaviour.transform.position, transform.position, 1f, 0.9f, 0.9f, 0f);
		}
	}

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        foreach (XmlSettings.IapConfig item in ELSingleton<XmlSettings>.Instance.iapConfig)
        {
            if (string.Equals(args.purchasedProduct.definition.id, item.iap, StringComparison.Ordinal))
            {
                UnityEngine.Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
                initialOfferConsummed = true;
                ELSingleton<CoinsManager>.Instance.AddCoins(item.coins);
                ELSingleton<HintManager>.Instance.ChangeAmount(LevelHint.Expose, item.expose);
                ELSingleton<HintManager>.Instance.ChangeAmount(LevelHint.GoodStart, item.goodStart);
                ELSingleton<HintManager>.Instance.ChangeAmount(LevelHint.StartAndFinish, item.startFinish);
                ELSingleton<HintManager>.Instance.ChangeAmount(LevelHint.Tip, item.tip);
                if (ELSingleton<GameWindow>.Instance.isActiveAndEnabled)
                {
                    ELSingleton<GameWindow>.Instance.goodStartButton.Setup();
                    ELSingleton<GameWindow>.Instance.startAndFinishButton.Setup();
                    ELSingleton<GameWindow>.Instance.exposeButton.Setup();
                    ELSingleton<GameWindow>.Instance.tipButton.Setup();
                }
                if (item.noads)
                {
                    ELSingleton<AdsManager>.Instance.TurnOff();
                }
                if (purchaseStarted)
                {
                    ELSingleton<MessagePopUp>.Instance.ShowPopUp("Shop", "Purchase completed!", 1.5f);
                }
                if (restoreStarted)
                {
                    ELSingleton<MessagePopUp>.Instance.ShowPopUp("Shop", "Purchases restore completed!", 1f);
                }
                CollectItems(item);
                if (purchaseStarted)
                {
                    ELSingleton<AnalyticsManager>.Instance.IAPTransaction("shop", args.purchasedProduct.metadata.isoCurrencyCode, decimal.ToInt32(args.purchasedProduct.metadata.localizedPrice * 100m), decimal.ToSingle(args.purchasedProduct.metadata.localizedPrice), args.purchasedProduct.definition.id);
                }
            }
        }
        purchaseStarted = false;
        restoreStarted = false;
        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        purchaseStarted = false;
        restoreStarted = false;
        ReportError((failureReason == PurchaseFailureReason.UserCancelled) ? "User cancelled the purchase." : "Purchase Failed. Try again later.");
    }

    public void ReportError(string message)
	{
		UnityEngine.Debug.Log(message);
		ELSingleton<MessagePopUp>.Instance.ShowPopUp("Shop", message);
	}

	public void Awake()
	{
		rewardedVideoAdDateLast = 0L;
	}

	public long GetRewardedVideoTimeSpan()
	{
		return GetTimeSpan() - ELUtils.GetEpochTime() + rewardedVideoAdDateLast;
	}

	public bool ShowRewardedVideo(bool aIsExecute)
	{
		long timeSpan = GetTimeSpan();
		bool num = ELUtils.GetEpochTime() - RewardedVideoAdDateLast >= timeSpan && ELSingleton<AdsManager>.Instance.ShowRewardedVideoAd(AdsManager.AdType.FreeCoins, aIsExecute);
		if (num & aIsExecute)
		{
			rewardedVideoAdDateLast = ELUtils.GetEpochTime();
			ELSingleton<ApplicationSettings>.Instance.Save();
		}
		return num;
	}

	private long GetTimeSpan()
	{
		return (long)(ELSingleton<XmlSettings>.Instance.rewardedVideoAdConfig.timeSpan * (float)(ELSingleton<ApplicationSettings>.Instance.DeploymentSettings.isDebug ? 60 : 3600));
	}
}
