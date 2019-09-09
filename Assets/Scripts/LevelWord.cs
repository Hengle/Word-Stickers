using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelWord
{
	public string word;

	public List<LevelLetter> letters;

	public bool isCompleted;

	public LevelHint hint;

	public void Save(BinaryWriter aBinaryWriter)
	{
		aBinaryWriter.Write(isCompleted);
		aBinaryWriter.Write((int)hint);
	}

	public void Load(BinaryReader aBinaryReader, int aVersion)
	{
		if (aVersion >= 100)
		{
			isCompleted = aBinaryReader.ReadBoolean();
			hint = (LevelHint)aBinaryReader.ReadInt32();
		}
	}

	public static void Skip(BinaryReader aBinaryReader, int aVersion)
	{
		if (aVersion >= 100)
		{
			aBinaryReader.ReadBoolean();
			aBinaryReader.ReadInt32();
		}
	}

	public void Reset()
	{
		isCompleted = false;
		hint = LevelHint.None;
	}

	public bool GenerateCoin()
	{
		int num = Random.Range(0, letters.Count);
		for (int i = 0; i < letters.Count; i++)
		{
			int index = (num + i) % letters.Count;
			if (!letters[index].isCoin)
			{
				letters[index].isCoin = true;
				return true;
			}
		}
		return false;
	}
}
