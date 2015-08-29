using UnityEngine;
using System.Collections;

public class SceneLoader : MonoBehaviour 
{

	/// <summary>
	/// Deelay to load next scene.
	/// </summary>
	public float delay = 5.0f;
	
	/// <summary>
	/// Time until load starts
	/// </summary>
	private float timeToStart = 0;

	/// <summary>
	/// Time until load finishes
	/// </summary>
	private float timeToFinish = 0;


	/// <summary>
	/// Checks if time is accumulating.
	/// </summary>
	private bool isCounting = false;

	/// <summary>
	/// Async operantion to load next scene.
	/// </summary>
	private AsyncOperation async;

	// Use this for initialization
	void Start () 
	{
		System.GC.Collect();
		Resources.UnloadUnusedAssets();
		StartCoroutine("load");
		this.isCounting = true;
		this.timeToStart = 0;
	}
	
	IEnumerator load()
	{
		Debug.LogWarning("ASYNC LOAD STARTED - " +
						 "DO NOT EXIT PLAY MODE UNTIL SCENE LOADS... UNITY WILL CRASH");
		async = Application.LoadLevelAsync(SceneManager.SceneToLoad);
		async.allowSceneActivation = false;
		yield return async;
	}

	public void ActivateScene()
	{
		async.allowSceneActivation = true;
	}

	void Update()
	{
		if (this.isCounting)
		{
			if (this.timeToStart == 0)
			{
				this.timeToStart = Time.time;
				this.timeToFinish = this.timeToStart + delay;
			}

			if (Time.time < this.timeToFinish)
			{

			}
			else
			{
				this.isCounting = false;
				ActivateScene();
			}
		}
	}

}
