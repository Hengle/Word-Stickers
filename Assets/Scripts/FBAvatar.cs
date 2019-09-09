using UnityEngine;
using UnityEngine.UI;

public class FBAvatar : CommonButton
{
	public Image avatar;

	public GameObject loading;

	public FBAvatarSprite avatarSprite;

	private void Start()
	{
		loading.SetActive(value: false);
	}

	private void Update()
	{
		FBAvatarSprite fBAvatarSprite = avatarSprite ?? ELSingleton<FacebookManager>.Instance.fbAvatar;
		if (fBAvatarSprite != null && fBAvatarSprite.stateChanged)
		{
			switch (fBAvatarSprite.state)
			{
			case FBAvatarSprite.State.LOADING:
				loading.SetActive(value: true);
				break;
			case FBAvatarSprite.State.READY:
				loading.SetActive(value: false);
				avatar.sprite = fBAvatarSprite.sprite;
				break;
			case FBAvatarSprite.State.ERROR:
				loading.SetActive(value: false);
				break;
			}
			fBAvatarSprite.stateChanged = false;
		}
	}
}
