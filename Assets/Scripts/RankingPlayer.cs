using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankingPlayer : MonoBehaviour
{
	private bool isVisible = true;

	public Text rankText;

	public Text nameText;

	public TextMeshProUGUI scoreText;

	public FBAvatar avatar;

	public void Prepare(Player player, FBAvatarSprite a)
	{
		rankText.text = string.Concat(player.rank);
		nameText.text = player.displayName;
		scoreText.text = string.Concat(player.score);
		avatar.avatarSprite = a;
	}

	public void CheckVisible()
	{
		Vector3 vector = Camera.main.WorldToViewportPoint(base.gameObject.transform.position);
		bool flag = vector.y >= -0.1f && vector.y <= 1.1f;
		if (flag != isVisible)
		{
			base.gameObject.SetActive(flag);
		}
		isVisible = flag;
	}

	private void OnEnable()
	{
		avatar.Show();
	}
}
