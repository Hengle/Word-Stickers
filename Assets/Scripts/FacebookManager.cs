using Facebook.Unity;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FacebookManager : ELSingleton<FacebookManager>
{
	public enum FBRankingState
	{
		INIT,
		LOADING,
		ERROR,
		DONE
	}

	public enum FBRankingType
	{
		GLOBAL,
		FRIENDS
	}

	public class FBRanking
	{
		public List<Player> players;

		public FBRankingState state;

		public FBRankingType type;

		public FBRanking(FBRankingType atype)
		{
			players = new List<Player>();
			type = atype;
			state = FBRankingState.INIT;
		}
	}

	[HideInInspector]
	public string lastError;

	[HideInInspector]
	public bool connected;

	[HideInInspector]
	public bool popupConsumed;

	private MonoBehaviour finishInvoke;

	private string finishInvokeMethod;

	private string fbLog = "";

	private Dictionary<string, object> FBUserDetails;

	public FBAvatarSprite fbAvatar;

	public Player player;

	private FBRanking globalRanking = new FBRanking(FBRankingType.GLOBAL);

	private FBRanking friendsRanking = new FBRanking(FBRankingType.FRIENDS);

	public FBRanking currentRanking;

	private MonoBehaviour rankingCallback;

	private string rankingCallbackMethod;

	private List<string> friendsIdList;

	private Dictionary<string, FBAvatarSprite> avatars = new Dictionary<string, FBAvatarSprite>();

	private void Awake()
	{
		if (!FB.IsInitialized)
		{
			FB.Init(InitCallback, OnHideUnity);
		}
		else
		{
			FB.ActivateApp();
		}
	}

	private void Update()
	{
		UpdateAvatars();
	}

	public bool ShouldShow()
	{
		if (!connected && !popupConsumed)
		{
			popupConsumed = true;
			ELSingleton<ApplicationSettings>.Instance.Save();
			return true;
		}
		return false;
	}

	private void InitCallback()
	{
		if (FB.IsInitialized)
		{
			FB.ActivateApp();
		}
		else
		{
			UnityEngine.Debug.Log("Failed to Initialize the Facebook SDK");
		}
	}

	private void OnHideUnity(bool isGameShown)
	{
		if (!isGameShown)
		{
			Time.timeScale = 0f;
		}
		else
		{
			Time.timeScale = 1f;
		}
	}

	private void FBSynchroLog(string msg, bool finish, bool error)
	{
		UnityEngine.Debug.Log("FacebookManager: " + msg);
		if (ELSingleton<ApplicationSettings>.Instance.deploymentEnvironment != 0)
		{
			fbLog = fbLog + msg + "\n";
		}
		lastError = (error ? msg : null);
		if (finish && finishInvoke != null && finishInvokeMethod != null)
		{
			finishInvoke.Invoke(finishInvokeMethod, 0f);
			finishInvoke = null;
			finishInvokeMethod = null;
		}
	}

	public void FBDisconnect()
	{
		connected = false;
		if (FB.IsInitialized && FB.IsLoggedIn)
		{
			FB.LogOut();
		}
	}

	public void connectedAction(MonoBehaviour invoke, string invokeMethod, bool firstConnect = false)
	{
		finishInvoke = invoke;
		finishInvokeMethod = invokeMethod;
		if (connected | firstConnect)
		{
			if (!FB.IsInitialized)
			{
				FBSynchroLog("Failed to Initialize the Facebook SDK", finish: true, error: true);
			}
			else if (FB.IsLoggedIn)
			{
				FBLoginGetInfo();
			}
			else
			{
				FB.LogInWithReadPermissions(new List<string>
				{
					"public_profile, user_friends"
				}, FBConnectedActionAuthCallback);
			}
		}
	}

	private void FBConnectedActionAuthCallback(ILoginResult result)
	{
		if (FB.IsLoggedIn)
		{
			FBLoginGetInfo();
		}
		else if (result.Cancelled)
		{
			FBSynchroLog("User cancelled login", finish: true, error: true);
		}
		else
		{
			FBSynchroLog(result.Error, finish: true, error: true);
		}
	}

	private void FBLoginGetInfo()
	{
		FB.API("/me?fields=name,picture.width(100).height(100),id", HttpMethod.GET, FBLoginGetInfoCallback, new Dictionary<string, string>());
	}

	private void FBLoginGetInfoCallback(IGraphResult result)
	{
		UnityEngine.Debug.Log(result.RawResult);
		FBUserDetails = (Dictionary<string, object>)result.ResultDictionary;
		if (FBUserDetails.ContainsKey("name") && FBUserDetails.ContainsKey("id") && FBUserDetails.ContainsKey("picture"))
		{
			StartCoroutine(FBLoginCoroutine());
		}
		if (!FBUserDetails.ContainsKey("picture"))
		{
			return;
		}
		Dictionary<string, object> dictionary = (Dictionary<string, object>)FBUserDetails["picture"];
		if (dictionary.ContainsKey("data"))
		{
			Dictionary<string, object> dictionary2 = (Dictionary<string, object>)dictionary["data"];
			if (dictionary2.ContainsKey("url"))
			{
				fbAvatar = new FBAvatarSprite((string)dictionary2["url"], start: true);
			}
		}
	}

	private string GetServerUrl()
	{
		if (ELSingleton<ApplicationSettings>.Instance.deploymentEnvironment == DeploymentEnvironment.Live)
		{
			return "https://eerylab.com/rank_service_wordgame/api-game-live.php";
		}
		return "https://eerylab.com/rank_service_wordgame/api-game.php";
	}

	private IEnumerator FBLoginCoroutine()
	{
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("method", "login");
		wWWForm.AddField("fb_id", string.Concat(FBUserDetails["id"]));
		wWWForm.AddField("fb_name", string.Concat(FBUserDetails["name"]));
		wWWForm.AddField("api_key", GetAPIKey("login", string.Concat(FBUserDetails["id"]), 0, null));
		UnityWebRequest www = UnityWebRequest.Post(GetServerUrl(), wWWForm);
		www.chunkedTransfer = false;
		yield return www.SendWebRequest();
		if (www.isNetworkError)
		{
			FBSynchroLog(www.error + "(" + www.responseCode + ")", finish: true, error: true);
			UnityEngine.Debug.Log(www.downloadHandler.text);
			yield break;
		}
		JSONNode jSONNode = HandleGeneralError(www.downloadHandler.text, finish: true, aerror: true);
		if (!(jSONNode != null))
		{
			yield break;
		}
		JSONNode jSONNode2 = jSONNode["player"];
		if (jSONNode2 != null && jSONNode2.IsObject)
		{
			player = Player.Create(jSONNode2.AsObject);
			if (player != null)
			{
				player.Update();
				if (finishInvoke != null && finishInvokeMethod != null)
				{
					finishInvoke.Invoke(finishInvokeMethod, 0f);
					finishInvoke = null;
					finishInvokeMethod = null;
				}
				connected = true;
				ELSingleton<ApplicationSettings>.Instance.Save();
			}
			else
			{
				FBSynchroLog("What the...? JSON format error.", finish: true, error: true);
			}
		}
		else
		{
			FBSynchroLog("What the...? JSON format error.", finish: true, error: true);
		}
	}

	public void FBSet()
	{
		if (player != null)
		{
			player.Update();
			StartCoroutine(FBSetCoroutine());
		}
	}

	private IEnumerator FBSetCoroutine()
	{
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("method", "set");
		wWWForm.AddField("id", player.id ?? "");
		wWWForm.AddField("score", string.Concat(player.score));
		wWWForm.AddField("data", player.sdata.GetData() ?? "");
		wWWForm.AddField("api_key", GetAPIKey("set", player.id ?? "", player.score, player.sdata.GetData()));
		UnityWebRequest www = UnityWebRequest.Post(GetServerUrl(), wWWForm);
		www.chunkedTransfer = false;
		yield return www.SendWebRequest();
		if (www.isNetworkError)
		{
			FBSynchroLog(www.error + "(" + www.responseCode + ")", finish: false, error: false);
			UnityEngine.Debug.Log(www.downloadHandler.text);
			yield break;
		}
		JSONNode jSONNode = HandleGeneralError(www.downloadHandler.text, finish: false, aerror: false);
		if (!(jSONNode != null))
		{
			yield break;
		}
		JSONNode jSONNode2 = jSONNode["player"];
		if (jSONNode2 != null && jSONNode2.IsObject)
		{
			player = Player.Create(jSONNode2.AsObject);
			if (player != null)
			{
				player.Update();
			}
			else
			{
				FBSynchroLog("What the...? JSON format error.", finish: false, error: false);
			}
		}
		else
		{
			FBSynchroLog("What the...? JSON format error.", finish: false, error: false);
		}
	}

	private JSONNode HandleGeneralError(string sjson, bool finish, bool aerror)
	{
		JSONNode jSONNode = (sjson != null) ? JSON.Parse(sjson) : null;
		if (jSONNode != null)
		{
			JSONNode jSONNode2 = jSONNode["error"];
			if (!(jSONNode2 == null) && (!jSONNode2.IsString || !jSONNode2.Value.Equals("false", StringComparison.OrdinalIgnoreCase)))
			{
				JSONNode jSONNode3 = jSONNode["msg"];
				JSONNode jSONNode4 = jSONNode["id"];
				if (jSONNode3 != null && jSONNode3.IsString && jSONNode4 != null && jSONNode4.IsNumber)
				{
					FBSynchroLog(jSONNode3.Value + " (" + jSONNode4.AsLong + ")", finish, aerror);
				}
				else
				{
					FBSynchroLog("What the...? JSON format error.", finish, aerror);
				}
				return null;
			}
			return jSONNode;
		}
		FBSynchroLog("What the...? JSON format error.", finish, aerror);
		return null;
	}

	private string GetRandomString(int aLength)
	{
		string text = "";
		for (int i = 0; i < aLength; i++)
		{
			text += $"{UnityEngine.Random.Range(0, 15):X}";
		}
		return text.ToLowerInvariant();
	}

	private string GetAPIKey(string aMethod, string aId, int aValue, string aValueString)
	{
		string randomString;
		string str = randomString = GetRandomString(8);
		randomString += "ABC5347834687ABC873646FF";
		randomString += aMethod;
		if (aId != null)
		{
			randomString += aId;
		}
		randomString += aValue;
		if (aValueString != null)
		{
			randomString += aValueString;
		}
		string str2 = ELUtils.SHA256(randomString);
		return string.Concat(str1: GetRandomString(32), str0: str + str2).ToLowerInvariant();
	}

	private void ReportRankingResult(string msg, FBRankingState state)
	{
		currentRanking.state = state;
		lastError = msg;
		if (rankingCallback != null && rankingCallbackMethod != null)
		{
			rankingCallback.Invoke(rankingCallbackMethod, 0f);
		}
	}

	public void UpdateRanking(FBRankingType atype, MonoBehaviour callbackObject, string callbackMethod)
	{
		rankingCallback = callbackObject;
		rankingCallbackMethod = callbackMethod;
		if (atype == FBRankingType.GLOBAL)
		{
			currentRanking = globalRanking;
			StartCoroutine(PlayerRankUpdateCorutine(PlayerGetGlobalRanking));
		}
		else
		{
			currentRanking = friendsRanking;
			StartCoroutine(PlayerRankUpdateCorutine(PlayerGetFriendsRanking));
		}
		currentRanking.players.Clear();
		currentRanking.state = FBRankingState.LOADING;
	}

	private IEnumerator PlayerRankUpdateCorutine(Func<IEnumerator> func)
	{
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("method", "update");
		wWWForm.AddField("id", this.player.id);
		wWWForm.AddField("data", this.player.sdata.GetData());
		wWWForm.AddField("api_key", GetAPIKey("update", this.player.id, 0, this.player.sdata.GetData()));
		UnityWebRequest www = UnityWebRequest.Post(GetServerUrl(), wWWForm);
		www.chunkedTransfer = false;
		yield return www.SendWebRequest();
		if (www.isNetworkError)
		{
			ReportRankingResult(www.error + "(" + www.responseCode + ")", FBRankingState.ERROR);
			UnityEngine.Debug.Log(www.downloadHandler.text);
			yield break;
		}
		JSONNode jSONNode = HandleGeneralError(www.downloadHandler.text, finish: false, aerror: false);
		if (!(jSONNode != null))
		{
			yield break;
		}
		JSONNode jSONNode2 = jSONNode["player"];
		if (jSONNode2 != null && jSONNode2.IsObject)
		{
			Player player = Player.Create(jSONNode2.AsObject);
			if (this.player != null)
			{
				this.player = player;
				this.player.Update();
				ELSingleton<ApplicationSettings>.Instance.Save();
				yield return StartCoroutine(func());
			}
			else
			{
				ReportRankingResult("What the...? JSON format error.", FBRankingState.ERROR);
			}
		}
		else
		{
			ReportRankingResult("What the...? JSON format error.", FBRankingState.ERROR);
		}
	}

	private IEnumerator PlayerGetGlobalRanking()
	{
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("method", "leadboard");
		wWWForm.AddField("start", "1");
		wWWForm.AddField("end", "10");
		wWWForm.AddField("api_key", GetAPIKey("leadboard", "1", 10, null));
		UnityWebRequest www = UnityWebRequest.Post(GetServerUrl(), wWWForm);
		www.chunkedTransfer = false;
		yield return www.SendWebRequest();
		if (www.isNetworkError)
		{
			ReportRankingResult(www.error + "(" + www.responseCode + ")", FBRankingState.ERROR);
			UnityEngine.Debug.Log(www.downloadHandler.text);
			yield break;
		}
		JSONNode jSONNode = HandleGeneralError(www.downloadHandler.text, finish: false, aerror: false);
		if (!(jSONNode != null))
		{
			yield break;
		}
		JSONNode jSONNode2 = jSONNode["leadboard"];
		if (jSONNode2 != null && jSONNode2.IsArray)
		{
			bool flag = false;
			JSONArray asArray = jSONNode2.AsArray;
			for (int i = 0; i < asArray.Count; i++)
			{
				JSONNode jSONNode3 = asArray[i];
				if (!jSONNode3.IsObject)
				{
					continue;
				}
				Player player = Player.Create(jSONNode3.AsObject);
				if (player != null)
				{
					if (player.id.Equals(this.player.id))
					{
						flag = true;
					}
					player.rank = i + 1;
					currentRanking.players.Add(player);
				}
			}
			if (!flag)
			{
				currentRanking.players.Add(this.player);
			}
			ReportRankingResult(null, FBRankingState.DONE);
		}
		else
		{
			ReportRankingResult("What the...? JSON format error.", FBRankingState.ERROR);
		}
	}

	private IEnumerator PlayerGetFriendsRanking()
	{
		if (friendsIdList == null)
		{
			FB.API("/me/friends?fields=id", HttpMethod.GET, PlayerGetFriendsRankingCallback, new Dictionary<string, string>());
			yield return null;
		}
		else if (friendsIdList.Count > 0)
		{
			yield return StartCoroutine(PlayerGetFriendsRankingOurService(friendsIdList));
		}
		else
		{
			ReportRankingResult(null, FBRankingState.DONE);
			yield return null;
		}
	}

	private void PlayerGetFriendsRankingCallback(IGraphResult result)
	{
		UnityEngine.Debug.Log(result.RawResult);
		friendsIdList = new List<string>();
		Dictionary<string, object> dictionary = (Dictionary<string, object>)result.ResultDictionary;
		if (dictionary.ContainsKey("data"))
		{
			foreach (Dictionary<string, object> item in (List<object>)dictionary["data"])
			{
				if (item.ContainsKey("id"))
				{
					string text = (string)item["id"];
					friendsIdList.Add(text);
					UnityEngine.Debug.Log(text);
				}
			}
			if (friendsIdList.Count > 0)
			{
				friendsIdList.Add((string)FBUserDetails["id"]);
				StartCoroutine(PlayerGetFriendsRankingOurService(friendsIdList));
			}
			else
			{
				ReportRankingResult(null, FBRankingState.DONE);
			}
		}
		else
		{
			ReportRankingResult("", FBRankingState.ERROR);
		}
	}

	private IEnumerator PlayerGetFriendsRankingOurService(List<string> ids)
	{
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("method", "get");
		wWWForm.AddField("count", string.Concat(ids.Count));
		int num = 1;
		foreach (string id in ids)
		{
			wWWForm.AddField("fb_id" + num, id);
			num++;
		}
		UnityWebRequest www = UnityWebRequest.Post(GetServerUrl(), wWWForm);
		www.chunkedTransfer = false;
		yield return www.SendWebRequest();
		if (www.isNetworkError)
		{
			ReportRankingResult(www.error + "(" + www.responseCode + ")", FBRankingState.ERROR);
			UnityEngine.Debug.Log(www.downloadHandler.text);
			yield break;
		}
		JSONNode jSONNode = HandleGeneralError(www.downloadHandler.text, finish: false, aerror: false);
		if (!(jSONNode != null))
		{
			yield break;
		}
		if (jSONNode != null && jSONNode.IsArray)
		{
			JSONArray asArray = jSONNode.AsArray;
			for (int i = 0; i < asArray.Count; i++)
			{
				JSONNode jSONNode2 = asArray[i];
				if (jSONNode2.IsObject)
				{
					Player player = Player.Create(jSONNode2.AsObject);
					if (player != null)
					{
						currentRanking.players.Add(player);
					}
				}
			}
			currentRanking.players.Sort((Player q, Player p) => p.score.CompareTo(q.score));
			ReportRankingResult(null, FBRankingState.DONE);
		}
		else
		{
			ReportRankingResult("What the...? JSON format error.", FBRankingState.ERROR);
		}
	}

	public FBAvatarSprite GetAvatarSprite(string fbId)
	{
		FBAvatarSprite fBAvatarSprite = null;
		if (avatars.ContainsKey(fbId))
		{
			fBAvatarSprite = avatars[fbId];
		}
		else
		{
			FBAvatarSprite fBAvatarSprite2 = new FBAvatarSprite("https://graph.facebook.com/" + fbId + "/picture?height=200&type=square&width=200");
			avatars.Add(fbId, fBAvatarSprite2);
			fBAvatarSprite = fBAvatarSprite2;
		}
		fBAvatarSprite.stateChanged = true;
		return fBAvatarSprite;
	}

	public void UpdateAvatars()
	{
		bool flag = false;
		foreach (FBAvatarSprite value in avatars.Values)
		{
			if (value.state == FBAvatarSprite.State.LOADING)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			foreach (FBAvatarSprite value2 in avatars.Values)
			{
				if (value2.state == FBAvatarSprite.State.INIT)
				{
					value2.StartDownload();
					break;
				}
			}
		}
	}
}
