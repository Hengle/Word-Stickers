using System.Collections.Generic;

public class TutorialManager : ELSingleton<TutorialManager>
{
	private List<Tutorial> tutorials = new List<Tutorial>();

	private bool isWorking;

	public List<Tutorial> Tutorials => tutorials;

	public void Reset()
	{
		isWorking = false;
	}

	public void Setup()
	{
		for (int i = 0; i < ELSingleton<XmlSettings>.Instance.tutorialsConfig.Count; i++)
		{
			Tutorial tutorial = new Tutorial();
			tutorial.type = ELSingleton<XmlSettings>.Instance.tutorialsConfig[i].type;
			tutorial.level = ELSingleton<XmlSettings>.Instance.tutorialsConfig[i].level;
			tutorial.delay = ELSingleton<XmlSettings>.Instance.tutorialsConfig[i].delay;
			tutorial.hand = ELSingleton<XmlSettings>.Instance.tutorialsConfig[i].hand;
			tutorial.message = ELSingleton<XmlSettings>.Instance.tutorialsConfig[i].message;
			tutorial.isPerfect = ELSingleton<XmlSettings>.Instance.tutorialsConfig[i].isPerfect;
			tutorial.isDictionary = ELSingleton<XmlSettings>.Instance.tutorialsConfig[i].isDictionary;
			tutorial.isShop = ELSingleton<XmlSettings>.Instance.tutorialsConfig[i].isShop;
			tutorial.isExtraWord = ELSingleton<XmlSettings>.Instance.tutorialsConfig[i].isExtraWord;
			tutorial.isConsumed = false;
			tutorials.Add(tutorial);
		}
	}

	public Tutorial CheckTrigger(LevelType aLevelType, int aLevelNumber, bool aIsPerfect, bool aIsDictionary, bool aIsExtraWord)
	{
		if (aLevelType == LevelType.Normal && !isWorking)
		{
			foreach (Tutorial tutorial in tutorials)
			{
				if (!tutorial.isConsumed && tutorial.type != 0 && (tutorial.level == -1 || tutorial.level - 1 == aLevelNumber) && (!tutorial.isPerfect || (tutorial.isPerfect && aIsPerfect)) && (!tutorial.isDictionary || (tutorial.isDictionary && aIsDictionary)) && (!tutorial.isShop || (tutorial.isShop && !ELSingleton<HintManager>.Instance.CanUseHint())) && (!tutorial.isExtraWord || (tutorial.isExtraWord && aIsExtraWord)))
				{
					isWorking = true;
					tutorial.isConsumed = true;
					foreach (Tutorial tutorial2 in tutorials)
					{
						if (tutorial2.type == tutorial.type)
						{
							tutorial2.isConsumed = true;
						}
					}
					ELSingleton<ApplicationSettings>.Instance.Save();
					return tutorial;
				}
			}
		}
		return null;
	}
}
