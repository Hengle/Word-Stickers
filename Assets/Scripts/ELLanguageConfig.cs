using System;
using System.Collections;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

public class ELLanguageConfig<T> : ELSingleton<T> where T : MonoBehaviour
{
	public bool forceDownload;

	private MonoBehaviour compleateInvoke;

	private string compleateInvokeMethod;

	private string fileName;

	private string[] strings;

	private string[] names;

	private string[] languages;

	private int languageCount;

	private int languageTextCount;

	private int currentLanguage;

	public void LoadSettings(MonoBehaviour compleateInvoke, string compleateInvokeMethod, string name, string url)
	{
		fileName = name;
		this.compleateInvoke = compleateInvoke;
		this.compleateInvokeMethod = compleateInvokeMethod;
		if (ELSingleton<ApplicationSettings>.Instance.DeploymentSettings.isDownloadConfigs || forceDownload)
		{
			UnityEngine.Debug.Log("Downloading: " + url + name + ".txt");
			StartCoroutine(WaitForLoadSettings(UnityWebRequest.Get(url + name + ".txt")));
		}
		else
		{
			UnityEngine.Debug.Log("Using local: " + name + ".txt");
			ParseSettingsLocal();
		}
	}

	private IEnumerator WaitForLoadSettings(UnityWebRequest www)
	{
		yield return www;
		if (!www.isHttpError && !www.isNetworkError)
		{
			UnityEngine.Debug.Log("Downloading completed");
			ParseSettings(www.downloadHandler.text);
		}
		else
		{
			UnityEngine.Debug.Log(www.error);
			ParseSettingsLocal();
		}
	}

	public void LoadSettingsCompleted()
	{
		SetLanguage(DetectLanguage());
		compleateInvoke.Invoke(compleateInvokeMethod, 0f);
	}

	public void ParseSettingsLocal()
	{
		TextAsset textAsset = new TextAsset();
		textAsset = (TextAsset)Resources.Load(fileName, typeof(TextAsset));
		if (!textAsset)
		{
			UnityEngine.Debug.Log("Can't load txt file! - " + fileName);
		}
		else
		{
			ParseSettings(textAsset.text);
		}
	}

	public void ParseSettings(string data)
	{
		languageCount = Convert.ToInt32(GetProperty(data, "lang_count"));
		languageTextCount = Convert.ToInt32(GetProperty(data, "text_count"));
		strings = new string[languageCount * languageTextCount];
		names = new string[languageCount];
		languages = new string[languageCount];
		for (int i = 0; i < languageCount; i++)
		{
			string aName = "lang" + i.ToString("D2") + "_name";
			names[i] = GetProperty(data, aName);
			aName = "lang" + i.ToString("D2") + "_lang";
			languages[i] = GetProperty(data, aName);
			for (int j = 0; j < languageTextCount; j++)
			{
				aName = "lang" + i.ToString("D2") + "_text" + j.ToString("D3");
				strings[i * languageTextCount + j] = GetProperty(data, aName);
			}
		}
		LoadSettingsCompleted();
	}

	private string GetProperty(string aText, string aName)
	{
		int num = aText.IndexOf(aName);
		int num2 = aText.IndexOf('"', num + 1);
		num2 = aText.IndexOf('"', num2 + 1) + 1;
		int num3 = aText.IndexOf('"', num2 + 1);
		if (num2 != -1 && num3 != -1)
		{
			return aText.Substring(num2, num3 - num2).Replace("\\n", "\n");
		}
		return "";
	}

	public int DetectLanguage()
	{
		UnityEngine.Debug.Log("Language detected: " + Application.systemLanguage.ToString());
		for (int i = 0; i < languageCount; i++)
		{
			if (string.Equals(languages[i], Application.systemLanguage.ToString(), StringComparison.OrdinalIgnoreCase))
			{
				UnityEngine.Debug.Log("Found config for language.");
				return i;
			}
		}
		UnityEngine.Debug.Log("Config for language not found. Using default " + languages[0]);
		return 0;
	}

	public void SetLanguage(int aLanguage)
	{
		currentLanguage = aLanguage;
	}

	public string GetString(int aIndex)
	{
		if (aIndex < 0)
		{
			return GetLanguageName(-1 - aIndex);
		}
		return strings[currentLanguage * languageTextCount + aIndex];
	}

	public string GetLanguageName(int aLanguage)
	{
		return names[aLanguage];
	}

	public int GetLanguageCount()
	{
		return languageCount;
	}

	public int GetCurrentLanguage()
	{
		return currentLanguage;
	}

	private bool BadDigit(char c)
	{
		bool num = c >= '0' && c <= '9';
		bool flag = c >= 'a' && c <= 'f';
		bool flag2 = c >= 'A' && c <= 'F';
		return !(num | flag | flag2);
	}

	public void DecodeEncodedNonAsciiCharactersTest(string value)
	{
		string text = "Emoji Test \"" + value + "\"";
		bool flag = false;
		string text2;
		for (int num = value.IndexOf("\\u"); num > -1; num = value.IndexOf("\\u", num + text2.Length))
		{
			int length = Math.Min(value.Length - num, 12);
			text2 = value.Substring(num, length);
			if (text2.Length < 12 || text2[0] != '\\' || text2[1] != 'u' || BadDigit(text2[2]) || BadDigit(text2[3]) || BadDigit(text2[4]) || BadDigit(text2[5]) || text2[6] != '\\' || text2[7] != 'u' || BadDigit(text2[8]) || BadDigit(text2[9]) || BadDigit(text2[10]) || BadDigit(text2[11]))
			{
				text = text + " Unicode : " + text2 + " ---> FORMAT ERROR!!! (\\uAAAA\\uBBBB)";
				flag = true;
			}
			else
			{
				int num2 = int.Parse(text2.Substring(2, 4), NumberStyles.HexNumber);
				int num3 = int.Parse(text2.Substring(8, 4), NumberStyles.HexNumber);
				if (num2 < 55296 || num2 > 56319 || num3 < 56320 || num3 > 57343)
				{
					text = text + " Unicode : " + text2 + " ---> SURROGATE PAIR FORMAT ERROR!!! (D800-DBFF and DC00-DFFF)";
					flag = true;
				}
			}
		}
		if (flag)
		{
			UnityEngine.Debug.Log(text);
		}
	}

	public string DecodeEncodedNonAsciiCharacters(string value)
	{
		value = Regex.Replace(value, "\\\\u(?<ValueA>[a-fA-F0-9]{4})\\\\u(?<ValueB>[a-fA-F0-9]{4})", delegate(Match m)
		{
			int num = int.Parse(m.Groups["ValueA"].Value, NumberStyles.HexNumber);
			int num2 = int.Parse(m.Groups["ValueB"].Value, NumberStyles.HexNumber);
			return (num >= 55296 && num <= 56319 && num2 >= 56320 && num2 <= 57343) ? (((char)(ushort)num).ToString() + ((char)(ushort)num2).ToString()) : "<Unicode surrogate pair error>";
		});
		value = Regex.Replace(value, "\\\\u(?<ValueA>[a-fA-F0-9]{4})", (Match m) => ((char)(ushort)int.Parse(m.Groups["ValueA"].Value, NumberStyles.HexNumber)).ToString());
		return value;
	}
}
