using UnityEngine;

public class IconsManager : ELSingleton<IconsManager>
{
	private string fileName;

	private Sprite icon;

	public Sprite Icon => icon;

	public Sprite Load(string aFileName)
	{
		if (!string.Equals(fileName, aFileName))
		{
			fileName = aFileName;
			TextAsset textAsset = Resources.Load($"Icons/{fileName.Replace(' ', '_')}") as TextAsset;
			if ((bool)textAsset)
			{
				Texture2D texture2D = new Texture2D(1, 1);
				texture2D.LoadImage(textAsset.bytes);
				texture2D.wrapMode = TextureWrapMode.Clamp;
				icon = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), new Vector2(0f, 0f));
				Resources.UnloadUnusedAssets();
			}
		}
		return icon;
	}

	public Sprite LoadIcon(LevelInfo aLevelInfo, Level aLevel)
	{
		if (aLevel.type == LevelType.Normal)
		{
			TextAsset textAsset = Resources.Load($"Icons/icon{aLevelInfo.currentWorld:D2}{aLevelInfo.currentPack:D2}{aLevelInfo.currentLevel:D2}") as TextAsset;
			if ((bool)textAsset)
			{
				Texture2D texture2D = new Texture2D(1, 1);
				texture2D.LoadImage(textAsset.bytes);
				texture2D.wrapMode = TextureWrapMode.Clamp;
				Sprite result = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), new Vector2(0f, 0f));
				Resources.UnloadUnusedAssets();
				return result;
			}
		}
		else if (aLevel.type == LevelType.DailyPuzzle)
		{
			TextAsset textAsset2 = Resources.Load("Icons/dicon") as TextAsset;
			if ((bool)textAsset2)
			{
				Texture2D texture2D2 = new Texture2D(1, 1);
				texture2D2.LoadImage(textAsset2.bytes);
				texture2D2.wrapMode = TextureWrapMode.Clamp;
				Sprite result2 = Sprite.Create(texture2D2, new Rect(0f, 0f, texture2D2.width, texture2D2.height), new Vector2(0f, 0f));
				Resources.UnloadUnusedAssets();
				return result2;
			}
		}
		else
		{
			TextAsset textAsset3 = Resources.Load("Icons/sicon") as TextAsset;
			if ((bool)textAsset3)
			{
				Texture2D texture2D3 = new Texture2D(1, 1);
				texture2D3.LoadImage(textAsset3.bytes);
				texture2D3.wrapMode = TextureWrapMode.Clamp;
				Sprite result3 = Sprite.Create(texture2D3, new Rect(0f, 0f, texture2D3.width, texture2D3.height), new Vector2(0f, 0f));
				Resources.UnloadUnusedAssets();
				return result3;
			}
		}
		return null;
	}

	public Sprite LoadIconBack(LevelInfo aLevelInfo, Level aLevel)
	{
		if (aLevel.type != LevelType.BonusRound && aLevel.type != LevelType.DailyPuzzle)
		{
			TextAsset textAsset = Resources.Load($"Icons/icon_back{aLevelInfo.currentWorld:D2}{aLevelInfo.currentPack:D2}{aLevelInfo.currentLevel:D2}") as TextAsset;
			if ((bool)textAsset)
			{
				Texture2D texture2D = new Texture2D(1, 1);
				texture2D.LoadImage(textAsset.bytes);
				texture2D.wrapMode = TextureWrapMode.Clamp;
				Sprite result = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), new Vector2(0f, 0f));
				Resources.UnloadUnusedAssets();
				return result;
			}
		}
		else
		{
			TextAsset textAsset2 = Resources.Load("Icons/sicon_back") as TextAsset;
			if ((bool)textAsset2)
			{
				Texture2D texture2D2 = new Texture2D(1, 1);
				texture2D2.LoadImage(textAsset2.bytes);
				texture2D2.wrapMode = TextureWrapMode.Clamp;
				Sprite result2 = Sprite.Create(texture2D2, new Rect(0f, 0f, texture2D2.width, texture2D2.height), new Vector2(0f, 0f));
				Resources.UnloadUnusedAssets();
				return result2;
			}
		}
		return null;
	}
}
