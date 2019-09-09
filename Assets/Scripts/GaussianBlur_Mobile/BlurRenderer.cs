using UnityEngine;

namespace GaussianBlur_Mobile
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	public class BlurRenderer : MonoBehaviour
	{
		[Range(0f, 25f)]
		public int Iterations;

		[Range(0f, 5f)]
		public int DownRes;

		public bool UpdateBlur = true;

		public float UpdateRate = 0.02f;

		private float lastUpdate;

		public RenderTexture BlurTexture;

		private Material mat;

		private void Start()
		{
			BlurTexture = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
			BlurTexture.Create();
			if (mat == null)
			{
				mat = new Material(Shader.Find("Hidden/GaussianBlur_Mobile"));
			}
		}

		private void OnRenderImage(RenderTexture src, RenderTexture dst)
		{
			if (Time.time - lastUpdate >= UpdateRate && UpdateBlur)
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
				lastUpdate = Time.time;
			}
			else
			{
				Shader.SetGlobalTexture("_MobileBlur", BlurTexture);
			}
			Graphics.Blit(src, dst);
		}

		public static BlurRenderer Create()
		{
			BlurRenderer blurRenderer = Camera.main.gameObject.GetComponent<BlurRenderer>();
			if (blurRenderer == null)
			{
				blurRenderer = Camera.main.gameObject.AddComponent<BlurRenderer>();
			}
			return blurRenderer;
		}

		public static BlurRenderer Create(Camera ThisCamera)
		{
			BlurRenderer blurRenderer = ThisCamera.gameObject.GetComponent<BlurRenderer>();
			if (blurRenderer == null)
			{
				blurRenderer = ThisCamera.gameObject.AddComponent<BlurRenderer>();
			}
			return blurRenderer;
		}
	}
}
