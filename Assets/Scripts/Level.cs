using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Level
{
	public int index;

	public string name;

	public int number;

	public LevelType type;

	public bool isCompleted;

	public int width;

	public int height;

	public List<LevelWord> words;

	public List<string> extraWords;

	public Rect iconRect;

	public LevelStats stats;

	public int hintTipIndex;

	public void Save(BinaryWriter aBinaryWriter)
	{
		aBinaryWriter.Write(isCompleted);
		aBinaryWriter.Write((short)words.Count);
		foreach (LevelWord word in words)
		{
			aBinaryWriter.Write(word.word);
			word.Save(aBinaryWriter);
		}
		aBinaryWriter.Write((short)extraWords.Count);
		foreach (string extraWord in extraWords)
		{
			aBinaryWriter.Write(extraWord);
		}
		aBinaryWriter.Write((short)stats.coins);
		aBinaryWriter.Write((short)stats.pointsNormal);
		aBinaryWriter.Write((short)stats.pointsExtra);
		aBinaryWriter.Write(stats.isPerfect);
		aBinaryWriter.Write((short)stats.valid);
		aBinaryWriter.Write((short)hintTipIndex);
	}

	public void Load(BinaryReader aBinaryReader, int aVersion)
	{
		if (aVersion >= 100)
		{
			isCompleted = aBinaryReader.ReadBoolean();
			int num = aBinaryReader.ReadInt16();
			for (int i = 0; i < num; i++)
			{
				string value = aBinaryReader.ReadString();
				bool flag = true;
				foreach (LevelWord word in words)
				{
					if (word.word.Equals(value))
					{
						word.Load(aBinaryReader, aVersion);
						flag = false;
						break;
					}
				}
				if (flag)
				{
					LevelWord.Skip(aBinaryReader, aVersion);
				}
			}
			num = aBinaryReader.ReadInt16();
			for (int j = 0; j < num; j++)
			{
				extraWords.Add(aBinaryReader.ReadString());
			}
			stats.coins = aBinaryReader.ReadInt16();
			stats.pointsNormal = aBinaryReader.ReadInt16();
			stats.pointsExtra = aBinaryReader.ReadInt16();
			stats.isPerfect = aBinaryReader.ReadBoolean();
			stats.valid = aBinaryReader.ReadInt16();
		}
		if (aVersion >= 101)
		{
			hintTipIndex = aBinaryReader.ReadInt16();
		}
	}

	public static void Skip(BinaryReader aBinaryReader, int aVersion)
	{
		if (aVersion >= 100)
		{
			aBinaryReader.ReadBoolean();
			int num = aBinaryReader.ReadInt16();
			for (int i = 0; i < num; i++)
			{
				aBinaryReader.ReadString();
				LevelWord.Skip(aBinaryReader, aVersion);
			}
			num = aBinaryReader.ReadInt16();
			for (int j = 0; j < num; j++)
			{
				aBinaryReader.ReadString();
			}
			aBinaryReader.ReadInt16();
			aBinaryReader.ReadInt16();
			aBinaryReader.ReadInt16();
			aBinaryReader.ReadBoolean();
			aBinaryReader.ReadInt16();
		}
		if (aVersion >= 101)
		{
			aBinaryReader.ReadInt16();
		}
	}

	public void ResetWords()
	{
		foreach (LevelWord word in words)
		{
			word.Reset();
		}
	}

	public int GetWordsModified()
	{
		int num = 0;
		foreach (LevelWord word in words)
		{
			if (word.isCompleted || word.hint != 0)
			{
				num++;
			}
		}
		return num;
	}

	public void GenerateCoins(XmlSettings.CoinsInLevelsConfig config, int index)
	{
		int num = (index % config.bTierEachLevel == config.bTierEachLevel - 1) ? Random.Range(config.minCoinsB, config.maxCoinsB + 1) : Random.Range(config.minCoinsA, config.maxCoinsA + 1);
		int num2 = 100;
		while (num > 0 && num2 > 0)
		{
			num2--;
			if (words[Random.Range(0, words.Count)].GenerateCoin())
			{
				num--;
			}
		}
	}

	public void ResetStats()
	{
		stats.Reset();
	}

	public bool IsStatsModified()
	{
		if (stats.coins == 0 && stats.pointsNormal == 0 && stats.pointsExtra == 0 && stats.isPerfect)
		{
			return stats.valid != 0;
		}
		return true;
	}
}
