using UnityEngine;
using UnityEngine.UI;

namespace GaussianBlur_LiveBlur
{
	public class DemoSliderControl : MonoBehaviour
	{
		public Text ScreenBlurText;

		public Slider ScreenBlur;

		public Text ScreenLightnessText;

		public Slider ScreenLightness;

		public Text PanelBlurText;

		public Slider PanelBlur;

		public Text PanelLightnessText;

		public Slider PanelLightness;

		public Text WSPanelBlurText;

		public Slider WSPanelBlur;

		public Text WSPanelLightnessText;

		public Slider WSPanelLightness;

		public Material ScreenBlurLayer;

		public Material PanelBlurLayer;

		public Material WSPanelBlurLayer;

		private void Start()
		{
			Reset();
		}

		private void Update()
		{
			ScreenBlurLayer.SetFloat("_BlurSize", ScreenBlur.value);
			ScreenBlurText.text = "BlurSize: " + ScreenBlur.value.ToString("F3");
			ScreenBlurLayer.SetFloat("_Lightness", ScreenLightness.value);
			ScreenLightnessText.text = "Lightness: " + ScreenLightness.value.ToString("F3");
			PanelBlurLayer.SetFloat("_BlurSize", PanelBlur.value);
			PanelBlurText.text = "BlurSize: " + PanelBlur.value.ToString("F3");
			PanelBlurLayer.SetFloat("_Lightness", PanelLightness.value);
			PanelLightnessText.text = "Lightness: " + PanelLightness.value.ToString("F3");
			WSPanelBlurLayer.SetFloat("_BlurSize", WSPanelBlur.value);
			WSPanelBlurText.text = "BlurSize: " + WSPanelBlur.value.ToString("F3");
			WSPanelBlurLayer.SetFloat("_Lightness", WSPanelLightness.value);
			WSPanelLightnessText.text = "Lightness: " + WSPanelLightness.value.ToString("F3");
		}

		public void Reset()
		{
			ScreenBlur.value = 0f;
			ScreenLightness.value = 0f;
			PanelBlur.value = 5f;
			PanelLightness.value = 0.2f;
			WSPanelBlur.value = 50f;
			WSPanelLightness.value = -0.25f;
		}
	}
}
