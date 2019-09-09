public class CoinsManager : ELSingleton<CoinsManager>
{
	private int coins;

	public int Coins
	{
		get
		{
			return coins;
		}
		set
		{
			coins = value;
		}
	}

	public void AddCoins(int aCoins)
	{
		coins += aCoins;
		if (coins < 0)
		{
			coins = 0;
		}
		ELSingleton<ApplicationSettings>.Instance.Save();
	}
}
