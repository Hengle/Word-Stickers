using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class FBAvatarSprite
{
	public enum State
	{
		INIT,
		LOADING,
		READY,
		ERROR
	}

	public string url;

	public Sprite sprite;

	public State state;

	public bool stateChanged;

	public FBAvatarSprite(string aurl, bool start = false)
	{
		url = aurl;
		state = State.INIT;
		if (start)
		{
			StartDownload();
		}
	}

	public void StartDownload()
	{
		ELSingleton<FacebookManager>.Instance.StartCoroutine(Download());
	}

	private IEnumerator Download()
	{
		state = State.LOADING;
		stateChanged = true;
		UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
		yield return www.SendWebRequest();
		if (www.isNetworkError || www.isHttpError)
		{
			state = State.ERROR;
			stateChanged = true;
			yield break;
		}
		state = State.READY;
		stateChanged = true;
		Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
		sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0f, 0f));
	}
}
