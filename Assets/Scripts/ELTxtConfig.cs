using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ELTxtConfig<T> : ELSingleton<T> where T : MonoBehaviour
{
	public bool forceDownload;

	[HideInInspector]
	public string config;

	private MonoBehaviour compleateInvoke;

	private string compleateInvokeMethod;

	private string fileName;

	public void LoadSettings(MonoBehaviour compleateInvoke, string compleateInvokeMethod, string name, string url)
	{
		fileName = name;
		this.compleateInvoke = compleateInvoke;
		this.compleateInvokeMethod = compleateInvokeMethod;
		if (ELSingleton<ApplicationSettings>.Instance.DeploymentSettings.isDownloadConfigs || forceDownload)
		{
			StartCoroutine(WaitForLoadSettings(UnityWebRequest.Get(url + name + ".txt")));
		}
		else
		{
			ParseSettingsLocal();
		}
	}

	private IEnumerator WaitForLoadSettings(UnityWebRequest www)
	{
		yield return www;
		if (www.error == null)
		{
			ParseSettings(www.downloadHandler.text);
		}
		else
		{
			ParseSettingsLocal();
		}
	}

	public virtual void LoadSettingsCompleted()
	{
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
		config = data;
		LoadSettingsCompleted();
	}
}
