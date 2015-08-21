using UnityEngine;
using System.Collections;
using PlayFab;

public class GameController : MonoBehaviour 
{
	public static GameController Instance { get; private set; }

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
	void Start () 
	{
		SmashPlayFabManager.Instance.Initialise();
		SmashPlayFabManager.Instance.LoginWithAndroidDeviceID();

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
