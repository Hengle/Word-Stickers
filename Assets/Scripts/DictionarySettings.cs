using System;
using System.Collections.Generic;
using System.Linq;

public class DictionarySettings : ELTxtConfig<DictionarySettings>
{
	public Dictionary<string, string> dictionary;

	public override void LoadSettingsCompleted()
	{
		base.LoadSettingsCompleted();
		dictionary = config.Split(new string[3]
		{
			"\r\n",
			"\r",
			"\n"
		}, StringSplitOptions.None).ToDictionary((string x) => x, (string x) => x);
	}

	public bool CheckWord(string aWord)
	{
		return dictionary.ContainsKey(aWord);
	}
}
