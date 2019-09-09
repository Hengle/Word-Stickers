using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class BlurRenderer_Mobile : MonoBehaviour
{
	[Range(0f, 25f)]
	public int Iterations;

	[Range(0f, 5f)]
	public int DownRes;

	public bool UpdateBlur = true;

	public float UpdateRate = 0.5f;

	public bool updateUsingGameTime = true;

	private float timeSinceUpdate;

	private RenderTexture BlurTexture;

	private Material mat;
    public Shader _shader;

	private void Start()
	{
		BlurTexture = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
		BlurTexture.Create();
		if (mat == null)
		{
			mat = new Material(/*Shader.Find("Hidden/GaussianBlur_Mobile")*/_shader);
		}
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		if (timeSinceUpdate >= UpdateRate && UpdateBlur)
		{
			int width = src.width >> DownRes;
			int height = src.height >> DownRes;
			RenderTexture renderTexture = RenderTexture.GetTemporary(width, height);
			Graphics.Blit(src, renderTexture);
			for (int i = 0; i < Iterations; i++)
			{
				RenderTexture temporary = RenderTexture.GetTemporary(width, height);
				Graphics.Blit(renderTexture, temporary, mat);
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture = temporary;
			}
			Graphics.Blit(renderTexture, BlurTexture);
			Shader.SetGlobalTexture("_MobileBlur", renderTexture);
			RenderTexture.ReleaseTemporary(renderTexture);
			timeSinceUpdate = 0f;
		}
		else
		{
			Shader.SetGlobalTexture("_MobileBlur", BlurTexture);
		}
		timeSinceUpdate += (updateUsingGameTime ? Time.deltaTime : 0.02f);
		Graphics.Blit(src, dst);
	}

	public static BlurRenderer_Mobile Create()
	{
		BlurRenderer_Mobile blurRenderer_Mobile = Camera.main.gameObject.GetComponent<BlurRenderer_Mobile>();
		if (blurRenderer_Mobile == null)
		{
			blurRenderer_Mobile = Camera.main.gameObject.AddComponent<BlurRenderer_Mobile>();
		}
		return blurRenderer_Mobile;
	}

	public static BlurRenderer_Mobile Create(Camera ThisCamera)
	{
		BlurRenderer_Mobile blurRenderer_Mobile = ThisCamera.gameObject.GetComponent<BlurRenderer_Mobile>();
		if (blurRenderer_Mobile == null)
		{
			blurRenderer_Mobile = ThisCamera.gameObject.AddComponent<BlurRenderer_Mobile>();
		}
		return blurRenderer_Mobile;
	}
}
