using UnityEngine;
using UnityEngine.UI;

public class IapItem : MonoBehaviour
{
	public Text title;

	public Text price;

	public Text coins;

	public Text goodStart;

	public Text startFinish;

	public Text expose;

	public Text tip;

	public Text hint;

	public GameObject noads;

	public Image icon;

	public Sprite[] icons;

	private XmlSettings.IapConfig iap;

	private bool isVisible = true;

	public void Prepare(XmlSettings.IapConfig iap)
	{
		this.iap = iap;
		if (title != null)
		{
			title.text = ELSingleton<IapManager>.Instance.getName(iap.iap);
		}
		if (price != null)
		{
			price.text = ELSingleton<IapManager>.Instance.getPriceString(iap.iap);
		}
		if (coins != null)
		{
			coins.text = string.Concat(iap.coins);
		}
		if (goodStart != null)
		{
			goodStart.text = string.Concat(iap.goodStart);
		}
		if (startFinish != null)
		{
			startFinish.text = string.Concat(iap.startFinish);
		}
		if (expose != null)
		{
			expose.text = string.Concat(iap.expose);
		}
		if (tip != null)
		{
			tip.text = string.Concat(iap.tip);
		}
		if (hint != null)
		{
			hint.text = string.Concat(iap.tip + iap.goodStart + iap.startFinish + iap.expose);
		}
		if (noads != null)
		{
			noads.SetActive(iap.noads);
		}
		if (icon != null)
		{
			icon.sprite = icons[iap.icon];
			icon.SetNativeSize();
		}
	}

	public void BuyButton()
	{
		if (ELSingleton<ShopPopUp>.Instance.isActiveAndEnabled)
		{
			ELSingleton<ShopPopUp>.Instance.HidePopUp();
		}
		ELSingleton<IapManager>.Instance.BuyProductID(iap.iap);
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
		CommonButton[] componentsInChildren = base.gameObject.GetComponentsInChildren<CommonButton>(includeInactive: true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Show();
		}
	}
}
