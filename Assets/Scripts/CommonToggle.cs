using UnityEngine;
using UnityEngine.EventSystems;

public class CommonToggle : CommonButton
{
	public bool toggle;

	public Animator sliderAnimator;

	public new void Reset()
	{
		base.Reset();
		sliderAnimator.Play(toggle ? "On" : "Off");
	}

	public void SetToggle(bool value, bool animate)
	{
		toggle = value;
		sliderAnimator.Play((!toggle) ? (animate ? "TurnOff" : "Off") : (animate ? "TurnOn" : "On"));
	}

	public override void OnPointerClick(PointerEventData eventData)
	{
		base.OnPointerClick(eventData);
		if (isClickable && base.IsEnabled)
		{
			SetToggle(!toggle, animate: true);
		}
	}
}
