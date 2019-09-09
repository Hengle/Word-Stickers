using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingWindow : ELWindow<LoadingWindow>
{
	public RectTransform inside;

	private new void Start()
	{
		ShowWindow();
		if (ELSingleton<ELCanvas>.Instance.CanvasRatio == ELCanvas.Ratio.iPad)
		{
			inside.localScale = new Vector3(0.8f, 0.8f, 0.8f);
		}
		if (ELSingleton<ELCanvas>.Instance.CanvasRatio == ELCanvas.Ratio.iPhone)
		{
			inside.localScale = new Vector3(0.9f, 0.9f, 0.9f);
		}
		if (ELSingleton<MenuWindow>.Instance == null)
		{
			StartCoroutine(AsynchronousLoad("Main"));
		}
	}

	private IEnumerator AsynchronousLoad(string scene)
	{
		yield return new WaitForSecondsRealtime(1f);
		SceneManager.LoadScene(scene);
	}

	private void Update()
	{
		InitWindow();
	}
}
