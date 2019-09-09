using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelSet
{
	public List<World> worlds = new List<World>();

	public LevelInfo currentLevelInfo;

	public List<Level>[] specialLevels;

	private string saveFileName;

	public LevelSet(string aSaveName)
	{
		specialLevels = new List<Level>[11];
		for (int i = 0; i < specialLevels.Length; i++)
		{
			specialLevels[i] = new List<Level>();
		}
		saveFileName = aSaveName;
	}

	public void Save()
	{
		FileStream fileStream = File.Open(Application.persistentDataPath + "/" + saveFileName + ".dat", FileMode.Create);
		BinaryWriter binaryWriter = new BinaryWriter(fileStream);
		Save(binaryWriter);
		binaryWriter.Flush();
		binaryWriter.Close();
		fileStream.Close();
	}

	private void Save(BinaryWriter aBinaryWriter)
	{
		aBinaryWriter.Write((short)101);
		List<LevelInfo> list = new List<LevelInfo>();
		LevelInfo levelInfo = new LevelInfo(0, 0, -1);
		for (int i = 0; i < worlds.Count; i++)
		{
			for (int j = 0; j < worlds[i].packs.Count; j++)
			{
				for (int k = 0; k < worlds[i].packs[j].levels.Count; k++)
				{
					Level level = worlds[i].packs[j].levels[k];
					if (level.isCompleted)
					{
						levelInfo.currentWorld = i;
						levelInfo.currentPack = j;
						levelInfo.currentLevel = k;
					}
					if (level.GetWordsModified() > 0 || (!level.isCompleted && level.extraWords.Count > 0) || level.IsStatsModified() || level.hintTipIndex != -1)
					{
						if (list.Count >= 5)
						{
							list.RemoveAt(0);
						}
						list.Add(new LevelInfo(i, j, k));
					}
				}
			}
		}
		aBinaryWriter.Write((short)levelInfo.currentWorld);
		aBinaryWriter.Write((short)levelInfo.currentPack);
		aBinaryWriter.Write((short)levelInfo.currentLevel);
		aBinaryWriter.Write((short)list.Count);
		foreach (LevelInfo item in list)
		{
			aBinaryWriter.Write(worlds[item.currentWorld].name);
			aBinaryWriter.Write(worlds[item.currentWorld].packs[item.currentPack].name);
			aBinaryWriter.Write(worlds[item.currentWorld].packs[item.currentPack].levels[item.currentLevel].name);
			worlds[item.currentWorld].packs[item.currentPack].levels[item.currentLevel].Save(aBinaryWriter);
		}
		aBinaryWriter.Write((short)worlds.Count);
		for (int l = 0; l < worlds.Count; l++)
		{
			aBinaryWriter.Write(worlds[l].name);
			aBinaryWriter.Write((short)worlds[l].packs.Count);
			for (int m = 0; m < worlds[l].packs.Count; m++)
			{
				aBinaryWriter.Write(worlds[l].packs[m].name);
				aBinaryWriter.Write((short)worlds[l].packs[m].progressIndex);
			}
		}
	}

	public void Load()
	{
		string path = Application.persistentDataPath + "/" + saveFileName + ".dat";
		if (File.Exists(path))
		{
			FileStream fileStream = File.Open(path, FileMode.Open);
			BinaryReader binaryReader = new BinaryReader(fileStream);
			Load(binaryReader);
			binaryReader.Close();
			fileStream.Close();
		}
	}

	private void Load(BinaryReader aBinaryReader)
	{
		int num = aBinaryReader.ReadInt16();
		if (num < 100)
		{
			return;
		}
		LevelInfo levelInfo = default(LevelInfo);
		levelInfo.currentWorld = aBinaryReader.ReadInt16();
		levelInfo.currentPack = aBinaryReader.ReadInt16();
		levelInfo.currentLevel = aBinaryReader.ReadInt16();
		for (int i = 0; i < worlds.Count; i++)
		{
			int num2 = 0;
			for (int j = 0; j < worlds[i].packs.Count; j++)
			{
				int num3 = 0;
				for (int k = 0; k < worlds[i].packs[j].levels.Count; k++)
				{
					if (i < levelInfo.currentWorld || (i == levelInfo.currentWorld && j < levelInfo.currentPack) || (i == levelInfo.currentWorld && j == levelInfo.currentPack && k <= levelInfo.currentLevel))
					{
						worlds[i].packs[j].levels[k].isCompleted = true;
						num3++;
					}
				}
				if (num3 == worlds[i].packs[j].levels.Count)
				{
					worlds[i].packs[j].isCompleted = true;
					num2++;
				}
			}
			if (num2 == worlds[i].packs.Count)
			{
				worlds[i].isCompleted = true;
			}
		}
		int num4 = aBinaryReader.ReadInt16();
		for (int l = 0; l < num4; l++)
		{
			string value = aBinaryReader.ReadString();
			string value2 = aBinaryReader.ReadString();
			string value3 = aBinaryReader.ReadString();
			bool flag = false;
			foreach (World world2 in worlds)
			{
				if (world2.name.Equals(value))
				{
					foreach (Pack pack in world2.packs)
					{
						if (pack.name.Equals(value2))
						{
							foreach (Level level in pack.levels)
							{
								if (level.name.Equals(value3))
								{
									level.Load(aBinaryReader, num);
									flag = true;
									break;
								}
							}
							break;
						}
					}
					break;
				}
			}
			if (!flag)
			{
				Level.Skip(aBinaryReader, num);
			}
		}
		num4 = aBinaryReader.ReadInt16();
		for (int m = 0; m < num4; m++)
		{
			string value4 = aBinaryReader.ReadString();
			World world = null;
			foreach (World world3 in worlds)
			{
				if (world3.name.Equals(value4))
				{
					world = world3;
					break;
				}
			}
			int num5 = aBinaryReader.ReadInt16();
			for (int n = 0; n < num5; n++)
			{
				string value5 = aBinaryReader.ReadString();
				int progressIndex = aBinaryReader.ReadInt16();
				if (world != null)
				{
					foreach (Pack pack2 in world.packs)
					{
						if (pack2.name.Equals(value5))
						{
							pack2.progressIndex = progressIndex;
							break;
						}
					}
				}
			}
		}
	}

	public void AfterParseAction()
	{
		Load();
		GenerateCoins();
	}

	public Level GetNextBonusRoundLevel(int size)
	{
		if (size < 4)
		{
			size = 4;
		}
		if (size > 10)
		{
			size = 10;
		}
		Level level = specialLevels[size][UnityEngine.Random.Range(0, specialLevels[size].Count - 1)];
		level.type = LevelType.BonusRound;
		return level;
	}

	public Level GetNextDailyPuzzleLevel(int size)
	{
		if (size < 4)
		{
			size = 4;
		}
		if (size > 10)
		{
			size = 10;
		}
		Level level = specialLevels[size][UnityEngine.Random.Range(0, specialLevels[size].Count - 1)];
		level.type = LevelType.DailyPuzzle;
		return level;
	}

	public World GetWorld(LevelInfo aLevelInfo)
	{
		return worlds[aLevelInfo.currentWorld];
	}

	public World GetCurrentWorld()
	{
		return GetWorld(currentLevelInfo);
	}

	public Pack GetPack(LevelInfo aLevelInfo)
	{
		return GetWorld(aLevelInfo).packs[aLevelInfo.currentPack];
	}

	public Pack GetCurrentPack()
	{
		return GetPack(currentLevelInfo);
	}

	public Level GetLevel(LevelInfo aLevelInfo)
	{
		return GetPack(aLevelInfo).levels[aLevelInfo.currentLevel];
	}

	public Level GetCurrentLevel()
	{
		return GetLevel(currentLevelInfo);
	}

	public LevelInfo GetCurrentLevelInfo()
	{
		return currentLevelInfo;
	}

	public LevelInfo GetPreviousPackCompleteInfo(LevelInfo levelInfo)
	{
		int num = levelInfo.currentWorld;
		int num2 = levelInfo.currentPack - 1;
		if (num2 < 0)
		{
			if (num > 0)
			{
				num--;
			}
			num2 = worlds[num].packs.Count - 1;
		}
		int currentLevel = worlds[num].packs[num2].levels.Count - 1;
		LevelInfo result = default(LevelInfo);
		result.currentLevel = currentLevel;
		result.currentPack = num2;
		result.currentWorld = num;
		return result;
	}

	public LevelInfo GetNextLevelInfo(LevelInfo levelInfo)
	{
		int num = levelInfo.currentWorld;
		int count = worlds.Count;
		int num2 = levelInfo.currentPack;
		int count2 = worlds[num].packs.Count;
		int count3 = worlds[num].packs[num2].levels.Count;
		int currentLevel = levelInfo.currentLevel;
		currentLevel++;
		if (currentLevel >= count3)
		{
			currentLevel = 0;
			num2++;
			if (num2 >= count2)
			{
				num2 = 0;
				num++;
				if (num >= count)
				{
					num = 0;
				}
			}
		}
		LevelInfo levelInfo2 = default(LevelInfo);
		levelInfo2.currentLevel = currentLevel;
		levelInfo2.currentPack = num2;
		levelInfo2.currentWorld = num;
		Level level = GetLevel(levelInfo2);
		if (level.type != 0 && level.isCompleted)
		{
			levelInfo2 = GetNextLevelInfo(levelInfo2);
			return levelInfo2;
		}
		return levelInfo2;
	}

	public bool IsLastLevelInPack()
	{
		return IsLastLevelInPack(currentLevelInfo);
	}

	public bool IsLastLevelInPack(LevelInfo levelInfo)
	{
		int currentWorld = levelInfo.currentWorld;
		int currentPack = levelInfo.currentPack;
		int count = worlds[currentWorld].packs[currentPack].levels.Count;
		return levelInfo.currentLevel == count - 1;
	}

	public bool IsLastLevelInWorld()
	{
		return IsLastLevelInWorld(currentLevelInfo);
	}

	public bool IsLastLevelInWorld(LevelInfo levelInfo)
	{
		int currentWorld = levelInfo.currentWorld;
		int count = worlds[currentWorld].packs.Count;
		if (IsLastLevelInPack(levelInfo))
		{
			return levelInfo.currentPack == count - 1;
		}
		return false;
	}

	public bool IsGameCompleted(LevelInfo li)
	{
		return li.currentWorld == worlds.Count;
	}

	public bool IsLastPackInGame(LevelInfo li)
	{
		int count = worlds[li.currentWorld].packs.Count;
		if (li.currentWorld == worlds.Count - 1)
		{
			return li.currentPack == count - 1;
		}
		return false;
	}

	public LevelInfo GetFirstNotCompleateLevel()
	{
		LevelInfo result;
		for (int i = 0; i < worlds.Count; i++)
		{
			for (int j = 0; j < worlds[i].packs.Count; j++)
			{
				for (int k = 0; k < worlds[i].packs[j].levels.Count; k++)
				{
					if (!worlds[i].packs[j].levels[k].isCompleted)
					{
						result = default(LevelInfo);
						result.currentWorld = i;
						result.currentPack = j;
						result.currentLevel = k;
						return result;
					}
				}
			}
		}
		result = default(LevelInfo);
		result.currentWorld = worlds.Count;
		result.currentPack = 0;
		result.currentLevel = 0;
		return result;
	}

	public void SetCurrentLevel(LevelInfo aLevelInfo, bool forceProgress = false)
	{
		if (ELSingleton<ApplicationSettings>.Instance.DeploymentSettings.isCheatsEnabled | forceProgress)
		{
			for (int i = 0; i < aLevelInfo.currentWorld; i++)
			{
				worlds[i].isCompleted = true;
				for (int j = 0; j < worlds[i].packs.Count; j++)
				{
					worlds[i].packs[j].isCompleted = true;
					if (forceProgress)
					{
						worlds[i].packs[j].progressIndex = worlds[i].packs[j].levels.Count;
					}
					for (int k = 0; k < worlds[i].packs[j].levels.Count; k++)
					{
						worlds[i].packs[j].levels[k].isCompleted = true;
						if (forceProgress)
						{
							worlds[i].packs[j].levels[k].ResetWords();
							worlds[i].packs[j].levels[k].ResetStats();
						}
					}
				}
			}
			for (int l = 0; l < aLevelInfo.currentPack; l++)
			{
				worlds[aLevelInfo.currentWorld].packs[l].isCompleted = true;
				if (forceProgress)
				{
					worlds[aLevelInfo.currentWorld].packs[l].progressIndex = worlds[aLevelInfo.currentWorld].packs[l].levels.Count;
				}
				for (int m = 0; m < worlds[aLevelInfo.currentWorld].packs[l].levels.Count; m++)
				{
					worlds[aLevelInfo.currentWorld].packs[l].levels[m].isCompleted = true;
					if (forceProgress)
					{
						worlds[aLevelInfo.currentWorld].packs[l].levels[m].ResetWords();
						worlds[aLevelInfo.currentWorld].packs[l].levels[m].ResetStats();
					}
				}
			}
			for (int n = 0; n < aLevelInfo.currentLevel; n++)
			{
				worlds[aLevelInfo.currentWorld].packs[aLevelInfo.currentPack].levels[n].isCompleted = true;
				if (forceProgress)
				{
					worlds[aLevelInfo.currentWorld].packs[aLevelInfo.currentPack].levels[n].ResetWords();
					worlds[aLevelInfo.currentWorld].packs[aLevelInfo.currentPack].levels[n].ResetStats();
				}
			}
			Save();
		}
		currentLevelInfo = aLevelInfo;
	}

	public void CompleteLevel(bool aIsCompleteLevel)
	{
		CompleteLevel(currentLevelInfo, aIsCompleteLevel);
	}

	public void CompleteLevel(LevelInfo aLevelInfo, bool aIsCompleteLevel)
	{
		int count = worlds.Count;
		int currentWorld = aLevelInfo.currentWorld;
		int currentPack = aLevelInfo.currentPack;
		int count2 = worlds[currentWorld].packs.Count;
		int count3 = worlds[currentWorld].packs[currentPack].levels.Count;
		int currentLevel = aLevelInfo.currentLevel;
		worlds[currentWorld].packs[currentPack].levels[currentLevel].ResetWords();
		worlds[currentWorld].packs[currentPack].levels[currentLevel].ResetStats();
		worlds[currentWorld].packs[currentPack].levels[currentLevel].isCompleted = aIsCompleteLevel;
		aLevelInfo.currentLevel++;
		if (aLevelInfo.currentLevel >= count3)
		{
			worlds[currentWorld].packs[currentPack].isCompleted = true;
			aLevelInfo.currentLevel = 0;
			aLevelInfo.currentPack++;
			if (aLevelInfo.currentPack >= count2)
			{
				aLevelInfo.currentPack = 0;
				worlds[currentWorld].isCompleted = true;
				aLevelInfo.currentWorld++;
				if (aLevelInfo.currentWorld >= count)
				{
					aLevelInfo.currentWorld = 0;
				}
			}
		}
		ELSingleton<FacebookManager>.Instance.FBSet();
		Save();
	}

	public void SetNextLevel()
	{
		currentLevelInfo = GetNextLevelInfo(currentLevelInfo);
	}

	public void GenerateCoins()
	{
		UnityEngine.Random.State state = UnityEngine.Random.state;
		UnityEngine.Random.InitState(0);
		int num = 0;
		int num2 = 0;
		foreach (World world in worlds)
		{
			foreach (Pack pack in world.packs)
			{
				int num3 = 0;
				foreach (Level level in pack.levels)
				{
					level.GenerateCoins(ELSingleton<XmlSettings>.Instance.coinsInLevelsConfig[num], num3);
					num3++;
				}
				num2++;
				if (num2 >= ELSingleton<XmlSettings>.Instance.coinsInLevelsConfig[num].packs)
				{
					num2 = 0;
					num++;
					num = Math.Min(num, ELSingleton<XmlSettings>.Instance.coinsInLevelsConfig.Count - 1);
				}
			}
		}
		UnityEngine.Random.state = state;
	}

	public bool IsExtraWord(string aWord)
	{
		return GetCurrentLevel().extraWords.Contains(aWord);
	}

	public void AddExtraWord(string aWord)
	{
		GetCurrentLevel().extraWords.Add(aWord);
		Save();
	}
}
