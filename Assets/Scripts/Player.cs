using SimpleJSON;

public class Player
{
	public string id;

	public string fbId;

	public int score;

	public int time_epoch;

	private string data;

	public PlayerData sdata;

	public string displayName;

	public int rank;

	public static Player Create(JSONNode json)
	{
		Player player = new Player();
		if (!player.Parse(json))
		{
			return null;
		}
		return player;
	}

	private bool Parse(JSONNode json)
	{
		JSONNode jSONNode = json["id"];
		JSONNode jSONNode2 = json["fb_id"];
		JSONNode jSONNode3 = json["score"];
		JSONNode jSONNode4 = json["time_epoch"];
		JSONNode jSONNode5 = json["data"];
		JSONNode jSONNode6 = json["display_name"];
		JSONNode jSONNode7 = json["rank"];
		if (jSONNode != null)
		{
			id = jSONNode.Value;
		}
		if (jSONNode2 != null)
		{
			fbId = jSONNode2.Value;
		}
		if (jSONNode3 != null)
		{
			score = int.Parse(jSONNode3.Value);
		}
		if (jSONNode4 != null)
		{
			time_epoch = int.Parse(jSONNode4.Value);
		}
		if (jSONNode7 != null)
		{
			rank = int.Parse(jSONNode7.Value);
		}
		if (jSONNode5 != null)
		{
			data = jSONNode5.Value;
			sdata = PlayerData.Create(data);
		}
		if (jSONNode6 != null)
		{
			displayName = jSONNode6.Value;
		}
		if (jSONNode != null && jSONNode2 != null && jSONNode3 != null && jSONNode4 != null && jSONNode5 != null && jSONNode6 != null && jSONNode7 != null)
		{
			return sdata != null;
		}
		return false;
	}

	public void Update()
	{
		if (score > ELSingleton<PointsManager>.Instance.Points)
		{
			ELSingleton<PointsManager>.Instance.Points = score;
		}
		else
		{
			score = ELSingleton<PointsManager>.Instance.Points;
		}
		if (sdata != null)
		{
			sdata.Update();
		}
	}
}
