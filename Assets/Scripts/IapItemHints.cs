using UnityEngine;
using UnityEngine.UI;

public class IapItemHints : IapItem
{
	public Image hintIcon;

	public Image hintIconName;

	public Sprite[] hintIcons;

	public Sprite[] hintIconNames;

	public new void Prepare(XmlSettings.IapConfig iap)
	{
		base.Prepare(iap);
		int num = (iap.goodStart <= 0) ? ((iap.startFinish > 0) ? 1 : ((iap.expose > 0) ? 2 : 3)) : 0;
		hintIcon.sprite = hintIcons[num];
		hintIconName.sprite = hintIconNames[num];
	}
}
