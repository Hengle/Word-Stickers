using GameAnalyticsSDK;
using System.Collections.Generic;
using UnityEngine.Analytics;

public class AnalyticsManager : ELSingleton<AnalyticsManager>
{
	private void Start()
	{
		GameAnalytics.Initialize();
	}

	public void Event(string aEventName, Dictionary<string, object> aDictionary = null)
	{
		Analytics.CustomEvent(aEventName, aDictionary ?? new Dictionary<string, object>());
	}

	public void ScreenVisit(string aScreenName)
	{
	}

	public void LevelStart(string aName, IDictionary<string, object> aDictionary = null)
	{
		GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, aName);
	}

	public void LevelComplete(string aName, IDictionary<string, object> aDictionary = null)
	{
		AnalyticsEvent.LevelComplete(aName, aDictionary ?? new Dictionary<string, object>());
		GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, aName);
	}

	public void TutorialStart(string aTutorialId, IDictionary<string, object> aDictionary = null)
	{
		AnalyticsEvent.TutorialStart(aTutorialId, aDictionary ?? new Dictionary<string, object>());
	}

	public void TutorialStep(int aStepIndex, string aTutorialId, IDictionary<string, object> aDictionary = null)
	{
		AnalyticsEvent.TutorialStep(aStepIndex, aTutorialId, aDictionary ?? new Dictionary<string, object>());
	}

	public void TutorialSkip(string aTutorialId, IDictionary<string, object> aDictionary = null)
	{
		AnalyticsEvent.TutorialSkip(aTutorialId, aDictionary ?? new Dictionary<string, object>());
	}

	public void AdStart(bool aIsRewarded, string aNetwork)
	{
		AnalyticsEvent.AdStart(aIsRewarded, aNetwork);
	}

	public void AdComplete(bool aIsRewarded, string aNetwork)
	{
		AnalyticsEvent.AdComplete(aIsRewarded, aNetwork);
	}

	public void IAPTransaction(string aTransactionContext, string aCurrency, int aPriceInt, float aPriceFloat, string aItemId)
	{
		AnalyticsEvent.IAPTransaction(aTransactionContext, aPriceFloat, aItemId);
		GameAnalytics.NewBusinessEventGooglePlay(aCurrency, aPriceInt, "shop_item", aItemId, aTransactionContext, null, null);
	}
}
