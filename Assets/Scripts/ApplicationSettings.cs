using System.IO;
using UnityEngine;

public class ApplicationSettings : ELSingleton<ApplicationSettings>
{
	public DeploymentEnvironment deploymentEnvironment;

	public ShopVersion shopVersion;

	public DeploymentSettings deploymentSettingsLive;

	public DeploymentSettings deploymentSettingsStaging;

	private const int ApplicationSettingsVersion = 101;

	private string ApplicationSettingsFileName;

	private bool isSaveRequest;

	private MonoBehaviour compleateInvoke;

	private string compleateInvokeMethod;

	public ApplicationData applicationData = new ApplicationData();

	public bool IsLoadCompleted
	{
		get;
		private set;
	}

	public DeploymentSettings DeploymentSettings
	{
		get
		{
			if (deploymentEnvironment != 0)
			{
				return deploymentSettingsStaging;
			}
			return deploymentSettingsLive;
		}
	}

	private void Awake()
	{
		ApplicationSettingsFileName = Application.persistentDataPath + "/data.dat";
		isSaveRequest = false;
		IsLoadCompleted = false;
	}

	private void Update()
	{
		if (isSaveRequest && IsLoadCompleted)
		{
			isSaveRequest = false;
			SaveInvoke();
		}
	}

	public void LoadSettings(MonoBehaviour aCompleateInvoke, string aCompleateInvokeMethod)
	{
		compleateInvoke = aCompleateInvoke;
		compleateInvokeMethod = aCompleateInvokeMethod;
		ELSingleton<XmlSettings>.Instance.LoadSettings(this, "XmlSettingsCompleted", "XmlSettings", DeploymentSettings.configURL + Application.version.Replace('.', '_') + "/");
	}

	public void XmlSettingsCompleted()
	{
		ELSingleton<LevelsSettings>.Instance.LoadSettings(this, "LevelsSettingsCompleted");
	}

	public void LevelsSettingsCompleted()
	{
		ELSingleton<LanguageSettings>.Instance.LoadSettings(this, "LanguageSettingsCompleted", "LanguageSettings", DeploymentSettings.configURL);
	}

	public void LanguageSettingsCompleted()
	{
		ELSingleton<DictionarySettings>.Instance.LoadSettings(this, "LoadSettingCompleted", "DictionarySettings", DeploymentSettings.configURL);
	}

	public void LoadSettingCompleted()
	{
		Load();
		compleateInvoke.Invoke(compleateInvokeMethod, 0f);
		IsLoadCompleted = true;
	}

	public void Save(bool aIsForceSave = false)
	{
		if (aIsForceSave && IsLoadCompleted)
		{
			SaveInvoke();
		}
		else
		{
			isSaveRequest = true;
		}
	}

	private void SaveInvoke(BinaryWriter aBinaryWriter)
	{
		aBinaryWriter.Write(101);
		aBinaryWriter.Write(ELSingleton<AudioManager>.Instance.sfxMute);
		aBinaryWriter.Write(ELSingleton<AudioManager>.Instance.musicMute);
		aBinaryWriter.Write(ELSingleton<RateUsManager>.Instance.consumed);
		aBinaryWriter.Write(ELSingleton<RateUsManager>.Instance.firstShowed);
		aBinaryWriter.Write((short)ELSingleton<RateUsManager>.Instance.trigerCount);
		aBinaryWriter.Write(ELSingleton<RateUsManager>.Instance.readyToDisplay);
		aBinaryWriter.Write((short)ELSingleton<ShareManager>.Instance.consumedCount);
		aBinaryWriter.Write(ELSingleton<ShareManager>.Instance.firstShowed);
		aBinaryWriter.Write((short)ELSingleton<ShareManager>.Instance.trigerCount);
		aBinaryWriter.Write(ELSingleton<ShareManager>.Instance.readyToDisplay);
		aBinaryWriter.Write(ELSingleton<GDPRManager>.Instance.isDisplayed);
		aBinaryWriter.Write(ELSingleton<FacebookManager>.Instance.connected);
		aBinaryWriter.Write(ELSingleton<CoinsManager>.Instance.Coins);
		aBinaryWriter.Write(ELSingleton<PointsManager>.Instance.Points);
		aBinaryWriter.Write(ELSingleton<ExtraWordsManager>.Instance.Index);
		aBinaryWriter.Write(ELSingleton<ExtraWordsManager>.Instance.WordsCurrent);
		aBinaryWriter.Write(ELSingleton<HintManager>.Instance.Hints.Count);
		for (int i = 0; i < ELSingleton<HintManager>.Instance.Hints.Count; i++)
		{
			aBinaryWriter.Write(ELSingleton<HintManager>.Instance.Hints[i].amount);
			aBinaryWriter.Write(ELSingleton<HintManager>.Instance.Hints[i].isAvailable);
		}
		aBinaryWriter.Write(ELSingleton<AdsManager>.Instance.IsTurnedOn);
		aBinaryWriter.Write(ELSingleton<IapManager>.Instance.RewardedVideoAdDateLast);
		aBinaryWriter.Write(ELSingleton<IapManager>.Instance.initialOfferConsummed);
		aBinaryWriter.Write(ELSingleton<DailyPuzzleManager>.Instance.DateLast);
		aBinaryWriter.Write(ELSingleton<GameWindow>.Instance.IsHowToPlayPopUp);
		aBinaryWriter.Write(applicationData.session);
		aBinaryWriter.Write(ELSingleton<TutorialManager>.Instance.Tutorials.Count);
		for (int j = 0; j < ELSingleton<TutorialManager>.Instance.Tutorials.Count; j++)
		{
			aBinaryWriter.Write(ELSingleton<TutorialManager>.Instance.Tutorials[j].isConsumed);
		}
		aBinaryWriter.Write(ELSingleton<FacebookManager>.Instance.popupConsumed);
	}

