using System.Collections.Generic;
using System.IO;

public class World
{
	public string name;

	public List<Pack> packs;

	public bool isCompleted;

	public void Save(BinaryWriter aBinaryWriter)
	{
		aBinaryWriter.Write(isCompleted);
		aBinaryWriter.Write((short)packs.Count);
		foreach (Pack pack in packs)
		{
			aBinaryWriter.Write(pack.name);
			pack.Save(aBinaryWriter);
		}
	}

	public void Load(BinaryReader aBinaryReader, int aVersion)
	{
		if (aVersion < 100)
		{
			return;
		}
		isCompleted = aBinaryReader.ReadBoolean();
		int num = aBinaryReader.ReadInt16();
		for (int i = 0; i < num; i++)
		{
			string value = aBinaryReader.ReadString();
			bool flag = true;
			foreach (Pack pack in packs)
			{
				if (pack.name.Equals(value))
				{
					pack.Load(aBinaryReader, aVersion);
					flag = false;
					break;
				}
			}
			if (flag)
			{
				Pack.Skip(aBinaryReader, aVersion);
			}
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
				Pack.Skip(aBinaryReader, aVersion);
			}
		}
	}
}
