using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEngine.Networking;

public class ELXmlConfig<T> : ELSingleton<T> where T : MonoBehaviour
{
	public bool forceDownload;

	public bool dontWaitForDownload;

	public bool cacheFile;

	public string downloadCustomUrlDevelopment;

	public string downloadCustomUrlStaging;

	public string downloadCustomUrlLive;

	public XmlNode config;

	private MonoBehaviour compleateInvoke;

	private string compleateInvokeMethod;

	private string fileName;

	private string persistentDataPath;

	private string persistentXmlDataPath;

	private void Awake()
	{
		persistentDataPath = Application.persistentDataPath;
	}

	public void LoadSettings(MonoBehaviour compleateInvoke, string compleateInvokeMethod, string name, string url)
	{
		fileName = name;
		persistentXmlDataPath = persistentDataPath + "/" + name + Application.version.GetHashCode().ToString("X") + ".xml";
		this.compleateInvoke = compleateInvoke;
		this.compleateInvokeMethod = compleateInvokeMethod;
		if (ELSingleton<ApplicationSettings>.Instance.DeploymentSettings.isDownloadConfigs || forceDownload)
		{
			if (ELSingleton<ApplicationSettings>.Instance.deploymentEnvironment == DeploymentEnvironment.Staging && downloadCustomUrlStaging != null && downloadCustomUrlStaging.Length > 0)
			{
				StartCoroutine(WaitForLoadSettings(UnityWebRequest.Get(downloadCustomUrlStaging)));
			}
			else if (ELSingleton<ApplicationSettings>.Instance.deploymentEnvironment == DeploymentEnvironment.Live && downloadCustomUrlLive != null && downloadCustomUrlLive.Length > 0)
			{
				StartCoroutine(WaitForLoadSettings(UnityWebRequest.Get(downloadCustomUrlLive)));
			}
			else
			{
				StartCoroutine(WaitForLoadSettings(UnityWebRequest.Get(url + name + ".xml")));
			}
			if (dontWaitForDownload)
			{
				ParseSettingsLocal();
			}
		}
		else
		{
			ParseSettingsLocal();
		}
	}

	private IEnumerator WaitForLoadSettings(UnityWebRequest www)
	{
		yield return www;
		if (!www.isNetworkError && !www.isHttpError)
		{
			if (!dontWaitForDownload)
			{
				ParseSettings(www.downloadHandler.text);
			}
			if (cacheFile)
			{
				FileStream fileStream = File.Open(persistentXmlDataPath, FileMode.Create);
				StreamWriter streamWriter = new StreamWriter(fileStream);
				streamWriter.WriteLine(www.downloadHandler.text);
				streamWriter.Close();
				fileStream.Close();
			}
		}
		else if (!dontWaitForDownload)
		{
			ParseSettingsLocal();
		}
	}

	public virtual void LoadSettingsCompleted()
	{
		compleateInvoke.GetType().GetMethod(compleateInvokeMethod).Invoke(compleateInvoke, null);
	}

	public void ParseSettingsLocal()
	{
		if (cacheFile && File.Exists(persistentXmlDataPath))
		{
			FileStream fileStream = File.Open(persistentXmlDataPath, FileMode.Open);
			StreamReader streamReader = new StreamReader(fileStream);
			string data = streamReader.ReadToEnd();
			streamReader.Close();
			fileStream.Close();
			ParseSettings(data);
		}
		else
		{
			TextAsset textAsset = new TextAsset();
			textAsset = (TextAsset)Resources.Load(fileName, typeof(TextAsset));
			if (!textAsset)
			{
				UnityEngine.Debug.Log("Can't load xml file! - " + fileName);
			}
			else
			{
				ParseSettings(textAsset.text);
			}
		}
	}

	public void ParseSettings(string data)
	{
		XmlDocument xmlDocument = new XmlDocument();
		if (data[0] != '<')
		{
			data = data.Substring(1);
		}
		xmlDocument.LoadXml(data);
		config = xmlDocument.DocumentElement;
		LoadSettingsCompleted();
	}

	public int ElementCount(string xPath)
	{
		return config.SelectNodes(xPath).Count;
	}

	public int IntXPathed(string xPath)
	{
		return Convert.ToInt32(config.SelectSingleNode(xPath).Value);
	}

	public long Int64XPathed(string xPath)
	{
		return Convert.ToInt64(config.SelectSingleNode(xPath).Value);
	}

	public float FloatXPathed(string xPath)
	{
		return Convert.ToSingle(config.SelectSingleNode(xPath).Value);
	}

	public bool BoolXPathed(string xPath)
	{
		return string.Equals(config.SelectSingleNode(xPath).Value, "true", StringComparison.OrdinalIgnoreCase);
	}

	public string StringXPathed(string xPath)
	{
		return config.SelectSingleNode(xPath).Value;
	}

	public XmlNode ElementXPathed(string xPath)
	{
		return config.SelectSingleNode(xPath);
	}
}
