using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class LevelsSettings : ELSingleton<LevelsSettings>
{
	private string URLTemplate = "https://docs.google.com/spreadsheets/export?format=csv&id={0}";

	private string persistentLevelDataPath;

	private string levelDataName = "LevelsSettings";

	public LevelSet levelSet = new LevelSet("w");

	private bool isSaveRequest;

	private MonoBehaviour compleateInvoke;

	private string compleateInvokeMethod;

	private void Awake()
	{
		persistentLevelDataPath = Application.persistentDataPath + "/" + levelDataName + Application.version.GetHashCode().ToString("X") + ".csv";
		isSaveRequest = false;
	}

	private void Update()
	{
		if (isSaveRequest)
		{
			isSaveRequest = false;
			SaveInvoke();
		}
	}

	public void Save(bool aIsForceSave = false)
	{
		if (aIsForceSave)
		{
			SaveInvoke();
		}
		else
		{
			isSaveRequest = true;
		}
	}

	private void SaveInvoke()
	{
		levelSet.Save();
	}

	public void LoadSettings(MonoBehaviour aCompleateInvoke, string aCompleateInvokeMethod)
	{
		compleateInvoke = aCompleateInvoke;
		compleateInvokeMethod = aCompleateInvokeMethod;
		if (ELSingleton<ApplicationSettings>.Instance.deploymentEnvironment == DeploymentEnvironment.Live)
		{
			LoadLocalSettings(persistentLevelDataPath, levelDataName, compleateInvoke, compleateInvokeMethod);
			return;
		}
		string levelConfigDocId = ELSingleton<ApplicationSettings>.Instance.DeploymentSettings.levelConfigDocId;
		string uri = string.Format(URLTemplate, levelConfigDocId);
		StartCoroutine(WaitForLoadSettings(UnityWebRequest.Get(uri), persistentLevelDataPath, levelDataName, compleateInvoke, compleateInvokeMethod));
	}

	public void LoadLocalSettings(string aCachePath, string aLevelDataName, MonoBehaviour aCompleteInv, string aCompleteMet)
	{
		if (File.Exists(persistentLevelDataPath))
		{
			string aData = new StreamReader(File.Open(aCachePath, FileMode.Open)).ReadToEnd();
			ParseSettings(aData, aCompleteInv, aCompleteMet);
		}
		else
		{
			TextAsset textAsset = (TextAsset)Resources.Load(aLevelDataName, typeof(TextAsset));
			ParseSettings(textAsset.text, aCompleteInv, aCompleteMet);
		}
	}

	private IEnumerator WaitForLoadSettings(UnityWebRequest aWWW, string aCachePath, string aLevelDataName, MonoBehaviour aCompleteInv, string aCompleteMet)
	{
		yield return aWWW.SendWebRequest();
		if (aWWW.isNetworkError || aWWW.isHttpError)
		{
			LoadLocalSettings(aCachePath, aLevelDataName, aCompleteInv, aCompleteMet);
		}
		else
		{
			ParseSettings(aWWW.downloadHandler.text, aCompleteInv, aCompleteMet);
		}
	}

	private List<List<string>> ParseCSV(string aSrcCSV)
	{
		List<List<string>> list = new List<List<string>>();
		int length = aSrcCSV.Length;
		int num = 0;
		List<string> list2 = new List<string>();
		string text = "";
		bool flag = false;
		while (num < length)
		{
			char c = aSrcCSV[num++];
			switch (c)
			{
			case '"':
				if (!flag)
				{
					flag = true;
				}
				else if (num < length)
				{
					if (aSrcCSV[num] == '"')
					{
						text += "\"";
						num++;
					}
					else
					{
						flag = false;
					}
				}
				break;
			case ',':
				if (flag)
				{
					text += c.ToString();
					break;
				}
				list2.Add(text);
				text = "";
				break;
			case '\n':
				if (flag)
				{
					text += c.ToString();
					break;
				}
				list2.Add(text);
				list.Add(list2);
				list2 = new List<string>();
				text = "";
				break;
			default:
				text += c.ToString();
				break;
			case '\r':
				break;
			}
		}
		list2.Add(text);
		list.Add(list2);
		list2 = new List<string>();
		return list;
	}

	private Dictionary<string, int> ParseCSVTitles(List<List<string>> aCsvArray)
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		List<string> list = aCsvArray[0];
		int num = 0;
		foreach (string item in list)
		{
			if (item != null && item.Length > 0)
			{
				dictionary.Add(item, num);
			}
			num++;
		}
		return dictionary;
	}

	public void ParseSettings(string aData, MonoBehaviour aCompleteInv, string aCompleteMet)
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		List<List<string>> list = new List<List<string>>();
		int length = aData.Length;
		int num = 0;
		List<string> list2 = new List<string>();
		string text = "";
		bool flag = false;
		while (num < length)
		{
			char c = aData[num++];
			switch (c)
			{
			case '"':
				if (!flag)
				{
					flag = true;
				}
				else if (num < length)
				{
					if (aData[num] == '"')
					{
						text += "\"";
						num++;
					}
					else
					{
						flag = false;
					}
				}
				break;
			case ',':
				if (flag)
				{
					text += c.ToString();
					break;
				}
				list2.Add(text);
				text = "";
				break;
			case '\n':
				if (flag)
				{
					text += c.ToString();
					break;
				}
				list2.Add(text);
				if (list2.Count >= 1)
				{
					list.Add(list2);
				}
				list2 = new List<string>();
				text = "";
				break;
			default:
				text += c.ToString();
				break;
			case '\r':
				break;
			}
		}
		list2.Add(text);
		if (list2.Count >= 1)
		{
			list.Add(list2);
		}
		list2 = new List<string>();
		List<List<string>> list3 = list;
		UnityEngine.Debug.Log("Time CSV parse: " + (Time.realtimeSinceStartup - realtimeSinceStartup));
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		bool flag2 = ELSingleton<ApplicationSettings>.Instance.deploymentEnvironment == DeploymentEnvironment.Live;
		Dictionary<string, int> dictionary = ParseCSVTitles(list3);
		World world = null;
		Pack pack = null;
		Level level = null;
		LevelWord levelWord = null;
		LevelLetter levelLetter = null;
		for (int i = 1; i < list3.Count; i++)
		{
			List<string> list4 = list3[i];
			string text2 = list4[0];
			if (text2 == null || text2.Length <= 0)
			{
				continue;
			}
			if ((flag2 && text2[0] == 'W') || (!flag2 && string.Equals(text2, "W")))
			{
				world = new World();
				world.name = list4[dictionary["Name"]];
				world.packs = new List<Pack>();
				world.isCompleted = false;
			}
			if ((flag2 && text2[0] == 'P') || (!flag2 && string.Equals(text2, "P")))
			{
				pack = new Pack();
				pack.index = num3;
				pack.name = list4[dictionary["Name"]];
				pack.levels = new List<Level>();
				pack.isCompleted = false;
				num3++;
				num4 = 0;
			}
			if ((flag2 && text2[0] == 'L') || (!flag2 && string.Equals(text2, "L")))
			{
				level = new Level();
				level.index = num4;
				level.name = list4[dictionary["Name"]];
				level.number = num5;
				level.type = LevelType.Normal;
				level.isCompleted = false;
				num5++;
				num4++;
				string[] array = Chop(list4[dictionary["Value"]], 1);
				if (array.Length > 1)
				{
					level.width = Convert.ToInt32(array[0], 16);
					level.height = Convert.ToInt32(array[1], 16);
				}
				else
				{
					level.width = 10;
					level.height = 10;
					UnityEngine.Debug.LogError("LevelSettings: wrong level width or height at level: " + (level.number + 1) + " " + level.name);
				}
				level.words = new List<LevelWord>();
				if (!levelSet.worlds.Contains(world))
				{
					levelSet.worlds.Add(world);
				}
				if (!world.packs.Contains(pack))
				{
					world.packs.Add(pack);
				}
				level.extraWords = new List<string>();
				level.iconRect = new Rect(Convert.ToInt32(list4[3]), Convert.ToInt32(list4[4]), Convert.ToInt32(list4[5]), Convert.ToInt32(list4[6]));
				level.stats = new LevelStats();
				level.hintTipIndex = -1;
				pack.levels.Add(level);
			}
			if ((flag2 && text2[0] == 'S') || (!flag2 && string.Equals(text2, "S")))
			{
				num6++;
				level = new Level();
				level.name = list4[dictionary["Name"]];
				level.number = num6;
				level.type = LevelType.BonusRound;
				level.isCompleted = false;
				string[] array2 = Chop(list4[dictionary["Value"]], 1);
				if (array2.Length > 1)
				{
					level.width = Convert.ToInt32(array2[0], 16);
					level.height = Convert.ToInt32(array2[1], 16);
				}
				else
				{
					level.width = 10;
					level.height = 10;
					UnityEngine.Debug.LogError("LevelSettings: wrong level width or height at level: " + (level.number + 1) + " " + level.name);
				}
				level.words = new List<LevelWord>();
				level.extraWords = new List<string>();
				level.iconRect = new Rect(Convert.ToInt32(list4[3]), Convert.ToInt32(list4[4]), Convert.ToInt32(list4[5]), Convert.ToInt32(list4[6]));
				level.stats = new LevelStats();
				level.hintTipIndex = -1;
				levelSet.specialLevels[level.height].Add(level);
			}
			if ((!flag2 || text2[0] != 'w') && (flag2 || !string.Equals(text2, "w")))
			{
				continue;
			}
			levelWord = new LevelWord();
			levelWord.word = list4[dictionary["Name"]];
			levelWord.letters = new List<LevelLetter>();
			levelWord.Reset();
			string[] array3 = Chop(list4[dictionary["Value"]], 1);
			for (int j = 0; j < levelWord.word.Length; j++)
			{
				levelLetter = new LevelLetter();
				levelLetter.letter = levelWord.word.Substring(j, 1);
				if (j * 2 + 1 < array3.Length)
				{
					levelLetter.x = Convert.ToInt32(array3[j * 2], 16);
					levelLetter.y = Convert.ToInt32(array3[j * 2 + 1], 16);
				}
				else
				{
					levelLetter.x = -1;
					levelLetter.y = -1;
					UnityEngine.Debug.LogError("LevelSettings: wrong letter x or y at level: " + (level.number + 1) + " " + level.name + " word: " + levelWord.word + " letter: " + levelLetter.letter);
				}
				levelLetter.isCoin = false;
				levelWord.letters.Add(levelLetter);
			}
			level.words.Add(levelWord);
		}
		UnityEngine.Debug.Log("Time Levels Parse: " + (Time.realtimeSinceStartup - realtimeSinceStartup));
		realtimeSinceStartup = Time.realtimeSinceStartup;
		UnityEngine.Debug.Log(levelSet);
		levelSet.AfterParseAction();
		UnityEngine.Debug.Log("Time Other: " + (Time.realtimeSinceStartup - realtimeSinceStartup));
		realtimeSinceStartup = Time.realtimeSinceStartup;
		aCompleteInv.GetType().GetMethod(aCompleteMet).Invoke(aCompleteInv, null);
	}

	private static string[] Chop(string aString, int aLength)
	{
		int num = aString.Length;
		int num2 = (num + aLength - 1) / aLength;
		string[] array = new string[num2];
		for (int i = 0; i < num2; i++)
		{
			array[i] = aString.Substring(i * aLength, Mathf.Min(aLength, num));
			num -= aLength;
		}
		return array;
	}
}
