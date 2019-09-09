using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ELCanvas : ELSingleton<ELCanvas>
{
	public enum Ratio
	{
		iPhoneX,
		iPhone5,
		iPhone,
		iPad
	}

	public float referenceWidth;

	public float iphone;

	public float iphone5;

	public float iphoneX;

	public float ipad;

	private float scale;

	private Ratio canvasRatio;

	public Ratio CanvasRatio => canvasRatio;

	private void ResizeCanvas()
	{
		float num = (float)Screen.width * 1f / (float)Screen.height;
		if (num < 0.5f)
		{
			scale = iphoneX;
			canvasRatio = Ratio.iPhoneX;
		}
		else if (num < 0.6f)
		{
			scale = iphone5;
			canvasRatio = Ratio.iPhone5;
		}
		else if (num < 0.7f)
		{
			scale = iphone;
			canvasRatio = Ratio.iPhone;
		}
		else
		{
			scale = ipad;
			canvasRatio = Ratio.iPad;
		}
		scale *= (float)Screen.width / referenceWidth;
		GetComponent<CanvasScaler>().scaleFactor = scale;
	}

	private void Start()
	{
		ResizeCanvas();
	}

	private void Awake()
	{
		ResizeCanvas();
	}

	private void Update()
	{
	}
}