	private void SaveInvoke()
	{
		FileStream fileStream = File.Open(ApplicationSettingsFileName, FileMode.Create);
		BinaryWriter binaryWriter = new BinaryWriter(fileStream);
		SaveInvoke(binaryWriter);
		binaryWriter.Flush();
		binaryWriter.Close();
		fileStream.Close();
	}

	public void Load(BinaryReader aBinaryReader)
	{
		int num = aBinaryReader.ReadInt32();
		if (num >= 100)
		{
			ELSingleton<AudioManager>.Instance.sfxMute = aBinaryReader.ReadBoolean();
			ELSingleton<AudioManager>.Instance.musicMute = aBinaryReader.ReadBoolean();
			ELSingleton<RateUsManager>.Instance.consumed = aBinaryReader.ReadBoolean();
			ELSingleton<RateUsManager>.Instance.firstShowed = aBinaryReader.ReadBoolean();
			ELSingleton<RateUsManager>.Instance.trigerCount = aBinaryReader.ReadInt16();
			ELSingleton<RateUsManager>.Instance.readyToDisplay = aBinaryReader.ReadBoolean();
			ELSingleton<ShareManager>.Instance.consumedCount = aBinaryReader.ReadInt16();
			ELSingleton<ShareManager>.Instance.firstShowed = aBinaryReader.ReadBoolean();
			ELSingleton<ShareManager>.Instance.trigerCount = aBinaryReader.ReadInt16();
			ELSingleton<ShareManager>.Instance.readyToDisplay = aBinaryReader.ReadBoolean();
			ELSingleton<GDPRManager>.Instance.isDisplayed = aBinaryReader.ReadBoolean();
			ELSingleton<FacebookManager>.Instance.connected = aBinaryReader.ReadBoolean();
			ELSingleton<CoinsManager>.Instance.Coins = aBinaryReader.ReadInt32();
			ELSingleton<PointsManager>.Instance.Points = aBinaryReader.ReadInt32();
			ELSingleton<ExtraWordsManager>.Instance.Index = aBinaryReader.ReadInt32();
			ELSingleton<ExtraWordsManager>.Instance.WordsCurrent = aBinaryReader.ReadInt32();
			int num2 = aBinaryReader.ReadInt32();
			for (int i = 0; i < num2; i++)
			{
				int amount = aBinaryReader.ReadInt32();
				bool isAvailable = aBinaryReader.ReadBoolean();
				if (i < ELSingleton<HintManager>.Instance.Hints.Count)
				{
					ELSingleton<HintManager>.Instance.Hints[i].amount = amount;
					ELSingleton<HintManager>.Instance.Hints[i].isAvailable = isAvailable;
				}
			}
			ELSingleton<AdsManager>.Instance.IsTurnedOn = aBinaryReader.ReadBoolean();
			ELSingleton<IapManager>.Instance.RewardedVideoAdDateLast = aBinaryReader.ReadInt64();
			ELSingleton<IapManager>.Instance.initialOfferConsummed = aBinaryReader.ReadBoolean();
			ELSingleton<DailyPuzzleManager>.Instance.DateLast = aBinaryReader.ReadInt64();
			ELSingleton<GameWindow>.Instance.IsHowToPlayPopUp = aBinaryReader.ReadBoolean();
			applicationData.session = aBinaryReader.ReadInt32() + 1;
			int num3 = aBinaryReader.ReadInt32();
			for (int j = 0; j < num3; j++)
			{
				bool isConsumed = aBinaryReader.ReadBoolean();
				if (j < ELSingleton<TutorialManager>.Instance.Tutorials.Count)
				{
					ELSingleton<TutorialManager>.Instance.Tutorials[j].isConsumed = isConsumed;
				}
			}
		}
		if (num >= 101)
		{
			ELSingleton<FacebookManager>.Instance.popupConsumed = aBinaryReader.ReadBoolean();
		}
	}

	public void Load()
	{
		ELSingleton<CoinsManager>.Instance.Coins = ELSingleton<XmlSettings>.Instance.coinsConfig.initial;
		if (File.Exists(ApplicationSettingsFileName))
		{
			FileStream fileStream = File.Open(ApplicationSettingsFileName, FileMode.Open);
			BinaryReader binaryReader = new BinaryReader(fileStream);
			Load(binaryReader);
			binaryReader.Close();
			fileStream.Close();
		}
		ELSingleton<ExtraWordsManager>.Instance.Check();
	}
}
