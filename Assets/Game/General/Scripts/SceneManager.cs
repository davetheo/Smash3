using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour 
{
	/// <summary>
	/// Enum contaning all scenes names.
	/// </summary>
	public enum Scenes { Landing, Loading, MainMenu, ModeSelect, Tutorial, Game }


	public static SceneManager Instance { get; private set; }
    public const float defaultDelayTime = 0.5f;

	public static string SceneToLoad;

	void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
		}
		Instance = this;
		// Furthermore we make sure that we don't destroy between scenes (this is optional)
		DontDestroyOnLoad(gameObject);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ChangeScene(Scenes loadScene, float delayTime = defaultDelayTime)
	{
		SceneToLoad = loadScene.ToString();
		// start a coroutine that wais for the delay time and then loads the scene
		// also load the loading screen here and then load the requested scene.
		//Application.LoadLevel(sceneName);
		Invoke("CallChangeScene", defaultDelayTime);
	}

	private void CallChangeScene()
	{
		Application.LoadLevel(Scenes.Loading.ToString());
	}

}
