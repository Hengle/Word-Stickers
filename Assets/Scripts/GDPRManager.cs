using System;

public class GDPRManager : ELSingleton<GDPRManager>
{
	private static string[] eu = new string[32]
	{
		"AT",
		"BE",
		"BG",
		"HR",
		"CY",
		"CZ",
		"DK",
		"EE",
		"FI",
		"FR",
		"DE",
		"GR",
		"HU",
		"IE",
		"IT",
		"LV",
		"LT",
		"LU",
		"MT",
		"NL",
		"PL",
		"PT",
		"RO",
		"SK",
		"SI",
		"ES",
		"SE",
		"GB",
		"IS",
		"LI",
		"NO",
		"CH"
	};

	public bool isDisplayed;

	public bool isGDPRAction()
	{
		if (!isDisplayed && IsGDPR())
		{
			ELSingleton<GDPRPopUp>.Instance.ShowPopUp();
			return true;
		}
		return false;
	}

	public void GDPRConsume()
	{
		isDisplayed = true;
		ELSingleton<ApplicationSettings>.Instance.Save();
	}

	public bool IsGDPR()
	{
		string region = PreciseLocale.GetRegion();
		for (int i = 0; i < eu.Length; i++)
		{
			if (string.Equals(region, eu[i], StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
		}
		if (ELSingleton<ApplicationSettings>.Instance.deploymentEnvironment != 0)
		{
			return true;
		}
		return false;
	}
}
