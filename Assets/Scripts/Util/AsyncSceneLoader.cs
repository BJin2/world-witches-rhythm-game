using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;

public class AsyncSceneLoader
{
	public static void LoadAsyncAdditive(string sceneName, MonoBehaviour caller)
	{
		caller.StartCoroutine(LoadSceneAsyncAdditive(sceneName));
	}

	private static IEnumerator LoadSceneAsyncAdditive(string sceneName)
	{
		//var watch = System.Diagnostics.Stopwatch.StartNew();
		//Create Loading UI
		Slider loadingBar = CreateLoadingScreen();

		//Start loading scene asynchronously
		AsyncOperation async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

		//Will run when loading is done
		async.completed += (AsyncOperation value) =>
		{
			//Start unloading current scene asynchronously
			SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
			//Set loaded scene as active scene
			SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));


			//watch.Stop();
			//Debug.Log("Loading time : "+(watch.ElapsedMilliseconds / 1000.0f));
		};

		//While loading(every frame)
		while (!async.isDone)
		{
			//Update loading bar
			float progress = Mathf.Clamp01(async.progress / 0.9f);
			loadingBar.value = progress;
			//Debug.Log(progress);
			yield return null;
		}
	}

	private static Slider CreateLoadingScreen()
	{
		GameObject loadingCanvas = Resources.Load("Prefabs/UI/LoadingCanvas", typeof(GameObject)) as GameObject;
		GameObject loadingCanvasInstance = MonoBehaviour.Instantiate(loadingCanvas);
		Camera uiCamera = loadingCanvasInstance.transform.Find("UICamera").GetComponent<Camera>();
		Camera.main.GetUniversalAdditionalCameraData().cameraStack.Add(uiCamera);
		return loadingCanvasInstance.transform.Find("LoadingBar").GetComponent<Slider>();
	}
}
