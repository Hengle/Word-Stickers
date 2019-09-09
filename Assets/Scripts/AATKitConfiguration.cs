using System;
using System.Collections.Generic;

[Serializable]
public class AATKitConfiguration
{
	public enum Consent
	{
		OBTAINED,
		UNKNOWN,
		WITHHELD
	}

	public enum DetailedConsentTypes
	{
		ConsentAutomatic,
		ConsentString,
		ManagedConsent
	}

	public enum ManagedConsentLanguage
	{
		BULGARIAN,
		CROATIAN,
		CZECH,
		DANISH,
		DUTCH,
		ESTONIAN,
		ENGLISH,
		FINNISH,
		FRENCH,
		GERMAN,
		GREEK,
		HUNGARIAN,
		IRISH,
		ITALIAN,
		LATVIAN,
		LITHUANIAN,
		MALTESE,
		POLISH,
		PORTUGUESE,
		ROMANIAN,
		SLOVAK,
		SLOVENIAN,
		SPANISH,
		SWEDISH
	}

	public string AlternativeBundleId = string.Empty;

	public bool ConsentRequired = true;

	public string InitialRules = string.Empty;

	public bool ShouldCacheRules = true;

	public bool ShouldReportUsingAlternativeBundleId = true;

	public int TestModeAccountId;

	public bool UseDebugShake = true;

	public bool UseGeoLocation;

	[Obsolete]
	public Consent SimpleConsent = Consent.UNKNOWN;

	public DetailedConsentTypes DetailedConsent;

	public string ConsentString = string.Empty;

	public List<ManagedConsentLanguage> ManagedConsentLanguages = new List<ManagedConsentLanguage>();
}
