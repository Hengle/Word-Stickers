using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StickerMainMenu : MonoBehaviour
{
	public RectTransform icon;

	public RectTransform tile;

	public Image image;

	public Text text;

	private RectTransform parentRt;

	private float speedY = 1f;

	private float angle;

	public static int paused = 0;

	public static List<Rect> itemBounds = new List<Rect>();

	public static void Reset()
	{
		itemBounds.Clear();
	}

	private void Start()
	{
		bool flag = UnityEngine.Random.Range(0, 5) == 0;
		parentRt = base.transform.parent.parent.GetComponent<RectTransform>();
		float num = UnityEngine.Random.Range(2f, 3f);
		if (flag)
		{
			num = 1.2f;
		}
		icon.localScale = new Vector3(num, num, num);
		speedY = 0.05f;
		angle = UnityEngine.Random.Range(-30f, 30f);
		icon.gameObject.SetActive(!flag);
		tile.gameObject.SetActive(flag);
		Color color = image.color;
		color.a = UnityEngine.Random.Range(0.2f, 0.2f);
		image.color = color;
		text.text = (((char)(ushort)UnityEngine.Random.Range(65, 90)).ToString() ?? "");
		LevelInfo firstNotCompleateLevel = ELSingleton<LevelsSettings>.Instance.levelSet.GetFirstNotCompleateLevel();
		if (firstNotCompleateLevel.currentWorld >= ELSingleton<LevelsSettings>.Instance.levelSet.worlds.Count)
		{
			firstNotCompleateLevel.currentWorld = ELSingleton<LevelsSettings>.Instance.levelSet.worlds.Count - 1;
		}
		Pack pack = ELSingleton<LevelsSettings>.Instance.levelSet.GetPack(firstNotCompleateLevel);
		int num2 = UnityEngine.Random.Range(0, pack.levels.Count);
		if (pack.levels.Count < 25)
		{
			num2 = 0;
		}
		ELSingleton<IconsManager>.Instance.Load(pack.name);
		Sprite sprite = Sprite.Create(ELSingleton<IconsManager>.Instance.Icon.texture, new Rect(num2 % 5 * 96, num2 / 5 * 96, 96f, 96f), new Vector2(0f, 0f));
		image.sprite = sprite;
		bool flag2 = false;
		int num3 = 0;
		while (!flag2 && num3 < 10)
		{
			flag2 = true;
			Vector2 vector = new Vector2(UnityEngine.Random.Range((0f - parentRt.sizeDelta.x) / 2f, parentRt.sizeDelta.x / 2f), UnityEngine.Random.Range((0f - parentRt.sizeDelta.y) / 2f, parentRt.sizeDelta.y / 2f));
			Rect rect = new Rect(vector.x - icon.sizeDelta.x / 2f * num * 1.25f, vector.y - icon.sizeDelta.y * num * 1.25f, icon.sizeDelta.x * num * 1.25f, icon.sizeDelta.y * num * 1.25f);
			base.transform.localPosition = vector;
			foreach (Rect itemBound in itemBounds)
			{
				if (itemBound.Overlaps(rect))
				{
					flag2 = false;
					break;
				}
			}
			if (flag2)
			{
				itemBounds.Add(rect);
			}
			num3++;
		}
		if (num3 >= 10)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void Update()
	{
		if (paused == 0)
		{
			icon.rotation = Quaternion.Euler(0f, 0f, angle);
			tile.rotation = Quaternion.Euler(0f, 0f, angle);
			base.transform.Translate(new Vector2(0f, speedY * Time.deltaTime));
			if (base.transform.localPosition.y > parentRt.sizeDelta.y / 2f + icon.sizeDelta.y)
			{
				base.transform.localPosition = new Vector2(base.transform.localPosition.x, base.transform.localPosition.y - parentRt.sizeDelta.y - icon.sizeDelta.y * 2f);
			}
		}
	}
}
