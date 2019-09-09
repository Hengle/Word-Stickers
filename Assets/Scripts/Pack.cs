using System.Collections.Generic;
using System.IO;

public class Pack
{
	public int index;

	public string name;

	public List<Level> levels;

	public bool isCompleted;

	public int progressIndex = -1;

	public int GetLevelsToCompleteCount()
	{
		int num = levels.Count - 1;
		foreach (Level level in levels)
		{
			if (level.isCompleted)
			{
				num--;
			}
		}
		return num;
	}

	public void Save(BinaryWriter aBinaryWriter)
	{
		aBinaryWriter.Write(isCompleted);
		aBinaryWriter.Write((short)progressIndex);
		aBinaryWriter.Write((short)levels.Count);
		foreach (Level level in levels)
		{
			aBinaryWriter.Write(level.name);
			level.Save(aBinaryWriter);
		}
	}

	public void Load(BinaryReader aBinaryReader, int aVersion)
	{
		if (aVersion < 100)
		{
			return;
		}
		isCompleted = aBinaryReader.ReadBoolean();
		progressIndex = aBinaryReader.ReadInt16();
		int num = aBinaryReader.ReadInt16();
		for (int i = 0; i < num; i++)
		{
			string value = aBinaryReader.ReadString();
			bool flag = true;
			foreach (Level level in levels)
			{
				if (level.name.Equals(value))
				{
					level.Load(aBinaryReader, aVersion);
					flag = false;
					break;
				}
			}
			if (flag)
			{
				Level.Skip(aBinaryReader, aVersion);
			}
		}
	}

	public static void Skip(BinaryReader aBinaryReader, int aVersion)
	{
		if (aVersion >= 100)
		{
			aBinaryReader.ReadBoolean();
			aBinaryReader.ReadInt16();
			int num = aBinaryReader.ReadInt16();
			for (int i = 0; i < num; i++)
			{
				aBinaryReader.ReadString();
				Level.Skip(aBinaryReader, aVersion);
			}
		}
	}
}
